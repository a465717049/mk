<template>
  <div>
    <TopBar class="center-one-search" :option="topBarOption">錢包地址</TopBar>
    <ScrollRefresh @getData="TogetUserInfo" :residualHeight="160" :isNeedUp="false">
      <div class="wallet-body">
        <ul>
          <li>
            <div class="title">您的錢包地址</div>
            <div class="t-img">
              <img src="@/assets/imgs/T.png" alt />
              <input
                type="text"
                placeholder="請輸入ERC20地址"
                :disabled="this.address!==''&&this.Taddress!==''"
                v-model="form.location"
              />
            </div>
            <div class="t-img">
              <img src="@/assets/imgs/T.png" alt />
              <input
                type="text"
                placeholder="請輸入TRC20地址"   
                :disabled="this.address!==''&&this.Taddress!==''"
                v-model="form.addr"
              />
            </div>
          </li>
          <li>
            <div class="title" v-show="this.address==''||this.Taddress==''">交易密码</div>
            <input type="password" v-show="this.address==''||this.Taddress==''" v-model="form.tpwd" />
          </li>
          <li>
            <div class="title" v-show="this.address==''||this.Taddress==''">身份證號碼</div>
            <input type="password" v-show="this.address==''||this.Taddress==''" v-model="form.idnum" />
          </li>
          <li>
            <div class="title" v-show="this.address==''||this.Taddress==''">谷歌驗證碼</div>
            <input type="number" v-show="this.address==''||this.Taddress==''" v-model="form.google" />
          </li>
        </ul>
        <button class="button" v-show="this.address==''||this.Taddress==''" @click="goNext">確定</button>
        <div class="auth-card">
          <img src="@/assets/imgs/two-persion.png" alt />
          <div>
            <div class="auth-top-tit">溫馨提示：</div>
            <div class="auth-top-tip">錢包地址一經綁定將無法修改！</div>
          </div>
        </div>
      </div>
      <YellowComfirm
        :show="isEnter"
        @clickOver="clickOverpay"
        :tipTitle="tips"
        @clickOk="clickOk()"
        @changeModel="changeModel"
      ></YellowComfirm>
    </ScrollRefresh>
  </div>
</template>

<script>
import TopBar from "components/TopBar";
import YellowComfirm from "components/YellowComfirm";
import ScrollRefresh from "components/ScrollRefresh";
import { GetUserInfo, SetUserlocationweb } from "util/netApi";
import { http } from "util/request";
import { storage } from "util/storage";
import { accessToken, loginPro } from "util/const.js";
export default {
  name: "AuthenticatorOne",
  components: {
    TopBar,
    YellowComfirm,
    ScrollRefresh
  },
  data() {
    return {
      form: {
        location: "",
        addr:"",
        tpwd: "",
        idnum: "",
        google: ""
      },
      address: "",
      Taddress: "",
      //   password: "",
      isEnter: false,
      topBarOption: {
        iconLeft: "back",
        iconRight: ""
      },
      tips: "",
      tipsObj: {
        nosucceed: "設置錢包地址失敗",
        nolong: "請設置正確的ERC錢包地址",
        nolong1: "請設置正確的TRC錢包地址",
        noex: "ERC錢包地址長度不正確",
        noex1: "TRC錢包地址長度不正確",
        succeed: "設置錢包地址成功"
      }
    };
  },
  methods: {
    goNext() {
      if (!this.form.location) {
        this.tips = this.tipsObj.noex;
        this.isEnter = true;
        return;
      }
      if (this.form.location.length != 42) {
        this.tips = this.tipsObj.noex;
        this.isEnter = true;
        return;
      }
       if (this.form.addr.length != 34) {
        this.tips = this.tipsObj.noex1;
        this.isEnter = true;
        return;
      }
      if (this.form.location.substring(0, 2).toLowerCase() !== "0x") {
        this.tips = this.tipsObj.nolong;
        this.isEnter = true;
        return;
      }
    if (this.form.addr.substring(0, 1).toLowerCase() !== "t") {
        this.tips = this.tipsObj.nolong1;
        this.isEnter = true;
        return;
      }
      http(SetUserlocationweb, this.form, json => {
        if (json.code === 0) {
          this.isEnter = true;
          this.tips = this.tipsObj.succeed;
          this.address = this.form.location;
        } else {
          this.isEnter = true;
          if (!json.success) {
            this.tips = json.msg;
          } else {
            this.tips = this.tipsObj.nosucceed;
          }
        }
      });
      //this.isEnter = true
    },
    clickOk() {
      this.isEnter = false;
    },
    changeModel(v) {
      this.isEnter = v;
    },
    TogetUserInfo() {
      http(GetUserInfo, null, json => {
        if (json.code === 0) {
          this.form.location = json.response.coin_location;
          this.form.location = this.form.location ? this.form.location : "";
          this.address = json.response.coin_location
            ? json.response.coin_location
            : "";
        }
      });
    }
  },
  mounted() {
    this.TogetUserInfo();
  },
  created() {}
};
</script>

<style lang="less" scope>
.wallet-body {
  background: #ebeaf0;
  // position: absolute;
  // top: 260px;
  width: 100%;
  // overflow: scroll;
  z-index: 999;
  border-radius: 50px 50px 0 0;
  padding: 98px 60px 60px 60px;
  // height: calc(100vh - 260px);
  .auth-card {
    background: #fff;
    display: flex;
    padding: 40px 50px;
    margin-bottom: 98px;
    margin-top: 60px;
    border-radius: 20px;
    img {
      width: 148px;
      height: 98px;
      margin-right: 60px;
      margin-top: 20px;
    }
    .auth-top-tit {
      font-size: 55px;
      line-height: 60px;
      margin-bottom: 18px;
    }
    .auth-top-tip {
      font-size: 30px;
    }
  }
 .t-img{
   margin-top: 20px;
 }
  ul {
    li {
      .title {
        font-size: 42px;
        margin: 42px 20px 20px 0;
        color: #191819;
        font-weight: bold;
      }
      input {
        height: 148px;
        font-size: 42px;
        width: 100%;
        padding: 30px 20px;
        border-radius: 20px;
      }
      .t-img {
        position: relative;
        img {
          width: 80px;
          height: 80px;
          position: absolute;
          left: 30px;
          top: 36px;
          z-index: 999;
        }
        input {
          padding-left: 150px;
        }
      }
    }
  }
  .button {
    width: 100%;
    background: #f5c148;
    height: 185px;
    font-size: 52px;
    color: #fff;
    border-radius: 40px;
    position: relative;
    margin: 100px 0;
    .jiantou {
      position: absolute;
      right: 30px;
      top: 46px;
      font-size: 60px;
    }
  }
}
</style>
