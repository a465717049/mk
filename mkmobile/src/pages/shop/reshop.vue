<template>
  <div class="shopWrap">
    <TopBar class="center-one-search" :option="topBarOption" :badge="carNum">
      <div>
        <div class="three-tit-t">商店(再次购买)</div>
        <TopSearch @onSearch="onSearch" placeinputValue></TopSearch>
      </div>
    </TopBar>
    <div>
      <div class="shop clearfix">
        <!-- <TopSearch @onSearch="search" :placeinputValue="''"></TopSearch> -->
        <ScrollRefresh
          @getData="ToGetShopList"
          :residualHeight="topbarHeight+bottomTabBarHeight+10"
          :isNeedUp="false"
          class="innerScroll"
        >
          <div class="shop-info center">
            <waterfall
              :col="col"
              :width="itemWidth"
              :gutterWidth="gutterWidth"
              :data="data"
              @loadmore="loadmore"
            >
              <template>
                <div
                  class="container p-58"
                  v-for="(item,index) in data"
                  :key="index"
                  :style="'background-color:'+item.bgc"
                >
                  <!-- 后期根据将图片设置为自动高 -->
                  <img
                    :src="item.icon_url"
                    :class="index===1?'imgSpecial':''"
                    class="img-info mt-100 center"
                    alt
                  />
                  <div class="title font-weight Tleft">{{item.price}}</div>
                  <div class="detail Tleft">{{item.name}}</div>
                  <div class="circle" @click="goDetail(item.id)">+</div>
                  <div class="tag" v-show="item.tag">{{item.tag}}</div>
                </div>
              </template>
            </waterfall>
          </div>
        </ScrollRefresh>
      </div>
    </div>
    <YellowComfirm :show="showComfirm" :tipTitle="tips"></YellowComfirm>
  </div>
</template>
<script>
import TopBar from 'components/TopBar'
import TopSearch from 'components/TopSearch'
import { http } from 'util/request'
import {
  GetShopList,
  GetUserInfo,
  GetShopDeatilLike,
  GetShopCartsweb
} from 'util/netApi'
import { storage } from 'util/storage'
import { accessToken, loginPro } from 'util/const.js'
import YellowComfirm from 'components/YellowComfirm'
import ScrollRefresh from 'components/ScrollRefresh'
export default {
  name: 'Shop',
  components: {
    TopBar,
    TopSearch,
    YellowComfirm,
    ScrollRefresh
  },
  data () {
    return {
      carNum: 1,
      showChat: false,
      placeinputValue: '',
      topBarOption: {
        iconLeft: 'iconmenu2',
        iconRight: 'icongouwucheman'
      },
      showComfirm: true,
      tips: '即將更新！',
      data: [
        {
          icon_url: require('@/assets/imgs/shop/camea.png'),
          price: '70$',
          name: 'Children Camera Visionkids Happycamu',
          id: 1,
          bgc: '#F5E0BA',
          tag: '促销中'
        }
      ],
      col: 2
    }
  },
  computed: {
    itemWidth () {
      return (document.documentElement.clientWidth - 70) / 2
    },
    gutterWidth () {
      return 30 * 0.5 * (document.documentElement.clientWidth / 375)
    }
  },
  methods: {
    onSearch (value) {
      this.ToGetShopDeatilLike(value)
    },
    goDetail (shopid) {
      this.$router.push('./shopDetail?id=' + shopid)
    },
    loadmore (index) {
      this.data = this.data.concat(this.data)
    },
    ToGetShopList () {
      http(GetShopList, {ptype:1}, json => {
        if (json.code === 0) {
          this.data = json.response.list
          this.data.forEach(el => {
            let img = null
            try {
              img = require('@/assets/imgs/shop/goods-' + el.id + '.png')
            } catch (err) {
              // 图片 不存在则使用默认的图片
              img = require('@/assets/imgs/shop/camea.png')
            }
            return (el.icon_url = img)
          })
        }
      })
    },
    getshopcartnum () {
      http(GetShopCartsweb, null, json => {
        if (json.code === 0) {
          this.carNum = json.response.count
        }
      })
    },
    ToGetShopDeatilLike (value) {
      console.log(value)
      http(GetShopDeatilLike, { key: value }, json => {
        if (json.code === 0) {
          console.log(json)
          this.data = json.response.list
          this.data.forEach(el => {
            let img = null
            try {
              img = require('@/assets/imgs/shop/goods-' + el.id + '.png')
            } catch (err) {
              // 图片 不存在则使用默认的图片
              img = require('@/assets/imgs/shop/camea.png')
            }
            return (el.icon_url = img)
          })
          // this.username = json.response.nickname
        }
      })
    }
  },
  created () {
    this.ToGetShopList()
    this.getshopcartnum()
  }
}
</script>
<style lang='less' scoped>
.shopWrap {
  /deep/.top-bar .img-r {
    font-size: 70px;
  }
  /deep/.toptab_search{
    margin-top: 60px;
  }
}
.shop {
  position: relative;
  padding: 0 58px;
  .innerScroll {
    /deep/.wrapper {
      padding-top: 80px;
      .bscroll-container {
        min-height: calc(100vh - 550px) !important;
      }
    }
  }

  .shop-info {
    // margin-top: 20px;
    padding-top: 50px;
    .container {
      position: relative;
      border-radius: 80px;
      background: #f5e0ba;
      // height:700px;
      width: 100%;
      float: left;
      margin-bottom: 5%;
      &:nth-child(odd) {
        margin-right: 5%;
      }
      .img-info {
        width: 285px;
        height: 232px;
      }
      .imgSpecial {
        height: 300px;
      }
      .title {
        font-size: 54px;
        color: rgba(0, 0, 0, 1);
        opacity: 0.72;
      }
      .detail {
        font-size: 26px;
        color: rgba(0, 0, 0, 1);
        opacity: 0.75;
        line-height: 50px;
      }
      .circle {
        position: absolute;
        right: 0;
        top: 0;
        width: 124px;
        height: 127px;
        line-height: 127px;
        background: white;
        opacity: 0.32;
        border-top-right-radius: 70px;
        border-bottom-left-radius: 70px;
        font-size: 80px;
      }
    }
  }
  .tag {
    height: 64px;
    line-height: 64px;
    position: absolute;
    top: 40px;
    left: 78px;
    background: #7d0d0d;
    border-radius: 32px;
    padding: 0 26px;
    color: #fff;
    font-size: 34px;
  }
}
</style>
