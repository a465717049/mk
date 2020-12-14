<template>
  <div class="sellEpWrapper">
    <TopBar class="center-one-search">完善个人资料</TopBar>
    <div class="innerWrap">
      <div class="tips base-flex flex-start p-58 bg-white borderR mb-80">
        <img src="@/assets/imgs/tipimg.png" class="img" alt />
        <div class="tips-part">
          <div class="tip-titl">提示</div>
          <div>请阅读完整《 摩奇猴用户协议和隐私政策》</div>
          <!-- <div>即將更新！</div> -->
        </div>
      </div>
      <ul>
        <li>
          <div class="title">真实姓名</div>
          <input type="text" v-model="initData.name" />
          <i class="iconfont iconlock" v-show="initData.name.length"></i>
        </li>
          <li>
          <div class="title">身份证</div>
          <input type="text" v-model="initData.typeNumber" />
          <i class="iconfont iconlock" v-show="initData.typeNumber.length"></i>
        </li>
        <li>
          <div class="title">手机：</div>
          <input type="text" v-model="initData.phone" />
          <i class="iconfont iconlock" v-show="initData.phone.length"></i>
        </li>
        <!-- <li>
          <div class="title">國家</div>
          <van-dropdown-menu>
            <van-dropdown-item v-model="initData.country" :options="option1" />
          </van-dropdown-menu>
        </li> -->

        <!-- <li>
          <div class="title">身份類型</div>
          <van-dropdown-menu>
            <van-dropdown-item v-model="initData.type" :options="option2" />
          </van-dropdown-menu>
        </li>
        <li>
          <div class="title">身份證號</div>
          <input type="text" v-model="initData.typeNumber" />
        </li>
        <li>
          <div class="title">擴展區域</div>
          <van-radio-group v-model="initData.radioValue" disabled direction="horizontal">
            <van-radio name="1">水果區</van-radio>
            <van-radio name="0">蔬菜區</van-radio>
          </van-radio-group>
        </li> -->
          <li>
          <div class="title">配送地址：</div>
          <input type="text" v-model="initData.addr" />
          <i class="iconfont iconlock" v-show="initData.addr.length"></i>
        </li>
        <li>
          <div class="title">密码验证(交易密码）:</div>
          <input type="password" v-model="initData.password" />
        </li>
      </ul>
      <button class="next" @click="goCheckData">提交申请</button>
    </div>
     <YellowComfirm :show="showComfirm" :tipTitle="tips" @clickOk="clickOk"></YellowComfirm>
  </div>
</template>
<script type="text/javascript">
import TabBar from 'components/TabBar'
import TopBar from 'components/TopBar'
import headerImg from '../../assets/imgs/headerImg.png'
import YellowComfirm from 'components/YellowComfirm'
import { http } from 'util/request'
import { CheckUpwd } from 'util/netApi'
import { storage } from 'util/storage'
import { accessToken, loginPro } from 'util/const.js'
export default {
  data () {
    var addmodel
    return {
      showComfirm: false,
      tips: '',
      tipsObj: {
        norealname: '请输入真实姓名！',
        nocountry: '請选择国家！',
        noidtype: '请选择身份证类型!',
        noidcard: '请输入身份证号码！',
        notpwd: '请输入交易密碼！'
      },
      topBarOption: {
        iconLeft: 'back',
        iconRight: ''
      },
      initData: {
        phone: '',
        addr: '',
        name: '',
        country: '',
        type: 2,
        password: '',
        typeNumber: '',
        radioValue: '0'
      },
      option1: [
        { text: '中国', value: 86 }
        //   { text: '东南亚', value: 2 }
      ],
      option2: [
        { text: '身份证', value: 2 }
        //     { text: '港澳通行证', value: 2 }
      ]
    }
  },
  components: {
    TabBar,
    TopBar,
    YellowComfirm
  },
  mounted () {},
  computed: {},
  methods: {
    clickOk () {
      this.showComfirm = false
    },
    goCheckData () {
      if (!this.initData.name) {
        this.showComfirm = true
        this.tips = this.tipsObj.norealname
        return
      }
      if (!this.initData.country) {
        this.showComfirm = true
        this.tips = this.tipsObj.nocountry
        return
      }

      if (!this.initData.type) {
        this.showComfirm = true
        this.tips = this.tipsObj.noidtype
        return
      }

      if (!this.initData.typeNumber) {
        this.showComfirm = true
        this.tips = this.tipsObj.noidcard
        return
      }

      if (!this.initData.password) {
        this.showComfirm = true
        this.tips = this.tipsObj.notpwd
        return
      }
      http(
        CheckUpwd,
        {
          pwd: this.initData.password,
          idcard: this.initData.typeNumber,
          idname: this.initData.name
        },
        json => {
          if (json.code === 0) {
            this.addmodel.uRealName = this.initData.name
            this.addmodel.idType = this.initData.type
            this.addmodel.CountryPhoneCode = this.initData.country
            this.addmodel.idNumber = this.initData.typeNumber
            this.addmodel.TradePass = this.initData.password
            this.addmodel.phone = this.initData.phone
            this.addmodel.addr = this.initData.addr
            storage.setLocalStorage('joindata', JSON.stringify(this.addmodel))
            this.$router.push({ name: 'CheckData' })
          } else {
            this.showComfirm = true
            this.tips = json.msg
          }
        }
      )
    }
  },
  created () {
    if (storage.getLocalStorage('joindata')) {
      this.addmodel = JSON.parse(storage.getLocalStorage('joindata'))
      this.initData.name = this.addmodel.uRealName
      this.initData.type = this.addmodel.idType
      this.initData.country = this.addmodel.CountryPhoneCode
      this.initData.typeNumber = this.addmodel.idNumber
      this.initData.password = this.addmodel.TradePass
      this.initData.phone = this.addmodel.phone
      this.initData.addr = this.addmodel.addr
      if (this.addmodel.L == 0) {
        this.initData.radioValue = '0'
      } else {
        this.initData.radioValue = '1'
      }
    }
  }
}
</script>

<style lang="less" scoped>
.sellEpWrapper {
  .innerWrap {
    width: 100vw;
    border-radius: 40px 40px 0 0;
      height: calc(100vh - 300px);
    overflow: auto;
    margin-top: -20px;
    padding-top: 30px;
    padding-bottom: 200px;
    .tips-part {
      font-weight: bold;
      color: rgba(52, 52, 52, 1);
    }
    .tips {
      width: 90%;
      min-height: 158px;
      align-items: center;
      padding: 30px;
      margin: 0 auto;
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
      position: relative;
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
        color: #6f6d72;
        font-weight: 600;
        letter-spacing: 4px;
        padding-right: 100px;
      }
      .iconfont {
        position: absolute;
        font-size: 60px;
        color: #4493d5;
        top: 100px;
        right: 20px;
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
        font-size: 60px;
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
        font-size: 50px;
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

      /deep/ .van-radio-group {
        // padding: 20px 0;
        padding-left: 20px;
        .van-radio__icon {
          font-size: 50px;
          background: #fff;
          border-radius: 50%;
          border-color: #ccc;
          .van-icon::before {
            content: "";
          }
        }
        .van-radio__label {
          font-size: 40px;
          margin-right: 30px;
        }

        .van-radio__icon--checked {
          .van-icon {
            border-color: #ccc;
            display: flex;
            justify-content: center;
            align-items: center;
            background: #fff;
          }
          .van-icon::before {
            content: "";
            color: #fff;
            width: 24px;
            height: 24px;
            background: #6e21d1;
            border-radius: 50%;
          }
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
}
</style>
