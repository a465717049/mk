<template>
  <div class="shopCarWrapper">
    <TopBar class="center-one-search" :option="topBarOption" :badge="carNum">购物车</TopBar>
    <ScrollRefresh
      @getData="TogetUserInfo"
      :residualHeight="160"
      :isNeedUp="false"
      class="innerScroll"
    >
      <div class="innerWrap">
        <div class="goods base-flex flex-start p-58 borderR mb-80">
          <img src="@/assets/imgs/tipimg.png" class="img" alt />
          <div class="goods-info">
            <div class="tip-titl">摩奇猴套装系列（A001)</div>
            <div>赠送：面膜+修复水</div>
          </div>
          <van-field name="stepper" class="font42">
            <template #input>
              <van-stepper
                v-model="stepper"
                @plus="onPlus"
                @minus="onMinus"
                :max="startmax"
                class="font42"
              />
            </template>
          </van-field>
        </div>
        <div class="distribution">
          <div class="dTitle">配送信息：</div>
          <ul class="dInfo">
            <li>广东省深圳市龙华福龙大厦530室</li>
            <li>150****890 李红</li>
          </ul>
        </div>

        <div class="remarkTitle">备注：</div>
        <ul class="remarkInfo">
          <li>摩奇猴套装系列（A001) 尺寸：XL 码</li>
        </ul>
        <div class="sumTitle">合计</div>
        <div class="sumInfo">
          <span class="tit">需扣除您的RP：</span>
          <span class="num">666</span>
          <span class="unit">（RMB)</span>
        </div>
        <div class="sumInfo">
          <span class="tit">您目前帐户可购买商品的余额为：666（RMB)</span>
        </div>
        <div class="buttonWrap">
          <button class="back" @click="goNext">继续购物</button>
          <button class="sure" @click="goNext">立即购买</button>
        </div>
        <!-- <button class="next" @click="goNext">确认提交</button> -->
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
        iconRight: "icongouwucheman"
      },
      carNum:1,
      tips:
        "恭喜！注册成功了！登录ID: 100012登录密码：123456交易密码：123456请尽快登录修改并完善个人资料",
      isEnter: false,
      account: "2,000",
      price: 0,
      shopprice: 0,
      stepper: 1,
      startmax: 8,
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
    },
    onPlus() {
      //增加
      this.price += this.shopprice;
    },
    onMinus() {
      //减少
      this.price -= this.shopprice;
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
.innerScroll {
  /deep/ .wrapper .bscroll-container {
    min-height: calc(100vh - 420px);
  }
}
.shopCarWrapper {
  .innerWrap {
    width: 100vw;
    margin-top: 0px;
    padding-top: 30px;
    padding-bottom: 100px;
    /deep/.van-stepper__plus {
    width: 58px;
    height: 58px;
    // margin-left: 15px;
    background: #b3bdbe;
  }
  /deep/.van-stepper__plus::after {
    color: black;
  }
  /deep/.van-stepper__plus::before {
    color: black;
    height: 4px;
  }
  /deep/.van-stepper__plus::after {
    color: black;
    width: 4px;
  }
  /deep/.van-stepper__minus::before {
    color: black;
    height: 4px;
  }
  /deep/.van-stepper__minus::after {
    color: black;
    width: 4px;
  }
  /deep/.van-stepper__minus {
    width: 58px;
    height: 58px;
    // margin-right: 15px;
    background: #b3bdbe;
  }
  /deep/.van-stepper__input {
    font-size: 42px;

    font-weight: bold;
    color: #010000;
    line-height: 50px;
    height: 50px;
    background: #b3bdbe;
    width: 100px;
  }

  /deep/.van-cell {
    width: 220px;
    background: #b3bdbe;
    border-radius: 30px;
    height: 60px;
    padding: 0;
  }

  }
  
  .goods-info {
    font-weight: bold;
    font-size: 33px;
    color: #000000;
    opacity: 0.75;
    flex: 1;
  }
  .goods {
    width: 90%;
    min-height: 158px;
    align-items: center;
    padding: 30px;
    margin: 0 auto 50px;
    background-color: #dee3ee;
    border-radius: 36px;
    .img {
      width: 151px;
      height: 184px;
      margin-right: 70px;
    }
  }
  .remarkTitle {
    font-size: 33px;
    font-weight: bold;
    color: #dee3ee;
    height: 100px;
    line-height: 100px;
    margin-top: 20px;
    padding-left: 60px;
  }
  .distribution {
    background: #dee3ee;
    border-radius: 36px;
    width: 90%;
    margin: 40px auto 0;
    padding: 30px 40px;
    .dTitle {
      font-weight: bold;
      color: #000000;
      line-height: 130px;
      opacity: 0.75;
    }
    .dInfo {
      li {
        font-size: 36px;
        font-weight: bold;
        color: #6f6d72;
        line-height: 50px;
      }
    }
  }
  .sumTitle {
    font-size: 71px;
    font-weight: bold;
    color: #ffffff;
    margin-top: 60px;
    padding-left: 50px;
  }
  .sumInfo {
    color: #ffffff;
    font-weight: bold;
    padding-left: 50px;
    .tit {
      font-size: 40px;
    }
    .num {
      font-size: 71px;
      font-weight: bold;
    }
  }
  // ul {
  //   width: 90%;
  //   margin: 0 auto;
  //   li {
  //     .title {
  //       color: #ffffff;
  //       font-size: 42px;
  //       margin: 60px 0 22px;
  //       font-weight: 800;
  //       letter-spacing: 10px;
  //       padding-left: 20px;
  //     }
  //     input {
  //       height: 130px;
  //       line-height: 130px;
  //       color: #4a494c;
  //       font-size: 42px;
  //       font-weight: 600;
  //       letter-spacing: 10px;
  //       width: 100%;
  //       padding: 30px 80px;
  //       border-radius: 20px;
  //     }

  //     .verification {
  //       display: flex;
  //       border-radius: 40px;
  //       overflow: hidden;
  //       padding: 20px 0;
  //       background-color: #fff;
  //       height: 148px;
  //       input {
  //         display: inline-block;
  //         width: 60%;
  //         box-sizing: border-box;
  //         height: 110px;
  //         border-radius: 0;

  //         border-right: 1px solid #999;
  //       }
  //       img {
  //         display: inline-block;
  //         width: 39%;
  //       }
  //     }
  //   }
  // }
  .remarkInfo {
    border-radius: 27px;
    background: #ffffff;
    width: 90%;
    margin: 0 auto;
    padding: 30px 40px;
    li {
      font-size: 37px;
      line-height: 70px;
      font-weight: bold;
      color: #0f010f;
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
        color: #4a494c;
        margin-top: 20px;
        height: 100px;
        line-height: 100px;
        opacity: 0.62;
      }
      .bottom {
        color: #4a494c;
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
  .buttonWrap {
    margin-top: 60px;
    padding: 50px;
    width: 100%;
    display: flex;
    justify-content: space-between;
    button {
      width: 40vw;
      height: 144px;
      line-height: 144px;
      border-radius: 20px;
      text-align: center;
      position: relative;
      color: #fff;
      font-size: 40px;
    }
    .back {
      background-color: #7895e8;
    }
    .sure {
      background-color: #113d79;
    }
  }
}
</style>

