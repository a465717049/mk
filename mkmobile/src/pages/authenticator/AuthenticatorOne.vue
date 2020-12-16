<template>
  <div>
    <TopBar class="center-one-search">谷歌验证</TopBar>
    <div class="authenticator-body">
      <div class="auth-card">
        <img src="@/assets/imgs/auth-header.png" alt />
        <div>
          <div class="auth-top-tit">Google 身份验证</div>
          <div class="auth-top-tip">
            只需2步，获取激活码用于您的账号进行安全校验
          </div>
        </div>
      </div>
      <ul>
        <li>
          <div class="title">邀请码</div>
          <input type="text" v-model="activationCode" />
        </li>
        <li>
          <div class="title">交易密码</div>
          <input type="password" v-model="password" />
        </li>
      </ul>
      <button class="button" @click="goNext">
        激活帐户
      </button>
      <!-- <div class="auth-buttom">
        * 只需2步，Google身份驗證器安裝到你手機. <br>
        1、輸入激活碼和交易密碼獲取專屬與您的Google密鑰. <br>
        2、將Google密鑰複製到Google身份驗證去中產生校驗碼.
      </div> -->
    </div>
    <YellowComfirm :show="showComfirm" :tipTitle="tips" @clickOk="clickOk()" @changeModel="changeModel"></YellowComfirm>
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
  data () {
    return {
      activationCode: '',
      showComfirm: false,
      password: '',
      tips: '',
      tipsObj: {
        nosucceed: '检验失败',
        notpwd: '请输入交易密码',
        nogcode: '请输入邀请码',
        succeed: '验证成功'
      }
    }
  },
  methods: {
    clickOk () {
      this.showComfirm = false
    },
    changeModel (v) {
      this.showComfirm = v
    },
    goNext () {
      this.ToSetAuthenticator()
      // this.$router.push({ name: 'AuthenticatorThree' })
    },
    ToSetAuthenticator () {
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
  ul {
    li {
      .title {
        font-size: 42px;
        margin: 42px 20px 20px 0;
        color: #fff;
        font-weight: bold;
      }
      input {
        height: 123px;
        line-height: 123px;
        font-size: 42px;
        width: 100%;
        padding: 30px 20px;
        border-radius: 20px;
        color: #6F6D72;
        font-weight: 600;
        letter-spacing: 4px;
        background-color: #fff;
      }
    }
  }
  .button {
    width: 100%;
    background: #EFB618;
    height: 128px;
    font-size: 42px;
    color: #fff;
    border-radius: 20px;
    position: relative;
    margin-top: 120px;

  }
  .auth-buttom {
    padding: 98px 0;
    color: #414042;
    font-size: 26px;
  }
}
</style>
