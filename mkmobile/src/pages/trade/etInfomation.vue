<template>
  <div class="ep-info">
    <TopBar class="center-one-search" :option="topBarOption">EP交易</TopBar>
      <div class="border-top-radius relative bg-gray">
        <div class="trade borderR bg-gray clearfix p-58">
          <!-- 详细信息 -->
          <div class="items">
            <div class="title base-text">賣家信息</div>
            <div class="detail">
              <div class="info">
                <img src="@/assets/imgs/set/person.png" alt />
                <div class="num">
                  <div class="font50">{{epuid}}</div>
                  <div class="sub-num">
                    纍計出售:
                    <span class="base-purple">{{eptcount}}</span> 完成交易:
                    <span class="base-purple">{{epfcount}}</span>
                  </div>
                </div>
              </div>
              <div class="price">
                <div>
                  <ul>
                    <li v-for="index in rate" :key="index" class="active lis">
                      <i class="iconfont iconstar"></i>
                    </li>
                    <li v-for="index in 5 - rate" class="lis" :key="index">
                      <i class="iconfont iconstar"></i>
                    </li>
                  </ul>
                </div>
                <div class="p-text center">信用</div>
              </div>
            </div>
          </div>

          <!-- 详细信息 -->
          <div class="items mt-80 ep-date">
            <div class="title base-text">詳情</div>
            <div class="relative special-date">
              <div class="info">
                <div class="ok">ok</div>
                <img src="@/assets/imgs/set/flag.png" class="flag-img" alt />
              </div>
              <div class="date">
                <div class="ep-price">{{epamount}} EP</div>
                <div class="p-date base-text">{{epdate}}</div>
              </div>
            </div>
          </div>
          <!-- 详细信息 -->
          <div class="items mt-80 e-price-info">
            <div class="title base-text">價格</div>
            <div class="payBox">
              <ul>
                <li class="box left">
                  <span class="money">{{epcnymoney.toFixed(3)}}</span>
                  <span class="type">CNY</span>
                </li>
                <li class="line"></li>
                <li class="box right">
                  <span class="money">{{epusdtmoney.toFixed(3)}}</span>
                  <span class="type">USDT</span>
                </li>
              </ul>
              <div class="bottom">1 EP = {{EPRate}}CNY 1 USDT = {{USDTRate}}CNY</div>
            </div>
          </div>
          <p class="closedTime">關閉時間: {{epdate}}</p>
        </div>
      </div>
  </div>
</template>
<script>
import TopBar from "components/TopBar";
import { http } from "util/request";
import { GetEPExchangeById } from "util/netApi";
import { storage } from "util/storage";
import { accessToken, loginPro } from "util/const.js";
export default {
  name: "Set",
  components: {
    TopBar
  },
  data() {
    return {
      epid: 0,
      showChat: true,
      rate: 5,
      epuid: 0,
      epamount: 0,
      eptcount: 0,
      epfcount: 0,
      epcnymoney: 0.0,
      epusdtmoney: 0.0,
      EPRate: "6.175",
      USDTRate: "7.01",
      epdate: "",
      topBarOption: {
        iconLeft: "back",
        iconRight: ""
      },
      active: 2
    };
  },
  methods: {
    ToGetEPExchangeById(value) {
      if (!this.$route.query.id) {
        return false;
      }
      this.epid = this.$route.query.id;
      http(GetEPExchangeById, { id: this.epid }, json => {
        if (json.code === 0) {
          // initData.name  times  finish  complaint  price  date phone
          var model = json.response[0];
          this.epuid = model.uid;
          this.epamount = model.amount;
          this.eptcount = model.tcount;
          this.epcnymoney = model.MoneyCNY;
          this.epusdtmoney = model.MoneyUSDT;
          this.EPRate = model.RMBrate;
          this.USDTRate = model.USDTrate;
          this.epfcount = model.fcount;
          this.epdate = model.createTime;
        }
      });
    }
  },
  created() {
    this.ToGetEPExchangeById();
  },
  mounted() {}
};
</script>
<style lang='less' scoped>
.epScroll{
  /deep/ .wrapper  .bscroll-container{
    min-height: calc(100vh - 340px);
  }
}
.ep-info {
  .trade {
    height: 80vh;
    margin-top: -20px;
    border-radius: 40px 40px 0 0;
    .title {
      font-size: 50px;
      margin-top: 30px;
      margin-bottom: 30px;
    }
    .detail {
      display: flex;
      flex-direction: row;
      justify-content: space-aroud;
      padding: 58px 20px;
      background: #fff;
      margin-top: 40px;
      border-radius: 40px;
      height: 189px;
      .price {
        height: 50px;
        line-height: 50px;
        color: #767c8f;
        font-size: 42px;
        font-weight: 600;
        width: 25%;
        text-align: right;
        .p-text {
          color: #767c8f;
        }
        ul {
          display: flex;
          justify-content: space-around;
        }
        .lis {
          display: flex;
          flex-direction: row;
        }
        li.active {
          .iconstar:before {
            color: #f4c405;
          }
          .iconstar {
            font-size: 40px;
          }
        }
      }
      .info {
        margin-top: -40px;
        width: 84%;
        display: flex;
        flex-direction: row;
        justify-content: space-aroud;
        .num {
          margin-top: 10px;
          font-weight: 600;
          opacity: 0.62;
          .sub-num {
            color: #767c8f;
            font-size: 40px;
          }
        }
        > img {
          margin-top: 10px;
          margin-right: 40px;
          width: 123px;
          height: 133px;
        }
      }
    }

    // 第二部分
    .ep-date {
      .special-date {
        padding: 58px 20px;
        background: #fff;
        margin-top: 40px;
        border-radius: 40px;
        height: 189px;
      }
      .flag-img {
        position: absolute;
        left: 50px;
        top: 0;
        width: 123px;
        height: 133px;
      }
      .ok {
        position: absolute;
        left: 60px;
        top: 0;
        z-index: 22;
        font-size: 80px;
        color: white;
      }
      .date {
        margin: 0 auto;
        margin-top: -40px;
        font-weight: 600;
        text-align: center;
        //.p-date {
        // }
        .ep-price {
          font-size: 60px;
          color: #289f11;
        }
      }
    }
    .e-price-info {
      .payBox {
        width: 100%;
        background-color: #fff;
        border-radius: 40px;
        overflow: hidden;
        ul {
          display: flex;
          text-align: center;
          height: 188px;
          line-height: 188px;
        }
        .box {
          flex: 1;
          .money {
            font-size: 65px;
            font-weight: 600;
          }
          .type {
            font-weight: 600;
            font-size: 40px;
          }
        }
        .box.left {
          color: #6318c3;
        }
        .box.right {
          color: #f4bf05;
        }
        .line {
          width: 6px;
          height: 80px;
          margin-top: 50px;
          background-color: #e3e3e2;
        }
        .bottom {
          background-color: #f3f5f3;
          height: 76px;
          line-height: 76px;
          color: #6c707a;
          font-size: 40px;
          text-align: center;
        }
      }
    }
  }
  .closedTime {
    font-size: 42px;
    color: rgba(118, 124, 143, 1);
    opacity: 0.39;
    margin-top: 100px;
    text-align: center;
  }
}
</style>
