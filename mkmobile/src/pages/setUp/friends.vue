<template>
  <div class="relative">
    <TopBar class="center-one-search" :option="topBarOption">
      <div>
        <div class="three-tit-t">朋友</div>
        <div class="three-tit-b">{{count}}</div>
      </div>
    </TopBar>
     <ScrollRefresh @getData="loadinfo" :residualHeight="220" :isNeedUp="false" class="friendScroll">
    <div class="friends borderR clearfix p-38">
      <div class="innerWrap mt-40">
        <div class="status borderR bg-white">
          <div class="font60 title"></div>
          <div class="man base-flex">
            <div class="left-man base-flex"   @click="gofriend(left)">
              <img src="@/assets/imgs/set/blue-man.png"   class="author" alt />
              <div class="Tright mt-100">
                <div class="rate font60">{{Lpercentage}}%</div>
                <div class="num font-weight font60">{{LProfit}}</div>
              </div>
            </div>
            <div class="left-man base-flex"   @click="gofriend(right)">
              <img src="@/assets/imgs/set/red-man.png" class="author" alt />
              <div class="Tright mt-100">
                <div class="rate font60">{{Rpercentage}}%</div>
                <div class="num font-weight font60">{{RProfit}}</div>
              </div>
            </div>
          </div>
        </div>
        <div class="frind-title base-flex">
          <div class="opa-text font60 mb-40">我的朋友</div>
          <div class="t-yellow font60 mb-40">
            <router-link to="friendsList" class="t-yellow">全部</router-link>
          </div>
        </div>
        <div class="list bg-gray p-38 borderR border-s">
          <div class="listWrap" ref="listWrap">
            <div class="details-cou">
              <div class="relative" v-for="(item,index) in friendsList" :key="index">
                <img :src="item.photo" class="img" alt />
                <van-cell
                  class="cell-info borderR mb-40"
                  :title="item.NickName+'('+item.uID+')'"
                  is-link
                  @click="gofriend(item.uID)"
                />
              </div>
            </div>
          </div>
        </div>
        <div class="frind-title base-flex">
          <div class="opa-text font60 mb-40">子账号</div>
          <div class="t-yellow font60 mb-40">
            <router-link to="myFamily" class="t-yellow">全部</router-link>
          </div>
        </div>
        <div class="list bg-gray p-38 borderR border-s">
          <div class="listWrap" ref="listWrap">
            <div class="details-cou">
              <div class="relative" v-for="(item,index) in familyList" :key="index">
                <img :src="item.photo" class="img" alt />
                <van-cell
                  class="cell-info borderR mb-40"
                  :title="item.NickName+'('+item.uID+')'"
                  is-link
                  @click="gofriend(item.uID)"
                />
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
     </ScrollRefresh>
  </div>
</template>

<script>
import TopBar from 'components/TopBar'
import TopSearch from 'components/TopSearch'
import ScrollRefresh from "components/ScrollRefresh";
import { http } from 'util/request'
import { storage } from 'util/storage'
import { photoList } from 'util/const.js'
import { GetFriendsList,GetSearchFimaly, GetUserInfo,GetSearchRelation } from 'util/netApi'
export default {
  components: {
    TopBar,
    TopSearch,
    ScrollRefresh
  },
  data() {
    return {
      LProfit: 0,
      RProfit: 0,
      Lpercentage: 0,
      Rpercentage: 0,
      topBarOption: {
        iconLeft: 'iconlist2f',
        iconRight: ''
      },
      count:null,
      left:0,
      right:0,
      friendsList: [{ photo: photoList.head01, name: '' }],
      familyList: [{ photo: photoList.head01, name: '' }],
    }
  },
  mounted() {},
  computed: {},
  methods: {
    loadinfo() {
      var _this = this
      http(GetFriendsList, { uid: 0 }, json => {
        if (json.response != 'undefined') {
           _this.friendsList = json.response
           _this.count=json.response.length
           _this.friendsList = _this.friendsList.slice(0,3)
            _this.friendsList.forEach(element => {
            element.photo=photoList[element.photo];
          });
         
        }
      })
       http(GetSearchFimaly,{uid : 0}, json => {
        if (json.response != 'undefined') {
           _this.familyList = json.response
           _this.familyList = _this.familyList.slice(0,3)
            _this.familyList.forEach(element => {
            element.photo=photoList[element.photo];
          });
         
        }
      })
    },
    gofriend(uID) {
      this.$router.push({ name: 'relation', params: { uid: uID } })
    },
    TogetUserInfo() {
      http(GetUserInfo, null, json => {
        if (json.code === 0) {
          this.LProfit = json.response.lprofit
          this.RProfit = json.response.rprofit
          var resultl =
            (json.response.lprofit / (this.LProfit + this.RProfit)) * 100
          var resultr =
            (json.response.rprofit / (this.LProfit + this.RProfit)) * 100
          this.Lpercentage = isNaN(resultl) ? 0 : resultl.toFixed(2)
          this.Rpercentage = isNaN(resultr) ? 0 : resultr.toFixed(2)
        }
      })
      http(GetSearchRelation, { uid: 0 }, json => {
        if (json.response) {
            this.left = json.response[1].uID;
            if(this.left ==0)
              this.right =0
            else
              this.right = json.response[2].uID;
        }
      })
    } 
  },
  created() {
    this.loadinfo()
    this.TogetUserInfo()
  }
}
</script>
<style lang='less' scoped>
.friendScroll{
  /deep/.wrapper {
    padding-top: 20px;
    & .bscroll-container{
       min-height: calc(100vh - 260px);
    }
  }
}
.p-38{
  padding: 28px;
}
.friends {
  margin-top: -20px;
  
  .frind-title {
    height: 174px;
    line-height: 174px;
  }
  .innerWrap {
    // height: calc(100vh - 650px);
    // overflow: scroll;
  }
  .status {
    padding: 38px;
    box-shadow: 0px 5px 5px 0px rgba(0, 0, 0, 0.24);
    background: url('../../assets/imgs/set/BG.png') center center / 100% 100%;
    .title {
      color: rgba(25, 24, 25, 1);
      opacity: 0.52;
    }
    .man {
      height: 400px;
      padding: 40px 0;
      .left-man {
        width: 50%;
        padding: 30px;
        .rate {
          color: rgba(25, 24, 25, 1);
          opacity: 0.52;
        }
        .num {
          color: rgba(25, 24, 25, 1);
          opacity: 0.62;
        }
      }
      .author {
        width: 109px;
        height: 281px;
      }
    }
  }
  .list {
    background: #f2f3f7;
    .cell-info {
      height: 137px;
      line-height: 120px;
    }
    .img {
      position: absolute;
      left: 20px;
      top: 10px;
      width: 106px;
      height: 106px;
      z-index: 99;
    }
    /deep/.van-cell__right-icon {
      font-size: 60px;
      height: 137px;
      line-height: 120px;
      float: right;
    }
    /deep/.van-cell__title {
      font-size: 42px;
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
  // .listWrap {
  //   position: relative;
  //   height: calc(100vh - 1800px);
  //   overflow: hidden;
  // }
}
</style>
