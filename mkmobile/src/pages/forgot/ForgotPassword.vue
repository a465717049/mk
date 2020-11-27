<template>
  <div class="forgotWrap">
    <TopBar class="center-one-search" :option="topBarOption">
      <div>Forgot Password</div>
    </TopBar>
    <div class="innerWrap">
      <ul>
        <li>
          <div class="title">請輸入您的用戶ID</div>
          <input type="text" v-model="id" />
        </li>
        <!--    <li>
          <div class="title">重置登录密码</div>
             <van-dropdown-menu>
            <van-dropdown-item v-model="passwordType" :options="passwordTypeList" />
          </van-dropdown-menu>
        </li>-->
        <!-- <li>
          <div class="title">請輸入驗證碼</div>
          <div class="verification">
            <input type="text" v-model="verification" />
            <img :src="image" @click='getVerificationCode' alt />
          </div>
        </li> -->
        <li>
          <div class="photoWrap">
            <img class="photo" src="../../assets/imgs/forgot1-img1.png" alt />
            <span>
              溫馨提示：重置登錄密碼
              <br />下一步將驗證您的預留信息。
            </span>
          </div>
        </li>
      </ul>

      <button class="next" @click='gox'>
        提交
      </button>
    </div>
    <YellowComfirm :show="showComfirm" :tipTitle="tips" @clickOk="clickOk" @changeModel="changeModel"></YellowComfirm>
  </div>
</template>

<script>
import YellowComfirm from "components/YellowComfirm";
import TopBar from "components/TopBar";
import { storage } from "util/storage";
import { http } from "util/request";
import { getVerificationCode, checkUser } from "util/netApi";
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
      passwordType: 0,
      id: null,
      verification: null,
      code: null,
      image: null,
      showComfirm: false,
      //  receiptId: null,
      //  transPassword: null,
      //  verificationCode: null,
      tips: "",
      passwordTypeList: [
        { text: "登录密码", value: 0 }
        //      { text: "交易密码", value: 1 }
      ]
    };
  },
  mounted() {
    //this.getVerificationCode();
  },
  methods: {
    clickOk() {
      this.showComfirm = false;
    },
    changeModel(v) {
      this.showComfirm = v;
    },
    getVerificationCode() {
      http(getVerificationCode, null, json => {
        if (json.code === 0) {
          console.log(json.response.vcode)
          let img = json.response.vcode;
          var tem1 = img.substring(0, img.length - 21);
          var tem2 = img.substring(img.length - 16);
          this.code = img.substring(img.length - 21, img.length - 16);
          this.image = "data:image/png;base64," + tem1 + tem2;
        }
      });
    },
    gox() {
      // if (this.verification.toLowerCase() != this.code.toLowerCase()) {
      //   this.tips = "验证码错误！请确认验证码是否正确！";
      //   this.showComfirm = true;
      //   return;
      // }
      http(checkUser, { uid: this.id }, json => {
        console.log(this.id);
        if (json.code === 0) {
          this.isEnter = false;
          this.$router.push({
            name: "Verification",
            params: { uid: this.id }
          });
        }
      });
    }
  }
};
</script>

<style lang="less" scoped>
.forgotWrap {
  .innerWrap {
    width: 100vw;
    padding-bottom: 300px;
    overflow-y: scroll;
    height: calc(100vh - 260px);
    background-color: #ebeaf0;
    border-radius: 40px 40px 0 0;
    margin-top: -20px;
    padding-top: 90px;
    ul {
      width: 90%;
      margin: 0 auto;
      li {
        .title {
          color: #333;
          font-size: 42px;
          margin: 42px 0;
          font-weight: 800;
          letter-spacing: 10px;
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
            width: 50%;
            box-sizing: border-box;
            height: 110px;
            border-radius: 0;
          }
          img {
            display: inline-block;
            width: 49%;
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
    font-size: 50px;
    color: #fff;
    margin-top: 100px;
    position: relative;
    font-weight: 800;
    letter-spacing: 10px;
    a {
      color: #fff;
      letter-spacing: 30px;
    }
  }
}
</style>
