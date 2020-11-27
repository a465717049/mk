<template>
  <div class="relative">
    <TopBar class="center-one-search" :option="topBarOption">
      <div>
        <div class="three-tit-t">好友列表</div>
        <div class="three-tit-b">{{friendsList.length}}</div>
        <TopSearch @onSearch="search"></TopSearch>
      </div>
    </TopBar>
    <ScrollRefresh
      @getData="loadinfo"
      :residualHeight="260"
      :isNeedUp="false"
      class="relativeScroll"
    >
      <div class="listWrap borderR" ref="listWrap">
        <div class="friendsList">
          <div class="list borderR bg-gray p-38">
            <div class="relative" v-for="(item,index) in friendsList" :key="index">
              <img :src="item.photo" class="img" alt />
              <van-cell class="cell-info borderR mb-40" is-link @click="gofriend(item.uID)">
                <div slot="title" class="data-title">
                  <div class="left">{{item.NickName+'('+item.uID+')'}}</div>
                  <div class="right">{{item.uCreateTime}}</div>
                </div>
                 <div slot="title" class="data-info">
                  <div>
                    <span>農 &nbsp;&nbsp;&nbsp;場：</span>
                    {{item.InvestmentLevel==1?'高級農場':'中級農場'}}
                  </div>
                 <div>
                    <span>榮  &nbsp;&nbsp;&nbsp;譽：</span>
                    {{ getLv(item.honorLevel)}}
                  </div>
                </div>
                <div slot="title" class="data-info">
                  <div>
                    <span>水果區：</span>
                    {{item.LProfit}}
                  </div>
                  <div>
                    <span>蔬菜區：</span>
                    {{item.RProfit}}
                  </div>
                </div>
              </van-cell>
            </div>
          </div>
        </div>
      </div>
    </ScrollRefresh>
  </div>
</template>
<script>
import TopBar from "components/TopBar";
import { http } from "util/request";
import { photoList } from "util/const.js";
import TopSearch from "components/TopSearch";
import ScrollRefresh from "components/ScrollRefresh";
import { GetFriendsList, GetSearchFimaly, GetUserInfo } from "util/netApi";
export default {
  components: {
    TopBar,
    TopSearch,
    ScrollRefresh
  },
  data() {
    return {
      topBarOption: {
        iconLeft: "back",
        iconRight: ""
      },
      friendsList: [],
      uid: 0
    };
  },
  mounted() {},
  computed: {},
  methods: {
    loadinfo() {
      var _this = this;
      http(GetFriendsList, { uid: this.uid }, json => {
        if (json.response) {
          _this.friendsList = json.response;
          _this.friendsList.forEach(element => {
            element.photo = photoList[element.photo];
          });
        }
      });
    },
    search(uid) {
      if (uid == "") {
        this.uid = 0;
      } else {
        this.uid = uid;
      }
      this.loadinfo();
    },
    getLv(level){
      var lv=''
          if(level==0)  lv='普通玩家'
          if(level==1)  lv='達標社區'
          if(level==2)  lv='初級社區'
          if(level==3)  lv='中級社區'
          if(level==4)  lv='高級社區'
          if(level==5)  lv='超級社區'
          if(level==6)  lv='橘子派'
          if(level==7)  lv='香瓜派'
          if(level==8)  lv='菠蘿派'
          if(level==9)  lv='柚子派'
          if(level==10)  lv='蘋果派'
          return lv;
    },
    gofriend(uID) {
      this.$router.push({ name: "relation", params: { uid: uID } });
    }
  },
  created() {
    this.loadinfo();
  }
};
</script>
<style lang='less' scoped>
.relativeScroll {
  /deep/.wrapper {
    padding-top: 20px;
  }
}
.p-38 {
  padding: 28px;
}
.listWrap {
  position: relative;
  // height: calc(100vh - 650px);
  // overflow: scroll;
  width: 90%;
  margin: -40px auto 0;
}
.friendsList {
  margin-top: 60px;
  .list {
    background: #f2f3f7;
    .cell-info {
      line-height: 120px;
    }
    .img {
      position: absolute;
      left: 20px;
      top: 80px;
      width: 106px;
      height: 106px;
      z-index: 99;
    }
    /deep/.van-cell__right-icon {
      font-size: 60px;
      height: 137px;
      margin-top: 50px;
      line-height: 120px;
    }
    /deep/.van-cell__title {
      font-size: 42px;
      margin-left: 140px;
      font-weight: bold;
      color: rgba(118, 124, 143, 1);
      position: relative;
    }
    /deep/.van-cell__value {
      font-size: 35px;
      margin-left: 140px;
      font-weight: bold;
      color: rgba(118, 124, 143, 1);
      position: relative;
    }
    /deep/.van-progress__pivot {
      display: none;
    }
    .cell-special {
      /deep/.van-cell__title {
        margin-left: 600px;
      }
    }
  }
}

.data-info {
  height: 60px;
  line-height: 60px;
  div {
    display: inline-flex;
    width: 48%;
    color: #e28b07;
    text-align: left;
    span {
      color: #660a79;
      text-align: right;
      display: inline-block;
      width: 150px;
      white-space: nowrap;
    }
  }
}
.data-title {
  div.left {
    display: inline-flex;
    width: 46%;
  }
  div.right {
    display: inline-flex;
    width: 48%;
    text-align: right;
    margin-left: 0px;
  }
}
</style>