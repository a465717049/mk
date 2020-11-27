<template>
  <div class="login-page">
    <div class="top-part">
      <div class="img-part">
        <div class="circle"></div>
        <div class="quer"></div>
        <div class="small-quer"></div>
        <div class="left-quer"></div>
        <div class="welcome"></div>
        <div class="bottom-s-circle"></div>
        <!-- <div class="bottom-circle"></div>-->
        <!-- <div class="right-circle"></div> -->
      </div>
      <div class="label">
        <div class="label-info">
          <span class="title p-color">用戶ID</span>
          <input class="input"
                 @focus="onFocus"
                 v-model="form.name"
                 type="text" maxlength='18' />
          <van-dropdown-menu>
            <van-dropdown-item v-model="value"
                                @change="dealChange()"
                               :options="option" />
          </van-dropdown-menu>
        </div>
        <div class="label-info">
          <span class="title p-color">密碼</span>
          <i class="iconfont iconeye eye"
             @click="showP=!showP"></i>
          <input class="input"
                 v-if="!showP"
                 @focus="onFocus"
                 v-model="form.pass"
                 type="password" maxlength='20' />
          <input class="input"
                 v-if="showP"
                 @focus="onFocus"
                 v-model="form.pass"
                 type="text" maxlength='20' />
        </div>
        <div class="save">
          <van-field name="checkbox">
            <template #input>
              <van-checkbox v-model="ifSave"
                            shape="square">記住密碼</van-checkbox>
            </template>
          </van-field>
        </div>
        <div class="secret">
          <router-link class="router"
                       to="forgot">忘記密碼？</router-link>
        </div>
        <van-button type="primary"
                    @click="login"
                    class="submit-btn">登 錄</van-button>
      </div>
    </div>

    <div class="bottom-part">
      <div class="version">Ver 4.1.0</div>
    </div>
    <YellowComfirm :show="showComfirm"
     @clickOver="clickOverpay"
                   :tipTitle="tips"
                   @clickOk="clickOk() "
                   @changeModel="changeModel"></YellowComfirm>
  </div>
</template>
<script>
import YellowComfirm from 'components/YellowComfirm'
import { http } from 'util/request'
import { getLogin } from 'util/netApi'
import { storage } from 'util/storage'
import { accessToken, loginPro } from 'util/const.js'
export default {
  components: {
    YellowComfirm
  },
  data () {
    return {
      form: {
        name: '',
        pass: ''
      },
      value: 0,
      ifSave: false,//是否保存密码
      showP: false,
      showComfirm: false,
      tips: '',
      redirect: null,
      tipsObj: {
        noId: '用戶ID沒有輸入',
        nolong: '用戶ID為5-18位',
        noP: '密碼沒有輸入',
        noRegister: '用戶ID不存在',
        errorP: '登錄密碼不正確'
      },
      option: [
        {text:"全部清除",value:""}
      ],
      clientWidth: document.documentElement.clientWidth
    }
  },
 
  methods: {
    login () {
      if (!this.form.name) {
        this.showComfirm = true
        this.tips = this.tipsObj.noId
        return
      }
       if (this.form.name.length<5||this.form.name.length>18) {
        this.showComfirm = true
        this.tips = this.tipsObj.nolong
        return
      }
      if (!this.form.pass) {
        this.showComfirm = true
        this.tips = this.tipsObj.noP
        return
      }
      let _this = this
      http(getLogin, this.form, json => {
        if (json.code === 0) {
          storage.setLocalStorage(accessToken, 'Bearer ' + json.response.token)
          if(_this.ifSave){
            let str='{"name":"'+_this.form.name+'","pass":"'+_this.form.name+':'+_this.form.pass+'"}';
            let account=storage.getLocalStorage("accountList");
            if(!account)
            {
                account='[]'
            }
            let list = JSON.parse(account)
            let add=false;
            list.forEach((item,index) => {
              if(item.name==_this.form.name)
                {
                  list.splice(index, 1); 
                  list.push(JSON.parse(str))
                  add=true;
                }
            });
            if(!add){
              list.push(JSON.parse(str))
            }
            storage.setLocalStorage("accountList",JSON.stringify(list) )
          }
          if (_this.redirect) {
            _this.$router.push(_this.redirect)
            storage.delLocalStorage(loginPro)
          } else {
            _this.$router.push({ name: 'Home',params:{name:this.form.name} })
          }
        } else {
          this.showComfirm = true
          this.tips = json.msg==''?this.tipsObj.noRegister:json.msg
        }
      })
    },
    loadstorage(){
         let account=storage.getLocalStorage("accountList");
        
           if(!account)
          {
              account='[]'
          }
         let list = JSON.parse(account)
        var that=this;
         that.option=[]
         list.forEach(element => {
           that.option.push({text:element.name,value:element.pass})
         });
         if( that.option.length>0){
          that.option.push({text:"全部清除",value:""})
         }

    },
    clickOk () {
      this.showComfirm = false
    },
    changeModel(v){
      this.showComfirm = v 
    },
    onFocus () { },
    dealChange(index){
     let val = this.option.filter(item => item.value===this.value)[0].value
     if(val=="")
     {
        storage.delLocalStorage("accountList")
     }else{
       console.log(val)
       let list= val.split(':')
       this.form.name =list[0]
       this.form.pass =list[1]
    }
    }
  }, created () {
    this.loadstorage()
  },mounted () {
      this.redirect = storage.getLocalStorage(loginPro)
       storage.delLocalStorage(accessToken)
    }
}
</script>
<style lang="less" scoped>
@keyframes myfirst {
  from {
    transform: rotate(0deg);
  }
  to {
    transform: rotate(360deg);
  }
}
.login-page {
  width: 100%;
  height: 100%;
  overflow-y: auto;
  position: relative;
  background-color: #6e21d1;
  min-height: 1900px;
  .top-part {
    height: 75%;
    position: relative;
    background: #6e21d1 url('../../assets/imgs/login/bg@2x.png') no-repeat left
      top / 100% 80%;
    .img-part {
      height: 50%;
    }
    .circle {
      position: absolute;
      top: 219px;
      left: 144px;
      width: 230px;
      height: 228px;
      z-index: 99;
      background: url('../../assets/imgs/login/circle1.png') no-repeat center
        center / 100% 100%;
      // animation: myfirst 3s 1s infinite;
    }
    .quer {
      position: absolute;
      top: 257px;
      left: 830px;
      width: 106px;
      height: 106px;
      background: url('../../assets/imgs/login/squr.png') no-repeat center
        center / 100% 100%;
      animation: myfirst 2s 3s infinite;
    }
    .small-quer {
      position: absolute;
      top: 515px;
      left: 813px;
      width: 61px;
      height: 61px;
      background: url('../../assets/imgs/login/small-squr.png') no-repeat center
        center / 100% 100%;
      animation: myfirst 2s 4s infinite;
    }
    .left-quer {
      position: absolute;
      top: 653px;
      left: 132px;
      width: 89px;
      height: 89px;
      background: url('../../assets/imgs/login/left-squr.png') no-repeat center
        center / 100% 100%;
      animation: myfirst 3s 2s infinite;
    }
    .right-circle {
      position: absolute;
      top: 664px;
      left: 873px;
      width: 101px;
      height: 55px;
      background: url('../../assets/imgs/login/right-circle.png') no-repeat
        center center / 100% 100%;
      animation: myfirst 4s 2s infinite;
    }
    .bottom-circle {
      position: absolute;
      top: 891px;
      left: 107px;
      width: 140px;
      height: 180px;
      background: url('../../assets/imgs/login/circle.png') no-repeat center
        center / 100% 100%;
      animation: myfirst 4s 6s infinite;
    }
    .bottom-s-circle {
      position: absolute;
      top: 664px;
      left: 873px;
      width: 101px;
      height: 55px;
      background: url('../../assets/imgs/login/bottom-s-circle.png') no-repeat
        center center / 100% 100%;
      animation: myfirst 3s 4s infinite;
    }
    .welcome {
      position: absolute;
      top: 405px;
      left: 325px;
      width: 480px;
      height: 260px;
      background: url('../../assets/imgs/login/Welcome.png') no-repeat center
        center / 100% 100%;
    }
    .label {
      width: 90%;
      margin: 0 auto;
      font-size: 40px;
      position: relative;
      color: #ffffff;
      .label-info {
        margin-top: 20px;
        display: flex;
        flex-direction: column;
        justify-content: flex-start;
        position: relative;
        border-bottom: 5px solid rgba(225, 225, 225, 0.6);
        .eye {
          position: absolute;
          right: 0px;
          top: 50px;
          font-size: 60px;
          color: rgba(225, 225, 225, 0.6);
        }
        .input {
          height: 106px;
          border: 0;
          font-size: 60px;
          color: #ffffff;
          background: rgba(110, 33, 209, 0.1);
          width: 95%;
        }
        .title {
          font-size: 42px;
        }
        .icon-down {
          transform: rotate(90deg);
          position: absolute;
          right: 0px;
          top: 80px;
          font-size: 60px;
          opacity: 0.7;
        }
      }
      .secret {
        opacity: 0.5;
        float: right;
        font-size: 40px;
        margin-top: 30px;
        .router {
          color: #ccc!important;
        }
      }
      .save {
        opacity: 0.5;
        float: left;
        font-size: 40px;
        margin-top: 30px;
        .router {
          color: #ccc!important;
        }
        /deep/.van-field__label {
          text-align: right;
          font-size: 40px;
          color: white;
        }
        /deep/.van-checkbox__label {
          font-size: 40px;
          color: white;
          border-radius: 10px;
          margin-left: 15px;
        }
        /deep/.van-cell {
          padding-left: 0;
          height: 100px;
        }
        /deep/.van-checkbox__icon {
          font-size: 42px;
        }
        /deep/.van-checkbox__icon--checked {
          .van-icon {
            color: #fff;
            background-color: #6e21d1 !important;
            border-color: #fff;
            opacity: 0.6;
          }
        }
        /deep/.van-cell {
          background: none;
        }
      }
      .submit-btn {
        background: #facc5d;
        margin-top: 60px;
        height: 160px;
        width: 100%;
        text-align: center;
        border-radius: 21px;
        font-size: 70px;
        color: #fff;
        font-weight: 900;
      }
      /deep/.van-dropdown-item--down {
        position: absolute;
        width: 100vw;
        height: 1000px;
        left: -960%;
        top: 116% !important;
      }
      // 下拉框
      /deep/.van-dropdown-menu {
        width: 10%;
        position: absolute;
        right: 0;
        height: 60px;
        line-height:60px;
        bottom: 20px;
      }
      /deep/.van-overlay {
        top: 28px;
      }
      /deep/.van-ellipsis {
        display: none;
      }
      /deep/ .van-dropdown-menu__bar {
        color: white;
        background: #6e21d1;
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
        top: 0px;
      }
      /deep/  .van-cell::after{
        left: 5%;
        width: 90%;
      }
      /deep/ .van-dropdown-item__option {
        height: 128px;
        line-height: 128px;
        color: #fff;
        padding: 0 60px;
        font-size: 42px;
        background-color: #6e21d1;
      }
      /deep/ .van-dropdown-item__option--active {
        color: #facc5d;
      }
      /deep/.van-dropdown-item__option--active{
        .van-dropdown-item__icon{
          color:#facc5d;
        }
      }
    }
  }

  .bottom-part {
    position: relative;
    height: 25%;
  }
  .version {
    color: #fff;
    font-size: 40px;
    margin-top: 40px;
    text-align: center;
    position: absolute;
    bottom: 50px;
    left: 50%;
    transform: translate(-50%);
  }
  /deep/.van-button--primary {
    border: none;
  }
}
</style>
