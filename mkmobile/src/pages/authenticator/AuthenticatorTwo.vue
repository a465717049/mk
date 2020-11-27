<template>
  <div>
    <TopBar class="center-one-search" :option="topBarOption">密鑰</TopBar>
    <div class="authenticator-two-body">
      <div class="auth-card">
        <img src="@/assets/imgs/auth-header.png" alt />
        <div>
          <div class="auth-top-tit">Google 身份驗證</div>
          <div class="auth-top-tip">
           只需2步，獲取激活碼用於您的賬號進行安全校驗
          </div>
        </div>
      </div>
      <div class="code-body">
        <div class="c-cell">
          <div class="c-cell-label">Android code</div>
          <div class="c-cell-value">
            {{ androidCode }}
            <i class="iconfont iconcopy"></i>
          </div>
        </div>
        <div class="c-cell">
          <div class="c-cell-label">IOS code</div>
          <div class="c-cell-value">
            {{ iosCode }}
            <i class="iconfont iconcopy"></i>
          </div>
        </div>
      </div>
      <button class="button" @click="goNext">
        下一步
        <i class="jiantou iconfont iconarrow-right"></i>
      </button>
    </div>
  </div>
</template>

<script>
import TopBar from 'components/TopBar'
import { http } from 'util/request'
import { SetAuthenticatorGooglekey } from 'util/netApi'
import { storage } from 'util/storage'
import { accessToken, loginPro } from 'util/const.js'
export default {
  name: 'AuthenticatorOne',
  components: {
    TopBar
  },
  data() {
    return {
      androidCode: '',
      iosCode: '',
      topBarOption: {
        iconLeft: 'back',
        iconRight: ''
      }
    }
  },
  methods: {
    goNext() {
      this.$router.push({ name: 'AuthenticatorThree' })
    },
    ToSetAuthenticatorGooglekey() {
      http(SetAuthenticatorGooglekey, null, json => {
        if (json.code === 0) {
          this.androidCode = json.response.code
          this.iosCode = json.response.code
        }
      })
    }
  },
  created() {
    this.ToSetAuthenticatorGooglekey()
  }
}
</script>

<style lang="less" scope>
.authenticator-two-body {
  background: #ebeaf0;
  top: 260px;
  width: 100%;
  overflow: scroll;
  z-index: 999;
  border-radius: 50px 50px 0 0;
  padding: 98px 60px 60px 60px;
  height: calc(100vh - 260px);
  .auth-card {
    background: #fff;
    display: flex;
    padding: 30px 50px;
    margin-bottom: 98px;
    border-radius: 20px;
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
        line-height: 30px;
      }
    }
  }
  .code-body {
    .c-cell {
      .c-cell-label {
        width: 100%;
        height: 42px;
        font-size: 40px;
        font-weight: bold;
        margin-bottom: 35px;
        color: #191819;
      }
      .c-cell-value {
        font-size: 32px;
        background: #fff;
        height: 150px;
        border-radius: 20px;
        line-height: 150px;
        margin-bottom: 60px;
        position: relative;
        padding-left: 20px;
        width: 100vw - 40px;
        overflow: hidden;
        i {
          position: absolute;
          right: 30px;
          font-size: 70px;
          color: #b6b6b6;
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
