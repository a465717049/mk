<template>
  <div class="sellEpWrapper">
    <TopBar  class="center-one-search" :option="topBarOption">
      創建新賬號
    </TopBar>
    <div class="innerWrap">
      <ul>
        <li>
          <div class="title">账号:必須以字母開頭6-18位</div>
          <input type="text" v-model="initData.code" />
        </li>
        <li>
          <div class="title">昵稱</div>
          <input type="text" v-model="initData.nickName" />
        </li>
        <li>
          <div class="title">級別</div>
          <van-dropdown-menu>
            <van-dropdown-item v-model="initData.level" :options="option1" />
          </van-dropdown-menu>
        </li>
        <li>
          <div class="title">密碼</div>
          <input type="password" v-model="initData.password" />
        </li>
        <li>
          <div class="title">重複</div>
          <input type="password" v-model="initData.comfirmPassword" />
        </li>
      </ul>
      <button class="next" @click="goEditData">下一步</button>
    </div>
    <YellowComfirm :show="showComfirm" :tipTitle="tips" @clickOk="clickOk"></YellowComfirm>
  </div>
</template>
<script type="text/javascript">
import TabBar from 'components/TabBar'
import TopBar from 'components/TopBar'
import { storage } from 'util/storage'
import YellowComfirm from 'components/YellowComfirm'
export default {
  data() {
    return {
      showComfirm: false,
      tips: '',
      tipsObj: {
        nomember: '請輸入账号！',
        nonickname: '請輸入昵稱！',
        nolevel: '請選擇級別！',
        nopwd: '請輸入密碼！',
        norpwd: '請輸入重複密碼',
        nopwdreset: '兩次密碼不一致請重新確認！'
      },
      topBarOption: {
        iconLeft: 'back',
        iconRight: ''
      },
      initData: {
        Jid:0,
        code: "",
        nickName: '',
        level: 1,
        password: '',
        comfirmPassword: '',
        radioValue: '1',
        parentID: 0,
        L: 0
      },
      option1: [
        { text: '高級農場', value: 1 },
        { text: '中級農場', value: 2 }
      ]
    }
  },
  components: {
    TabBar,
    TopBar,
    YellowComfirm
  },
  mounted() {},
  computed: {},
  methods: {
    clickOk() {
      this.showComfirm = false
    },
    goEditData() {
      if (this.initData.code === 0) {
        this.showComfirm = true
        this.tips = this.tipsObj.nomember
        return
      }
      if (!this.initData.nickName) {
        this.showComfirm = true
        this.tips = this.tipsObj.nonickname
        return
      }

      if (!this.initData.level) {
        this.showComfirm = true
        this.tips = this.tipsObj.nolevel
        return
      }

      if (!this.initData.password) {
        this.showComfirm = true
        this.tips = this.tipsObj.nopwd
        return
      }

      if (!this.initData.comfirmPassword) {
        this.showComfirm = true
        this.tips = this.tipsObj.norpwd
        return
      }

      if (this.initData.password !== this.initData.comfirmPassword) {
        this.showComfirm = true
        this.tips = this.tipsObj.nopwdreset
        return
      }

      var Joindata = {
        Jid: this.initData.Jid, // uid
        idType: 2,
        idNumber: '',
        uRealName: '',
        bankCardName: '',
        loginPass: this.initData.password,
        investmentAmount: this.initData.level == 1 ? 1000 : 500,
        CountryPhoneCode: 86,
        MemberNo: this.initData.code,
        NickName: this.initData.nickName,
        googleCode: '',
        TradePass: '',
        TransUserID: 0,
        parentID: 0, //parentID 当前ID
        L: this.initData.L
      }
     // alert(this.initData.Jid)
      storage.setLocalStorage('joindata', JSON.stringify(Joindata))
      this.$router.push({ name: 'Additional' })
    }
  },
  created() {
 

    if (storage.getLocalStorage('joindata')) {
    var modeldata = JSON.parse(storage.getLocalStorage('joindata'))
    this.initData.code = modeldata.MemberNo
    this.initData.Jid = modeldata.Jid
    this.initData.nickName = modeldata.NickName
    this.initData.L = modeldata.L
    this.initData.MemberNo = modeldata.MemberNo
    this.initData.loginPass = modeldata.loginPass
    if (modeldata.investmentAmount == 500) {
      this.initData.level = 2
      } else {
       this.initData.level = 1
      }
     }
    if (this.$route.params.uid) {
      this.initData.Jid = this.$route.params.uid
      this.initData.L = this.$route.params.isLeft
        ? this.$route.params.isLeft
        : 0
    }
      console.log(this.$route.params.isLeft)
      console.log(this.initData.L)
  }
}
</script>

<style lang="less" scoped>
.sellEpWrapper {
  .innerWrap {
    width: 100vw;
    background-color: #ebeaf0;
    border-radius: 40px 40px 0 0;
    margin-top: -20px;
    padding-top: 30px;
    padding-bottom: 400px;
  }
  ul {
    width: 90%;
    margin: 0 auto;
    li {
      .title {
        font-size: 42px;
        margin: 42px;
        margin-bottom: 20px;
        color: #4a494c;
        font-weight: bold;
        opacity: 0.62;
      }
      input {
        height: 148px;
        color: #4a494c;
        font-size: 42px;
        width: 100%;
        padding: 30px 80px;
        border-radius: 20px;
      }
      /deep/ .van-dropdown-menu__bar {
        height: 148px;
        line-height: 148px;
        color: #4a494c;
        width: 100%;
        padding: 0 20px;
        border-radius: 20px;
      }
      /deep/ .van-ellipsis {
        font-size: 42px;
        color: #4a494c;
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

      /deep/ .van-radio-group {
        padding: 67px 0 120px 0;
        .van-radio__icon {
          font-size: 50px;
          background: #fff;
          border-radius: 50%;
          border-color: #ccc;
          .van-icon::before {
            content: '';
          }
        }
        .van-radio__label {
          font-size: 40px;
          margin-right: 30px;
        }

        .van-radio__icon--checked {
          .van-icon {
            display: flex;
            justify-content: center;
            align-items: center;
            background: #fff;
            border-color: #ccc;
          }
          .van-icon::before {
            content: '';
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
    border-radius: 40px;
    height: 164px;
    line-height: 164px;
    font-size: 52px;
    color: #fff;
    margin-top: 100px;
    position: relative;
    letter-spacing: 10px;
  }
  .disabled {
    background: #ccc;
    color: #666;
  }
}
</style>
