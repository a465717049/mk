<template>
  <div class="chargeWrapper">
    <TopBar  class="center-one-search" :option="topBarOption">
      充值
    </TopBar>
    <div class="innerWrap">
      <div class="homeWrap">
        <img src="../../assets/imgs/homeIcon.png" alt />
        <span>服務中心專用</span>
      </div>
      <div class="qrCodeWrap">
        <p class="copy">（ 长按二维码可复制地址 ）</p>
        <img class="qrcode" src="../../assets/imgs/qr.png" alt />
        <p class="code">0xeC333554c4d410899D6dBaBE4ED885ade4bE275F</p>
      </div>
      <div class="chargeBox">
        <div class="header clearfix">
          <div class="left fl">RP 充值</div>
          <div class="see fr">
            <router-link to="ChargeAll" class="see">全部</router-link>
          </div>
        </div>
        <div class="listWrap" ref="listWrap">
          <ul>
            <li v-for="item in list" :key="item.id">
              <div class="left fl">{{ item.date }}</div>
              <div class="right fr">$ {{ item.price }}</div>
            </li>
          </ul>
        </div>
      </div>
    </div>
  </div>
</template>
<script type="text/javascript">
import TopBar from "components/TopBar";
import headerImg from "../../assets/imgs/headerImg.png";
export default {
  data() {
    return {
      topBarOption: {
        iconLeft: "iconShapecopy",
        iconRight: "",
        //image: headerImg
      },
      list: [
        
      ]
    };
  },
  components: {
    TopBar
  },
  mounted() {
  },
  computed: {},
  methods: {
  }
};
</script>

<style lang="less" scoped>
.chargeWrapper {
  height: 100vh;
  overflow: hidden;
  .innerWrap {
    width: 100vw;
    background-color: #ebeaf0;
    border-radius: 40px 40px 0 0;
    margin-top: -20px;
    padding-top: 30px;
  }
  .homeWrap {
    height: 226px;
    line-height: 226px;
    padding: 70px;
    box-sizing: border-box;
    background: #efb618;
    display: flex;
    vertical-align: middle;
    width: 96%;
    margin: 0 auto;
    border-radius: 40px;
    img {
      width: 120px;
      height: 100px;
      margin-right: 160px;
    }
    span {
      color: #fff;
      font-size: 58px;
      height: 100px;
      line-height: 100px;
    }
  }
  .qrCodeWrap {
    width: 96%;
    background-color: #fff;
    margin: 0 auto;
    margin-top: 16px;
    padding: 60px;
    border-radius: 40px;
    .copy {
      font-size: 40px;
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
    margin-bottom: 200px;
    .header {
      height: 140px;
      line-height: 140px;
      padding: 0 52px;
      .left {
        color: #888;
        font-size: 50px;
        font-weight: 600;
      }
      .see {
        color: #fdae05;
        font-size: 50px;
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
      height: calc(100vh - 1500px);
      overflow: scroll;
    }
  }
}
</style>
