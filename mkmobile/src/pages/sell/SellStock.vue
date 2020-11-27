<template>
  <div class="sellEpWrapper">
    <TopBar class="center-one-search" :option="topBarOption">
          出售股票
    </TopBar>
    <div class="innerWrap">
       <div class="accountWrap clearfix " @click="showFamily">
        <div class="left fl">
          <div class="top">賬號</div>
        </div>
        <div class="right fr">{{uid}}({{name}})
          <i data-v-6e4a7acf="" data-v-0c59bf64="" class="van-icon van-icon-arrow van-cell__right-icon"><!----></i>
        </div>
      </div>
     
      <div class="moneyWrap clearfix">
        <div class="left fl">
          <div class="top">STOCK</div>
          <div class="bottom">數量</div>
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

      <van-popup v-model="modelShow" :close-on-click-overlay="false"  class="wrap">
      <div class="brown-border">
          <div class="item">
            <h3 class="item-tit">請選擇出售賬號</h3>
            <div class="item-tip">
             <div class="friendsList borderR" ref="listWrap">
                <div class="list  borderR bg-gray p-38">
                  <div class="relative" v-for="(item,index) in familyList" :key="index">
                    <img :src="item.photo" class="img" alt />

                    <van-cell class="cell-info borderR mb-40" is-link @click="select(item.uID,item.NickName,item.DPE)">
                      <div slot="title" class="data-title">
                        <div class="left">{{item.NickName+'('+item.uID+')'}}</div>
                        <div class="right"> {{item.DPE}}</div>
                      </div>
                    </van-cell>
                  </div>
                </div>
              </div>
          </div>
        </div>
      </div>
    </van-popup>
  </div>
</template>
<script type="text/javascript">
import YellowComfirm from 'components/YellowComfirm'
import { http } from 'util/request'
import { SellStock, GetUserInfo,GetSearchFimaly } from 'util/netApi'
import { storage } from 'util/storage'

import { photoList } from "util/const.js";
import TopBar from 'components/TopBar'
export default {
  data() {
    return {
      form: {
        uid:0,
        num: null,
        tpwd: '',
        gcode: ''
      },
      topBarOption: {
        iconLeft: 'back',
        iconRight: '',
      //  image: headerImg
      },
      uid:0,
      name:"",
      familyList: [],
      modelShow:false,
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
          this.uid= json.response.uid
          this.name= json.response.nickname
        }
      })
    },
    loadinfo() {
      var _this = this;
      http(GetSearchFimaly, { uid: this.uid }, json => {
        if (json.response) {
          _this.familyList = json.response;
          _this.familyList.forEach(element => {
            element.photo = photoList[element.photo];
          });
        }
      });
    },
    showFamily(){
      this.modelShow=true;
    },
    select(id,name,dpe){
      this.uid=id
      this.name=name
      this.account=dpe
      this.modelShow=false;
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
       this.form.uid=this.uid
      let _this = this
      console.log(this.form.uid)
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
    this.loadinfo();
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
    background-color: #ebeaf0;
    border-radius: 40px 40px 0 0;
    margin-top: -20px;
    padding-top: 90px;
  }
  ul {
    width: 90%;
    margin: 0 auto;
    li {
      .title {
        color: #9d9d9f;
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
    border-radius: 40px;
    height: 164px;
    line-height: 164px;
    font-size: 42px;
    color: #fff;
    margin-top: 100px;
    position: relative;
    font-weight: 800;
    letter-spacing: 10px;
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
        color: #999;
        height: 140px;
        line-height: 140px;
      }
      .bottom {
        color: #999;
        font-size: 42px;

        height: 80px;
        line-height: 80px;
      }
    }
    .right {
      font-size: 104px;
      font-weight: 600;
      color: #6318c3;
    }
  }
    .accountWrap {
    height: 144px;
    width: 90%;
    background-color: #fff;
    margin: 0px auto 40px;
    border-radius: 40px;
    padding: 0 40px;
    .left {
      .top {
        font-size: 60px;
        font-weight: 600;
        color: #999;
        height: 100%;
        line-height:  144px;
      }
    }
    .right {
      font-size: 60px;
      font-weight: 600;
      color: #6318c3;
      height: 100%;
      line-height:  144px;
    }
  }
}
.wrap{
  width: 80%;
  border: #6318c3 1px solid;
  border-radius: 20px;
  .item-tit{
    text-align: center;
    margin-bottom: 40px;
    height: 120px;
    line-height: 120px;
    font-size: 800;
    font-weight: bolder;
    color: #ffffff;
    background-color: #6318c3;
  }
  .item-tip{
      position: relative;
   
    padding: 10px 60px;
  }
  .btn{
    text-align: center;
    color: #ffffff;
    border: #eca80a 1px solid;
    background-color: #eca80a;
    width: 280px;
    margin: 30px auto 30px;
    height: 120px;
    line-height: 120px;
     border-radius: 20px;

  }
}
.friendsList {
  margin-top:0px;
   background: #fff;
   overflow: auto;
      overflow: scroll;
       height: calc(100vh - 1000px);
  // border-radius: 42px;
  .list {
  
      background: #fff;
    // height: auto;
    // padding: 28px;
    // border-radius: 42px;
    .cell-info {
      line-height: 120px;
      background: #eee;

    }
    .img {
      position: absolute;
      left: 30px;
      top: 20px;
      width: 106px;
      z-index: 99;
    }
    /deep/.van-cell__right-icon {
      font-size: 60px;
      line-height: 120px;
    }
    /deep/.van-cell__title {
      font-size: 42px;
      margin-left: 140px;
      font-weight: bold;
      color: rgba(118, 124, 143, 1);
      position: relative;
    }
    /deep/.van-cell__value {
      font-size: 35px;
      color: rgba(118, 124, 143, 1);
    }
    /deep/.van-progress__pivot {
      display: none;
    }
    .cell-special {
      /deep/.van-cell__title {
        margin-left: 600px;
      }
    }
  }
}
.data-title {
  div.left {
    display: inline-flex;
    width: 58%;
    text-align: center;
  }
  div.right {
    display: inline-flex;
    width: 32%;
    text-align: center;
    margin-left: 40px;
  }
}
</style>
