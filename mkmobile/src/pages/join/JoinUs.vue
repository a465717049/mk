<template>
  <div class="joinWrapper">
    <TopBar class="center-one-search">注册成为会员</TopBar>
    <div class="innerWrap">
      <div class="tips base-flex flex-start p-58 bg-white borderR mb-80">
        <img src="@/assets/imgs/login/head.png" class="img" alt />
        <div class="tips-part">
          <div class="tip-titl">提示</div>
          <div>密码最少6位,必须包含大小写及特殊字符。</div>
          <!-- <div>即將更新！</div> -->
        </div>
      </div>
      <ul>
        <!-- <li>
          <div class="title">账号:必須以字母開頭6-18位</div>
          <input type="text" v-model="initData.code" />
        </li>-->
        <li>
          <div class="title">昵称:</div>
          <input type="text" v-model="initData.nickName" />
          <i class="iconfont icondui" v-show="initData.nickName.length"/>
        </li>
        <li>
          <div class="title">密码:</div>
          <input type="password" v-model="initData.password" />
          <i class="iconfont icondui" v-show="initData.password.length"/>
        </li>
        <li>
          <div class="title">推荐ID:</div>
          <input type="text" @blur="checkpid" v-model="initData.Tid" />
          <i class="iconfont icondui" v-show="initData.Tid.length"/>
        </li>
        <li>
          <div class="title">安置ID:</div>
          <input type="text" @blur="checkjid" v-model="initData.Jid" />
          <i class="iconfont icondui" v-show="initData.Jid.length"/>
        </li>
        <li>
          <div class="title">会员级别：</div>
          <van-dropdown-menu>
            <van-dropdown-item v-model="initData.level" :options="option1" />
          </van-dropdown-menu>
        </li>
        <!-- <li>
          <div class="title">重複</div>
          <input type="password" v-model="initData.comfirmPassword" />
        </li>-->
      </ul>
      <button class="next" @click="goEditData">
        下一步
        <i class="jiantou iconfont iconarrow-right"></i>
      </button>
    </div>
    <YellowComfirm :show="showComfirm" :tipTitle="tips" @clickOk="clickOk" @changeModel="changeModel"></YellowComfirm>
  </div>
</template>
<script type="text/javascript">
import TabBar from 'components/TabBar'
import TopBar from 'components/TopBar'
import { http } from 'util/request'
import { GetUserInfo, checkUser } from 'util/netApi'
import { accessToken, loginPro } from 'util/const.js'
import { storage } from 'util/storage'
import YellowComfirm from 'components/YellowComfirm'
export default {
  data () {
    return {
      successjid: true,
      successpid: true,
      showComfirm: false,
      tips: '',
      tipsObj: {
        nomember: '请输入账号！',
        nonickname: '请输入昵称！',
        nolevel: '请选择级别！',
        nopwd: '请输入密码！',
        nopwdreset: '两次密码不一致请重新确认！'
      },
      initData: {
        Uid: 0,
        Jid: 0,
        Tid: 0,
        code: '',
        nickName: '',
        level: 666,
        password: '',
        comfirmPassword: '',
        radioValue: '1',
        L: 0
      },
      option1: [
        { text: '初级会员', value: 666 },
        { text: '中级会员', value: 2000 },
        { text: '高级会员', value: 10000 },
        { text: '超级会员', value: 30000 }
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
    checkjid () {
      http(checkUser, {uid: this.initData.Jid}, json => {
        if (json.code === 0) {
          this.successjid = true
        } else {
          this.successjid = false
          this.showComfirm = true
          this.tips = '安置ID不存在'
        }
      })
    },
    checkpid () {
      http(checkUser, {uid: this.initData.Tid}, json => {
        if (json.code === 0) {
          this.successpid = true
        } else {
          this.successpid = false
          this.showComfirm = true
          this.tips = '推荐ID不存在'
        }
      })
    },
    TogetUserInfo () {
      http(GetUserInfo, null, json => {
        if (json.code === 0) {
          this.initData.Tid = json.response.uid
          this.initData.Jid = json.response.uid
          this.initData.Uid = json.response.uid
          // console.log(json)
          // this.account = json.response.apple
        }
      })
    },
    clickOk () {
      this.showComfirm = false
    },
    changeModel (v) {
      this.showComfirm = v
    },
    goEditData () {
      if (this.initData.code === 0) {
        this.showComfirm = true
        this.tips = this.tipsObj.nomember
        return
      }
      if (!this.initData.nickName) {
        console.log(2255)
        this.showComfirm = true
        console.log(this.showComfirm)
        this.tips = this.tipsObj.nonickname
        return
      }

      if (!this.initData.level) {
        this.showComfirm = true
        this.tips = this.tipsObj.nolevel
        return
      }

      if (!this.initData.Jid || this.initData.Jid == 0) {
        this.showComfirm = true
        this.tips = '请输入安置id'
        return
      }

      if (!this.initData.Tid || this.initData.Tid == 0) {
        this.showComfirm = true
        this.tips = '请输入推荐id'
        return
      }

      if (!this.successjid) {
        this.showComfirm = true
        this.tips = '请检查接点Id'
        return
      }

      if (!this.successpid) {
        this.showComfirm = true
        this.tips = '请检查安置Id'
        return
      }

      if (!this.initData.password) {
        this.showComfirm = true
        this.tips = this.tipsObj.nopwd
        return
      }

      var Joindata = {
        Tid: this.initData.Tid, // uid
        Jid: this.initData.Jid, // uid
        idType: 2,
        idNumber: '',
        uRealName: '',
        bankCardName: '',
        loginPass: this.initData.password,
        investmentAmount: this.initData.level,
        CountryPhoneCode: 86,
        MemberNo: this.initData.code,
        NickName: this.initData.nickName,
        googleCode: '',
        TradePass: '',
        TransUserID: 0,
        parentID: this.initData.Uid, // parentID 当前ID
        L: this.initData.L,
        phone: '',
        addr: '',
        levlename: this.initData.level
      }
      storage.setLocalStorage('joindata', JSON.stringify(Joindata))

      this.$router.push({ name: 'CheckData' })
    }
  },
  created () {
    if (storage.getLocalStorage('joindata')) {
      var modeldata = JSON.parse(storage.getLocalStorage('joindata'))
      this.initData.code = modeldata.MemberNo
      this.initData.Jid = modeldata.Jid
      this.initData.nickName = modeldata.NickName
      this.initData.L = modeldata.L
      this.initData.MemberNo = modeldata.MemberNo
      this.initData.loginPass = modeldata.loginPass
      this.initData.levlename = modeldata.levlename
      this.initData.password = modeldata.loginPass
      this.initData.parentID = modeldata.parentID
    }
    if (this.$route.params.uid) {
      this.initData.Jid = this.$route.params.uid
      this.initData.L = this.$route.params.isLeft
        ? this.$route.params.isLeft
        : 0
    } else {
      this.TogetUserInfo()
    }
  }
}
</script>

<style lang="less" scoped>
.joinWrapper {
  height: 100vh;
  overflow: auto;
  .innerWrap {
    width: 100vw;
    margin-top: -20px;
    padding-top: 30px;
    padding-bottom: 400px;
    .tips-part {
      font-weight: bold;
      color: rgba(52, 52, 52, 1);
      div{
          font-size: 40px;
          line-height: 60px;
       }
    }
    .tips {
      width: 90%;
      min-height: 158px;
      align-items: center;
      padding: 30px ;
      margin: 0 auto;
      box-shadow: 0px 5px 5px 0px rgba(0, 0, 0, 0.24);
     .img {
        width: 122px;
        height: 128px;
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
        font-size: 42px;
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
        font-size: 40px;
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
        padding: 67px 0 120px 0;
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
            display: flex;
            justify-content: center;
            align-items: center;
            background: #fff;
            border-color: #ccc;
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
    margin: 100px auto 0;
    background: #f5c148;
    border-radius: 20px;
    height: 130px;
    line-height: 130px;
    font-size: 52px;
    color: #fff;
    font-weight: 600;
    letter-spacing: 4px;
    position: relative;
    .jiantou {
      position: absolute;
      right: 30px;
      top: 0px;
      font-size: 60px;
    }
  }
  .disabled {
    background: #ccc;
    color: #666;
  }
}
</style>
