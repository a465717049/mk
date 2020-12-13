<template>
  <div class="chargeWrapper">
    <TopBar class="center-one-search" :option="topBarOption">EP交易</TopBar>
    <ScrollRefresh
      @getData="getAllData"
      :residualHeight="160"
      :isNeedUp="false"
      class="innerScroll"
    >
      <div class="innerWrap">
        <div class="innerbox">
          <div class="title">待售信息</div>
          <div class="saleInfo clearfix">
            <div class="left fl">
              <img :src="initData.Img" alt />
              <ul>
                <li v-for="(item,index) in rate" :key="index+'1'" class="active">
                  <i class="iconfont iconstar"></i>
                </li>
                <li v-for="(item,index) in 5 - rate" :key="index">
                  <i class="iconfont iconstar"></i>
                </li>
              </ul>
              <p>信用</p>
            </div>
            <div class="right fr">
              <div class="rightCount">{{ initData.name }}</div>
              <ul>
                <li>次数: {{ initData.times }}</li>
                <li>完成 : {{ initData.finish }}</li>
                <li>投诉{{ initData.complaint }}</li>
              </ul>
              <div class="money">{{ initData.price }}</div>
              <p class="time">出售时间 ：{{ initData.date }}</p>
              <p v-if="this.initData.status==2" class="tel">Tel:{{ initData.phone }}</p>
            </div>
          </div>
          <div class="title">付款信息</div>
          <div class="qrCodeWrap">
            <!-- <p class="copy">（ 掃描二維碼進行付款 ）</p>
            <img class="qrcode" src="../../assets/imgs/qr.png" alt />-->
            <p class="code">{{ initData.priceCode }}</p>
          </div>
          <div class="title">待付</div>
          <div class="payBox">
            <ul>
              <li class="box">
                <span class="money cny">{{ initData.receiveMoneyCNY.toFixed(3) }}</span>
                <span class="type">CNY</span>
              </li>
              <li class="huo">或</li>
              <li class="box">
                <span class="money usdt">{{ initData.receiveMoneyUSDT.toFixed(3) }}</span>
                <span class="type">USDT</span>
              </li>
            </ul>
            <div class="bottom">
              1 EP =
              <span>{{ initData.EPRate }}</span> CNY 1 USDT =
              <span>{{ initData.USDTRate }}</span>CNY
            </div>
          </div>
          <div class="buttonWrap">
            <div
              class="confirm"
              v-if="this.initData.status==1&&this.initData.name!=this.uid"
              @click="epbuy"
            >确认购买</div>
            <div
              class="cancel"
              v-if="this.initData.status==2&&this.initData.name!=this.uid"
              @click="cancebuy"
            >放弃付款</div>
            <div
              class="confirm"
              v-if="this.initData.status==2&&this.initData.name!=this.uid"
              @click="epbuy"
            >确认付款</div>
          </div>
        </div>
      </div>
    </ScrollRefresh>
  </div>
</template>
<script type="text/javascript">
import TopBar from "components/TopBar";
import ScrollRefresh from "components/ScrollRefresh";
import headerImg from "../../assets/imgs/headerImg.png";
import { http } from "util/request";
import { GetEPExchangeById, EPBuy, EPPay,GetUserInfo } from "util/netApi";

import { storage } from "util/storage";
import { accessToken, photoList } from "util/const.js";

export default {
  data() {
    return {
      epid: 0,
      topBarOption: {
        iconLeft: "back"
        // iconRight: '11',
        //  image: headerImg
      },
      rate: 3,
      uid: 0,
      initData: {
        name: "",
        Img: null,
        times: 0,
        finish: 0,
        complaint: 0,
        price: "",
        phone: "",
        date: "",
        receiveMoneyCNY: 0.0,
        receiveMoneyUSDT: 0.0,
        EPRate: "6.175",
        USDTRate: "7.01",
        priceCode: "",
        status: 1
      }
    };
  },
  components: {
    TopBar,
    ScrollRefresh
  },
  mounted() {},
  computed: {},
  methods: {
    cancebuy() {
      this.$router.push({ name: "epTrade" });
    },
    TogetUserInfo() {
      http(GetUserInfo, null, json => {
        if (json.code === 0) {
          this.uid = json.response.uid;
        }
      });
    },
    epbuy() {
      if (this.initData.status == 1) {
        http(EPBuy, { eid: this.epid }, json => {
          this.ToGetEPExchangeById(this.epid);
          this.$router.go(0);
        });
      } else if (this.initData.status == 2) {
        http(EPPay, { eid: this.epid }, json => {
          this.ToGetEPExchangeById(this.epid);
          this.$router.go(0);
        });
      }
    },
    ToGetEPExchangeById() {
      http(GetEPExchangeById, { id: this.epid }, json => {
        if (json.code === 0) {
          // initData.name  times  finish  complaint  price  date phone

          var model = json.response[0];
          this.initData.name = model.uid;
          this.initData.Img = photoList[model.img];
          this.initData.times = model.tcount;
          this.initData.receiveMoneyCNY = model.MoneyCNY;
          this.initData.receiveMoneyUSDT = model.MoneyUSDT;
          this.initData.EPRate = model.RMBrate;
          this.initData.USDTRate = model.USDTrate;
          this.initData.times = model.tcount;
          this.initData.finish = model.fcount;
          this.initData.complaint = model.appealcount;
          this.initData.price = model.amount;
          this.initData.date = model.createTime;
          this.initData.phone = model.phone;
          this.initData.priceCode = model.usdtAddress;
          this.initData.status = model.status;
        }
      });
    },
    getAllData() {
      if (this.$route.query.id) {
        this.epid = this.$route.query.id;
        this.ToGetEPExchangeById();
      }
      this.TogetUserInfo();
    }
  },
  created() {
    if (this.$route.query.id) {
      this.epid = this.$route.query.id;
      this.ToGetEPExchangeById(this.epid);
    }
    this.TogetUserInfo();
  }
};
</script>

<style lang="less" scoped>
.innerScroll{
  /deep/ .wrapper  .bscroll-container{
    min-height: calc(100vh - 400px);
  }
}
.chargeWrapper {
  .innerWrap {
    width: 100vw;
    background-color: #ebeaf0;
    border-radius: 40px 40px 0 0;
    margin-top: -20px;
    padding-bottom: 240px;
    .innerbox {
      width: 90%;
      margin: 0 auto;
    }
    .title {
      height: 147px;
      line-height: 147px;
      color: #191819;
      font-size: 48px;
      font-weight: 700;
    }
    .payBox {
      width: 100%;
      background-color: #fff;
      border-radius: 40px;
      overflow: hidden;
      ul {
        display: flex;
        text-align: center;
      }
      .box {
        flex: 1;
        height: 163px;
        line-height: 163px;
        .money {
          font-size: 65px;
          font-weight: 600;
        }
        .type {
          color: #bfc0c4;
          font-weight: 600;
          font-size: 40px;
        }
      }
      .huo {
        margin-top: 60px;
        font-size: 40px;
        color: #bfc0c4;
      }
      .bottom {
        background-color: #f3f5f3;
        height: 76px;
        line-height: 76px;
        color: #6c707a;
        font-size: 40px;
        text-align: center;
      }
      .cny {
        color: #6318c3;
      }
      .usdt {
        color: #f4b405;
      }
    }
    .saleInfo {
      width: 100%;
      background-color: #fff;
      border-radius: 40px;
      padding: 40px;
      .left {
        text-align: center;
        width: 20%;
        img {
          width: 124px;
          height: 124px;
        }
        ul {
          width: 100%;
          display: flex;
          justify-content: space-around;
        }
        li.active {
          .iconstar:before {
            color: #f4c405;
          }
        }
        li {
          .iconstar:before {
            font-size: 30px;
          }
        }
        p {
          color: #767c8f;
          font-size: 40px;
          font-weight: 600;
        }
      }
      .right {
        width: 70%;
        .rightCount {
          font-size: 48px;
          color: #6318c3;
          font-weight: 600;
        }
        ul {
          display: flex;
          justify-content: space-between;
        }
        li {
          font-size: 40px;
          color: #757c8f;
          font-weight: 700;
        }
        .money {
          font-size: 66px;
          font-weight: 600;
          color: #767c8f;
        }
        .time {
          font-size: 40px;
          color: #757c8f;
          font-weight: 700;
        }
        .tel {
          font-size: 36px;
          font-weight: 600;
          color: #767c8f;
          margin-top: 20px;
        }
      }
    }
  }

  .qrCodeWrap {
    width: 100%;
    background-color: #fff;
    border-radius: 40px;
    padding: 40px 0;
    .copy {
      font-size: 30px;
      color: #999;
      font-size: 36px;
      text-align: center;
      height: 60px;
      line-height: 60px;
    }
    .qrcode {
      width: 400px;
      height: 400px;
      display: block;
      margin: 0 auto;
    }
    .code {
      font-size: 30px;
      color: #333;
      font-size: 36px;
      text-align: center;
      height: 60px;
      line-height: 60px;
    }
  }
  .chargeBox {
    width: 96%;
    background-color: #fff;
    margin: 0 auto;
    margin-top: 16px;
    border-radius: 40px;
    .header {
      height: 140px;
      line-height: 140px;
      padding: 0 52px;
      .left {
        color: #888;
        font-size: 40px;
        font-weight: 600;
      }
      .see {
        color: #fdae05;
      }
    }
    ul {
      padding: 0 52px 20px;
      li {
        height: 130px;
        line-height: 130px;
        background: #e8e8e8;
        margin-bottom: 20px;
        border-radius: 30px;
        padding: 0 20px;
        .left {
          font-size: 40px;
          color: #666;
        }
        .right {
          font-size: 50px;
          color: #23bb05;
          font-weight: 6000;
        }
      }
    }
    .listWrap {
      height: calc(100vh - 1400px);
      overflow: auto;
    }
  }
  .buttonWrap {
    margin: 40px 0 80px;
    display: flex;
    justify-content: space-between;
    div {
      height: 160px;
      line-height: 160px;
      color: #fff;
      text-align: center;
      font-size: 46px;
      width: 42%;
      font-weight: 700;
      border-radius: 30px;
    }
    .cancel {
      background-color: #978dab;
    }
    .confirm {
      background-color: #f4c405;
    }
  }
}
</style>
