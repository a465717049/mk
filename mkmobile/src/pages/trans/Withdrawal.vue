<template>
  <div class="withdrawalWrapper">
    <TopBar class="center-one-search" >提 现</TopBar>
    <div class="innerWrap">
      <div class="tips base-flex flex-start p-58 bg-white borderR mb-80">
        <img src="@/assets/imgs/tipimg.png" class="img" alt />
        <div class="tips-part">
          <div class="tip-titl">提示</div>
          <div>EP提现要扣除10%的手续费</div>
          <!-- <div>即將更新！</div> -->
        </div>
      </div>
      <div class="moneyWrap clearfix">
        <div class="left fl">
          <div class="top" @click="ChangeTransType">{{form.oType}}</div>
          <div class="bottom">ACCOUNT</div>
        </div>
        <div class="right fr">{{account}}</div>
      </div>
      <ul>
        <li>
          <div class="title">提现金额</div>
          <input type="number" min="0" v-model="form.amount" />
        </li>
        <li>
          <div class="title">提现方式</div>
          <van-dropdown-menu>
            <van-dropdown-item v-model="form.dType" :options="receiptTypeList" />
          </van-dropdown-menu>
        </li>
        <li>
          <div class="title">交易密碼</div>
          <input type="password" v-model="form.tpwd" />
        </li>
        <li>
          <div class="title">谷歌验证码</div>
          <input type="text" v-model="form.gcode" />
        </li>
      </ul>
      <button class="next" @click="ToTranWithMe">确 定</button>
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
import YellowComfirm from "components/YellowComfirm";
import { http } from "util/request";
import { TransOtherType, GetUserInfo } from "util/netApi";
import { storage } from "util/storage";
import TopBar from "components/TopBar";
export default {
  data() {
    return {
      form: {
        oType: "EP",
        dType: "",
        amount: 0,
        tpwd: "",
        gcode: ""
      },
      account: 0,
      showComfirm: false,

      receiptTypeList: [
        { text: "RP", value: "RP" },
        { text: "SP", value: "SP" }
      ],
      transPassword: null,
      verificationCode: null,
      tips: "",
      tipsObj: {
        noamount: "請填寫轉換數量",
        amount: "餘額不足！",
        notype: "請選擇轉出類型",
        notpwd: "請填寫交易密碼",
        nosucceed: "轉換異常，稍後重試",
        succeed: "轉換成功"
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
      this.showComfirm = false;
    },
    changeModel(v) {
      this.showComfirm = v;
    },
    ChangeTransType() {
      this.TogetUserInfo();
      if (this.form.oType == "EP") {
        this.form.oType = "RP";
        this.receiptTypeList = [{ text: "SP", value: "SP" }];
      } else {
        this.form.oType = "EP";
        this.receiptTypeList = [
          { text: "RP", value: "RP" },
          { text: "SP", value: "SP" }
        ];
      }
    },
    TogetUserInfo() {
      http(GetUserInfo, null, json => {
        if (json.code === 0) {
          console.log(json);
          if (this.form.oType == "EP") {
            this.account = json.response.gold;
          } else if (this.form.oType == "RP") {
            this.account = json.response.rp;
          }
        }
      });
    },
    ToTranWithMe() {
      if (this.form.amount <= 0) {
        this.showComfirm = true;
        this.tips = this.tipsObj.noamount;
        return;
      }
      if (this.form.amount > this.account) {
        this.showComfirm = true;
        this.tips = this.tipsObj.amount;
        return;
      }

      if (!this.form.dType) {
        this.showComfirm = true;
        this.tips = this.tipsObj.notype;
        return;
      }

      if (!this.form.tpwd) {
        this.showComfirm = true;
        this.tips = this.tipsObj.notpwd;
        return;
      }

      console.log(this.form);
      let _this = this;
      http(TransOtherType, this.form, json => {
        if (json.code === 0) {
          this.showComfirm = true;
          this.tips = this.tipsObj.succeed;
          this.TogetUserInfo();
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
.withdrawalWrapper {
  .innerWrap {
    width: 100vw;
    padding-bottom: 300px;
    overflow-y: scroll;
    height: calc(100vh - 260px);
    border-radius: 40px 40px 0 0;
    margin-top: -0px;
    padding-top: 30px;
    .tips-part {
      font-weight: bold;
      color: rgba(52, 52, 52, 1);
    }
    .tips {
      width: 90%;
      min-height: 158px;
      align-items: center;
      padding: 30px ;
      margin: 0 auto 50px;
      box-shadow: 0px 5px 5px 0px rgba(0, 0, 0, 0.24);
      .img {
        width: 148px;
        height: 115px;
        margin-right: 70px;
      }
    }
  }
  ul {
    width: 90%;
    margin: 0 auto;
    li {
      .title {
        color: #ffffff;
        font-size: 42px;
        margin: 60px 0 22px;
        font-weight: 800;
        letter-spacing: 10px;
        padding-left: 20px;
      }
      input {
        height: 130px;
        line-height: 130px;
        color: #9e9e9f;
        width: 100%;
        padding: 0 30px;
        border-radius: 20px;
        font-size: 60px;
        font-weight: 600;
        letter-spacing: 10px;
      }
      /deep/ .van-dropdown-menu__bar {
        height: 130px;
        line-height: 130px;
        color: #9e9e9f;
        font-weight: 600;
        width: 100%;
        padding: 0 20px;
        border-radius: 20px;
        letter-spacing: 10px;
      }
      /deep/ .van-ellipsis {
        font-size: 42px;
        color: #9e9e9f;
        font-weight: 600;
        letter-spacing: 10px;
      }
      /deep/ .van-dropdown-menu__title {
        height: 130px;
        line-height: 130px;
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
        font-size: 40px;
      }
      /deep/ .van-dropdown-item__option--active {
        color: #efb618;
      }
      /deep/ .van-icon-success::before {
        color: #efb618;
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
        color: #4a494c;
        margin-top: 20px;
        height: 100px;
        line-height: 100px;
        opacity: 0.62;
      }
      .bottom {
        color: #4a494c;
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
