<template>
  <div class="sellEpWrapper">
    <TopBar class="center-one-search" :option="topBarOption">
      資料完善
    </TopBar>
    <div class="innerWrap">
      <ul>
        <li>
          <div class="title">真實姓名</div>
          <input type="text" v-model="initData.name" />
        </li>
        <li>
          <div class="title">國家</div>
          <van-dropdown-menu>
            <van-dropdown-item v-model="initData.country" :options="option1" />
          </van-dropdown-menu>
        </li>
        <li>
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
        </li>
        <li>
          <div class="title">交易密碼</div>
          <input type="password" v-model="initData.password" />
        </li>
      </ul>
      <button class="next" @click="goCheckData">下一步</button>
    </div>
    <YellowComfirm :show="showComfirm" :tipTitle="tips" @clickOk="clickOk()"></YellowComfirm>
  </div>
</template>
<script type="text/javascript">
import TabBar from "components/TabBar";
import TopBar from "components/TopBar";
import headerImg from "../../assets/imgs/headerImg.png";
import YellowComfirm from "components/YellowComfirm";
import { http } from "util/request";
import { CheckUpwd } from "util/netApi";
import { storage } from "util/storage";
import { accessToken, loginPro } from "util/const.js";
export default {
  data() {
    var addmodel;
    return {
      showComfirm: false,
      tips: "",
      tipsObj: {
        norealname: "請輸入真實姓名！",
        nocountry: "請選擇國家！",
        noidtype: "請選擇身份類型!",
        noidcard: "請輸入身份證號碼！",
        notpwd: "請輸入交易密碼！"
      },
      topBarOption: {
        iconLeft: "back",
        iconRight: ""
        
      },
      initData: {
        name: "",
        country: "",
        type: 2,
        password: "",
        typeNumber: "",
        radioValue: "0"
      },
      option1: [
        { text: "中国", value: 86 }
        //   { text: '东南亚', value: 2 }
      ],
      option2: [
        { text: "身份证", value: 2 }
        //     { text: '港澳通行证', value: 2 }
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
    clickOk() {
      this.showComfirm = false;
    },
    goCheckData() {
      if (!this.initData.name) {
        this.showComfirm = true;
        this.tips = this.tipsObj.norealname;
        return;
      }
      if (!this.initData.country) {
        this.showComfirm = true;
        this.tips = this.tipsObj.nocountry;
        return;
      }

      if (!this.initData.type) {
        this.showComfirm = true;
        this.tips = this.tipsObj.noidtype;
        return;
      }

      if (!this.initData.typeNumber) {
        this.showComfirm = true;
        this.tips = this.tipsObj.noidcard;
        return;
      }

      if (!this.initData.password) {
        this.showComfirm = true;
        this.tips = this.tipsObj.notpwd;
        return;
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
            this.addmodel.uRealName = this.initData.name;
            this.addmodel.idType = this.initData.type;
            this.addmodel.CountryPhoneCode = this.initData.country;
            this.addmodel.idNumber = this.initData.typeNumber;
            this.addmodel.TradePass = this.initData.password;
             storage.setLocalStorage("joindata", JSON.stringify(this.addmodel));
            this.$router.push({ name: "CheckData" });
          } else {
            this.showComfirm = true;
            this.tips = json.msg;
          }
        }
      );
    }
  },
  created() {
    if (storage.getLocalStorage('joindata')) {
      
      this.addmodel = JSON.parse(storage.getLocalStorage('joindata'));
       this.initData.name= this.addmodel.uRealName;
       this.initData.type=this.addmodel.idType ;
       this.initData.country= this.addmodel.CountryPhoneCode;
       this.initData.typeNumber=this.addmodel.idNumber ;
       this.initData.password=this.addmodel.TradePass ; 
      if (this.addmodel.L == 0) {
        this.initData.radioValue = "0";
      } else {
        this.initData.radioValue = "1";
      }
    }
  }
};
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
        opacity: 0.62;
        color: #4a494c;
        font-weight: bold;
      }
      input {
        height: 148px;
        color: #6f6d72;
        font-size: 42px;
        font-weight: 600;
        width: 100%;
        padding: 30px 80px;
        border-radius: 20px;
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
    border-radius: 40px;
    height: 164px;
    line-height: 164px;
    font-size: 42px;
    color: #fff;
    margin-top: 100px;
    position: relative;
    font-weight: 800;
    letter-spacing: 10px;
  }
}
</style>
