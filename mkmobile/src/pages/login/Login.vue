<template>
  <div class="login-page">
    <div class="top-part">
      <div class="img-part">
        <div class="head">
          <!-- <div class="eye eye1" v-show="time%7===1"></div>
          <div class="eye eye2" v-show="time%7===2"></div>
          <div class="eye eye3" v-show="time%7===3"></div>
          <div class="eye eye4" v-show="time%7===4"></div>
          <div class="eye eye5" v-show="time%7===5"></div>
          <div class="eye eye6" v-show="time%7===6"></div>
          <div class="eye eye7" v-show="time%7===0"></div> -->
        </div>
      </div>
      <div class="label">
        <div class="label-info">
          <span class="title">ID</span>
          <input class="input" @focus="onFocus" v-model="form.name" type="text" maxlength="18" />
          <van-dropdown-menu>
            <van-dropdown-item v-model="value" @change="dealChange()" :options="option" />
          </van-dropdown-menu>
        </div>
        <div class="label-info">
          <span class="title">密码</span>
          <i class="iconfont iconeye eye" @click="showP=!showP"></i>
          <input
            class="input"
            v-if="!showP"
            @focus="onFocus"
            v-model="form.pass"
            type="password"
            maxlength="20"
          />
          <input
            class="input"
            v-if="showP"
            @focus="onFocus"
            v-model="form.pass"
            type="text"
            maxlength="20"
          />
        </div>
        <div class="save">
          <van-field name="checkbox">
            <template #input>
              <van-checkbox v-model="ifSave" shape="square">记住密码</van-checkbox>
            </template>
          </van-field>
        </div>
        <div class="secret">
          <router-link class="router" to="forgot">忘记密码？</router-link>
        </div>
        <van-button type="primary" @click="login" class="submit-btn">登 录</van-button>
      </div>
    </div>

    <div class="bottom-part">
      <div class="version1">MOKI MONKEY 摩奇猴 ver 1.0</div>
      <div class="version2">O2 MONSTER CO.,LTD. COPYRIGHT 2020 © ALL RIGHT RESERVED</div>
    </div>
    <YellowComfirm
      :show="showComfirm"
      @clickOver="clickOverpay"
      :tipTitle="tips"
      @clickOk="clickOk() "
      @changeModel="changeModel"
    ></YellowComfirm>
  </div>
</template>
<script>
import YellowComfirm from 'components/YellowComfirm'
import { http } from 'util/request'
import { getLogin } from 'util/netApi'
import { storage } from 'util/storage'
import { accessToken, loginPro } from 'util/const.js'
export default {
  components: {
    YellowComfirm
  },
  data () {
    return {
      form: {
        name: '',
        pass: ''
      },
      timer: null,
      time: 1,
      value: 0,
      ifSave: false, // 是否保存密码
      showP: false,
      showComfirm: false,
      tips: '',
      redirect: null,
      tipsObj: {
        noId: '用户ID沒有输入',
        nolong: '用户ID为5-18位',
        noP: '密码没有输入',
        noRegister: '用户ID不存在',
        errorP: '登录密码不正确'
      },
      option: [{ text: '全部清除', value: '' }],
      clientWidth: document.documentElement.clientWidth
    }
  },

  methods: {
    login () {
      if (!this.form.name) {
        this.showComfirm = true
        this.tips = this.tipsObj.noId
        return
      }
      if (this.form.name.length < 5 || this.form.name.length > 18) {
        this.showComfirm = true
        this.tips = this.tipsObj.nolong
        return
      }
      if (!this.form.pass) {
        this.showComfirm = true
        this.tips = this.tipsObj.noP
        return
      }
      let _this = this
      http(getLogin, this.form, json => {
        if (json.code === 0) {
          storage.setLocalStorage(accessToken, 'Bearer ' + json.response.token)
          if (_this.ifSave) {
            let str =
              '{"name":"' +
              _this.form.name +
              '","pass":"' +
              _this.form.name +
              ':' +
              _this.form.pass +
              '"}'
            let account = storage.getLocalStorage('accountList')
            if (!account) {
              account = '[]'
            }
            let list = JSON.parse(account)
            let add = false
            list.forEach((item, index) => {
              if (item.name == _this.form.name) {
                list.splice(index, 1)
                list.push(JSON.parse(str))
                add = true
              }
            })
            if (!add) {
              list.push(JSON.parse(str))
            }
            storage.setLocalStorage('accountList', JSON.stringify(list))
          }
          if (_this.redirect) {
            _this.$router.push(_this.redirect)
            storage.delLocalStorage(loginPro)
          } else {
            _this.$router.push({
              name: 'Home',
              params: { name: this.form.name }
            })
          }
        } else {
          this.showComfirm = true
          this.tips = json.msg == '' ? this.tipsObj.noRegister : json.msg
        }
      })
    },
    loadstorage () {
      let account = storage.getLocalStorage('accountList')

      if (!account) {
        account = '[]'
      }
      let list = JSON.parse(account)
      var that = this
      that.option = []
      list.forEach(element => {
        that.option.push({ text: element.name, value: element.pass })
      })
      if (that.option.length > 0) {
        that.option.push({ text: '全部清除', value: '' })
      }
    },
    clickOk () {
      this.showComfirm = false
    },
    changeModel (v) {
      this.showComfirm = v
    },
    onFocus () {},
    dealChange (index) {
      let val = this.option.filter(item => item.value === this.value)[0].value
      if (val == '') {
        storage.delLocalStorage('accountList')
      } else {
        console.log(val)
        let list = val.split(':')
        this.form.name = list[0]
        this.form.pass = list[1]
      }
    }
  },
  created () {
    this.loadstorage()
  },
  mounted () {
    this.redirect = storage.getLocalStorage(loginPro)
    storage.delLocalStorage(accessToken)
    this.timer = setInterval(() => {
      this.time++
    }, 2000)
  },
  beforeDestroy () {
    clearInterval(this.timer)
  }
}
</script>
<style lang="less" scoped>
@keyframes myfirst {
  0% {
    opacity: 0;
  }
  10% {
    opacity: 1;
  }
  80% {
    opacity: 1;
  }
  100% {
    opacity: 0;
  }
}
.login-page {
  width: 100%;
  height: 100%;
  overflow-y: auto;
  position: relative;
  background-color: #efb618;
  min-height: 1900px;
  background: #efb618 url("../../assets/imgs/login/ybj.png") no-repeat left top/
    100% ;
    background-position-y: -160px;
  .top-part {
    height: 75%;
    position: relative;

    .img-part {
      position: relative;
      height: 50%;
      .head {
        height: 360px;
        position: absolute;
        width: 360px;
        background: url("../../assets/imgs/login/head.png") no-repeat left top /
          100% 100%;
        left: 50%;
        margin-left: -180px;
        top: 200px;
        // .eye {
        //   animation: myfirst 2s linear 0s infinite;
        //   position: absolute;
        //   top: 150px;
        //   left: 250px;
        //   width: 160px;
        //   height: 60px;
        // }
        // .eye1 {
        //   background: url("../../assets/imgs/login/eye1.png") no-repeat left top /
        //     100% 100%;
        // }
        // .eye2 {
        //   background: url("../../assets/imgs/login/eye2.png") no-repeat left top /
        //     100% 100%;
        // }
        // .eye3 {
        //   background: url("../../assets/imgs/login/eye3.png") no-repeat left top /
        //     100% 100%;
        // }
        // .eye4 {
        //   background: url("../../assets/imgs/login/eye4.png") no-repeat left top /
        //     100% 100%;
        // }
        // .eye5 {
        //   background: url("../../assets/imgs/login/eye5.png") no-repeat left top /
        //     100% 100%;
        // }
        // .eye6 {
        //   background: url("../../assets/imgs/login/eye6.png") no-repeat left top /
        //     100% 100%;
        // }
        // .eye7 {
        //   background: url("../../assets/imgs/login/eye7.png") no-repeat left top /
        //     100% 100%;
        // }
      }
    }

    .welcome {
      position: absolute;
      top: 405px;
      left: 325px;
      width: 480px;
      height: 260px;
      background: url("../../assets/imgs/login/Welcome.png") no-repeat center
        center / 100% 100%;
    }
    .label {
      width: 90%;
      margin: 0 auto;
      font-size: 40px;
      position: relative;
      color: #ffffff;
      .label-info {
        margin-top: 20px;
        display: flex;
        flex-direction: column;
        justify-content: flex-start;
        position: relative;
        border-bottom: 5px solid #fff;
        .eye {
          position: absolute;
          right: 0px;
          top: 50px;
          font-size: 60px;
          color: #fff;
        }
        .input {
          height: 106px;
          border: 0;
          font-size: 60px;
          color: #000000;
          background: rgba(110, 33, 209, 0);
          width: 95%;
        }
        .title {
          font-size: 42px;
        }
        .icon-down {
          transform: rotate(90deg);
          position: absolute;
          right: 0px;
          top: 80px;
          font-size: 60px;
          // opacity: 0.7;
        }
      }
      .secret {
        // opacity: 0.5;
        float: right;
        font-size: 40px;
        margin-top: 30px;
        .router {
          color: #fff !important;
        }
      }
      .save {
        // opacity: 0.5;
        float: left;
        font-size: 40px;
        margin-top: 30px;
        .router {
          color: #fff !important;
        }
        /deep/.van-field__label {
          text-align: right;
          font-size: 40px;
          color: white;
        }
        /deep/.van-checkbox__label {
          font-size: 40px;
          color: white;
          border-radius: 10px;
          margin-left: 15px;
        }
        /deep/.van-cell {
          padding-left: 0;
          height: 100px;
        }
        /deep/.van-checkbox__icon {
          font-size: 42px;
          .van-icon {
            border: 1px solid #fff;
          }
        }
        /deep/.van-checkbox__icon--checked {
          .van-icon {
            color: #fff;
            // background-color: #6e21d1 !important;
            border-color: #fff;
            // opacity: 0.6;
          }
        }
        /deep/.van-cell {
          background: none;
        }
      }
      .submit-btn {
        background: #006bb6;
        margin-top: 60px;
        height: 150px;
        width: 100%;
        text-align: center;
        border-radius: 80px;
        font-size: 52px;
        color: #fff;
        font-weight: 900;
      }
      /deep/.van-dropdown-item--down {
        position: absolute;
        width: 100vw;
        height: 1000px;
        left: -960%;
        top: 116% !important;
      }
      // 下拉框
      /deep/.van-dropdown-menu {
        width: 10%;
        position: absolute;
        right: 0;
        height: 60px;
        line-height: 60px;
        bottom: 20px;
      }
      /deep/.van-overlay {
        top: 28px;
      }
      /deep/.van-ellipsis {
        display: none;
      }
      /deep/ .van-dropdown-menu__bar {
        color: white;
        background: #efb618;
        padding: 0 20px;
        border-radius: 20px;
        box-shadow: none;
      }
      /deep/ .van-ellipsis {
        font-weight: 600;
        font-size: 42px;
        color: #6f6d72;
      }
      /deep/ .van-dropdown-menu__title {
        height: 148px;
        line-height: 148px;
        display: inline-block;
        width: 98%;
      }
      /deep/ .van-dropdown-menu__title::after {
        border: 0.1467rem solid;
        border-color: transparent transparent #fff #fff;
        margin-top: -10px;
        top: 37%;
      }
      /deep/ .van-dropdown-menu__title--down::after {
        top: 55% !important;
      }
      /deep/ .van-dropdown-item__content {
        top: 0px;
      }
      /deep/ .van-cell::after {
        left: 5%;
        width: 90%;
      }
      /deep/ .van-dropdown-item__option {
        height: 128px;
        line-height: 128px;
        color: #fff;
        padding: 0 60px;
        font-size: 42px;
        background-color: #efb618;
      }
      /deep/ .van-dropdown-item__option--active {
        color: #fff;
      }
      /deep/.van-dropdown-item__option--active {
        .van-dropdown-item__icon {
          color: #fff;
        }
      }
    }
  }

  .bottom-part {
    position: relative;
    height: 25%;
    font-family: HYYakuHei;
  }
  .version2 {
    color: #060000;
    font-size: 25px;
    margin-top: 40px;
    text-align: center;
    position: absolute;
    bottom: 50px;
    width: 100%;
    left: 50%;
    transform: translate(-50%);
    font-weight: bold;
  }
  .version1 {
    color: #060000;
    font-size: 42px;
    margin-top: 40px;
    text-align: center;
    position: absolute;
    bottom: 190px;
    width: 100%;
    left: 50%;
    transform: translate(-50%);
    font-weight: bold;
  }
  /deep/.van-button--primary {
    border: none;
  }
}
</style>
