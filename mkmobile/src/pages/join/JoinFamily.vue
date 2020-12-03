<template>
  <div class="sellEpWrapper">
    <TopBar class="center-one-search" :option="topBarOption">
      創建子賬號
    </TopBar>
    <div class="innerWrap">
      <div class="moneyWrap clearfix">
        <div class="left fl">
          <div class="top">SP</div>
          <div class="bottom">ACCOUNT</div>
        </div>
        <div class="right fr">{{ account }}</div>
      </div>
      <ul>
        <li>
          <div class="title">位置ID</div>
          <input type="text" v-model="initData.positionId" readonly />
        </li>
        <li>
          <div class="title">投資級別</div>
          <van-dropdown-menu>
            <van-dropdown-item v-model="initData.level" :options="levelList" />
          </van-dropdown-menu>
        </li>

        <li>
          <div class="title">擴展區域</div>
          <input type="text" v-model="initData.area" readonly />
        </li>
        <li>
          <div class="title">交易密碼：</div>
          <input type="password" v-model="initData.password" />
        </li>
      </ul>

      <button class="next" @click="goNext">確認提交</button>
    </div>
    <YellowComfirm :show="isEnter" :tipTitle="tips" @clickOver="clickOverpay" @clickOk="clickOk()"></YellowComfirm>
  </div>
</template>
<script type="text/javascript">
import TopBar from "components/TopBar";
import headerImg from "../../assets/imgs/headerImg.png";
import YellowComfirm from "components/YellowComfirm";
import { http } from "util/request";
import { GetUserInfo, CreateSubAccount } from "util/netApi";
import { storage } from "util/storage";
import { accessToken, loginPro } from "util/const.js";

export default {
  components: {
    TopBar,
    YellowComfirm
  },
  data() {
    return {
      topBarOption: {
        iconLeft: "back",
        iconRight: ""
      },
      isEnter: false,
      account: 0,
      isreturn: 0,
      isLeft:0,
      initData: {
        price: "",
        positionId: "",
        level: 1,
        password: "",
        area: ""
      },
      levelList: [
        { text: '高級農場', value: 1 },
        { text: '中級農場', value: 2 }
      ],
      tips: "",
      tipsObj: {
        success: "创建成功",
        noid: "請重新選擇創建！",
        nolevel: "請選擇級別！",
        nopwd: "請輸入交易密碼！"
      }
    };
  },
  mounted() {},
  computed: {},
  methods: {
    TogetUserInfo() {
      http(GetUserInfo, null, json => {
        if (json.code === 0) {
          this.account = json.response.seed;
        }
      });
    },
    goNext() {
      if (!this.initData.positionId) {
        this.isEnter = true;
        this.tips = this.tipsObj.noid;
        return;
      }
      // this.isEnter = true
      if (!this.initData.password) {
        this.isEnter = true;
        this.tips = this.tipsObj.nopwd;
        return;
      }

      if (!this.initData.level) {
        this.isEnter = true;
        this.tips = this.tipsObj.nolevel;
        return;
      }
      var tmpamount = this.initData.level == 1 ? 1000 : 500;
      if (this.account < tmpamount) {
        this.isEnter = true;
        this.tips = "當前金額不足";
        return;
      }
      http(
        CreateSubAccount,
        {
          uid: this.initData.positionId,
          amount: tmpamount,
          area: this.isLeft,
          tpwd: this.initData.password
        },
        json => {
          if (json.code === 0) {
            this.isEnter = true;
            this.tips = "子賬號：" + json.msg;
            this.account = (this.account - tmpamount).toFixed(2);
            this.isreturn = 1;
          } else {
            this.isEnter = true;
            this.tips = json.msg;
          }
        }
      );
    },
    changeModel(v) {
      this.isEnter = v;
    },
    clickOk() {
      this.isEnter = false;
      if (this.isreturn == 1) {
        this.$router.push({ name: 'relation', params: { uid: this.initData.positionId } })
      }
    }
  },
  created() {
    if (this.$route.params) {
      this.initData.positionId = this.$route.params.uid;
      this.isLeft=this.$route.params.isLeft
      if (this.$route.params.isLeft == 0) {
        this.initData.area = "蔬菜區";
      } else {
        this.initData.area = "水果區";
      }
    }

    this.TogetUserInfo();
  }
};
</script>

<style lang="less" scoped>
.sellEpWrapper {
  .innerWrap {
    width: 100vw;
    padding-bottom: 300px;
    overflow-y: scroll;
    height: calc(100vh - 260px);
    background-color: #ebeaf0;
    border-radius: 40px 40px 0 0;
    margin-top: -20px;
    padding-top: 90px;
  }
  ul {
    width: 90%;
    margin: 0 auto;
    li {
      .title {
        color: #333;
        font-size: 42px;
        margin: 42px;
      }
      input {
        height: 148px;
        line-height: 148px;
        color: #9d9d9f;
        width: 100%;
        padding: 0 30px;
        border-radius: 20px;
        font-size: 42px;
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
        font-size: 42px;
        color: #9d9d9f;
        font-weight: 700;
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
    height: 164px;
    line-height: 164px;
    font-size: 52px;
    color: #fff;
    margin-top: 100px;
    position: relative;
    letter-spacing: 10px;
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
      color: #4678BC;
    }
  }
}
</style>
