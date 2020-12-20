<template>
  <div class="openShopWrapper">
    <TopBar class="center-one-search">申请开店</TopBar>
    <div class="innerWrap">
      <div class="tips base-flex flex-start p-58 bg-white borderR mb-80">
        <img src="@/assets/imgs/login/head.png" class="img" alt />
        <div class="tips-part">
          <div class="tip-titl">提示</div>
          <div>不能重复申请，提交申请后会有专人与您联系！</div>
          <!-- <div>即將更新！</div> -->
        </div>
      </div>
      <ul>
       <li>
          <div class="title">ID</div>
          <input type="text"  v-model="initData.uid" readonly/>
          <i class="iconfont iconlock" ></i>
        </li>
        <li>
          <div class="title">昵称</div>
          <input type="text" v-model="initData.nickname" readonly/>
          <i class="iconfont iconlock"></i>
        </li>
        <li>
          <div class="title">姓名</div>
          <input type="text" v-model="initData.username" />
        </li>
        <li>
          <div class="title">联系电话：</div>
          <input type="text" v-model="initData.userphone" />
        </li>

      </ul>
      <button class="next" @click="goCheckData">立即申请</button>
    </div>
    <YellowComfirm
      :show="showComfirm"
      :tipTitle="tips"
      @clickOk="clickOk"
      @changeModel="changeModel"
    ></YellowComfirm>
  </div>
</template>
<script type="text/javascript">
import TabBar from 'components/TabBar'
import TopBar from 'components/TopBar'
import headerImg from '../../assets/imgs/headerImg.png'
import YellowComfirm from 'components/YellowComfirm'
import { http } from 'util/request'
import { CheckUpwd,ApplyOpenShop ,checkUser,GetUserInfo} from 'util/netApi'
import { storage } from 'util/storage'
import { accessToken, loginPro } from 'util/const.js'
export default {
  data () {
    var addmodel
    return {
      showComfirm: false,
      tips: '',
      tipsObj: {
      },
      topBarOption: {
        iconLeft: 'back',
        iconRight: ''
      },
      //uid nickname username username
      initData: {
        uid: 0,
        nickname: '',
        username: '',
        username: '',
      }
    }
  },
  components: {
    TabBar,
    TopBar,
    YellowComfirm
  },
  mounted () {},
  computed: {},
  methods: {
    clickOk () {
      this.showComfirm = false
    },
    changeModel (v) {
      this.showComfirm = v
    },
    goCheckData () { 
      if (!this.initData.nickname) {
        this.showComfirm = true
        this.tips ='请输入昵称'
        return
      }

      if (!this.initData.username) {
        this.showComfirm = true
        this.tips ='请输入姓名'
        return
      }

      if (!this.initData.userphone) {
        this.showComfirm = true
        this.tips ='请输入联系电话'
        return
      }
      http(
        ApplyOpenShop,
        {
          uid: this.initData.uid,
          nickname: this.initData.nickname,
          username: this.initData.username,
          userphone: this.initData.userphone
        },
        json => {
          if (json.code === 0) {
            this.showComfirm = true
            this.tips =  json.msg
          } else {
            this.showComfirm = true
            this.tips = json.msg
          }
        }
      )
      // this.$router.push({ name: 'CheckData' })
    }
  },
  created () {
     http(GetUserInfo, null, json => {
        if (json.code === 0) {
          this.initData.uid = json.response.uid
          this.initData.nickname = json.response.nickname
        }
      })
    
  }
}
</script>

<style lang="less" scoped>
.openShopWrapper {
  .innerWrap {
    width: 100vw;
    border-radius: 40px 40px 0 0;
    height: calc(100vh - 300px);
    overflow: auto;
    margin-top: -20px;
    padding-top: 30px;
    padding-bottom: 200px;
    .tips-part {
      font-weight: bold;
      color: rgba(52, 52, 52, 1);
      div {
        font-size: 40px;
        line-height: 60px;
      }
    }
    .tips {
      width: 90%;
      min-height: 158px;
      align-items: center;
      padding: 30px;
      margin: 0 auto;
      box-shadow: 0px 5px 5px 0px rgba(0, 0, 0, 0.24);
     .img {
        width: 122px;
        height: 128px;
        margin-right: 70px;
      }
    }
  }
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
        padding-right: 100px;
      }
      .iconfont {
        position: absolute;
        font-size: 60px;
        color: #4493d5;
        top: 100px;
        right: 20px;
      }
      /deep/ .van-dropdown-menu__bar {
        height: 130px;
        line-height: 130px;
        color: #9e9e9f;
        font-weight: 600;
        width: 100%;
        padding: 0 20px;
        border-radius: 20px;
        letter-spacing: 10px;
      }
      /deep/ .van-ellipsis {
        font-size: 60px;
        color: #9e9e9f;
        font-weight: 600;
        letter-spacing: 10px;
      }
      /deep/ .van-dropdown-menu__title {
        height: 130px;
        line-height: 130px;
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
        font-size: 50px;
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
        // padding: 20px 0;
        padding-left: 20px;
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
            border-color: #ccc;
            display: flex;
            justify-content: center;
            align-items: center;
            background: #fff;
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
}
</style>
