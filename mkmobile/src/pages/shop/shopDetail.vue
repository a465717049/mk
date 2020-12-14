<template>
  <div class="shop-part">
    <TopBar class="center-one-search" :option="topBarOption" :badge="carNum"
      >商品介绍</TopBar
    >
    <ScrollRefresh
      @getData="ToGetShopDeatilList"
      :residualHeight="140"
      :isNeedUp="false"
      class="innerScroll"
    >
      <div class="scrollPart" ref="scrollPart">
        <div class="shop-detail font40 center relative">
          <span class="tag" v-show="ptag">{{ ptag }}</span>
          <div class="monkey">
            <div class="monkeywrap">
              <img src="../../assets/imgs/login/head.png" alt class="head" />
              <img src="../../assets/imgs/login/eye5.png" alt class="eye" />
            </div>
            <span class="moki">MOKI minkey 摩奇猴</span>
          </div>
          <van-swipe :autoplay="4000" class="sweiper1" @change="onChangeSwiper">
            <van-swipe-item v-for="(image, index) in images" :key="index">
              <img :src="image.pIcon" />
            </van-swipe-item>
            <template #indicator>
              <div class="custom-indicator">
                {{ current + 1 }}/{{ images.length }}
              </div>
            </template>
          </van-swipe>
          <!-- <img class="shop-img center mt-100 mb-100" :src="pIcon" alt /> -->
          <div class="buy border-top-radius mt-80">
            <!-- <div class="title Tleft font-weight pt-60 font60">{{pName}}</div> -->
            <ul class="detail-info pt-60 Tleft font42">
              <li v-for="(item, index) in pInfo" :key="index" class="info-item">
                {{ item }}
              </li>
            </ul>
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
              <span class="price">$ {{ price }}</span>
            </div>
            <div class="base-flex mt-40">
              <div class="heart borderR">
                <i class="iconfont icongouwucheman" @click="addshop"></i>
              </div>
              <router-link to="shopCar" class="router">
                <!--@click="buyShop" -->
                <button class="buy-btn center borderR">立即购买</button>
              </router-link>
            </div>
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
import TopBar from "components/TopBar";
import YellowComfirm from "components/YellowComfirm";
import { http } from "util/request";
import {
  GetShopDeatilList,
  GetUserInfo,
  BuyGoodsweb,
  AddGoodsweb,
  GetShopCartsweb,
} from "util/netApi";
import { storage } from "util/storage";
import banner1 from "../../assets/imgs/banner-00.png";
import banner2 from "../../assets/imgs/banner-01.png";
import { accessToken, loginPro } from "util/const.js";
import ScrollRefresh from "components/ScrollRefresh";
export default {
  name: "shopDetail",
  components: {
    TopBar,
    YellowComfirm,
    ScrollRefresh,
  },
  data() {
    return {
      showComfirm: false,
      carNum: 1,
      shopid: 0,
      images: [{ image: banner1 }, { image: banner2 }],
      current: 0,
      // pName: "促销中",
      // pDesc: "促销中促销中促销中",
      pInfo: ["品牌： "],
      pIcon: require("@/assets/imgs/shop/camea.png"),
      ptag: "",
      price: 0,
      shopprice: 0,
      stepper: 1,
      startmax: 8,
      showChat: true,
      topBarOption: {
        iconLeft: "back",
        iconRight: "icongouwucheman",
      },
      tips: "",
      tipsObj: {},
    };
  },
  methods: {
    onChangeSwiper(index) {
      this.current = index;
    },
    clickOk() {
      this.showComfirm = false;
    },
    changeModel(v) {
      this.showComfirm = v;
    },
    onPlus() {
      //增加
      this.price += this.shopprice;
    },
    onMinus() {
      //减少
      this.price -= this.shopprice;
    },
    buyShop() {
      //shopCar
      http(
        BuyGoodsweb,
        { shopid: this.shopid, buynum: this.stepper },
        (json) => {
          if (json.code === 0) {
          } else {
            this.showComfirm = true;
            this.tips = json.msg;
          }
        }
      );
    },
    getshopcartnum() {
      http(GetShopCartsweb, null, (json) => {
        if (json.code === 0) {
          this.carNum = json.response.count;
        }
      });
    },
    addshop() {
      http(AddGoodsweb, { shopid: this.shopid, num: this.stepper }, (json) => {
        if (json.code === 0) {
          this.getshopcartnum();
        }
        //  console.log("111");
      });
    },
    ToGetShopDeatilList(tmpshopid) {
      http(GetShopDeatilList, { shopid: tmpshopid }, (json) => {
        console.log(json);
        if (json.code === 0) {
          var shop = json.response.list[0];
          console.log(shop);
          this.pName = shop.pName;
          this.pInfo = [shop.pDesc];
          this.pDesc = shop.pDesc;
          this.pIcon = shop.pIcon;
          this.price = shop.price;
          this.shopprice = shop.price;
          this.startmax = shop.pNum;
        }
      });
    },
  },
  created() {
    if (this.$route.query.id) {
      this.shopid = this.$route.query.id;
      this.ToGetShopDeatilList(this.shopid);
      this.getshopcartnum();
    }
  },
  mounted() {
    // this.scrollInit()
  },
};
</script>
<style lang='less' scoped>
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
      padding: 0 58px;
      background: #dce2eb;
      border-radius: 56px 56px 0 0;
      padding-bottom: 150px;
      /deep/.van-stepper__plus {
        width: 84px;
        height: 84px;
        border-radius: 42px;
        border: 4px solid rgba(100, 24, 195, 1);
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
        border: 4px solid rgba(100, 24, 195, 1);
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
    .detail-info {
      padding-left: 104px;
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
      color: #f00833;
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
      border-radius: 24px;
      overflow: hidden;
      border: 10px solid #fff;
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
      height: 107px;
      position: relative;

      .head {
        width: 135px;
        height: 107px;
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
}
</style>