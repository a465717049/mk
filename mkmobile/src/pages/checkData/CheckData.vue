<template>
  <div class="CheckDataWrapper">
    <TopBar class="center-one-search">注册成为会员</TopBar>
    <!-- <ScrollRefresh
      @getData="TogetUserInfo"
      :residualHeight="topbarHeight+bottomTabBarHeight+10"
      :isNeedUp="false"
      class="innerScroll"
    > -->
      <div class="innerWrap">
        <div class="tips base-flex flex-start p-58 bg-white borderR mb-80">
          <img src="@/assets/imgs/tipimg.png" class="img" alt />
          <div class="tips-part">
            <div class="tip-titl">提示</div>
            <div>EP可以提现，也可以兑换成为RP(注册分）</div>
            <!-- <div>即將更新！</div> -->
          </div>
        </div>
        <div class="moneyWrap clearfix">
          <div class="left fl">
            <div class="top">RP</div>
            <div class="bottom">Account</div>
          </div>
          <div class="right fr">{{ account }}</div>
        </div>
        <div class="checkTitle">等待注册会员信息：</div>
        <!-- <ul>
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
        </ul>-->
        <ul class="vipInfo">
          <li>呢称：{{initData.addname}}</li>
          <li>安置ID: {{initData.addpid}}</li>
          <li>接点ID: {{initData.addtid}}</li>
          <li>会员级别： {{nowvip}}</li>
        </ul>
        <div class="sumTitle">合计</div>
        <div class="sumInfo">
          <span class="tit">需扣除您的RP：</span>
          <span class="num">{{initData.addprice}}</span>
        </div>
        <div class="buttonWrap">
          <router-link to="joinUs" class="router">
          <button class="back" @click="goRetrun"><i class="iconfont iconfanhui"></i> 返回修改</button>
          </router-link>
          <button class="sure" @click="goNext">确认注册</button>
        </div>
        <!-- <button class="next" @click="goNext">确认提交</button> -->
      </div>
    <!-- </ScrollRefresh> -->
    <YellowComfirm :show="isEnter" :tipTitle="tips"  @clickNo="clickNo()" @clickOk="clickOk()"></YellowComfirm>
  </div>
</template>
<script type="text/javascript">
import TopBar from 'components/TopBar'
import headerImg from '../../assets/imgs/headerImg.png'
import YellowComfirm from 'components/YellowComfirm'
import ScrollRefresh from 'components/ScrollRefresh'
import { http } from 'util/request'
import { CreateNewAccount, GetUserInfo } from 'util/netApi'
import { storage } from 'util/storage'
import { accessToken, loginPro } from 'util/const.js'
export default {
  components: {
    TopBar,
    YellowComfirm,
    ScrollRefresh
  },
  data () {
    return {
      tips: '恭喜！注册成功了！<br/> 登录ID: 100012<br/>登录密码：123456<br/>交易密码：123456<br/>请尽快登录修改并完善个人资料',
      isEnter: false,
      account: '2,000',
      isreturn: 0,
      nowvip:"",
      addmodel: {},
      initData: {
        price: '',
        positionId: '',
        area: '',
        addname: '',
        addpid: 0,
        addtid: 0,
        addlevle: '',
        addprice: 0
      },
      option1: [
        { text: '1000', value: 1 },
        { text: '500', value: 2 }
      ]
    }
  },
  mounted () {},
  computed: {},
  methods: {
    clickNo()
    {
     if (this.isreturn == 1) {
        this.$router.push({
          name: 'friendsList',
          params: { uid: this.addmodel.Jid }
        })
      }  
    },
    goRetrun(){
          this.$router.push({
          name: 'joinUs'
        })
    },
    goNext () {
      if (this.account < this.addmodel.investmentAmount) {
        this.isEnter = true
        this.tips = '当前金额不足'
        return
      }
      http(
        CreateNewAccount,
        { jsondata: JSON.stringify(this.addmodel) },
        json => {
          if (json.code === 0) {
            var n = Number(json.msg)
          if (!isNaN(n)) {  // 数字
          // "恭喜！注册成功了！<br/> 登录ID: 100012<br/>登录密码：123456<br/>交易密码：123456<br/>请尽快登录修改并完善个人资料"
          this.isEnter = true
          this.tips = '恭喜！注册成功了！<br/> 登录ID:' + json.msg + '<br/>登录密码：' + this.addmodel.loginPass + '<br/>交易密码：' + this.addmodel.TradePass + '<br/>请尽快登录修改并完善个人资料'
          this.account = this.account - this.addmodel.investmentAmount
          this.isreturn = 1
          }
          else // 字符
          {
          this.isEnter = true
          this.tips=json.msg
          }           
          } else {
            this.isEnter = true
          }
        }
      )
    },
    changeModel (v) {
      this.isEnter = v
    },
    clickOk () {
      this.isEnter = false
      if (this.isreturn === 1) {
        this.$router.push({
          name: 'friendsList',
          params: { uid: this.addmodel.Jid }
        })
      }
    },
    TogetUserInfo () {
      http(GetUserInfo, null, json => {
        if (json.code === 0) {
          this.account = json.response.rp
        }
      })
    }
  },
  created () {
     
    this.TogetUserInfo()
    if (storage.getLocalStorage('joindata')) {
      this.addmodel = JSON.parse(storage.getLocalStorage('joindata'))
      console.log(this.addmodel)
      this.initData.price = this.addmodel.investmentAmount
      this.initData.addname = this.addmodel.NickName
      this.initData.addtid = this.addmodel.Jid
      this.initData.addpid = this.addmodel.parentID
      this.initData.addprice = this.addmodel.investmentAmount
      this.initData.addlevle = this.addmodel.levlename

         if (this.initData.addlevle == 666){ this.nowvip="初级会员";}
          if (this.initData.addlevle == 2000){ this.nowvip="中级会员";}
          if (this.initData.addlevle == 10000) {this.nowvip="高级会员";}
      if (this.addmodel.L == 0) {
        this.initData.area = '蔬菜區'
      } else {
        this.initData.area = '水果區'
      }
    }
    //this.isEnter = false
    
  }
}
</script>

<style lang="less" scoped>
// .CheckDataWrapper .innerScroll {
//   /deep/ .wrapper .bscroll-container {
//     min-height: calc(100vh - 420px);
//   }
// }
.CheckDataWrapper {
  // max-height: 100vh;
  // overflow: scroll;
  // padding-bottom: 130px;
  .innerWrap {
    width: 100vw;
    margin-top: 0px;
    padding-top: 30px;
    padding-bottom: 300px;
    height: calc(100vh - 260px);
    overflow-y: scroll;
  }
  .tips-part {
    font-weight: bold;
    color: rgba(52, 52, 52, 1);
      div{
          font-size: 40px;
          line-height: 60px;
       }
  }
  .tips {
    width: 90%;
    min-height: 158px;
    align-items: center;
    padding: 30px;
    margin: 0 auto 50px;
    box-shadow: 0px 5px 5px 0px rgba(0, 0, 0, 0.24);
    .img {
      width: 148px;
      height: 115px;
      margin-right: 70px;
    }
  }
  .checkTitle {
    font-size: 41px;
    font-weight: bold;
    color: #ffffff;
    height: 100px;
    line-height: 100px;
    margin-top: 80px;
    padding-left: 60px;
  }
  .sumTitle {
    font-size: 71px;
    font-weight: bold;
    color: #ffffff;
    margin-top: 120px;
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
  .vipInfo {
    border-radius: 27px;
    background: #ffffff;
    width: 90%;
    margin: 0 auto;
    padding: 60px 40px;
    li {
      font-size: 42px;
      line-height: 70px;
      font-weight: bold;
      color: #6f6d72;
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
      background-color: #0b41dc;
      position: relative;
      .iconfont{
        position: absolute;
        font-size: 64px;
        color: #fff;
        left: 40px;
      }
    }
    .sure {
      background-color: #eeb20a;
    }
  }
}
</style>
