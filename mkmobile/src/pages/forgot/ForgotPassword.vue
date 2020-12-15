<template>
  <div class="forgotWrap">
    <!-- <TopBar class="center-one-search" :option="topBarOption">
      <div>Forgot Password</div>
    </TopBar>-->

    <div class="innerWrap">
      <img :src="forgotImg" alt class="head" />
      <h6 class="forgotTitle">重置您的密码</h6>
      <ul>
        <li> 
          <div class="title">请输入您的ID</div>
          <input type="text" v-model="id" />
        </li>
        <li>
          <div class="title">重置密码的类型</div>
          <van-dropdown-menu>
            <van-dropdown-item v-model="passwordType" :options="passwordTypeList" />
          </van-dropdown-menu>
        </li>
        <li>
          <div class="title">输入验证码</div>
          <div class="verification">
            <input type="text" v-model="verification" />
            <img :src="image" @click="getVerificationCode" alt />
          </div>
        </li>
        <!-- <li>
          <div class="photoWrap">
            <img class="photo" src="../../assets/imgs/forgot1-img1.png" alt />
            <span>
              溫馨提示：重置登錄密碼
              <br />下一步將驗證您的預留信息。
            </span>
          </div>
        </li>-->
        <li class="tipWrap">
          <div class="tip">提示：</div>
          <div class="tip">下一步将验证您预留的信息。</div>
        </li>
        <li>
          <button class="next" @click="gox">下一步</button>
        </li>
      </ul>
      <div class="bottom-part">
        <div class="version1">MOKI MONKEY 摩奇猴</div>
        <div class="version2">O2 MONSTER CO.,LTD. COPYRIGHT 2020 © ALL RIGHT RESERVED</div>
      </div>
    </div>
    <YellowComfirm
      :show="showComfirm"
      :tipTitle="tips"
      @clickOk="clickOk"
      @changeModel="changeModel"
    ></YellowComfirm>
  </div>
</template>

<script>
import YellowComfirm from 'components/YellowComfirm'
import TopBar from 'components/TopBar'
import { storage } from 'util/storage'
import { http } from 'util/request'
import { getVerificationCode, checkUser } from 'util/netApi'
import forgot from '@/assets/imgs/login/head.png'
export default {
  components: {
    TopBar,
    YellowComfirm
  },
  data () {
    return {
      topBarOption: {
        iconLeft: 'back',
        iconRight: ''
      },
      passwordType: 0,
      id: null,
      verification: null,
      code: null,
      image: 'https://ss2.bdstatic.com/70cFvnSh_Q1YnxGkpoWK1HF6hhy/it/u=3649178992,1821853682&fm=26&gp=0.jpg',
      showComfirm: false,
      forgotImg: forgot,
      //  receiptId: null,
      //  transPassword: null,
      //  verificationCode: null,
      tips: '',
      passwordTypeList: [
        { text: '登录密码', value: 0 },
        { text: "交易密码", value: 1 }
      ]
    }
  },
  mounted () {
    this.getVerificationCode();
  },
  methods: {
    clickOk () {
      this.showComfirm = false
    },
    changeModel (v) {
      this.showComfirm = v
    },
    getVerificationCode () {
      http(getVerificationCode, null, json => {
        if (json.code === 0) {
          console.log(json.response.vcode)
          let img = json.response.vcode
          var tem1 = img.substring(0, img.length - 21)
          var tem2 = img.substring(img.length - 16)
          this.code = img.substring(img.length - 21, img.length - 16)
          this.image = 'data:image/png;base64,' + tem1 + tem2
        }
      })
    },
    gox () {
      //id  passwordType verification

      if(!this.id)
      {
        this.showComfirm=true;
        this.tips="请输入您的ID";
        return;
      }

      if(!this.verification)
      {
        this.showComfirm=true;
        this.tips="请输入验证码";
        return;
      }

      // if (this.verification.toLowerCase() != this.code.toLowerCase()) {
      //   this.tips = "验证码错误！请确认验证码是否正确！";
      //   this.showComfirm = true;
      //   return;
      // }

  try
  {
  http(checkUser, { uid: this.id }, json => {
        console.log(this.id)
        if (json.code === 0) {
          this.$router.push({
            name: 'Verification',
            params: { uid: this.id }
          })
        }else
        {
          this.showComfirm = true;
          this.tips="不存在该ID请重新输入";
          this.id="";
        }
      })

  }catch(err)
  {
     this.showComfirm = true;
          this.tips="不存在该ID请重新输入";
          this.id="";
  }
      
   
    }
  }
}
</script>

<style lang="less" scoped>
.forgotWrap {
  .forgotTitle {
    width: 100%;
    text-align: center;
    margin-bottom: 60px;
    font-size: 70px;
    color: #000403;
    font-weight: bolder;
    z-index: 2;
    margin-top: 160px;
    position: relative;
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
    // overflow-y: scroll;
    // height: calc(100vh);
    background: #efb618 url("../../assets/imgs/login/ybj.png") no-repeat left
      top / 100%;
    padding-top: 450px;
    background-position-y: -300px;
    .head {
      display: block;
      width: 360px;
      height: 360px;
      position: absolute;
      top: 80px;
      left: calc(50vw - 185px);
    }
    ul {
      width: 90%;
      margin: 0 auto;
      z-index: 2;
      li {
        padding-top: 20px;
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
          height: 139px;
          line-height: 139px;
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
          height: 138px;
          line-height: 138px;
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
          color: #efb618;
        }
        /deep/ .van-icon-success::before {
          color: #efb618;
        }

        .verification {
          display: flex;
          border-radius: 20px;
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
            box-shadow: none;
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
    background: #efb618;
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
    bottom: 190px;
    width: 100%;
    left: 50%;
    transform: translate(-50%);
    font-weight: bold;
  }
}
</style>
