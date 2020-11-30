<template>
  <div class="verificationWrap">
    <!-- <TopBar class="center-one-search" :option="topBarOption">
      驗證安全問題
    </TopBar>-->
    <div class="innerWrap">
      <img :src="forgotImg" alt class="head" />
      <h6 class="forgotTitle">验证问题</h6>
      <ul>
        <li>
          <div class="title">{{Q1}}</div>
          <input type="text" hidden v-model="initData.q1id" />
          <input type="text" v-model="initData.q1name" />
        </li>
        <li>
          <div class="title">{{Q2}}</div>
          <input type="text" hidden v-model="initData.q2id" />
          <input type="text" v-model="initData.q2name" />
        </li>
        <li>
          <div class="title">{{Q3}}</div>
          <input type="text" hidden v-model="initData.q3id" />
          <input type="text" v-model="initData.q3name" />
        </li>
        <li class="tipWrap">
          <div class="tip">提示：</div>
          <div class="tip">下一步将验证您预留的信息。</div>
        </li>
        <li>
          <button class="next" @click="submit">驗證</button>
        </li>
      </ul>
      <div class="bottom-part">
        <div class="version1">MOKI MONKEY 摩奇候</div>
        <div class="version2">MOKI MONKEY Co.,ltd.. Copyright 2020 (C) All right reserved</div>
      </div>
      <YellowComfirm
        @clickOver="clickOverpay"
        :show="isEnter"
        @clickOk="clickOk()"
        :tipTitle="tips"
        @changeModel="changeModel"
      ></YellowComfirm>
    </div>
  </div>
</template>

<script>
import TopBar from "components/TopBar";
import YellowComfirm from "components/YellowComfirm";
import { GetQuestionListByID, GetCheckQuestionAll } from "util/netApi";
import { http } from "util/request";
import { storage } from "util/storage";
import { accessToken, loginPro } from "util/const.js";
import forgot from "@/assets/imgs/login/forgot.png";
export default {
  components: {
    TopBar,
    YellowComfirm,
    tips: ""
  },
  data() {
    return {
      forgotImg: forgot,
      isEnter: false,
      showConfirm: true,
      topBarOption: {
        iconLeft: "back",
        iconRight: ""
      },
      Q1: "q1",
      Q2: "q2",
      Q3: "q3",
      initData: {
        q1id: 0,
        q2id: 0,
        q3id: 0,
        q1name: "",
        q2name: "",
        q3name: ""
      },
      isfig: false
    };
  },
  mounted() {},
  created() {
    this.getVerificationCode();
  },
  methods: {
    clickOk() {
      this.isEnter = false;
      if (this.isfig) {
        this.$router.push({ name: "/Login" });
      }
      //  this.$router.push({ name: "/Login" });
    },
    changeModel(v) {
      this.isEnter = v;
    },
    submit() {
      var answerslist = new Array();
      answerslist.push({
        qID: this.initData.q1id,
        answer: this.initData.q1name
      });
      answerslist.push({
        qID: this.initData.q2id,
        answer: this.initData.q2name
      });
      answerslist.push({
        qID: this.initData.q3id,
        answer: this.initData.q3name
      });
      http(
        GetCheckQuestionAll,
        {
          uid: this.$route.params.uid,
          answerslist: JSON.stringify(answerslist)
        },
        json => {
          if (json.code === 0) {
            this.isEnter = true;
            //let img = json.response.vcode;
            this.isfig = true;
            this.tips = "密碼重置為123456,請儘快登錄修改！";
          } else {
            this.tips = json.msg;
            this.isEnter = true;
          }
        }
      );
      //  this.isEnter = true;
    },
    getVerificationCode() {
      http(GetQuestionListByID, { uid: this.$route.params.uid }, json => {
        if (json.code === 0) {
          //let img = json.response.vcode;
          this.initData.q1id = json.response[0].Id;
          this.initData.q2id = json.response[1].Id;
          this.initData.q3id = json.response[2].Id;
          this.Q1 = json.response[0].Question_CN;
          this.Q2 = json.response[1].Question_CN;
          this.Q3 = json.response[2].Question_CN;
        }
      });
    }
  }
};
</script>

<style lang="less" scoped>
.verificationWrap {
  .forgotTitle {
    width: 100%;
    text-align: center;
    margin-bottom: 60px;
    font-size: 70px;
    color: #000403;
    font-weight: bolder;
  }
  .tipWrap {
    padding-top: 70px !important;
  }
  .tip {
    line-height: 54px;
    font-weight: bold;
    color: #353535;
    font-size: 38px;
  }
  .innerWrap {
    width: 100vw;
    overflow-y: scroll;
    height: calc(100vh);
    background: #efb618 url("../../assets/imgs/login/ybj.png") no-repeat left
      top / 100% 100%;
    .head {
      display: block;
      width: 370px;
      height: 294px;
      margin: 80px auto 230px;
    }
    ul {
      width: 90%;
      margin: 0 auto;
      li {
        .title {
          color: #353535;
          font-size: 42px;
          margin: 42px 0;
          font-weight: 800;
          letter-spacing: 6px;
        }
        input {
          height: 139px;
          box-shadow: 0px 5px 6px 0px rgba(0, 0, 0, 0.24);
          line-height: 139px;
          color: #6f6d72;
          width: 100%;
          padding: 0 38px;
          border-radius: 20px;
          font-size: 50px;
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
    .photoWrap {
      height: 222px;
      background-color: #fff;
      border-radius: 40px;
      padding: 50px;
      margin-top: 120px;
      img {
        width: 150px;
        vertical-align: middle;
        margin-right: 60px;
      }
      span {
        display: inline-block;
        vertical-align: middle;
        font-size: 38px;
        color: #343434;
        font-weight: 700;
      }
    }
  }
  .searchWrap {
    padding: 60px;
  }
  .next {
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
  .bottom-part {
    position: relative;
    height: 25vh;
    font-family: HYYakuHei;
  }
  .version2 {
    color: #060000;
    font-size: 28px;
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
    bottom: 160px;
    width: 100%;
    left: 50%;
    transform: translate(-50%);
    font-weight: bold;
  }
}
</style>
