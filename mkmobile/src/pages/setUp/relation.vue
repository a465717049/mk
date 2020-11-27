<template>

  <div class="relative">
    <TopBar class="center-one-search" :option="topBarOption">關係</TopBar>
    <ScrollRefresh @getData="loadData" :residualHeight="160" :isNeedUp="false">
    <div class="relation-box p-58 border-top-radius relative bg-gray">
      <div class="relation">
        <TopSearch class="search-area" @onSearch='Search'></TopSearch>
        <div class="concat">
          <div class="first-relation one base-flex around P-58 mt-100">
            <div class="opa-text Tright num1 font60 mt-100">{{First.LProfit}}</div>
            <div class="person center ver-center">
              <img :src="First.photo" class="author" alt />
              <div class="name">{{First.NickName+'('+First.uID+')'}}</div>
            </div>
            <div class="opa-text Tleft num2 font60 mt-100">{{First.RProfit}}</div>
          </div>
          <!-- 第二层 -->
          <div class="first-relation two base-flex around">
            <div v-for="(item,index) in SecList" :key="'s'+index">
              <div v-if="item.jid>0&&item.uID>0" @click="go(item.uID)" class="person1 center ver-center">
                <img :src="item.photo" class="author" alt />
                <div class="name">{{item.NickName+'('+item.uID+')'}}</div>
              </div>
              <div v-else-if="item.jid>0&&item.uID==0">
                <div class="person1 center ver-center" @click="goJoin(item.jid,item.isLeft)">
                  <i class="author author-icon iconfont icondingwei"></i>
                  <div class="name">入駐</div>
                </div>
              </div>
              <div v-else-if="item.jid==0&&item.uID==0">
                <div class="person1 center ver-center">
                  <i class="author author-icon iconfont icondingwei"></i>
                  <div class="name"></div>
                </div>
              </div>
            </div>
          </div>
          <!-- 第三层 -->
          <div class="first-relation thr base-flex around">
            <div v-for="(item,index) in ThrLeftList" :key="'tl'+index" class="base-flex around">
              <div v-if="item.jid>0&&item.uID>0" @click="go(item.uID)" class="person3 center ver-center">
                <img :src="item.photo" class="author" alt />
                <div class="name"  v-html="item.NickName+'<br/>('+item.uID+')'"></div>
              </div>
              <div v-if="item.jid>0&&item.uID==0">
                <div class="person3 center ver-center" @click="goJoin(item.jid,item.isLeft)">
                  <i class="author author-icon iconfont icondingwei"></i>
                  <div class="name">入駐</div>
                </div>
              </div>
              <div v-if="item.jid==0&&item.uID==0">
                <div class="person3 center ver-center">
                  <i class="author author-icon iconfont icondingwei"></i>
                  <div class="name"></div>
                </div>
              </div>
            </div>
            <div v-for="(item,index) in ThrRightList" :key="'tr'+index" class="base-flex around">
              <div v-if="item.jid>0&&item.uID>0" @click="go(item.uID)" class="person3 center ver-center">
                <img :src="item.photo" class="author" alt />
                <div class="name" v-html="item.NickName+'<br/>('+item.uID+')'" ></div>
              </div>
              <div v-if="item.jid>0&&item.uID==0">
                <div class="person3 center ver-center" @click="goJoin(item.jid,item.isLeft)">
                  <i class="author author-icon iconfont icondingwei"></i>
                  <div class="name">入駐</div>
                </div>
              </div>
              <div v-if="item.jid==0&&item.uID==0">
                <div class="person3 center ver-center">
                  <i class="author author-icon"></i>
                  <div class="name"></div>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div class="arrow base-flex flex-center">
          <div class="arrow-show center" @click="backUser()">
            <i class="iconfont iconarrow-left author font80"></i>
          </div>
          <div class="arrow-show center" @click="up()">
            <i class="iconfont iconarrowup author font80"></i>
          </div>
        </div>
      </div>
    </div>
    <YellowComfirm :show="isEnter" @clickOver="clickOverpay" @clickOk="clickOk()" :tipTitle="tipTitle" @clickNo="clickNo()" :showConfirmBtn="true"></YellowComfirm>
     </ScrollRefresh>
  </div>
 
</template>
<script>
import TopBar from "components/TopBar";
import TopSearch from "components/TopSearch";
import YellowComfirm from "components/YellowComfirm";
import ScrollRefresh from "components/ScrollRefresh";
import { GetSearchRelation } from "util/netApi";
import { http } from "util/request";
import { GetFriendList } from "util/netApi";
import { storage } from "util/storage";
import { photoList } from "util/const.js";
export default {
  components: {
    TopBar,
    TopSearch,
    YellowComfirm,
    ScrollRefresh
  },
  data() {
    return {
      realtionid: 0,
      topBarOption: {
        iconLeft: "back",
        iconRight: ""
      },
      tipTitle: "請選擇入駐方式",
      isEnter: false,
      upID: 0,
      parentId: 0,
      isLeft: 1,
      historyList: [],
      First: {},
      SecList: [],
      ThrLeftList: [],
      ThrRightList: []
    };
  },
  methods: {
    goJoin(parentId, isLeft) {
      this.isEnter = true;
      this.parentId = parentId;
      this.isLeft = isLeft;
    },
    clickOk() {
      this.isEnter = false;
      this.$router.push({
        name: "JoinUs",
        params: { uid: this.parentId, isLeft: this.isLeft }
      });
    },
    clickNo() {
      this.isEnter = false;
      this.$router.push({
        name: "JoinFamily",
        params: { uid: this.parentId, isLeft: this.isLeft }
      });
    },
    go(uid) {
      this.realtionid = uid;
      this.loadData();
    },
    up() {
      this.realtionid = this.upID;
      this.loadData();
    },
    backUser() {
      var that = this;
      for (var index = this.historyList.length; index >= 0; index--) {
        var element = this.historyList[index];
        if (element == this.realtionid) {
          let i = index > 0 ? index - 1 : index;
          this.realtionid = that.historyList[i];
          that.historyList.splice(index, 1);
          that.loadData(true);
          break;
        }
      }
    },
    Search(uid)
    {
      if(uid=="")
       uid=0
      this.realtionid =uid;
      this.loadData();
    },
    loadData(history) {
      var that = this;
      if (!history) {
        this.historyList.push(this.realtionid);
      }
      this.SecList = [];
      this.ThrLeftList = [];
      this.ThrRightList = [];
      http(GetSearchRelation, { uid: this.realtionid }, json => {
        if (json.response) {
          that.friendsList = json.response;
          that.First = that.friendsList[0];
          that.upID = that.friendsList[0].jid;

          that.friendsList.forEach(element => {
            element.photo = photoList[element.photo];
            if (element.ceng == 1) {
              that.SecList.push(element);
            } else if (element.ceng == 2) {
              if (element.pos <= 2) {
                that.ThrLeftList.push(element);
              } else {
                that.ThrRightList.push(element);
              }
            }
          });

          if (that.SecList.length == 1) {
            that.SecList.push({
              honorLevel: 0,
              InvestmentLevel: 0,
              NickName: "",
              photo: "",
              jid: 0,
              L: 1,
              R: 0,
              subAccount: 0,
              friends: 0,
              LProfit: 0,
              RProfit: 0,
              LCount: 0,
              RCount: 0,
              uID: 0
            });
          }
          if (that.ThrLeftList.length < 2) {
            that.ThrLeftList.push({
              honorLevel: 0,
              InvestmentLevel: 0,
              NickName: "",
              photo: "",
              jid: 0,
              L: 1,
              R: 0,
              subAccount: 0,
              friends: 0,
              LProfit: 0,
              RProfit: 0,
              LCount: 0,
              RCount: 0,
              uID: 0
            });
            if (that.ThrLeftList.length == 1) {
              that.ThrLeftList.push({
                honorLevel: 0,
                InvestmentLevel: 0,
                NickName: "",
                photo: "",
                jid: 0,
                L: 1,
                R: 0,
                subAccount: 0,
                friends: 0,
                LProfit: 0,
                RProfit: 0,
                LCount: 0,
                RCount: 0,
                uID: 0
              });
            }
          }
          if (that.ThrRightList.length < 2) {
            that.ThrRightList.push({
              honorLevel: 0,
              InvestmentLevel: 0,
              NickName: "",
              photo: "",
              jid: 0,
              L: 1,
              R: 0,
              subAccount: 0,
              friends: 0,
              LProfit: 0,
              RProfit: 0,
              LCount: 0,
              RCount: 0,
              uID: 0
            });
            if (that.ThrRightList.length == 1) {
              that.ThrRightList.push({
                honorLevel: 0,
                InvestmentLevel: 0,
                NickName: "",
                photo: "",
                jid: 0,
                L: 1,
                R: 0,
                subAccount: 0,
                friends: 0,
                LProfit: 0,
                RProfit: 0,
                LCount: 0,
                RCount: 0,
                uID: 0
              });
            }
          }
        }
      });
    }
  },
  created() {
    this.realtionid = this.$route.params.uid;
    this.loadData();
  },
  mounted() {}
};
</script>
<style lang='less' scoped>
.relation-box {
  margin-top: -20px;
  // height: 75vh;
  // overflow: hidden;
}
.author-icon {
  font-size: 90px;
  opacity: 0.4;
}
.name {
  font-weight: bold;
  color: rgba(94, 91, 94, 0.877);
}
.relation {
  .concat {
    height: 1195px;
    padding: 20px 58px;
    background: url("../../assets/imgs/set/BG2.png") center center / 100% 100%;
    .title {
      color: rgba(25, 24, 25, 1);
      opacity: 0.52;
    }
    .first-relation {
      .num1 {
        width: 33%;
      }
      margin-top: 120px;
      .num2 {
        width: 33%;
      }
      .person {
        width: 234px;
        height: 229px;
        margin: 0 60px;
        background: url("../../assets/imgs/set/circle-img.png") center center /
          100% 100%;
        .author {
          width: 118px;
          height: 118px;
          margin-top: 40px;
          margin-left: 10px;
          display: inline-block;
        }
        .name {
          // width: 132px;
          height: 32px;
          margin: 62px auto 0;
          padding-left: 6px;
        }
      }
    }
     .first-relation.one {
       margin-top: 60px;
     }
      .first-relation.two {
       margin-top: 100px;
     }
      .first-relation.thr {
       margin-top: 160px;
     }
    .person1 {
      width: 263px;
      height: 239px;
      background: url("../../assets/imgs/set/triger-img.png") center center /
        100% 100%;
      .author {
        width: 122px;
        height: 122px;
        margin-left: 20px;
        margin-top: 62px;
      }
      i.author {
        width: 122px;
        height: 122px;
        margin-left: 20px;
        display: inline-block;
        margin-top: 50px;
      }
      .name {
        // width: 132px;
        height: 32px;
        margin: 62px auto 0;
        padding-left: 16px;
      }
    }
    .person2 {
      width: 263px;
      height: 239px;
      background: url("../../assets/imgs/set/triger-img.png") center center /
        100% 100%;
      .author {
        width: 122px;
        height: 122px;
        margin-left: 10px;
        margin-top: 60px;
      }

      .name {
        width: 132px;
        height: 32px;
        margin: 22px auto 0;
        padding-left: 6px;
      }
    }
    .person3 {
      width: 184px;
      height: 184px;
      margin: 0 20px;
      justify-content: center;
      align-items: center;
      background: url("../../assets/imgs/set/circle-img.png") center center /
        100% 100%;
      .author {
        width: 119px;
        height: 119px;
        margin-left: 12px;
        margin-top: 20px;
        display: inline-block;
      }

      .name {
        width: 132px;
        height: 32px;
        display: block;
        text-align: center;
        margin: 32px auto 0;
        padding-left: 6px;
      }
      .name.add {
        margin: 22px auto 0;
      }
    }
  }
}
.arrow {
  margin-top: 50px;
  .arrow-show {
    width: 143px;
    height: 140px;
    margin: 0 30px;
    background: url("../../assets/imgs/set/squer-img.png") center center / 100%
      100%;
    .author {
      width: 122px;
      height: 122px;
      margin-left: 10px;
      margin-top: 64px;
    }

    .name {
      width: 132px;
      height: 32px;
      margin: 32px auto 0;
      padding-left: 6px;
    }
  }
  .person3 {
    width: 184px;
    height: 184px;
    margin: 0 20px;
    justify-content: center;
    align-items: center;
    background: url("../../assets/imgs/set/circle-img.png") center center / 100%
      100%;
    .author {
      width: 119px;
      height: 119px;
      margin-left: 12px;
      margin-top: 20px;
      display: inline-block;
    }

    .name {
      width: 132px;
      height: 32px;
      display: block;
      text-align: center;
      margin: 32px auto 0;
      padding-left: 6px;
    }
    .name.add {
      margin: 22px auto 0;
    }
  }
}
.person2 {
  width: 263px;
  height: 239px;
  background: url("../../assets/imgs/set/triger-img.png") center center / 100%
    100%;
  .author {
    width: 122px;
    height: 122px;
    margin-left: 10px;
    margin-top: 60px;
  }

  .name {
    width: 132px;
    height: 32px;
    margin: 22px auto 0;
    padding-left: 6px;
  }
}
.person3 {
  width: 184px;
  height: 184px;
  margin: 0 20px;
  justify-content: center;
  align-items: center;
  background: url("../../assets/imgs/set/circle-img.png") center center / 100%
    100%;
  .author {
    width: 119px;
    height: 119px;
    margin-left: 12px;
    margin-top: 20px;
    display: inline-block;
  }

  .name {
    width: 132px;
    height: 32px;
    display: block;
    text-align: center;
    margin: 22px auto 0;
    padding-left: 6px;
  }
  .name.add {
    margin: 22px auto 0;
  }
}

.arrow {
  margin-top: 50px;
  .arrow-show {
    width: 143px;
    height: 140px;
    margin: 0 30px;
    background: url("../../assets/imgs/set/squer-img.png") center center / 100%
      100%;
    .author {
      color: #7ab4b4;
      margin-top: 35px;
      margin-left: 10px;
    }
  }
}
</style>