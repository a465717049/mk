<template>
  <div class="bankWrapper">
    <TopBar class="center-one-search">银行卡设置</TopBar>
    <div class="innerWrap">
      <div class="tips base-flex flex-start p-58 bg-white borderR mb-80">
        <img src="@/assets/imgs/tipimg.png" class="img" alt />
        <div class="tips-part">
          <div class="tip-titl">提示</div>
          <div>一经设置后个人不能修改。</div>
        </div>
      </div>
      <ul class="selectUl">
        <li>
          <div class="title">选择银行卡类型</div>
          <van-dropdown-menu>
            <van-dropdown-item v-model="initData.level" :options="option1" />
          </van-dropdown-menu>
        </li>
         <li v-if="initData.level==1">
          <div class="title">账号</div>
          <input type="text" v-model="initData.alipayaccount" />
          <i class="iconfont iconlock"></i>
        </li>
        <li v-if="initData.level==1">
          <div  class="title">姓名</div>
          <input type="text" v-model="initData.alipayname" />
          <i class="iconfont iconlock"></i>
        </li>
         <li v-if="initData.level==2">
          <div  class="title">开户行</div>
          <input type="text" v-model="initData.bankaddr" />
          <i class="iconfont iconlock"></i>
        </li>
         <li v-if="initData.level==2">
          <div  class="title">卡号</div>
          <input type="text" v-model="initData.bankidcard" />
          <i class="iconfont iconlock"></i>
        </li>
          <li v-if="initData.level==2">
          <div  class="title">姓名</div>
          <input type="text" v-model="initData.bankname" />
          <i class="iconfont iconlock"></i>
        </li>

         <li v-if="initData.level==3">
          <div  class="title">地址</div>
          <input type="text" v-model="initData.addr" />
          <i class="iconfont iconlock"></i>
        </li>
      </ul>
      <button class="next" @click="goEditData">提交</button>
      <div class="infowrap">
        <div class="title">您的银行卡信息：</div>
        <ul>
          <li>
            <span class="lable">支付宝:</span>
            <span class="info">{{initData.alipayaccount}}  {{initData.alipayname}}</span>
          </li>
          <li>
            <span class="lable">usdt:</span>
            <span class="info">{{initData.addr}}</span>
          </li>
          <li>
            <span class="lable">银联卡:</span>
            <span class="info">{{initData.bankidcard}} {{initData.bankaddr}} :</span>
            <span class="info">{{initData.bankname}} </span>
          </li>
        </ul>
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
<script type="text/javascript">
import TabBar from 'components/TabBar'
import TopBar from 'components/TopBar'
import { http } from 'util/request'
import {  GetUserInfo,Getupdatebankinfo} from 'util/netApi'
import { storage } from 'util/storage'
import YellowComfirm from 'components/YellowComfirm'
export default {
  data () {
    return {
      showComfirm: false,
      tips: '',
      tipsObj: {
        nomember: '請輸入账号！',
        nonickname: '請輸入昵稱！',
        nolevel: '請選擇級別！',
        nopwd: '請輸入密碼！',
        norpwd: '請輸入重複密碼',
        nopwdreset: '兩次密碼不一致請重新確認！'
      },
      initData: {
        addr:'',
        bankaddr:'',
        bankname:'',
        bankidcard:'',
        alipayaccount:'',
        alipayname:'',
        Jid: 0,
        code: '',
        nickName: '',
        level: 0,
        password: '',
        comfirmPassword: '',
        radioValue: '1',
        parentID: 0,
        L: 0
      },
      option1: [
       
      ]
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
    TogetUserInfo () {
      http(GetUserInfo, null, json => {
        if (json.code === 0) {
          var reloadaary=[];
            if(!json.response.alipayaccount)
            {
               reloadaary.push({text: '支付宝', value: 1})
            }else
            {
              this.initData.alipayaccount=json.response.alipayaccount;
              this.initData.alipayname=json.response.alipayname;
            }

             if(!json.response.bankidcard)
            {
               reloadaary.push({text: '银行卡', value: 2})
            }else
            {
              this.initData.bankaddr=json.response.bankaddr;
              this.initData.bankname=json.response.bankname;
               this.initData.bankidcard=json.response.bankidcard;
            }

             if(!json.response.coin_location)
            {
               reloadaary.push({text: 'USDT', value: 3})
            }else
            {
              this.initData.addr=json.response.coin_location;
            }
            this.option1=reloadaary;
        }
      })
    },
    goEditData ()
     {
       var data={};
       if(this.initData.level==1)
       {
         if(!this.initData.alipayname)
         {
           this.showComfirm = true;
           this.tips="请输入支付宝名字"
           return;
         }
          if(!this.initData.alipayaccount)
         {
           this.showComfirm = true;
           this.tips="请输入支付宝账号"
           return;
         }
          data={
            type:this.initData.level,
            alipayname:this.initData.alipayname,
            alipayaccount:this.initData.alipayaccount}
       }else if (this.initData.level==2)
        {
           if(!this.initData.bankname)
         {
           this.showComfirm = true;
           this.tips="请输入银行卡姓名"
           return;
         }
          if(!this.initData.bankaddr)
         {
           this.showComfirm = true;
           this.tips="请输入银行卡地址"
           return;
         }
          if(!this.initData.bankidcard)
         {
           this.showComfirm = true;
           this.tips="请输入银行卡卡号"
           return;
         }
        data={
        type:this.initData.level,
        bankname:this.initData.bankname, 
        bankaddr:this.initData.bankaddr,
        bankidcard:this.initData.bankidcard}
       }else if(this.initData.level==3)
       {
            if(!this.initData.addr)
         {
           this.showComfirm = true;
           this.tips="请输入usdt地址"
           return;
         }
        data={  type:this.initData.level,addr:this.initData.addr}
       }
       http(
        Getupdatebankinfo,
        data,
        json => {
          if (json.code === 0) {
             this.tips='更新成功!'
             this.showComfirm=true
          }else
          {
             this.tips='更新失败!'
             this.showComfirm=true
          }
        }
      )

     }
  },
  created () {
    this.TogetUserInfo()
  
  }
}
</script>

<style lang="less" scoped>
.bankWrapper {
  .innerWrap {
    width: 100vw;
    border-radius: 40px 40px 0 0;
    margin-top: -20px;
    padding-top: 30px;
    padding-bottom: 300px;
    height: calc(100vh - 300px);
    overflow: auto;
    // background-color: #4678bc;
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
        width: 148px;
        height: 115px;
        margin-right: 70px;
      }
    }
  }
  .selectUl {
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
      .iconfont {
        position: absolute;
        font-size: 60px;
        color: #91a4dd;
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
        font-size: 42px;
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
  .next {
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
    .jiantou {
      position: absolute;
      right: 30px;
      top: 0px;
      font-size: 60px;
    }
  }
  .disabled {
    background: #ccc;
    color: #666;
  }
  .infowrap {
    width: 90%;
    margin: 120px auto 0;
    .title {
      font-size: 41px;
      font-weight: bold;
      color: #ffffff;
      margin-bottom: 40px;
    }
    ul {
      background: #ffffff;
      box-shadow: 0px 5px 5px 0px rgba(0, 0, 0, 0.24);
      border-radius: 34px;
      padding: 50px 40px;
    }
    li {
      font-size: 37px;
      font-family: STHeiti;
      font-weight: bold;
      color: #767c8f;
      opacity: 0.84;
      height: 60px;
      line-height: 60px;
      display: flex;
      .lable{
        width: 17%;
      }
    }
  }
}
</style>
