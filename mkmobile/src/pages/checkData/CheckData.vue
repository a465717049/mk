<template>
  <div class="sellEpWrapper">
    <TopBar class="center-one-search" :option="topBarOption">確認數據</TopBar>
    <ScrollRefresh
      @getData="TogetUserInfo"
      :residualHeight="160"
      :isNeedUp="false"
      class="innerScroll"
    >
      <div class="innerWrap">
        <div class="moneyWrap clearfix">
          <div class="left fl">
            <div class="top">RP</div>
            <div class="bottom">Account</div>
          </div>
          <div class="right fr">{{ account }}</div>
        </div>
        <ul>
          <li>
            <div class="title">需要費用</div>
            <input type="text" v-model="initData.price" disabled />
          </li>
          <li>
            <div class="title">位置ID</div>
            <input type="text" v-model="initData.positionId" disabled />
          </li>
          <li>
            <div class="title">擴展區域</div>
            <input type="text" v-model="initData.area" disabled />
          </li>
        </ul>

        <button class="next" @click="goNext">确认提交</button>
      </div>
    </ScrollRefresh>
    <YellowComfirm :show="isEnter" :tipTitle="tips" @clickOver="clickOverpay" @clickOk="clickOk()"></YellowComfirm>
  </div>
</template>
<script type="text/javascript">
import TopBar from "components/TopBar";
import headerImg from "../../assets/imgs/headerImg.png";
import YellowComfirm from "components/YellowComfirm";
import ScrollRefresh from "components/ScrollRefresh";
import { http } from "util/request";
import { CreateNewAccount, GetUserInfo } from "util/netApi";
import { storage } from "util/storage";
import { accessToken, loginPro } from "util/const.js";
export default {
  components: {
    TopBar,
    YellowComfirm,
    ScrollRefresh
  },
  data() {
    return {
      topBarOption: {
        iconLeft: "back",
        iconRight: ""
      },
      tips: "",
      isEnter: false,
      account: "2,000",
      isreturn: 0,
      addmodel: {},
      initData: {
        price: "",
        positionId: "",
        area: ""
      },
      option1: [
        { text: "1000", value: 1 },
        { text: "500", value: 2 }
      ]
    };
  },
  mounted() {},
  computed: {},
  methods: {
    goNext() {
      if (this.account < this.addmodel.investmentAmount) {
        this.isEnter = true;
        this.tips = "當前金額不足";
        return;
      }
      http(
        CreateNewAccount,
        { jsondata: JSON.stringify(this.addmodel) },
        json => {
          if (json.code === 0) {
            this.isEnter = true;
            this.tips = "新账号:" + json.msg;
            this.account = this.account - this.addmodel.investmentAmount;
            this.isreturn = 1;
          } else {
            this.isEnter = true;
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
        this.$router.push({
          name: "relation",
          params: { uid: this.addmodel.Jid }
        });
      }
    },
    TogetUserInfo() {
      http(GetUserInfo, null, json => {
        if (json.code === 0) {
          this.account = json.response.rp;
        }
      });
    }
  },
  created() {
    this.TogetUserInfo();
    if (storage.getLocalStorage("joindata")) {
      this.addmodel = JSON.parse(storage.getLocalStorage("joindata"));
      this.initData.price = this.addmodel.investmentAmount;
      this.initData.positionId = this.addmodel.Jid;
      if (this.addmodel.L == 0) {
        this.initData.area = "蔬菜區";
      } else {
        this.initData.area = "水果區";
      }
    }
    this.isEnter = false;
  }
};
</script>

<style lang="less" scoped>
.innerScroll{
  /deep/ .wrapper  .bscroll-container{
    min-height: calc(100vh - 420px);
  }
}
.sellEpWrapper {
  .innerWrap {
    width: 100vw;
    background-color: #ebeaf0;
    border-radius: 40px 40px 0 0;
    margin-top: 0px;
    padding-top: 30px;
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
        color: #4a494c;
        font-size: 42px;
        font-weight: 600;
        width: 100%;
        padding: 30px 80px;
        border-radius: 20px;
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
    border-radius: 40px;
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
        color: #999;
        height: 140px;
        line-height: 140px;
      }
      .bottom {
        color: #999;
        font-size: 42px;

        height: 80px;
        line-height: 80px;
      }
    }
    .right {
      font-size: 104px;
      font-weight: 600;
      color: #6318c3;
    }
  }
}
</style>
