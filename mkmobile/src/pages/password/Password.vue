<template>
  <div class="passwordWrap">
    <TopBar class="center-one-search">修改密码</TopBar>
    <div class="innerWrap">
      <div class="tips base-flex flex-start p-58 bg-white borderR mb-80">
        <img src="@/assets/imgs/tipimg.png" class="img" alt />
        <div class="tips-part">
          <div class="tip-titl">提示</div>
          <div>密码最少6位,必须包含大小写及特殊字符。</div>
        </div>
      </div>
      <ul>
        <li>
          <div class="title">修改密码类型</div>
          <van-dropdown-menu>
            <van-dropdown-item v-model="form.type" :options="option1" />
          </van-dropdown-menu>
        </li>
        <li>
          <div class="title">当前密码：</div>
          <input type="password" v-model="form.password" />
        </li>
        <li>
          <div class="title">新密码：</div>
          <input type="password" v-model="form.new_password" />
        </li>
        <li>
          <div class="title">再次输入：</div>
          <input type="password" v-model="initData.comfirmPassword" />
        </li>
        <li>
          <div class="title">
            您的父亲叫什么名字？
            <!-- {{DataQuestion}} -->
          </div>
          <input type="text" hidden v-model="form.qid" />
          <input type="text" v-model="form.qanswer" />
        </li>
        <li>
          <div class="title">输入您预留的身份证号或护照</div>
          <input type="text" v-model="form.idcard" />
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
import { SetUpdatePassword, GetMyQuestion } from "util/netApi";
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
      form: {
        type: 0,
        password: "",
        new_password: "",
        qid: 0,
        qanswer: "",
        idcard: ""
      },
      DataQuestion: "問題",
      showChat: true,
      topBarOption: {
        iconLeft: "back"
      },
      isEnter: false,
      initData: {
        password: "",
        newPassword: "",
        comfirmPassword: "",
        farterName: "",
        codoNumber: ""
      },
      tips: "",
      tipsObj: {
        nosucceed: "修改失敗",
        succeed: "修改成功"
      },
      option1: [
        { text: "登录密码", value: 0 },
        { text: "交易密码", value: 1 }
      ]
    };
  },
  mounted() {
    this.ToGetMyQuestion();
  },
  methods: {
    goNext() {
      http(SetUpdatePassword, this.form, json => {
        //console.log(json)
        if (json.code === 0) {
          this.isEnter = true;
          this.tips = this.tipsObj.succeed;
        } else {
          this.isEnter = true;
          if (!json.success) {
            this.tips = json.msg;
          } else {
            this.tips = this.tipsObj.nosucceed;
          }
        }
      });
    },
    ToGetMyQuestion() {
      http(GetMyQuestion, null, json => {
        if (json.code === 0) {
          //farterName   codeNumber
          this.DataQuestion = json.response.question;
          this.form.qid = json.response.qId;
        }
      });
    },
    clickOk() {
      this.isEnter = false;
    },
    changeModel(v) {
      this.isEnter = v;
    },
    loadQuestion() {
      http(loadQuestion, {}, json => {
        if (json.code === 0) {
          this.isEnter = true;
          this.form.qId = this.response.qID;
          this.initData.Question = this.response.Question_CN;
        }
      });
      this.isEnter = v;
    }
  },
  created() {}
};
</script>

<style lang="less" scoped>
.passwordWrap {
  .innerWrap {
    width: 100vw;
    height: calc(100vh - 300px);
    overflow: auto;
    border-radius: 40px 40px 0 0;
    margin-top: -20px;
    padding: 40px 0 240px;
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
        input[type="password"]{
          color: #4678BC;
        }
        .iconfont {
          position: absolute;
          font-size: 60px;
          color: #4493d5;
          top: 100px;
          right: 20px;
        }
        /deep/ .van-dropdown-menu__bar {
          height: 120px;
          line-height: 120px;
          color: #6F6D72;
          font-weight: 600;
          width: 100%;
          padding: 0 20px;
          border-radius: 20px;
          letter-spacing: 10px;
        }
        /deep/ .van-ellipsis {
          font-size: 42px;
          color: #6F6D72;
          font-weight: 600;
          letter-spacing: 10px;
        }
        /deep/ .van-dropdown-menu__title {
          height: 120px;
          line-height: 120px;
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
