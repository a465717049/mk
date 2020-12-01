<template>
  <div class="sellEpWrapper">
    <TopBar  class="center-one-search" >
    轉出
    </TopBar>
    <div class="innerWrap">
      <div class="moneyWrap clearfix">
        <div class="left fl">
          <div class="top">RP</div>
          <div class="bottom">ACCOUNT</div>
        </div>
        <div class="right fr">{{account}}888888</div>
      </div>
      <ul>
        <li>
          <div class="title">轉出數量</div>
          <input type="number" v-model="form.amount" />
        </li>
        <li>
          <div class="title">接收ID:{{name===''?'':name+'('+form.touid+')'}}</div>
          <input type="text" v-model="form.touid" @blur="checkUser" />
        </li>
        <li>
          <div class="title">交易密碼</div>
          <input type="password" v-model="form.tpwd" />
        </li>
        <li>
          <div class="title">谷歌驗證碼</div>
          <input type="text" v-model="form.gcode" />
        </li>
      </ul>
      <button class="next" @click="ToEPexchange">确定转出</button>
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
import YellowComfirm from 'components/YellowComfirm'
import { http } from 'util/request'
import { RPToEexchange, GetUserInfo, checkEPUser } from 'util/netApi'
import { storage } from 'util/storage'
import { accessToken, loginPro } from 'util/const.js'
import TopBar from 'components/TopBar'
import headerImg from '../../assets/imgs/headerImg.png'
export default {
  data() {
    return {
      form: {
        touid: null,
        amount: null,
        tpwd: '',
        gcode: ''
      },
      name: '',
      account: null,
      // transferNumber: null,
      showComfirm: false,
      //  receiptId: null,
      //  transPassword: null,
      //  verificationCode: null,
      tips: '',
      redirect: null,
      tipsObj: {
        noep: '请填写RP转出数量',
        notouid: '请填写接收id',
        notpwd: '请填写交易密码',
        nogcode: '请填写谷歌验证码',
        nosucceed: '转出异常请稍候再试',
        succeed: '转出RP成功'
      }
    }
  },
  components: {
    TopBar,
    YellowComfirm
  },
  mounted() {},
  computed: {},
  methods: {
    clickOk() {
      this.showComfirm = false
    },
    changeModel(v) {
      this.showComfirm = v
    },
    checkUser() {
      if (
        this.form.touid != '' &&
        this.form.touid != 0 &&
        this.form.touid != null
      ) {
        http(checkEPUser, { uid: this.form.touid, type: 'rp' }, json => {
          console.log(json)
          if (json.code === 0) {
            if (json.response.enable) {
              this.name=json.response.name
            }else{
              this.tips = '無法轉賬到此ID'
              this.showComfirm = true
              this.form.touid = ''
              this.name=''
            }
          }
        })
      }
    },
    TogetUserInfo() {
      http(GetUserInfo, null, json => {
        if (json.code === 0) {
          this.account = json.response.rp
        }
      })
    },
    ToEPexchange() {
      if (this.form.amount === 0) {
        this.showComfirm = true
        this.tips = this.tipsObj.noep
        return
      }

      if (this.form.touid === 0) {
        this.showComfirm = true
        this.tips = this.tipsObj.notouid
        return
      }

      if (!this.form.tpwd) {
        this.showComfirm = true
        this.tips = this.tipsObj.notpwd
        return
      }

      if (!this.form.gcode) {
        this.showComfirm = true
        this.tips = this.tipsObj.nogcode
        return
      }

      let _this = this
      http(RPToEexchange, this.form, json => {
        if (json.code === 0) {
          this.showComfirm = true
          this.tips = this.tipsObj.succeed
          this.account -= this.form.amount
        } else {
          this.showComfirm = true
          if (!json.success) {
            this.tips = json.msg
          } else {
            this.tips = this.tipsObj.nosucceed
          }
        }
      })
    }
  },
  created() {
    this.TogetUserInfo()
  }
}
</script>

<style lang="less" scoped>
.sellEpWrapper {
  .innerWrap {
    width: 100vw;
    padding-bottom: 300px;
    height: calc(100vh - 260px);
    overflow-y: scroll;
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
      color: #113D79;
    }
  }
}
</style>
