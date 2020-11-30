<template>
  <div class="passwordWrap">
    <TopBar class="center-one-search">
      <div>密保设置</div>
    </TopBar>
    <div class="innerWrap">
      <div class="tips base-flex flex-start p-58 bg-white borderR mb-80">
        <img src="@/assets/imgs/tipimg.png" class="img" alt />
        <div class="tips-part">
          <div class="tip-titl">提示</div>
          <div>您将设置三个问题及答案，以后在多场合会 涉及到密保验证。</div>
        </div>
      </div>
      <ul>
        <li>
          <div class="title">{{Q1}}</div>
          <input type="text" hidden v-model="q1id" />
          <input type="text" v-model="form.q1answer" />
        </li>
        <li>
          <div class="title">{{Q2}}</div>
          <input type="text" hidden v-model="q2id" />
          <input type="text" v-model="form.q2answer" />
        </li>
        <li>
          <div class="title">{{Q3}}</div>
          <input type="text" hidden v-model="q3id" />
          <input type="text" v-model="form.q3answer" />
        </li>
        <li>
          <div class="title">身份证号：</div>
          <input type="text" v-model="form.idcard" />
        </li>
        <li>
          <div class="title">交易密码：</div>
          <input type="text" hidden v-model="q3id" />
          <input type="password" v-model="form.tpwd" />
        </li>
      </ul>
      <button class="submit" @click="goNext">提交</button>
    </div>
    <YellowComfirm
      :show="isEnter"
      @clickOver="clickOverpay"
      @clickOk="clickOk()"
      :tipTitle="tips"
      @changeModel="changeModel"
    ></YellowComfirm>
  </div>
</template>

<script>
import TopBar from "components/TopBar";
import YellowComfirm from "components/YellowComfirm";
import { GetAllQuestionList, GetSetQuestionList } from "util/netApi";
import { http } from "util/request";
import { storage } from "util/storage";
import { accessToken, loginPro } from "util/const.js";

export default {
  components: {
    TopBar,
    YellowComfirm
  },
  data() {
    return {
      showChat: true,
      q1id: 0,
      q2id: 0,
      q3id: 0,
      Q1: "1",
      Q2: "2",
      Q3: "3",
      form: {
        idcard: "",
        tpwd: "",
        q1answer: "",
        q2answer: "",
        q3answer: ""
      },
      isEnter: false,
      tips: "",
      tipsObj: {
        nosucceed: "提交失败",
        succeed: "提交成功"
      }
    };
  },
  mounted() {},
  methods: {
    goNext() {
      var answerslist = new Array();
      answerslist.push({
        qID: this.q1id,
        answer: this.form.q1answer
      });
      answerslist.push({
        qID: this.q2id,
        answer: this.form.q2answer
      });
      answerslist.push({
        qID: this.q3id,
        answer: this.form.q3answer
      });

      http(
        GetSetQuestionList,
        {
          tpwd: this.form.tpwd,
          idcard: this.form.idcard,
          answerslist: JSON.stringify(answerslist)
        },
        json => {
          if (json.code === 0) {
            this.$router.push({ name: "setting" });
          } else {
            this.tips = json.msg;
            this.isEnter = true;
          }
        }
      );
    },
    ToGetAllQuestionList() {
      http(GetAllQuestionList, null, json => {
        console.log(json);
        let max = json.response.length - 1;
        if (json.code === 0) {
          let q1 = Math.ceil(Math.random() * max);
          let q2 = Math.ceil(Math.random() * max);
          while (q1 == q2) {
            q2 = Math.ceil(Math.random() * max);
          }
          let q3 = Math.ceil(Math.random() * max);
          while (q3 == q2 || q3 == q1) {
            q3 = Math.ceil(Math.random() * max);
          }

          this.Q1 = json.response[q1].Question_CN;
          this.Q2 = json.response[q2].Question_CN;
          this.Q3 = json.response[q3].Question_CN;
          this.q1id = json.response[q1].Id;
          this.q2id = json.response[q2].Id;
          this.q3id = json.response[q3].Id;
        }
      });
    },
    clickOk() {
      this.isEnter = false;
    },
    changeModel(v) {
      this.isEnter = v;
    }
  },
  created() {
    this.ToGetAllQuestionList();
  }
};
</script>

<style lang="less" scoped>
.passwordWrap {
  .innerWrap {
    width: 100vw;
    height: calc(100vh - 300px);
    overflow: auto;
    margin-top: -10px;
    padding: 40px 0 240px;
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
    ul {
      width: 90%;
      margin: 0 auto;
      li {
        .title {
          color: #353535;
          font-size: 42px;
          margin: 12px;
          opacity: 0.62;
          color: #4a494c;
          font-weight: bold;
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
          font-weight: 600;
          font-size: 42px;
          color: #6f6d72;
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
      img {
        width: 150px;
        vertical-align: middle;
        margin-right: 30px;
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
  .submit {
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
  }
}
</style>
