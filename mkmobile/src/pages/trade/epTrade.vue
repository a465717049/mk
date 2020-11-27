<template>
  <div class="ep-trade">
    <TopBar class="center-one-search" :option="topBarOption">EP交易</TopBar>
    <ScrollRefresh @getData="readloadinfo" :residualHeight="0" :isNeedUp="false" class="chartsScroll">

      <div class="border-top-radius relative bg-gray">
        <div class="trade borderR bg-gray clearfix p-58">
          <van-tabs @click="readloadinfo" v-model="active" color="#6e21d1" line-height="0">
            <van-tab title="出售中">
              <!-- 详细信息 -->
              <ScrollRefresh @getData="ToGetEPRecordList" :residualHeight="200" :active="active" :isNone="isNone">
                <div class="detail" @click="routerChagne('paying',item)" v-for="(item,index) in saleData" :key="index">
                  <div class="info">
                    <img :src="photoList[item.img]" alt />
                    <div class="num">
                      <div class="font42">{{item.euid+':'+item.name}}</div>
                      <div class="sub-num">{{item.date}}</div>
                    </div>
                  </div>
                  <div class="price">{{item.price}}</div>
                </div>
              </ScrollRefresh>
            </van-tab>
            <van-tab title="交易中">
              <!-- 详细信息 -->
              <ScrollRefresh @getData="ToGetEPRecordList" :residualHeight="200" :active="active" :isNone="isNone">
                <div class="detail" v-for="(item,index) in tranData" :key="index" @click="routerChagne('paying',item)">
                  <div class="info">
                    <img :src="photoList[item.img]" alt />
                    <div class="num">
                      <div class="font42">{{item.euid+':'+item.name}}</div>
                      <div class="sub-num">{{item.date}}</div>
                    </div>
                  </div>
                  <div class="price">{{item.price}}</div>
                </div>
              </ScrollRefresh>
            </van-tab>
            <van-tab title="已完成">
              <!-- 详细信息 -->
              <ScrollRefresh @getData="ToGetEPRecordList" :residualHeight="200" :active="active" :isNone="isNone">
                <div class="detail" v-for="(item,index) in compData" :key="index" @click="routerChagne('etInfomation',item)">
                  <div class="info">
                    <img :src="photoList[item.img]" alt />
                    <div class="num">
                      <div class="font42">{{item.euid+':'+item.name}}</div>
                      <div class="sub-num">{{item.date}}</div>
                    </div>
                  </div>
                  <div class="price">{{item.price}}</div>
                </div>
              </ScrollRefresh>
            </van-tab>
          </van-tabs>
        </div>
      </div>
    </ScrollRefresh>
  </div>

</template>
<script>
import { http } from "util/request";
import { GetEPRecordList } from "util/netApi";
import { storage } from "util/storage";
import { accessToken, photoList } from "util/const.js";
import ScrollRefresh from "components/ScrollRefresh";
import TopBar from "components/TopBar";
export default {
  name: "Set",
  components: {
    TopBar,
    ScrollRefresh,
  },
  data() {
    return {
      showChat: true,
      nowcktype: 0,
      saleData: [
        {
          id: 0,
          euid: 0,
          status: 0,
          price: 450,
          name: 500,
          date: "6 Mel,2020",
        },
      ],
      tranData: [],
      compData: [],
      photoList: photoList,
      topBarOption: {
        iconLeft: "back",
        iconRight: "",
      },
      active: 0,
      pageSize: 20,
      pageIndex: 1,
      isNone: false, //数据是否加载完毕
    };
  },
  methods: {
    readloadinfo(pageIndex = 1) {
      this.ToGetEPRecordList(pageIndex);
    },
    routerChagne(routeName, item) {
      if (item.status == 4) {
        routeName = "receiving";
      }
      this.$router.push("./" + routeName + "?id=" + item.id);
    },
    ToGetEPRecordList(pageIndex) {
      var cktype = 1;
      if (this.active == 0) {
        cktype = 1;
      } else if (this.active == 1) {
        cktype = 2;
      } else if (this.active == 2) {
        cktype = 8;
      }
      if (this.nowcktype !== cktype) {
        pageIndex = 1;
      }
      this.nowcktype = cktype;
      let _this = this;
      http(
        GetEPRecordList,
        { type: cktype, pageSize: this.pageSize, pageIndex },
        (json) => {
          if (json.response != "undefined") {
            if (pageIndex !== 1 && json.response.length === 0) {
              this.isNone = true;
            } else {
              this.isNone = false;
              if (pageIndex === 1) {
                if (cktype == 1) {
                  this.saleData = json.response;
                } else if (cktype == 2) {
                  this.tranData = json.response;
                } else if (cktype == 8) {
                  this.compData = json.response;
                }
              } else {
                if (cktype == 1) {
                  this.saleData = [...this.saleData, ...json.response];
                } else if (cktype == 2) {
                  this.tranData = [...this.tranData, ...json.response];
                } else if (cktype == 8) {
                  this.compData = [...this.compData, ...json.response];
                }
              }
            }
          }
        }
      );
    },
  },
  created() {
    this.ToGetEPRecordList(1);
  },
  mounted() {},
};
</script>
<style lang='less' scoped>
.ep-trade {
  /deep/.wrapper .bscroll-container {
    min-height: calc(100vh - 560px);
  }
}

.relative {
  width: 100vw;
  padding-bottom: 300px;
  overflow-x: none;
  height: calc(100vh - 280px);
  background-color: #ebeaf0;
  border-radius: 40px 40px 0 0;
}
.ep-trade {
  .trade {
    height: 75vh;
    margin-top: -20px;
    border-radius: 40px 40px 0 0;

    // height: 10;
    // border-radius: 40px 40px 0 0;
    /deep/.van-tabs__content {
      padding-top: 10px;
    }
    /deep/.van-tabs {
      height: 70px;
      line-height: 70px;
    }
    /deep/.van-tabs__wrap {
      height: 70px;
    }
    /deep/.van-tab {
      font-size: 42px;
      font-weight: 600;
      background: #ebeaf0;
      border-bottom: 6px solid #767c8f;
      color: #6e21d1;
    }
    /deep/.van-tab__text {
      color: #767c8f;
    }
    /deep/.van-tab--active {
      border-bottom: 6px solid #6e21d1;
      /deep/.van-tab__text {
        color: #6e21d1;
      }
    }

    .detail {
      display: flex;
      flex-direction: row;
      justify-content: space-aroud;
      padding: 58px;
      background: #fff;
      margin-top: 40px;
      border-radius: 40px;
      height: 189px;
      .price {
        color: #09b216;
        font-size: 48px;
        font-weight: 600;
        width: 40%;
        text-align: right;
      }
      .info {
        margin-top: -40px;
        width: 60%;
        display: flex;
        flex-direction: row;
        justify-content: space-aroud;
        .num {
          font-weight: 600;
          opacity: 0.62;
          .sub-num {
            color: #767c8f;
            font-size: 40px;
          }
        }
        > img {
          margin-right: 40px;
          width: 123px;
          height: 133px;
        }
      }
    }
  }
}
</style>