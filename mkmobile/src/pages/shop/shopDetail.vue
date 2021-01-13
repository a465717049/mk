<template>
  <div class="shop-part">
    <TopBar class="center-one-search" :option="topBarOption" :badge="carNum" @clickR="goShopCar"
      >商品介绍</TopBar
    >
    <ScrollRefresh
      @getData="ToGetShopDeatilList"
      :residualHeight="140"
      :isNeedUp="false"
      class="innerScroll"
    >
      <div class="scrollPart" ref="scrollPart">
        <div class="shop-detail font40  relative">
          <span class="tag" v-show="ptag">{{ ptag }}</span>
          <div class="monkey">
            <div class="monkeywrap">
              <img src="../../assets/imgs/login/head.png" alt class="head" />
              <!-- <img src="../../assets/imgs/login/eye5.png" alt class="eye" /> -->
            </div>
            <!-- <span class="moki">MOKI minkey 摩奇猴</span> -->
          </div>
          <van-swipe :autoplay="4000" class="sweiper1" @change="onChangeSwiper">
            <van-swipe-item v-for="(image, index) in images" :key="index">
              <img :src="image.image" />
            </van-swipe-item>
            <template #indicator>

              <div class="custom-indicator">
                {{ current + 1 }}/{{ images.length }}
              </div>
            </template>
          </van-swipe>
          <!-- <img class="shop-img center mt-100 mb-100" :src="pIcon" alt /> -->
          <div class="buy  mt-80">
            <div class="goodsName">{{pName}}
              南极人 男士内裤男3A抑菌平角裤纯色95%精梳棉质中腰男式四角裤衩u凸短裤头4条礼盒装NHT9999 混色4条XL
            </div>
             <div class="price">￥ {{ price }}</div>
            <div class="skuSelect">
              <ul class="classifyUl">
                <li class="classifyLi" v-for="(item,index) in skuList" :key="index">
                  <div class="itemName">{{item.itemName}}</div>
                  <ul class="itemInfo ">
                    <li class="itemDetail " :class="{active:itemIndex===1}" v-for="(goodsInfo,itemIndex) in item.goods" :key="itemIndex">
                      <img :src="goodsInfo.img" v-if="goodsInfo.img" alt="" class="goodsImg">
                      <span class="goodsInfo">{{goodsInfo.info}}</span>
                    </li>
                  </ul>
                </li>
              </ul>
            </div>
            <div class="base-flex mt-40">
              <van-field name="stepper" class="font42">
                <template #input>
                  <van-stepper
                    v-model="stepper"
                    @plus="onPlus"
                    @minus="onMinus"
                    :max="startmax"
                    class="font42"
                  />
                </template>
              </van-field>
              <div class="base-flex ">
                <div class="addShop borderR">
                  <i class="iconfont icongouwucheman" @click="addshop"></i>
                  <span>加入购物车</span>
                </div>
              </div>
            </div>
            <img style="width:100%;height:100%;" class="mt-100 mb-100" v-if="pDetailIcon"
            :src="getimgurl(pDetailIcon)" alt />
          </div>
          <div class="buy border-radius mt-80">
              <ul class="detail-info  font42">
              <li v-for="(item, index) in pInfoList" :key="index" class="info-item">
                {{ item }}
              </li>
            </ul>
          </div>
          <div class="buy border-radius mt-80 mb100">
              <img :src="banner1" alt="" class="adImg">
              <img :src="adInfo" alt="" class="adInfo">
          </div>
        </div>
      </div>
    </ScrollRefresh>
    <YellowComfirm
      :show="showComfirm"
      @clickOver="clickOverpay"
      :tipTitle="tips"
      @clickOk="clickOk()"
      @changeModel="changeModel"
    ></YellowComfirm>
  </div>
</template>
<script>
import TopBar from 'components/TopBar'
import YellowComfirm from 'components/YellowComfirm'
import { http } from 'util/request'
import {config} from 'util/config'
import {
  GetShopDeatilList,
  GetUserInfo,
  BuyGoodsweb,
  AddGoodsweb,
  GetShopCartsweb,
  GetShopSkuDetailList,
  GetShopSkuList
} from 'util/netApi'
import { storage } from 'util/storage'
import banner1 from '../../assets/imgs/banner-00.png'
import adInfo from '../../assets/imgs/adInfo.png'
import banner2 from '../../assets/imgs/banner-01.png'
import { accessToken, loginPro } from 'util/const.js'
import ScrollRefresh from 'components/ScrollRefresh'
export default {
  name: 'shopDetail',
  components: {
    TopBar,
    YellowComfirm,
    ScrollRefresh
  },
  data () {
    return {
      skuvalue: 0,
      skudetailvalue: 0,
      skuTypeList: [
        { text: '黑', value: 1 }
      ],
      skudetailTypeList: [ ],
      selectinfoList: [],
      selectinfo:
        {
          createtime: '2021-01-06 00:00',
          detaildesc: '黑色-L码',
          detailicon: 'shopimg_1.png',
          detailname: 'L',
          detailnum: 100,
          detailprice: 100,
          id: 3,
          skuid: 1
        },
      showComfirm: false,
      carNum: 1,
      shopid: 0,
      images: [],
      pDetailIcon: '',
      current: 0,
      // pName: "促销中",
      // pDesc: "促销中促销中促销中",
      pInfoList: [
        '品牌： MOKI MONKEY',
        '商品产地：孟加拉国、越南等（批次不同产地不同，以 实际发货为准）',
        '货号：MOKI- 100 ',
        '类别：平角裤腰型',
        '材质：聚合烯矿丝棉'
      ],
      pIcon: null,
      ptag: '',
      price: 0,
      shopprice: 0,
      stepper: 1,
      startmax: 8,
      showChat: true,
      topBarOption: {
        iconLeft: 'back',
        iconRight: 'icongouwucheman'
      },
      tips: '',
      tipsObj: {},
      banner1: banner1,
      adInfo: adInfo,
      skuList: [{itemName: '选择颜色', goods: [{img: banner1, info: '1呼吸棉4条'}, {img: banner1, info: '2绅士健康呼4条'}, {img: banner1, info: '3绅士健康呼吸棉4条'}, {img: banner1, info: '4绅士健康呼吸棉4条'}, {img: banner1, info: '5绅士健康呼吸棉4条'}]}, {itemName: '选择尺码', goods: [{info: 'L(体重80)'}, {info: 'XL(体重105)'}, {info: 'XL(体重105)'}, {info: 'XL(体重105)'}, {info: 'XL(体重105)'}]}]
    }
  },
  methods: {
    changetype () {
      http(GetShopSkuDetailList, {id: this.skuvalue}, (json) => {
        if (json.code === 0) {
          var tmparray = []
          this.selectinfoList = []
          json.response.list.forEach(el => {
            this.selectinfoList.push(el)
            tmparray.push({ text: el.detailname, value: el.id })
          })
          this.skudetailTypeList = tmparray
        }
      })
    },
    changetypedetail () {
      this.selectinfoList.forEach(el => {
        if (el.id == this.skudetailvalue) {
          this.price = el.detailprice
          this.shopprice = el.detailprice
          this.startmax = el.detailnum
          // this.pDetailIcon=el.detailicon
          this.images = [{image: this.getimgurl(el.detailicon)}]
        }
      })
    },
    getimgurl (imgurl) {
      return config.shopimgUrl + imgurl
    },
    onChangeSwiper (index) {
      this.current = index
    },
    clickOk () {
      this.showComfirm = false
    },
    changeModel (v) {
      this.showComfirm = v
    },
    goShopCar () {
      this.$router.push('shopCar')
    },
    onPlus () {
      // 增加
      this.price += this.shopprice
    },
    onMinus () {
      // 减少
      this.price -= this.shopprice
    },
    buyShop () {
      // shopCar
      http(
        BuyGoodsweb,
        { shopid: this.shopid, buynum: this.stepper },
        (json) => {
          if (json.code === 0) {
          } else {
            this.showComfirm = true
            this.tips = json.msg
          }
        }
      )
    },
    getshopskuinfo (shopid) {
      http(GetShopSkuList, {shopid: shopid}, (json) => {
        if (json.code === 0) {
          var tmparray = []
          json.response.list.forEach(el => {
            tmparray.push({ text: el.skuname, value: el.id })
          })
          this.skuTypeList = tmparray
        }
      })
    },
    getshopcartnum () {
      http(GetShopCartsweb, null, (json) => {
        if (json.code === 0) {
          this.carNum = json.response.count
        }
      })
    },
    addshop () {
      http(AddGoodsweb, { shopid: this.skudetailvalue, num: this.stepper }, (json) => {
        if (json.code === 0) {
          this.getshopcartnum()
        }
      })
    },
    ToGetShopDeatilList (tmpshopid) {
      http(GetShopDeatilList, { shopid: tmpshopid }, (json) => {
        if (json.code === 0) {
          var shop = json.response.list[0]
          this.pName = shop.pName
          this.pInfo = [shop.pDesc]
          this.pDesc = shop.pDesc
          //   this.price = shop.price
          //     this.shopprice = shop.price
          //  this.startmax = shop.pNum
          this.pDetailIcon = shop.pDetailIcon
          this.images = [{image: this.getimgurl(shop.pIcon)}]
          this.getshopskuinfo(tmpshopid)
        }
      })
    }
  },
  created () {
    if (this.$route.query.id) {
      this.shopid = this.$route.query.id
      this.ToGetShopDeatilList(this.shopid)
      this.getshopcartnum()
    }
  },
  mounted () {
    // this.scrollInit()
  }
}
</script>
<style lang='less' scoped>
  ul {
    width: 90%;
    margin: 0 auto;
    li {
      .title {
        font-size: 42px;
        margin: 60px 0 22px;
        font-weight: 800;
        letter-spacing: 10px;
        padding-left: 20px;
      }
      input {
        height: 130px;
        line-height: 130px;
        color: #9e9e9f;
        width: 120px;
        padding: 0 30px;
        border-radius: 20px;
        font-size: 60px;
        font-weight: 600;
        letter-spacing: 10px;
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
    }
  }
.shop-part {
  .innerScroll {
    /deep/.wrapper {
      background: #dce2eb;
      .bscroll-container {
        min-height: calc(100vh - 400px) !important;
      }
    }
  }
  /deep/.top-bar .img-r {
    font-size: 70px;
  }
  .shop-detail {
    background: #c6d0de;
    padding-top: 200px;
    .shop-img {
      width: 652px;
      height: 882px;
    }
    .buy {
      // height: 45vh;
      width: 98vw;
      margin: 40px auto 0;
      padding: 57px 46px;
      background: #dce2eb;
      border-radius: 56px;
      // padding-bottom: 150px;
      /deep/.van-stepper__plus {
        width: 84px;
        height: 84px;
        border-radius: 42px;
        border: 6px solid rgba(100, 24, 195, 1);
        margin-left: 15px;
        background: #dce2eb;
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
        width: 84px;
        height: 84px;
        border-radius: 42px;
        border:6px solid rgba(100, 24, 195, 1);
        margin-right: 15px;
        background: #dce2eb;
      }
      /deep/.van-stepper__input {
        font-size: 42px;
      }

      /deep/.van-cell {
        width: 40%;
        background: #dce2eb;
      }
      /deep/.van-stepper__input {
        background: #dce2eb;
        height: 84px;
        width: 60px;
        line-height: 84px;
      }
      .goodsName{
        font-size: 30px;
        font-weight: bold;
        color: #2E2E35;
        line-height: 50px;
        text-align: left;
      }
      .addShop {
        width: 360px;
        height: 103px;
        line-height: 103px;
        // left: 163px;
        background: #a9cd60;
        margin-right: 40px;
        font-size: 38px;
        font-family: Yuanti SC;
        font-weight: bold;
        color: #FFFFFF;
        text-align: center;
        vertical-align: middle;
        display: flex;
        justify-content: space-evenly;
        > i {
          font-size: 70px;
          color: white;
        }
        span{
          display: inline-block;
          line-height: 103px;
        }
      }
      .buy-btn {
        width: 739px;
        height: 163px;
        line-height: 163px;
        background: #eecb08;
        margin-bottom: 40px;
        font-weight: bold;
        font-size: 53px;
        color: #191819;
      }
      .adImg{
        width: 100%;
        margin-bottom: 20px;
      }
       .adInfo{
        width: 100%;
      }

    }
     .mb100{
        padding-bottom: 100px !important;
      }
    .detail-info {
      li {
        font-weight: bold;
        color: #191819;
        margin-bottom: 10px;
        font-size: 38px;
      }
    }
    .price {
      font-size: 67px;
      font-weight: bold;
      color: #B61313;
      padding-bottom: 20px;
      border-bottom: 1px dotted #999;
    }
    .tag {
      height: 69px;
      line-height: 69px;
      background: #f00833;
      border-radius: 34px;
      padding: 0 20px;
      font-size: 37px;
      color: #dee3ee;
      top: 92px;
      left: 57px;
      position: absolute;
    }
  }
  .sweiper1 {
    margin: 0 auto;
    padding-bottom: 222px;
    width: 602px;
    height: 600px;
    img {
      width: 100%;
      height: 373px;
    }
    /deep/ .van-swipe-item {
      overflow: hidden;
    }
    /deep/ .van-swipe__indicator {
      width: 20px;
      height: 20px;
    }
    /deep/ .van-swipe__indicator--active {
      background-color: #6318c3;
    }
    /deep/ .custom-indicator {
      position: absolute;
      bottom: 0;
      left: 50%;
      transform: translateX(-50%);
      background: #b3bdbe;
      border-radius: 57px;
      height: 70px;
      line-height: 70px;
      color: #dee3ee;
      font-size: 38px;
      padding: 0 30px;
      letter-spacing: 10px;
    }
  }
  .monkey {
    position: absolute;
    top: 625px;
    right: 66px;
    display: flex;
    flex-direction: column;
    align-items: center;
    .monkeywrap {
      width: 135px;
      height: 137px;
      position: relative;

      .head {
        width: 135px;
        height: 137px;
        position: absolute;
        top: 0;
        left: 0;
      }
      .eye {
        position: absolute;
        width: 50px;
        height: 16px;
        left: 42px;
        top: 30px;
      }
    }

    .moki {
      font-size: 20px;
      font-weight: bold;
      color: #191819;
    }
  }

  .skuSelect{
    padding-top: 30px;
    .classifyUl{
      .classifyLi{
        display: flex;
        .itemName{
          width: 200px;
        }
        .itemInfo{
          flex: 1;
          .itemDetail{
            height: 100px;
            margin-right: 30px;
            line-height: 100px;
            display: inline-block;
            border: 3px solid #AAAFB6;
            margin-bottom: 30px;
            padding: 0 20px;
            .goodsImg{
              width: 100px;
            }
            .goodsInfo{
              font-size: 26px;
              color:'#333'
            }

          }
            .itemDetail.active{
              border: 3px solid #B61313;
            }
        }
      }
    }
  }
}
</style>
