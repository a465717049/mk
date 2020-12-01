<template>
  <div class="sellEpWrapper">
    <TopBar  class="center-one-search" :option="topBarOption">
      出售EP
    </TopBar>
    <div class="innerWrap">
      <div class="moneyWrap clearfix">
        <div class="left fl">
          <div class="top">EP</div>
          <div class="bottom">ACCOUNT</div>
        </div>
        <div class="right fr">{{account}}</div>
      </div>
      <ul>
        <li class='usdt' >
          <div class="title">USDT地址：請再次核對錢包地址是否正確</div>
          <input type="text" disabled v-model="form.usdtAddress" />
          <input type="text" disabled v-model="form.trcAddress" />
        </li>
        <li>
          <div class="title">出售數量</div>
          <input type="number" v-model="form.epAmount" />
        </li>
        <li>
          <div class="title">聯係電話</div>
          <input type="text" v-model="form.phone" />
        </li>
        
        <li>
          <div class="title">交易密碼</div>
          <input type="password" v-model="form.password" />
        </li>
        <li>
          <div class="title">谷歌驗證碼</div>
          <input type="text" v-model="form.gCode" />
        </li>
      </ul>
      <button class="next" @click="ToEPSell">確認出售</button>
    </div>
    <YellowComfirm
      :show="showComfirm"
       @clickOver="clickOverpay"
      :tipTitle="tips"
      @clickOk="clickOk()"
      @changeModel="changeModel"
    ></YellowComfirm>
    <van-popup v-model="modelShow" :close-on-click-overlay="false"  class="wrap">
      <div class="brown-border">
          <div class="item">
            <h3 class="item-tit">EP交易規則操作指南</h3>
            <div class="item-tip">
              <p>1、出售EP：出售EP步驟完成後，出售EP數量系統自動扣除，訂單進入“EP交易”在“出售中”顯示；</p>
              <p>2、訂單成交：訂單被買方“確認購買”後，訂單進入“EP交易”在“交易中”顯示；買方進入支付環節，訂單進入支付倒計時30分鐘；</p>
              <p>3、確認付款：買方支付完成後，操作“確認付款”，此時訂單還在交易中，訂單顯示“已支付”，等待賣方確認收款，訂單進入“確認收款”倒計時30分鐘；</p>
              <p>4、確認收款：賣方收到相應的款項後，在“EP交易”的“交易中”，打開訂單，操作“確認收款”，系統自動釋放購買EP的數量，可在快速指引“台賬”的“EP記錄”查看；</p>
              <p>5、交易完成：訂單完成後進入“EP交易”在“已完成”中查看詳情，訂單關閉，此筆交易才算完成全部流程；</p></div>
          </div>
        <div class="btn" @click="clickKnow">我已知曉</div>
        <div></div>
      </div>
    </van-popup>
  </div>
</template>
<script type="text/javascript">
import TopBar from "components/TopBar";
import headerImg from "../../assets/imgs/headerImg.png";
import YellowComfirm from "components/YellowComfirm";
import { http } from "util/request";
import { EPSell, GetUserInfo } from "util/netApi";
import { storage } from "util/storage";
import { accessToken, loginPro } from "util/const.js";
export default {
  data() {
    return {
      form: {
        epAmount: 0,
        phone: "",
        usdtAddress: "",
        trcAddress: "",
        password: "",
        gCode: ""
      },
      account: 0,
      topBarOption: {
        iconLeft: "back",
        iconRight: "",
       // image: headerImg
      },
      //transPassword: null,
      // verificationCode: null,
      showComfirm: false,
      modelShow:true,
      tips: "",
      redirect: null,
      tipsObj: {
        nosucceed: "轉出異常請重試",
        nonum: "請填寫出售數量",
        nophone: "請輸入電話",
        noaddr: "請綁定USDT地址",
        nopwd: "請輸入交易密碼",
        nocode: "請輸入谷歌驗證碼",
        succeed: "轉出EP成功",
        noamount: "出售數量必須為100的整數倍",
        noamount: ""
      }
    };
  },
  components: {
    TopBar,
    YellowComfirm
  },
  mounted() {},
  computed: {},
  methods: {
    clickOk() {
      
      if(this.tips=='請先綁定錢包地址！')
      {
          this.$router.push({
          name: "Wallet"
        });
      }
      this.showComfirm = false;
    },
    changeModel(v) {
      this.showComfirm = v;
    },
    clickKnow(){
         this.modelShow = false;
    },
    TogetUserInfo() {
      http(GetUserInfo, null, json => {
        if (json.code === 0) {
          this.account = json.response.gold;
          this.form.usdtAddress = json.response.coin_location
          this.form.trcAddress = json.response.trc_address
          if(this.form.usdtAddress=='')//|| this.form.trcAddress==''
          {
              this.tips='請先綁定錢包地址！'
              this.showComfirm = true;
          }
          if(!this.form.usdtAddress)
          {
              this.tips='請先綁定錢包地址！'
              this.showComfirm = true;
          }
        }
      });
    },
    ToEPSell() {
      if (this.form.epAmount <= 0) {
        this.showComfirm = true;
        this.tips = this.tipsObj.nonum;
        return;
      }
      if (!this.form.phone) {
        this.showComfirm = true;
        this.tips = this.tipsObj.nophone;
        return;
      }
      if (!this.form.usdtAddress) {
        this.showComfirm = true;
        this.tips = this.tipsObj.noaddr;
        return;
      }
       if (!this.form.trcAddress) {
        this.showComfirm = true;
        this.tips = this.tipsObj.noaddr;
        return;
      }
      if (!this.form.password) {
        this.showComfirm = true;
        this.tips = this.tipsObj.nopwd;
        return;
      }
      if (!this.form.gCode) {
        this.showComfirm = true;
        this.tips = this.tipsObj.nocode;
        return;
      }
      if (this.form.epAmount%100!=0) {
        this.showComfirm = true;
        this.tips = this.tipsObj.noamount;
        return;
      }
      let _this = this;
      let d= {
        epAmount: this.form.epAmount,
        phone:this.form.phone,
        usdtAddress:this.form.usdtAddress,
        trcAddress:this.form.trcAddress,
        password:this.form.password,
        gCode: this.form.gCode
      }

      http(EPSell,{ jsondata:JSON.stringify(this.form) }, json => {
        if (json.code === 0) {
          this.showComfirm = true;
          this.tips = this.tipsObj.succeed;
          this.$router.push({ name: 'epTrade' })
        } else {
          this.showComfirm = true;
          if (!json.success) {
            this.tips = json.msg;
          } else {
            this.tips = this.tipsObj.nosucceed;
          }
        }
      });
    }
  },
  created() {
    this.TogetUserInfo();
  }
};
</script>

<style lang="less" scoped>
.sellEpWrapper {
  .innerWrap {
    width: 100vw;
    padding-bottom: 300px;
    overflow-y: scroll;
    height: calc(100vh - 260px);
    background-color: #ebeaf0;
    border-radius: 40px 40px 0 0;
    margin-top: -20px;
    padding-top: 90px;
  }
  ul {
    width: 90%;
    margin: 0 auto;
    li {
      .title {
        color: #9d9d9f;
        font-size: 42px;
        margin: 42px 0;
        font-weight: 800;
        letter-spacing: 10px;
      }
      input {
        height: 148px;
        line-height: 148px;
        color: #9d9d9f;
        width: 100%;
        padding: 0 30px;
        border-radius: 20px;
        font-size: 60px;
        font-weight: 600;
        letter-spacing: 10px;
      }
     
      /deep/ .van-dropdown-menu__bar {
        height: 148px;
        line-height: 148px;
        color: #6f6d72;
        width: 100%;
        padding: 0 20px;
        border-radius: 20px;
      }
      /deep/ .van-ellipsis {
        font-size: 60px;
        color: #9d9d9f;
        font-weight: 600;
      }
      /deep/ .van-dropdown-menu__title {
        height: 148px;
        line-height: 148px;
        display: inline-block;
        width: 98%;
      }
      /deep/ .van-dropdown-menu__title::after {
        border: 0.1467rem solid;
        border-color: transparent transparent #dcdee0 #dcdee0;
        margin-top: -10px;
        top: 37%;
      }
      /deep/ .van-dropdown-menu__title--down::after {
        top: 55% !important;
      }
      /deep/ .van-dropdown-item__content {
        // height: 148px;
        // line-height: 148px;
        display: inline-block;
        width: 90%;
        border-radius: 0 0 40px 40px;
      }
      /deep/ .van-dropdown-item__content {
        left: 5%;
      }
      /deep/ .van-dropdown-item__option {
        height: 148px;
        line-height: 148px;
        padding: 0 80px;
        font-size: 42px;
      }
      /deep/ .van-dropdown-item__option--active {
        color: #6e21d1;
      }
      /deep/ .van-icon-success::before {
        color: #6e21d1;
      }

      .verification {
        display: flex;
        border-radius: 40px;
        overflow: hidden;
        padding: 20px 0;
        background-color: #fff;
        height: 148px;
        input {
          display: inline-block;
          width: 60%;
          box-sizing: border-box;
          height: 110px;
          border-radius: 0;

          border-right: 1px solid #999;
        }
        img {
          display: inline-block;
          width: 39%;
        }
      }
    }
  }
   .usdt{
      .title{
        padding: 0;
        margin-top: 60px;
        margin-bottom: 20px;
        letter-spacing: 2px;
      }
        input{
          font-size: 40px;
          font-weight: normal;
          letter-spacing: 2px;
          height: 100px;
          line-height: 100px;
          color: #6318c3;
        }
      }
  .next {
    display: block;
    width: 90%;
    margin: 0 auto;
    background: #f5c148;
    border-radius: 40px;
    height: 164px;
    line-height: 164px;
    font-size: 42px;
    color: #fff;
    margin-top: 100px;
    position: relative;
    font-weight: 800;
    letter-spacing: 10px;
  }
  .moneyWrap {
    height: 214px;
    line-height: 214px;
    width: 90%;
    background-color: #fff;
    margin: 0 auto;
    border-radius: 40px;
    padding: 0 40px;
    .left {
      .top {
        font-size: 104px;
        font-weight: 600;
        color: #999;
        height: 140px;
        line-height: 140px;
      }
      .bottom {
        color: #999;
        font-size: 42px;

        height: 80px;
        line-height: 80px;
      }
    }
    .right {
      font-size: 104px;
      font-weight: 600;
      color: #6318c3;
    }
  }
}
.wrap{
  width: 80%;
  border: #6318c3 1px solid;
  border-radius: 20px;
  .item-tit{
    text-align: center;
    margin-bottom: 40px;
    height: 120px;
    line-height: 120px;
    font-size: 800;
    font-weight: bolder;
    color: #ffffff;
    background-color: #6318c3;
  }
  .item-tip{
    padding: 10px 60px;
  }
  .btn{
    text-align: center;
    color: #ffffff;
    border: #eca80a 1px solid;
    background-color: #eca80a;
    width: 280px;
    margin: 30px auto 30px;
    height: 120px;
    line-height: 120px;
     border-radius: 20px;

  }
}
</style>
