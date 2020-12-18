<template>
  <div>
    <TopBar class="center-one-search" >谷歌验证</TopBar>
    <div class="authenticator-two-body">
      <div class="auth-card">
        <img src="@/assets/imgs/auth-header.png" alt />
        <div>
          <div class="auth-top-tit">Google 身份验证</div>
          <div class="auth-top-tip">
           只需2步，仅获取激活码用于您的账号进行安全校验
          </div>
        </div>
      </div>
      <div class="code-body">
        <div class="c-cell">
          <div class="c-cell-label">长按验证码选择进行复制</div>
          <div class="c-cell-value">
          {{androidCode}}
          </div>
        </div>
      </div>
       <button class="button" @click="goNext">
        下一步
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
  data () {
    return {
      androidCode: '',
      iosCode: ''
    }
  },
  methods: {
    goNext () {
      this.$router.push({ name: 'AuthenticatorThree' })
    },
    ToSetAuthenticatorGooglekey () {
      http(SetAuthenticatorGooglekey, null, json => {
        if (json.code === 0) {
          this.androidCode = json.response.code
          this.iosCode = json.response.code
        }
      })
    }
  },
  created () {
    this.ToSetAuthenticatorGooglekey()
  }
}
</script>

<style lang="less" scope>
.authenticator-two-body {
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
  .code-body {
    .c-cell {
      .c-cell-label {
        font-size: 42px;
        margin: 42px 20px 20px 0;
        color: #fff;
        font-weight: bold;
      }
      .c-cell-value {
        height: 200px!important;
        line-height: 40px!important;
        font-size: 32px!important;
        width: 100%;
        padding: 0 20px;
        border-radius: 20px;
        color: #6F6D72;
        overflow: hidden;
        background: #fff;
        position: relative;
        display: flex;
        align-items: center;
        flex: 1;
        word-wrap: break-word;
        white-space: normal;
        word-break: break-all;
        
        i {
          // position: absolute;
          // right: 20px;
          // top: 8px;
          height: 80px;
          line-height: 80px;
          font-size: 80px;
          font-weight: normal;
          color: #b6b6b6;
        }
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
