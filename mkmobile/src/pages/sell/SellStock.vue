<template>
  <div class="sellEpWrapper">
    <TopBar class="center-one-search" >
          出售股票
    </TopBar>
    <div class="innerWrap">
      <div class="moneyWrap clearfix">
        <div class="left fl">
          <div class="top">STOCK</div>
          <div class="bottom">ACCOUNT</div>
        </div>
        <div class="right fr">{{account}}</div>
      </div>
      <ul>
        <li>
          <div class="title">出售數量</div>
          <input type="text" v-model="form.num" />
        </li>
        <!-- <li>
          <div class="title">交易金额</div>
          <input type="text" v-model="transamount" />
        </li> -->
        <li>
          <div class="title">交易密碼</div>
          <input type="password" v-model="form.tpwd" />
        </li>
        <li>
          <div class="title">谷歌驗證碼</div>
          <input type="text" v-model="form.gcode" />
        </li>
      </ul>
      <button class="next" @click="ToSellStock">確認出售</button>
    </div>
    <YellowComfirm :show="showComfirm"  @clickOver="clickOverpay" :tipTitle="tips" @clickOk="clickOk()" @changeModel="changeModel" ></YellowComfirm>
  </div>
</template>
<script type="text/javascript">
import YellowComfirm from 'components/YellowComfirm'
import { http } from 'util/request'
import { SellStock, GetUserInfo } from 'util/netApi'
import { storage } from 'util/storage'
import { accessToken, loginPro } from 'util/const.js'
import TopBar from 'components/TopBar'
import headerImg from '../../assets/imgs/headerImg.png'
export default {
  data() {
    return {
      form: {
        num: null,
        tpwd: '',
        gcode: ''
      },
      topBarOption: {
        iconLeft: 'back',
        iconRight: '',
      //  image: headerImg
      },
      account: null,
      sellNumber: null,
      transamount: '$',
      transPassword: null,
      showComfirm: false,
      verificationCode: null,
      tips: '',
      tipsObj: {
        nosucceed: '出售失敗',
        nonum: '請輸入出售數量',
        nopwd: '請輸入交易密碼',
        nocode: '請輸入谷歌驗證碼',
        succeed: '出售成功'
      }
    }
  },
  components: {
    TopBar,
    YellowComfirm
  },
  mounted() {},
  computed: {},
  methods: {
    clickOk() {
      this.showComfirm = false
    },
    changeModel(v){
      this.showComfirm = v 
    },
    TogetUserInfo() {
      http(GetUserInfo, null, json => {
        console.log(json)
        if (json.code === 0) {
          this.account = json.response.apple
        }
      })
    },
    ToSellStock() {
      if (this.form.num === 0) {
        this.showComfirm = true
        this.tips = this.tipsObj.nonum
        return
      }

      if (!this.form.tpwd) {
        this.showComfirm = true
        this.tips = this.tipsObj.nopwd
        return
      }

      if (!this.form.gcode) {
        this.showComfirm = true
        this.tips = this.tipsObj.nocode
        return
      }

      let _this = this
      http(SellStock, this.form, json => {
        if (json.code === 0) {
          this.showComfirm = true
          this.tips = this.tipsObj.succeed
        } else {
          this.showComfirm = true
          if (!json.success) {
            this.tips = json.msg
          } else {
            this.tips = this.tipsObj.nosucceed
          }
        }
      })
    }
  },
  created() {
    this.TogetUserInfo()
  }
}
</script>

<style lang="less" scoped>
.sellEpWrapper {
  .innerWrap {
    width: 100vw;
    padding-bottom: 300px;
    overflow-y: scroll;
    height: calc(100vh - 260px);
    border-radius: 40px 40px 0 0;
    margin-top: -20px;
    padding-top: 90px;
  }
  ul {
    width: 90%;
    margin: 0 auto;
    li {
      .title {
        color: #FFFFFF;
        font-size: 42px;
        margin: 60px 0 22px;
        font-weight: 800;
        letter-spacing: 10px;
        padding-left: 20px;
      }
      input {
        height: 130px;
        line-height: 130px;
        color: #9E9E9F;
        width: 100%;
        padding: 0 30px;
        border-radius: 20px;
        font-size: 60px;
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
      color: #113D79;
    }
  }
}
</style>
