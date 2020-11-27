<template>
  <div class="verificationWrap">
    <TopBar class="center-one-search" :option="topBarOption">
      驗證安全問題
    </TopBar>
    <div class="innerWrap">
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
        <li>
          <div class="photoWrap">
            <img class="photo" src="../../assets/imgs/chilui.png" alt />
            <span>
              溫馨提示：
              <br />驗證成功后將重置密碼。
            </span>
          </div>
        </li>
      </ul>

      <button class="next" @click="submit">驗證</button>
      <YellowComfirm @clickOver="clickOverpay" :show="isEnter" @clickOk="clickOk()" :tipTitle="tips" @changeModel="changeModel"></YellowComfirm>
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
export default {
  components: {
    TopBar,
    YellowComfirm,
    tips: ""
  },
  data() {
    return {
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
  .innerWrap {
    width: 100vw;
    background-color: #ebeaf0;
    border-radius: 40px 40px 0 0;
    margin-top: -20px;
    padding-top: 120px;
    ul {
      width: 90%;
      margin: 0 auto;
      li {
        .title {
          color: #333;
          font-size: 42px;
          margin: 42px 0;
          font-weight: 800;
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
    letter-spacing: 30px;
  }
}
</style>
