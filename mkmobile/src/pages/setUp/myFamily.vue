<template>
  <div class="relative my-family">
    <TopBar class="center-one-search" :option="topBarOption" @onSearch="search">
      <div>
        <div class="three-tit-t">子賬號</div>
        <div class="three-tit-b">{{familyList.length}}</div>
        <TopSearch></TopSearch>
      </div>
    </TopBar>
    <ScrollRefresh
      @getData="loadinfo"
      :residualHeight="340"
      :isNeedUp="false"
      class="relativeScroll"
    >
      <div class="listWrap">
        <div class="friendsList" ref="listWrap">
          <div class="list bg-gray">
            <div class="relative" v-for="(item,index) in familyList" :key="index">
              <img :src="item.photo" class="img" alt />
              <van-cell class="cell-info borderR mb-40" is-link @click="gofriend(item.uID)">
                <div slot="title" class="data-title">
                  <div class="left">{{item.NickName+'('+item.uID+')'}}</div>
                  <div class="right">{{item.uCreateTime}}</div>
                </div>
                <div slot="title" class="data-info">
                  <div>
                    <span>DPE：</span>
                    {{item.DPE}}
                  </div>
                  <div>
                    <span>EP：</span>
                    {{item.EP}}
                  </div>
                </div>
                <div slot="title" class="data-info">
                  <div>
                    <span>RP：</span>
                    {{item.RP}}
                  </div>
                  <div>
                    <span>SP：</span>
                    {{item.SP}}
                  </div>
                </div>
              </van-cell>
            </div>
          </div>
        </div>
      </div>
    </ScrollRefresh>
    <div class="set-btn base-flex around p-40">
      <button class="t-yellow cWhite s-btn" @click="onekey()">集结至主账户</button>
    </div>
    <YellowComfirm :show="isEnter" :tipTitle="tips" @clickOver="clickOverpay" @clickOk="clickOk()"></YellowComfirm>
  </div>
</template>
<script>
import TopBar from "components/TopBar";
import YellowComfirm from "components/YellowComfirm";
import ScrollRefresh from "components/ScrollRefresh";
import { http } from "util/request";
import TopSearch from "components/TopSearch";
import { photoList } from "util/const.js";
import { GetSearchFimaly, GetUserInfo, GetOneKeyReturn } from "util/netApi";
export default {
  components: {
    TopBar,
    TopSearch,
    YellowComfirm,
    ScrollRefresh
  },
  data() {
    return {
      topBarOption: {
        iconLeft: "back",
        iconRight: ""
      },
      isEnter: false,
      tips: "",
      tipsObj: {
        success: "處理成功！",
        nosuccess: "處理失败，請稍後再嘗試！"
      },
      familyList: [],
      uid: 0
    };
  },
  mounted() {},
  computed: {},
  methods: {
    search(uid) {
      if (uid == "") {
        this.uid = 0;
      } else {
        this.uid = uid;
      }
      this.loadinfo();
    },
    loadinfo() {
      var _this = this;
      http(GetSearchFimaly, { uid: this.uid }, json => {
        if (json.response) {
          _this.familyList = json.response;
          _this.familyList.forEach(element => {
            element.photo = photoList[element.photo];
          });
        }
      });
    },
    gofriend(uID) {
      this.$router.push({ name: "relation", params: { uid: uID } });
    },
    clickOk() {
      this.isEnter = false;
    },
    onekey() {
      var _this = this;
      http(GetOneKeyReturn, null, json => {
        if (json.code === 0) {
          _this.isEnter = true;
          _this.tips = _this.tipsObj.success;
          this.loadinfo(0);
        } else {
          _this.isEnter = true;
          _this.tips = _this.tipsObj.nosuccess;
        }
      });
    }
  },
  created() {
    this.loadinfo();
  }
};
</script>
<style lang='less' scoped>
.relativeScroll {
  /deep/.wrapper .bscroll-container {
    min-height: calc(100vh - 900px);
  }
}
.my-family {
  .set-btn {
    .s-btn {
      margin-top: 40px;
      height: 120px;
      width: 46%;
      text-align: center;
      border-radius: 21px;
      background: #ffbb18;
      font-size: 55px;
    }
  }
}
.listWrap {
  padding: 28px;
}
.p-38 {
  padding: 28px;
}
.friendsList {
  margin-top:60px;
  // position: relative;
  // height: calc(100vh - 1000px);
  // overflow: auto;
  // border-radius: 42px;
  .list {
    // overflow: scroll;
    // height: auto;
    // padding: 28px;
    background: #f2f3f7;
    // border-radius: 42px;
    .cell-info {
      line-height: 120px;
    }
    .img {
      position: absolute;
      left: 30px;
      top: 80px;
      width: 106px;
      height: 106px;
      z-index: 99;
    }
    /deep/.van-cell__right-icon {
      font-size: 60px;
      height: 137px;
      line-height: 120px;
      margin-top: 80px;
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
      color: rgba(118, 124, 143, 1);
      height: 120px;
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
    width: 50%;
    color: #e28b07;
    span {
      color: #660a79;
      text-align: right;
      display: inline-block;
      width: 120px;
    }
  }
}
.data-title {
  div.left {
    display: inline-flex;
    width: 32%;
  }
  div.right {
    display: inline-flex;
    width: 45%;
    text-align: right;
    margin-left: 100px;
  }
}
</style>