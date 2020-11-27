<template>
  <div>
    <TopBar class="center-one-search" :option="topBarOption">激活碼</TopBar>
    <div class="authenticator-body">
      <div class="auth-card">
        <img src="@/assets/imgs/auth-header.png" alt />
        <div>
          <div class="auth-top-tit">Google 身份驗證</div>
          <div class="auth-top-tip">
            只需2步，獲取激活碼用於您的賬號進行安全校驗
          </div>
        </div>
      </div>
      <ul>
        <li>
          <div class="title">激活碼</div>
          <input type="text" v-model="activationCode" />
        </li>
        <li>
          <div class="title">交易密碼</div>
          <input type="password" v-model="password" />
        </li>
      </ul>
      <button class="button" @click="goNext">
        下一步
        <i class="jiantou iconfont iconarrow-right"></i>
      </button>
      <div class="auth-buttom">
        * 只需2步，Google身份驗證器安裝到你手機. <br>
        1、輸入激活碼和交易密碼獲取專屬與您的Google密鑰. <br>
        2、將Google密鑰複製到Google身份驗證去中產生校驗碼.
      </div>
    </div>
    <YellowComfirm :show="showComfirm" :tipTitle="tips" @clickOk="clickOk()"></YellowComfirm>
  </div>
</template>

<script>
import TopBar from 'components/TopBar'
import YellowComfirm from 'components/YellowComfirm'
import { http } from 'util/request'
import { SetAuthenticator } from 'util/netApi'
import { storage } from 'util/storage'
import { accessToken, loginPro } from 'util/const.js'
export default {
  name: 'AuthenticatorOne',
  components: {
    TopBar,
    YellowComfirm
  },
  data() {
    return {
      activationCode: '',
      showComfirm: false,
      password: '',
      topBarOption: {
        iconLeft: 'back',
        iconRight: ''
      },
      tips: '',
      tipsObj: {
        nosucceed: '驗證失敗',
        notpwd: '請輸入交易密碼',
        nogcode: '請輸入邀請碼',
        succeed: '驗證成功'
      }
    }
  },
  methods: {
    clickOk() {
      this.showComfirm = false
    },
    goNext() {
      this.ToSetAuthenticator()
      // this.$router.push({ name: 'AuthenticatorThree' })
    },
    ToSetAuthenticator() {
      http(
        SetAuthenticator,
        { gcode: this.activationCode, tpwd: this.password },
        json => {
          if (json.code === 0) {
            this.$router.push('./Authenticatortwo')
          }
          if (!json.success) {
            this.showComfirm = true
            this.tips = json.msg
          } else {
            this.showComfirm = true
            this.tips = this.tipsObj.nosucceed
          }
        }
      )
    }
  }
}
</script>

<style lang="less" scope>
.authenticator-body {
  background: #ebeaf0;
  top: 260px;
  width: 100%;
  overflow: scroll;
  z-index: 999;
  border-radius: 50px 50px 0 0;
  padding: 98px 60px 300px 60px;
  height: calc(100vh - 260px);
  .auth-card {
    background: #fff;
    display: flex;
    padding: 30px 50px;
    margin-bottom: 98px;
    border-radius: 32px;
    img {
      width: 193px;
      height: 185px;
      margin-right: 20px;
      margin-top: 20px;
    }
    div {
      flex: 1;
      .auth-top-tit {
        font-size: 60px;
        text-align: center;
        line-height: 60px;
        margin-bottom: 25px;
      }
      .auth-top-tip {
        font-size: 30px;
        text-align: center;
        margin: 0 100px;
        line-height: 50px;
      }
    }
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
    margin-top: 120px;
    .jiantou {
      position: absolute;
      right: 30px;
      top: 46px;
      font-size: 60px;
    }
  }
  .auth-buttom {
    padding: 98px 0;
    color: #414042;
    font-size: 26px;
  }
}
</style>
