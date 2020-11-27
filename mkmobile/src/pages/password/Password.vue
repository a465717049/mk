<template>
  <div class="passwordWrap">
    <TopBar  class="center-one-search" :option="topBarOption" >
    重置密碼
    </TopBar>
    <div class="innerWrap">
      <ul>
        <li>
          <div class="photoWrap">
            <img class="photo" src="../../assets/imgs/forgot1-img1.png" alt />
            <span>
              提示：
              <br />密碼最少6位,必須包含大小寫和特殊字符。
            </span>
          </div>
        </li>
        <li>
          <div class="title">选择重置类型</div>
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
          <div class="title">{{DataQuestion}}</div>
          <input type="text" hidden v-model="form.qid" />
          <input type="text" v-model="form.qanswer" />
        </li>
        <li>
          <div class="title">身份證號碼</div>
          <input type="text" v-model="form.idcard" />
        </li>
      </ul>

      <button class="submit" @click="goNext">確認</button>
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

import TopBar from 'components/TopBar'
import YellowComfirm from 'components/YellowComfirm'
import { SetUpdatePassword, GetMyQuestion } from 'util/netApi'
import { http } from 'util/request'
import { storage } from 'util/storage'
import { accessToken, loginPro } from 'util/const.js'

export default {
  components: {
    TopBar,
    YellowComfirm
  },
  data() {
    return {
      form: {
        type: 0,
        password: '',
        new_password: '',
        qid: 0,
        qanswer: '',
        idcard: ''
      },
      DataQuestion: '問題',
      showChat: true,
      topBarOption: {
        iconLeft: 'back'
      },
      isEnter: false,
      initData: {
        password: '',
        newPassword: '',
        comfirmPassword: '',
        farterName: '',
        codoNumber: ''
      },
      tips: '',
      tipsObj: {
        nosucceed: '修改失敗',
        succeed: '修改成功'
      },
      option1: [
        { text: '登錄密碼', value: 0 },
        { text: '交易密碼', value: 1 }
      ]
    }
  },
  mounted() {this.ToGetMyQuestion()},
  methods: {
    goNext() {
      http(SetUpdatePassword, this.form, json => {
        //console.log(json)
        if (json.code === 0) {
          this.isEnter = true
          this.tips = this.tipsObj.succeed
        } else {
         
           this.isEnter = true
          if (!json.success) {
            this.tips = json.msg
          } else {
            this.tips = this.tipsObj.nosucceed
          }
        }
      })
    },
    ToGetMyQuestion() {
      http(GetMyQuestion, null, json => {
        if (json.code === 0) {
          //farterName   codeNumber
          this.DataQuestion = json.response.question
          this.form.qid = json.response.qId
        }
      })
    },
    clickOk() {
      this.isEnter = false
    },
    changeModel(v) {
      this.isEnter = v;
    },loadQuestion(){
        http(loadQuestion, {}, json => {
        
        if (json.code === 0) {
          this.isEnter = true;
          this.form.qId = this.response.qID;
          this.initData.Question = this.response.Question_CN;
        } 
      });
      this.isEnter = v
    }
  },
  created() {
    
  }
}
</script>

<style lang="less" scoped>
.passwordWrap {
  .innerWrap {
    width: 100vw;
    height: calc(100vh - 300px);
    overflow: auto;
    background-color: #ebeaf0;
    border-radius: 40px 40px 0 0;
    margin-top: -20px;
    padding: 40px 0 240px;
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
    margin: 0 auto;
    background: #f5c148;
    border-radius: 40px;
    height: 164px;
    line-height: 164px;
    font-size: 60px;
    letter-spacing: 10px;
    color: #fff;
    margin-top: 100px;
    position: relative;
  }
}
</style>
