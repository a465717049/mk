<template>
  <div class="joinWrapper">
    <TopBar class="center-one-search">注册成为会员</TopBar>
    <div class="innerWrap">
      <div class="tips base-flex flex-start p-58 bg-white borderR mb-80">
        <img src="@/assets/imgs/tipimg.png" class="img" alt />
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
          <div class="title">昵稱:</div>
          <input type="text" v-model="initData.nickName" />
          <i class="iconfont icondui"></i>
        </li>
        <li>
          <div class="title">密碼:</div>
          <input type="password" v-model="initData.password" />
          <i class="iconfont icondui"></i>
        </li>
        <li>
          <div class="title">安置ID:</div>
          <input type="text" v-model="initData.parentID" />
          <i class="iconfont icondui"></i>
        </li>
        <li>
          <div class="title">接点ID:</div>
          <input type="text" v-model="initData.Jid" />
          <i class="iconfont icondui"></i>
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
    <YellowComfirm :show="showComfirm" :tipTitle="tips" @clickOk="clickOk"></YellowComfirm>
  </div>
</template>
<script type="text/javascript">
import TabBar from "components/TabBar";
import TopBar from "components/TopBar";
import { http } from 'util/request'
import {  GetUserInfo } from 'util/netApi'
import { accessToken, loginPro } from 'util/const.js'
import { storage } from "util/storage";
import YellowComfirm from "components/YellowComfirm";
export default {
  data() {
    return {
      showComfirm: false,
      tips: "",
      tipsObj: {
        nomember: "請輸入账号！",
        nonickname: "請輸入昵稱！",
        nolevel: "請選擇級別！",
        nopwd: "請輸入密碼！",
        nopwdreset: "兩次密碼不一致請重新確認！"
      },
      initData: {
        Jid: 0,
        code: "",
        nickName: "",
        level: 666,
        password: "",
        comfirmPassword: "",
        radioValue: "1",
        parentID: 0,
        L: 0
      },
      option1: [
        { text: "666", value: 666 },
        { text: "2000", value: 2000 },
        { text: "10000", value: 10000 }
      ]
    };
  },
  components: {
    TabBar,
    TopBar,
    YellowComfirm
  },
  mounted() {},
  computed: {},
  methods: {
     TogetUserInfo() {
      http(GetUserInfo, null, json => {
        if (json.code === 0) {
           this.initData.parentID = json.response.uid;
          console.log(json)
         // this.account = json.response.apple
        }
      })
    },
    clickOk() {
      this.showComfirm = false;
    },
    goEditData() {
      if (this.initData.code === 0) {
        this.showComfirm = true;
        this.tips = this.tipsObj.nomember;
        return;
      }
      if (!this.initData.nickName) {
        this.showComfirm = true;
        this.tips = this.tipsObj.nonickname;
        return;
      }

      if (!this.initData.level) {
        this.showComfirm = true;
        this.tips = this.tipsObj.nolevel;
        return;
      }

      if (!this.initData.password) {
        this.showComfirm = true;
        this.tips = this.tipsObj.nopwd;
        return;
      }

      var Joindata = {
        Jid: this.initData.Jid, // uid
        idType: 2,
        idNumber: "",
        uRealName: "",
        bankCardName: "",
        loginPass: this.initData.password,
        investmentAmount: this.initData.level,
        CountryPhoneCode: 86,
        MemberNo: this.initData.code,
        NickName: this.initData.nickName,
        googleCode: "",
        TradePass: "",
        TransUserID: 0,
        parentID: this.initData.parentID, //parentID 当前ID
        L: this.initData.L,
        phone:"",
        addr:"",
        levlename:this.initData.level,
      };
      // alert(this.initData.Jid)
      storage.setLocalStorage("joindata", JSON.stringify(Joindata));
      this.$router.push({ name: "Additional" });
    }
  },
  created() {
    if (storage.getLocalStorage("joindata")) {
      var modeldata = JSON.parse(storage.getLocalStorage("joindata"));
      this.initData.code = modeldata.MemberNo;
      this.initData.Jid = modeldata.Jid;
      this.initData.nickName = modeldata.NickName;
      this.initData.L = modeldata.L;
      this.initData.MemberNo = modeldata.MemberNo;
      this.initData.loginPass = modeldata.loginPass;
      this.initData.levlename = modeldata.levlename;
    }
    if (this.$route.params.uid) {
      this.initData.Jid = this.$route.params.uid;
      this.initData.L = this.$route.params.isLeft
        ? this.$route.params.isLeft
        : 0;
    }else
    {
      this.TogetUserInfo();
    }

    console.log(this.$route.params.isLeft);
    console.log(this.initData.L);
  }
};
</script>

<style lang="less" scoped>
.joinWrapper {
  .innerWrap {
    width: 100vw;
    margin-top: -20px;
    padding-top: 30px;
    padding-bottom: 400px;
    .tips-part {
      font-weight: bold;
      color: rgba(52, 52, 52, 1);
    }
    .tips {
      width: 90%;
      min-height: 158px;
      align-items: center;
      padding: 30px ;
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
