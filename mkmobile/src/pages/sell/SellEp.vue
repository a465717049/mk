<template>
  <div class="sellEpWrapper">
    <TopBar  class="center-one-search" >
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
          <div class="title">USDT地址：请再次核对钱包地址是否正确</div>
          <input type="text" disabled v-model="form.usdtAddress" />
        </li>
        <li>
          <div class="title">出售数量</div>
          <input type="number" v-model="form.epAmount" />
        </li>
        <li>
          <div class="title">联系电话</div>
          <input type="text" v-model="form.phone" />
        </li>
        
        <li>
          <div class="title">交易密码</div>
          <input type="password" v-model="form.password" />
        </li>
        <li>
          <div class="title">谷歌验证码</div>
          <input type="text" v-model="form.gCode" />
        </li>
      </ul>
      <button class="next" @click="ToEPSell">确认出售</button>
    </div>
    <YellowComfirm
      :show="showComfirm"
       @clickOver="clickOverpay"
      :tipTitle="tips"
      @clickOk="clickOk()"
      @changeModel="changeModel"
    ></YellowComfirm>
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
        password: "",
        gCode: ""
      },
      account: 0,
      //transPassword: null,
      // verificationCode: null,
      showComfirm: false,
      tips: "",
      redirect: null,
      tipsObj: {
        nosucceed: "转出异常请重试",
        nonum: "请填写出售数量",
        nophone: "请输入电话",
        noaddr: "请绑定USDT地址",
        nopwd: "请输入交易密码",
        nocode: "请输入谷歌验证码",
        succeed: "转出EP成功",
        noamount: "出售数量必须为100的整数倍",
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
      
      if(this.tips=='请先绑定钱包地址！')
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
    TogetUserInfo() {
      http(GetUserInfo, null, json => {
        if (json.code === 0) {
          this.account = json.response.gold;
          this.form.usdtAddress = json.response.coin_location
          if(this.form.usdtAddress&& this.form.usdtAddress=='')
          {
              this.tips='请先绑定钱包地址！'
              this.showComfirm = true;
          }
          if(!this.form.usdtAddress)
          {
              this.tips='请先绑定钱包地址！'
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
    border-radius: 40px 40px 0 0;
    margin-top: -20px;
    padding-top: 90px;
  }
  ul {
    width: 90%;
    margin: 0 auto;
    li {
      .title {
        color: #FFFFFF;
        font-size: 42px;
        margin: 60px 0 22px;
        font-weight: 800;
        letter-spacing: 10px;
        padding-left: 20px;
      }
      input {
        height: 130px;
        line-height: 130px;
        color: #9E9E9F;
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
          height: 130px;
          line-height: 130px;
          color: #6318c3;
        }
      }
  .next {
    display: block;
    width: 90%;
    margin: 0 auto;
    background: #f5c148;
    border-radius: 20px;
    height: 130px;
    line-height: 130px;
    font-size: 52px;
    color: #fff;
    margin-top: 100px;
    font-weight: 600;
    letter-spacing: 4px;
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
        color: #4A494C;
        margin-top: 20px;
        height: 100px;
        line-height: 100px;
        opacity: 0.62;
      }
      .bottom {
        color: #4A494C;
        font-size: 42px;
        height: 80px;
        line-height: 80px;
        opacity: 0.62;
      }
    }
    .right {
      font-size: 104px;
      font-weight: 600;
      color: #4678BC;
    }
  }
}
</style>
