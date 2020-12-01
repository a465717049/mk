<template>
  <div>
    <TopBar class="center-one-search" >谷歌验证</TopBar>
    <div class="password-body">
      <div class="auth-card">
        <img src="@/assets/imgs/auth-header.png" alt />
        <div>
          <div class="auth-top-tit">Google 身份驗證</div>
          <div class="auth-top-tip">
            只需2步，获取激活码用于您的帐号进行安全校验
            <!-- 只需2步，獲取激活碼用於您的賬號進行安全校驗 -->
          </div>
        </div>
      </div>
      <div class="pwd_box">
        <input ref="pwd" type="tel" maxlength="6" v-model="msg" class="pwd" unselectable="on" />
        <ul class="pwd-wrap" @click="focus">
          <li :class="msg.length == 0 ? 'psd-blink' : ''">
            <span v-if="msg.length > 0">{{ msg[0] }}</span>
          </li>
          <li :class="msg.length == 1 ? 'psd-blink' : ''">
            <span v-if="msg.length > 1">{{ msg[1] }}</span>
          </li>
          <li :class="msg.length == 2 ? 'psd-blink' : ''">
            <span v-if="msg.length > 2">{{ msg[2] }}</span>
          </li>
          <li :class="msg.length == 3 ? 'psd-blink' : ''">
            <span v-if="msg.length > 3">{{ msg[3] }}</span>
          </li>
          <li :class="msg.length == 4 ? 'psd-blink' : ''">
            <span v-if="msg.length > 4">{{ msg[4] }}</span>
          </li>
          <li class="last-child" :class="msg.length == 5 ? 'psd-blink' : ''">
            <span v-if="msg.length > 5">{{ msg[5] }}</span>
          </li>
        </ul>
      </div>
    </div>
    <YellowComfirm :show="isEnter" :tipTitle="tips" @clickOver="clickOverpay" @clickOk="clickOk()" @changeModel="changeModel"></YellowComfirm>
  </div>
</template>

<script>
import TopBar from "components/TopBar";
import YellowComfirm from "components/YellowComfirm";
import { http } from "util/request";
import { CheckGoogleKey } from "util/netApi";
import { storage } from "util/storage";
import { accessToken, loginPro } from "util/const.js";
export default {
  name: "AuthenticatorThree",
  components: {
    TopBar,
    YellowComfirm
  },
  data() {
    return {
      msg: "", // 支付密码
      isEnter: false,
      tips: ""
    };
  },
  watch: {
    msg(curVal) {
      this.msg = curVal.replace(/\D/g, "");
      if (this.msg.length === 6) {
        http(CheckGoogleKey, { gcode: this.msg }, json => {
          this.isEnter = true;
          if (json.code === 0) {
            this.$router.push({name:'AuthenticatorLast',params:{googlekey:json.response}})
         
          } else {
            this.tips = '校驗失敗1111111111111111111111111111111111111111111111111111!'

          }
        });
      }
    }
  },
  methods: {
    //输入支付密码
    focus() {
      this.$refs.pwd.focus();
    },
    clickOk() {
      this.isEnter = false;
    },
    changeModel(v) {
      this.isEnter = v;
    }
  }
};
</script>

<style lang="less" scoped>
.password-body {
  top: 260px;
  width: 100%;
  overflow: scroll;
  z-index: 999;
  border-radius: 50px 50px 0 0;
  padding: 98px 4vw 4vw 4vw;
  height: calc(100vh - 260px);
  .auth-card {
    background: #fff;
    display: flex;
    padding: 20px 30px;
    margin-bottom: 138px;
    border-radius: 20px;
    box-shadow: 0px 5px 5px 0px rgba(0, 0, 0, 0.24);
    img {
      width: 140px;
      height: 140px;
      margin-right: 20px;
      margin-top: 20px;
    }
    div {
      flex: 1;
      .auth-top-tit {
        font-size: 46px;
        text-align: center;
             color: #191819;
        font-weight: 600;
        line-height: 60px;
        margin-bottom: 15px;
        margin-top: 20px;
      }
      .auth-top-tip {
        font-size: 25px;
        text-align: center;
        color: #191819;
        font-weight: 600;
        letter-spacing: 0;
      }
    }
  }
}
// 输入支付密码样式
.pwd_box {
  height: auto;
  display: inline-flex;
  input[type="tel"] {
    width: 0.1px;
    height: 0.1px;
    color: transparent;
    // position: relative;
    // top: 23px;
    background: #000000;
    // left: 46px;
    border: none;
    font-size: 18px;
    opacity: 0;
    z-index: -1;
  }
  //光标

  .pwd-wrap {
    width: calc(100vw - 80px);
    height: 220px;
    padding-bottom: 1px;
    margin: 0 auto;
    display: flex;
    display: -webkit-box;
    display: -webkit-flex;
    padding: 40px 0;
    justify-content: space-between;
    cursor: pointer;
    z-index: 10;
    li {
      list-style-type: none;
      text-align: center;
      line-height: 130px;
      -webkit-box-flex: 1;
      flex: 1;
      background: #fff;
      -webkit-flex: 1;
      margin-right: 40px;
      border: 8px solid #410305;
      border-radius: 13px;
      width: 138px;
      box-sizing: border-box;
      span {
        color: #6318c3;
        font-weight: bold;
        font-size: 90px;
        line-height: 140px;
      }
    }
    .last-child {
      margin-right: 0;
    }
    .psd-blink {
      display: inline-block;
      // background: url("../../assets/imgs/auth-header.png") no-repeat center;
    }
  }
}
</style>
