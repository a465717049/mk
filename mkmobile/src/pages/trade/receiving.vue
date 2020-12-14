<template>
  <div class="receivingWrapper">
    <TopBar class="center-one-search" :option="topBarOption">
      <div>EP Trade</div>
    </TopBar>
    <ScrollRefresh
      @getData="ToGetEPExchangeById"
      :residualHeight="160"
      :isNeedUp="false"
      class="innerScroll"
    >
      <div class="innerWrap">
        <div class="innerbox">
          <div class="saleInfo clearfix">
            <div class="left fl">
              <img :src="initData.Img" alt />
              <ul>
                <li v-for="index in rate" :key="'rate'+index" class="active">
                  <i class="iconfont iconstar"></i>
                </li>
                <li v-for="index in 5 - rate" :key="index">
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
                <li>投诉：{{ initData.complaint }}</li>
              </ul>
              <div class="money">{{ initData.price }}</div>
              <p class="time">出售时间：{{ initData.date }}</p>
              <p class="tel">Tel: {{ initData.phone }}</p>
            </div>
          </div>

          <div class="title">待收：</div>
          <div class="payBox">
            <ul>
              <li class="box">
                <span class="money">{{ initData.receiveMoneyCNY.toFixed(3) }}</span>
                <span class="type">CNY</span>
              </li>
              <li class="huo">或</li>
              <li class="box">
                <span class="money">{{ initData.receiveMoneyUSDT.toFixed(3) }}</span>
                <span class="type">USDT</span>
              </li>
            </ul>
            <div class="bottom">
              1 EP =
              <span>{{ initData.EPRate }}</span> CNY 1 USDT =
              <span>{{ initData.USDTRate }}</span> CNY
            </div>
          </div>
          <div class="photoWrap">
            <img class="photo" src="../../assets/imgs/chilui.png" alt />
            <span>
              提示：
              <br />请确认收到款项后再点击确认收款。
            </span>
          </div>
          <div class="buttonWrap">
            <div class="cancel" v-if="this.initData.name==uid">投诉买家</div>
            <div class="reseive" v-if="this.initData.name==uid" @click="onEPFinish">确认收款</div>
          </div>
        </div>
      </div>
    </ScrollRefresh>
  </div>
</template>
<script type="text/javascript">
import TopBar from 'components/TopBar'
import ScrollRefresh from 'components/ScrollRefresh'
import headerImg from '../../assets/imgs/headerImg.png'
import { http } from 'util/request'
import { GetEPExchangeById, GetUserInfo, EPFinish } from 'util/netApi'
import { storage } from 'util/storage'
import { accessToken, photoList } from 'util/const.js'
export default {
  data () {
    return {
      showChat: true,
      topBarOption: {
        iconLeft: 'back',
        iconRight: ''
      },
      epid: 0,
      uid: 0,
      rate: 3,
      initData: {
        name: '1122',
        Img: null,
        buyId: null,
        times: '3',
        finish: '3',
        complaint: '0',
        price: '500 EP',
        phone: ' +86 130 4567 1234',
        date: '2020-06-15 23:46:12',
        receiveMoneyCNY: 0.0,
        receiveMoneyUSDT: 0.0,
        EPRate: '6.175',
        USDTRate: '7.01',
        priceCode: '0xeC333554c4d410899D6dBaBE4ED885ade4bE275F',
        status: 1
      }
    }
  },
  components: {
    TopBar,
    ScrollRefresh
  },
  mounted () {},
  computed: {},
  methods: {
    onEPFinish () {
      http(EPFinish, { eid: this.epid }, json => {
        if (json.code == 0) {
          this.$router.go(-1)
        }
      })
    },

    TogetUserInfo () {
      http(GetUserInfo, null, json => {
        if (json.code === 0) {
          this.uid = json.response.uid
        }
      })
    },
    toeppay () {},
    ToGetEPExchangeById () {
      if (!this.$route.query.id) {
        return false
      }
      this.TogetUserInfo()
      this.epid = this.$route.query.id
      http(GetEPExchangeById, { id: this.epid }, json => {
        if (json.code === 0) {
          // initData.name  times  finish  complaint  price  date phone
          var model = json.response[0]
          this.initData.name = model.uid
          this.initData.buyId = model.buyId
          this.initData.Img = photoList[model.img]
          this.initData.times = model.tcount
          this.initData.receiveMoneyCNY = model.MoneyCNY
          this.initData.receiveMoneyUSDT = model.MoneyUSDT
          this.initData.EPRate = model.RMBrate
          this.initData.USDTRate = model.USDTrate
          this.initData.times = model.tcount
          this.initData.finish = model.fcount
          this.initData.complaint = model.appealcount
          this.initData.price = model.amount
          this.initData.date = model.createTime
          this.initData.phone = model.phone
          this.initData.priceCode = model.usdtAddress
          this.initData.status = model.status
        }
      })
    }
  },

  created () {
    this.ToGetEPExchangeById()
  }
}
</script>

<style lang="less" scoped>
.receivingWrapper {
  .innerScroll {
    /deep/ .wrapper .bscroll-container {
      min-height: calc(100vh - 400px);
    }
  }
  .innerWrap {
    width: 100vw;
    background-color: #ebeaf0;
    border-radius: 40px 40px 0 0;
    margin-top: -20px;
    padding-top: 60px;
    .innerbox {
      width: 90%;
      margin: 0 auto;
      height: calc(100vh - 520px);
      overflow: auto;
    }
    .photoWrap {
      height: 222px;
      background-color: #fff;
      border-radius: 40px;
      padding: 50px;
      margin-top: 120px;
      img {
        width: 150px;
        vertical-align: middle;
        margin-right: 60px;
      }
      span {
        display: inline-block;
        vertical-align: middle;
        font-size: 38px;
        color: #343434;
        font-weight: 700;
      }
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
          color: #767c8f;
          font-weight: 600;
        }
        .type {
          color: #bfc0c4;
          font-weight: 600;
          font-size: 40px;
        }
      }
      .huo {
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
        }
      }
    }
  }

  .qrCodeWrap {
    width: 100%;
    background-color: #fff;
    border-radius: 40px;
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
      overflow: hidden;
    }
  }
  .buttonWrap {
    margin: 100px 0 80px;
    display: flex;
    justify-content: space-between;
    div {
      height: 160px;
      line-height: 160px;
      color: #fff;
      text-align: center;
      font-size: 46px;
      width: 42%;
      border-radius: 30px;
      font-weight: 700;
    }
    .cancel {
      background-color: #978dab;
    }
    .confirm {
      background-color: #f4c405;
    }
    .reseive {
      background-color: #11964c;
    }
  }
}
</style>
