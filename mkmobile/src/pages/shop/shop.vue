<!--  -->
<template>
  <div>
    <TopBar
      class="center-one-search"
      :option="topBarOption"
    >商店</TopBar>
    <div class>
      <div class="shop borderR bg-gray clearfix">
     <div class="tips-part">
            <div class="tip-titl"></div>
            <!-- <div>任何活动您只需交纳EP后剩下的事情由官方 合作伙伴来解决。</div> -->
            <div>即將更新！</div> 
          </div>
        <!-- 搜索 -->
        <div class="shop-info center">
          <waterfall
            :col="col"
            :width="itemWidth"
            :gutterWidth="gutterWidth"
            :data="data"
            @loadmore="loadmore"
            @scroll="scroll"
          >
            <template>
                
              <div class="container p-58" v-for="(item,index) in data" :key="index">
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
              </div>
            </template>
          </waterfall>
        </div>
      </div>
    </div>
     <YellowComfirm
      :show="showComfirm"
      :tipTitle="tips"
    ></YellowComfirm>
  </div>
</template>
<script>
import TopBar from 'components/TopBar'
import TopSearch from 'components/TopSearch'
import { http } from 'util/request'
import { GetShopList, GetUserInfo, GetShopDeatilLike } from 'util/netApi'
import { storage } from 'util/storage'
import { accessToken, loginPro } from 'util/const.js'
import YellowComfirm from 'components/YellowComfirm'
export default {
  name: 'Shop',
  components: {
    TopBar,
    TopSearch,
    YellowComfirm
  },
  data() {
    return {
      showChat: false,
      placeinputValue: '',
      topBarOption: {
        iconLeft: 'iconzhankai',
        iconRight: ''
      },
      showComfirm: true,
      tips:"即將更新！",
      data: [
        //  {
        //    icon_url: require('@/assets/imgs/shop/camea.png'),
        //    price: '70$',
        //    name: 'Children Camera Visionkids Happycamu'
        //   },
        //   {
        //     icon_url: require('@/assets/imgs/shop/bag.png'),
        //     price: '70$',
        //     name: 'Children Camera Visionkids Happycamu'
        //   }
      ],
      col: 2
    }
  },
  computed: {
    itemWidth() {
      return (document.documentElement.clientWidth - 50) / 2
    },
    gutterWidth() {
      return 9 * 0.5 * (document.documentElement.clientWidth / 375)
    }
  },
  methods: {
    onSearch(value) {
      this.ToGetShopDeatilLike(value)
    },
    goDetail(shopid) {
      this.$router.push('./shopDetail?id=' + shopid)
    },
    scroll(scrollData) {
      console.log(scrollData)
    },
    loadmore(index) {
      this.data = this.data.concat(this.data)
    },
    ToGetShopList() {
      http(GetShopList, null, json => {
        if (json.code === 0) {
          this.data = json.response.list
        }
      })
    },
    ToGetShopDeatilLike(value) {
      console.log(value)
      http(GetShopDeatilLike, { key: value }, json => {
        if (json.code === 0) {
          console.log(json)
          this.data = json.response.list
          // this.username = json.response.nickname
        }
      })
    }
  },
  created() {
    this.ToGetShopList()
  }
}
</script>
<style lang='less' scoped>
.shop {
  border-radius: 40px;
  border: 1px solid #ececec;
  margin-top: 20px;
  position: relative;
  padding: 0 58px;
  .shop-info {
    height: calc(100vh - 700px);
    overflow-y: auto;
    margin-top: 20px;
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
        font-size: 60px;
        color: rgba(0, 0, 0, 1);
        opacity: 0.72;
      }
      .detail {
        font-size: 42px;
      }
      .circle {
        position: absolute;
        right: 0;
        top: 0;
        width: 124px;
        height: 127px;
        background: white;
        opacity: 0.32;
        border-top-right-radius: 65px;
        border-bottom-left-radius: 65px;
        font-size: 80px;
      }
    }
  }
}
</style>