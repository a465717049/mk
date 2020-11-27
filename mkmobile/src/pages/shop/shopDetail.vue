<template>
  <div class="shop-part">
    <TopBar class="center-one-search" :option="topBarOption" :showChat="showChat">Shop</TopBar>
    <div class="scrollPart" ref="scrollPart">
      <div class="shop-detail font40 center relative">
        <img class="shop-img center mt-100 mb-100" v-bind:src="pIcon" alt />
        <div class="buy border-top-radius mt-80">
          <div class="title Tleft font-weight pt-60 font60">{{pName}}</div>
          <div class="detail-info pt-60 Tleft font42">{{pDesc}}</div>
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
            <span class="font65 base-purple">$ {{price}}</span>
          </div>
          <div class="base-flex mt-40">
            <div class="heart borderR">
              <i class="iconfont iconheart"></i>
            </div>
            <button class="buy-btn center borderR" @click="buyShop">立即购买</button>
          </div>
        </div>
      </div>
    </div>
    <YellowComfirm :show="showComfirm"  @clickOver="clickOverpay" :tipTitle="tips" @clickOk="clickOk()" @changeModel="changeModel"></YellowComfirm>
  </div>
</template>
<script>
import TopBar from 'components/TopBar'
import YellowComfirm from 'components/YellowComfirm'
import BScroll from 'better-scroll'
import { http } from 'util/request'
import { GetShopDeatilList, GetUserInfo, BuyGoodsweb } from 'util/netApi'
import { storage } from 'util/storage'
import { accessToken, loginPro } from 'util/const.js'
export default {
  name: 'shopDetail',
  components: {
    TopBar,
    YellowComfirm
  },
  data() {
    return {
      showComfirm: false,
      shopid: 0,
      pName: '',
      pDesc: '',
      pIcon: '',
      price: 0,
      shopprice: 0,
      stepper: 1,
      startmax: 8,
      showChat: true,
      topBarOption: {
        iconLeft: 'iconzhankai',
        iconRight: '11'
      },
      tips: '',
      tipsObj: {}
    }
  },
  methods: {
    clickOk() {
      this.showComfirm = false
    },
    changeModel(v) {
      this.showComfirm = v;
    },
    onPlus() {
      //增加
      this.price += this.shopprice
    },
    onMinus() {
      //减少
      this.price -= this.shopprice
    },
    buyShop() {
      http(BuyGoodsweb, { id: this.tmpshopid }, json => {
        if (json.code === 0) {
        } else {
          this.showComfirm = true
          this.tips = json.msg
        }
      })
    },
    scrollInit() {
      if (!this.scroll) {
        this.scroll = new BScroll(this.$refs.scrollPart, {
          scrollY: true,
          click: true,
          bounce: {
            top: true,
            bottom: true
          }
        })
      } else {
        this.scroll.refresh()
      }
    },
    ToGetShopDeatilList(tmpshopid) {
      http(GetShopDeatilList, { shopid: tmpshopid }, json => {
        console.log(json)
        if (json.code === 0) {
          var shop = json.response.list[0]
          this.pName = shop.pName
          this.pDesc = shop.pDesc
          this.pIcon = shop.pIcon
          this.price = shop.price
          this.shopprice = shop.price
          this.startmax = shop.pNum
        }
      })
    }
  },
  created() {
    if (this.$route.query.id) {
      this.shopid = this.$route.query.id
      this.ToGetShopDeatilList(this.shopid)
    }
  },
  mounted() {
    this.scrollInit()
  }
}
</script>
<style lang='less' scoped>
.shop-part {
  .scrollPart {
    height: calc(100vh - 400px);
    overflow: hidden;
    border-radius: 40px;
    margin-top: -120px;
    background: #ebeaf0;
  }
  .shop-detail {
    background: #cadba7;
    .shop-img {
      width: 652px;
      height: 882px;
    }
    .buy {
      height: 45vh;
      padding: 0 58px;
      background: #dee9c8;
      border-radius: 56px;
      /deep/.van-stepper__plus {
        width: 84px;
        height: 84px;
        border-radius: 42px;
        border: 4px solid rgba(100, 24, 195, 1);
        margin-left: 15px;
        background: #dee9c8;
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
        border: 4px solid rgba(100, 24, 195, 1);
        margin-right: 15px;
        background: #dee9c8;
      }
      /deep/.van-stepper__input {
        font-size: 42px;
      }

      /deep/.van-cell {
        width: 40%;
        background: #dee9c8;
      }
      /deep/.van-stepper__input {
        background: #dee9c8;
        height: 84px;
        width: 60px;
        line-height: 84px;
      }
      .heart {
        width: 221px;
        height: 163px;
        background: #a9cd60;
        margin-right: 40px;
        > i {
          font-size: 100px;
          color: white;
        }
      }
      .buy-btn {
        width: 739px;
        height: 163px;
        background: #eecb08;
        margin-bottom: 40px;
        font-weight: bold;
        font-size: 53px;
      }
    }
  }
}
</style>