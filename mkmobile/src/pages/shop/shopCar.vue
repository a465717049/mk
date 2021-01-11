<template>
  <div class="shopCarWrapper">
    <TopBar class="center-one-search" :option="topBarOption" :badge="carNum"
      >购物车</TopBar
    >
    <ScrollRefresh
      @getData="TogetUserInfo"
      :residualHeight="topbarHeight+bottomTabBarHeight+10"
      :isNeedUp="false"
      class="innerScroll"
    >
      <div class="innerWrap" >
        <div class="goods base-flex flex-start p-58 borderR mb-80"  v-for="(key,value) in data" :key="value">
          <img  :src="getimgurl(key.shopsku.detailicon)" class="img" alt />
          <div class="goods-info">
            <div class="tip-titl">{{ key.shopdetail.pName }} : {{key.shopsku.detaildesc}}</div>
            <div>价格：{{ key.shopsku.detailprice }}</div>
            <!-- <div>赠送：面膜+修复水</div>    v-model="stepper"  @plus="onPlus" @minus="onMinus" -->
          </div>
          <van-field name="stepper" class="font42">
            <template #input>
              <van-stepper
                v-model="key.shopnum"
                min="0"
                @plus="onPlus(key.id,key.shopdetail.ptype)"
                @minus="onMinus(key.id,key.shopnum,key.shopdetail.ptype)"
                class="font42"
              />
            </template>
          </van-field>
        </div>

        <div class="distribution">
          <div class="dTitle">配送信息：</div>
          <div class="dInfo">
            <input type="text" placeholder="地址" v-model="buyaddr" />
            <input type="text" placeholder="联系人" v-model="buyname" />
            <input type="text" placeholder="手机号" v-model="buyphone" />
            <!--<li>广东省深圳市龙华福龙大厦530室</li>
            <li>150****890 李红</li>  -->
          </div>
        </div>

        <div class="remarkTitle">备注：</div>
        <ul class="remarkInfo">
          <li><textarea type="text" v-model="buyremark" /></li>
          <!--   <li>摩奇猴套装系列（A001) 尺寸：XL 码</li> -->
        </ul>
        <div class="sumTitle">合计</div>
        <div class="sumInfo" v-if="buytotalep>0">
          <span class="tit">需扣除您的奖金：</span>
          <span class="num">{{buytotalep}}</span>
          <span class="unit">（EP)</span>
        </div>
          <div class="sumInfo" v-if="buytotalpp>0">
          <span class="tit">需扣除您的产品分：</span>
          <span class="num">{{buytotalpp}}</span>
          <span class="unit">（PP)</span>
        </div>
        <div class="sumInfo">
          <span class="tit">您目前帐户可购买商品的余额为：EP:{{ totalep}} PP:{{totalpp}} </span
          >
        </div>
        <div class="buttonWrap">
          <router-link to="shop" class="router">
            <!--@click="buyShop" totalpp -->
            <button class="back">继续购物</button>
          </router-link>
          <button class="sure" @click="buyShop">立即购买</button>
        </div>
        <!-- <button class="next" @click="goNext">确认提交</button> -->
      </div>
    </ScrollRefresh>
    <YellowComfirm
      :show="isEnter"
      :tipTitle="tips"
      :showConfirmBtn="isshowconfirm"
      @clickOver="clickOverpay"
      @clickOk="clickOk()"
      @clickNo="clickNo()"
      @changeModel="changeModel"
    ></YellowComfirm>
  </div>
</template>
<script type="text/javascript">
import TopBar from 'components/TopBar'
import headerImg from '../../assets/imgs/headerImg.png'
import YellowComfirm from 'components/YellowComfirm'
import ScrollRefresh from 'components/ScrollRefresh'
import { http } from 'util/request'
import { config } from 'util/config'
import {
  CreateNewAccount,
  GetUserInfo,
  GetShopCartsweb,
  AddGoodsweb,
  BuyGoodsweb,
  GetShopaddr,
  GetShopCartsbyweb,
  BuyGoodsbyweb
} from 'util/netApi'
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
      isshowconfirm:false,
      totalep: 0,
      totalpp:0,
      buytotalep: 0,
      buytotalpp: 0,
      buyaddr: '',
      buyname: '',
      buyphone: '',
      buyremark: '',
      data: [
        {
          icon_url: require('@/assets/imgs/shop/camea.png'),
          id: 1,
          shopsku:
          {
          createtime: "2021-01-06 00:00",
          detaildesc: "黑色-S码",
          detailicon: "shopimg_1.png",
          detailname: "S",
          detailnum: 200,
          detailprice: 300,
          id: 1,
          skuid: 1,
          },
          shopskudetail:{
            createtime: "2021-01-06 00:00",
            id: 1,
            shopid: 1,
            skuIcon: "shopimg_1.png",
            skudesc: "黑色的东西",
            skuname: "黑色",
          },
          shopdetail: {
            createTime: '',
            id: 0,
            minLevel: 0,
            pDesc: '',
            pIcon: '',
            pName: '',
            pNum: 0,
            price: 0,
            priceType: 0,
            status: 0,
            ptype:0,
          },
          shopid: 8,
          shopnum: 8,
          uid: 8
        }
      ],
      topBarOption: {
        iconLeft: 'back',
        iconRight: 'icongouwucheman'
      },
      carNum: 1,
      tips:
        '恭喜！注册成功了！登录ID: 100012登录密码：123456交易密码：123456请尽快登录修改并完善个人资料',
      isEnter: false,
      account: '2,000',
      price: 0,
      shopprice: 0,
      stepper: 1,
      startmax: 8,
      isreturn: 0,
      nowid:0,
      addmodel: {},
      initData: {
        price: '',
        positionId: '',
        area: ''
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
     getimgurl(imgurl)
    {
        return config.shopimgUrl+imgurl
    },
    getshopcartnum () {
      var that = this;
      http(GetShopCartsbyweb, null, json => {
        if (json.code === 0) {
          that.data = json.response.data.list
          this.sumallshop()
        }
      })
    },
    sumallshop () {
      var pp = 0
      var totalnum = 0
      var ep = 0
      this.data.forEach(function (item) {
        if(item.shopdetail.ptype==0)
        {

         trp += item.shopnum * item.shopsku.detailprice
        totalnum += item.shopnum
        }else if(item.shopdetail.ptype==1)
        {
         ep += item.shopnum * item.shopsku.detailprice
         totalnum += item.shopnum
        }
       
      })
      this.buytotalep = ep
      this.buytotalpp = pp
      this.carNum = totalnum
    },
    goNext () {
      if (this.totalep < this.buytotalep) {
        this.isEnter = true
        this.tips = '当前金额不足'
      }
    },
    buyShop () {
      if (this.totalep < this.buytotalep) {
        this.isEnter = true
        this.tips = '当前奖金不足'
        return
      }
       if (this.totalpp < this.buytotalpp) {
        this.isEnter = true
        this.tips = '当前产品分不足'
        return
      }
      http(
        BuyGoodsbyweb,
        {
          addr: this.buyaddr,
          phone: this.buyphone,
          name: this.buyname,
          remark: this.buyremark
        },
        (json) => {
          if (json.code === 0) {
            this.isEnter = true
            this.tips = json.msg
            this.getshopcartnum()
          } else {
            this.isEnter = true
            this.tips = json.msg
          }
        }
      )
    },
    changeModel (v) {
      this.isEnter = v
    },
    clickOk () {
      this.isEnter = false
      console.log(this.nowid)
      this.addshop(this.nowid, 1, '-')
      //location.reload();
    },
    clickNo()
    {
        this.isEnter = false
        this.data.forEach(el => {
          if(el.id==this.nowid)
          {
            el.shopnum++;
          }
        })
    },
    TogetUserInfo () {
      http(GetUserInfo, null, (json) => {
        if (json.code === 0) {
          this.totalep = json.response.gold
          this.totalpp=json.response.apple
        }
      })
    },
    onPlus (id) {
      // 增加
      this.price += this.shopprice
      this.addshop(id, 1, '')
    },
    onMinus (id, index) {
     // this.addshop(id, 1, '-')
      if (index == 1) {
      this.tips='是否要删除当前商品?'
      this.isshowconfirm=true
      this.isEnter=true
      this.nowid=id
      }else
      {
         this.price -= this.shopprice
         this.addshop(id, 1, '-')
      }
    },
    addshop (id, num, option) {
      http(AddGoodsweb, { shopid: id, num: num, option: option }, (json) => {
        if (json.code === 0) {
          this.sumallshop()
          this.getshopcartnum()
        }
      })
    }
  },
  created () {
    this.TogetUserInfo()
    this.getshopcartnum()
    http(GetShopaddr, null, (json) => {
      if (json.code === 0) {
        this.buyaddr = json.response.data.buyaddr
        this.buyphone = json.response.data.buyphone
        this.buyname = json.response.data.buyname
      }
    })
  }
}
</script>

<style lang="less" scoped>

.shopCarWrapper {
  .innerScroll {
    /deep/ .wrapper .bscroll-container {
      min-height: calc(100vh - 420px);
    }
  }
  /deep/.top-bar .img-r {
    font-size: 70px;
  }
  input,textarea {
    height: 130px;
    margin: 0 auto 50px;
    line-height: 130px;
    color: #6F6D72;
    width: 100%;
    padding: 0 30px;
    border-radius: 20px;
    font-size: 48px;
    font-weight: 600;
    letter-spacing: 10px;
  }
  textarea{
    height: auto;
    line-height: 60px;
    margin: 0;
  }
  input::-webkit-input-placeholder {
    color: #9e9e9f;
    font-size: 48px;
  }
    textarea::-webkit-input-placeholder {
    color: #9e9e9f;
    font-size: 48px;
  }
  .innerWrap {
    width: 100vw;
    margin-top: 0px;
    padding-top: 30px;
    padding-bottom: 100px;
    /deep/.van-stepper__plus {
      width: 58px;
      height: 58px;
      // margin-left: 15px;
      background: #b3bdbe;
    }
    /deep/.van-stepper__plus::after {
      color: black;
    }
    /deep/.van-stepper__plus::before {
      color: black;
      height: 4px;
    }
    /deep/.van-stepper__plus::after {
      color: black;
      width: 4px;
    }
    /deep/.van-stepper__minus::before {
      color: black;
      height: 4px;
    }
    /deep/.van-stepper__minus::after {
      color: black;
      width: 4px;
    }
    /deep/.van-stepper__minus {
      width: 58px;
      height: 58px;
      // margin-right: 15px;
      background: #b3bdbe;
    }
    /deep/.van-stepper__input {
      font-size: 42px;

      font-weight: bold;
      color: #010000;
      line-height: 50px;
      height: 50px;
      background: #b3bdbe;
      width: 100px;
    }

    /deep/.van-cell {
      width: 220px;
      background: #b3bdbe;
      border-radius: 30px;
      height: 60px;
      padding: 0;
    }
  }

  .goods-info {
    font-weight: bold;
    font-size: 33px;
    color: #000000;
    opacity: 0.75;
    flex: 1;
    div {
      line-height: 60px;
    }
  }
  .goods {
    width: 90%;
    min-height: 158px;
    align-items: flex-end;
    padding: 30px;
    margin: 0 auto 50px;
    background-color: #dee3ee;
    border-radius: 36px;
    .img {
      width: 151px;
      // height: 184px;
      margin-right: 70px;
    }
  }
  .remarkTitle {
    font-size: 33px;
    font-weight: bold;
    color: #dee3ee;
    height: 100px;
    line-height: 100px;
    margin-top: 20px;
    padding-left: 60px;
  }
  .distribution {
    background: #dee3ee;
    border-radius: 36px;
    width: 90%;
    margin: 40px auto 0;
    padding: 30px 40px;
    .dTitle {
      font-weight: bold;
      color: #000000;
      line-height: 130px;
      opacity: 0.75;
    }
    .dInfo {
      li {
        font-size: 36px;
        font-weight: bold;
        color: #6f6d72;
        line-height: 50px;
      }
    }
  }
  .sumTitle {
    font-size: 71px;
    font-weight: bold;
    color: #ffffff;
    margin-top: 60px;
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
  .remarkInfo {
    border-radius: 27px;
    background: #ffffff;
    width: 90%;
    margin: 0 auto;
    padding: 30px 40px;
    li {
      font-size: 37px;
      line-height: 70px;
      font-weight: bold;
      color: #0f010f;
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
      color: #4678bc;
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
      background-color: #7895e8;
    }
    .sure {
      background-color: #113d79;
    }
  }
}
</style>
