<template>
  <div class="shopListWrap">
    <TopBar class="center-one-search" :option="topBarOption">
      <div>
        <div class="three-tit-t">订 单</div>
         <TopSearch @click="search" placeinputValue=""></TopSearch>
      </div>
    </TopBar>
    <div>
      <div class="shop clearfix">

        <ScrollRefresh
          @getData="ToGetShopList"
          :residualHeight="topbarHeight+bottomTabBarHeight+10"
          :isNeedUp="false"
          class="innerScroll"
        >
          <div class="innerWrap">
            <ul class="info">
              <li v-for="(item,index) in data" :key="index">
                <span @click="goDetail(item.id)">{{item.shopordernumber}}</span>
                <div class="tag ytag" v-if="item.status===1">未发货</div>
                <div class="tag gtag" v-else-if="item.status===2">配送中</div>
                <div class="tag rtag" v-else-if="item.status===3">确认收货</div>
                <div class="tag graytag" v-else-if="item.status===4">己完成</div>
                 <div class="tag gtag" v-else-if="data.status===5">已发货</div>
              </li>
            </ul>
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
import { GetShopList, GetUserInfo, GetShopDeatilLike, GetMyShopList } from 'util/netApi'
import { storage } from 'util/storage'
import { accessToken, loginPro } from 'util/const.js'
import YellowComfirm from 'components/YellowComfirm'
import ScrollRefresh from 'components/ScrollRefresh'
export default {
  name: 'shopList',
  components: {
    TopBar,
    TopSearch,
    YellowComfirm,
    ScrollRefresh
  },
  data () {
    return {
      showChat: false,
      placeinputValue: '',
      topBarOption: {
        iconLeft: 'iconmenu2',
        iconRight: 'iconxinxi2'
      },
      showComfirm: true,
      tips: '即將更新！',
      data: [
        {
          shopordernumber: 'MT000000043432',
          status: 2
        },
        {
          shopordernumber: 'MT000000043433',
          status: 1
        },
        {
          shopordernumber: 'MT000000043434',
          status: 3
        },
        {
          shopordernumber: 'MT000000043435',
          status: 4
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
      console.log(value)
      this.ToGetShopList(value)
    },
    goDetail (shopid) {
      this.$router.push('./orderDetail?id=' + shopid)
    },
    scroll (scrollData) {
      console.log(scrollData)
    },
    loadmore (index) {
      this.data = this.data.concat(this.data)
    },
    ToGetShopList (ordernumber) {
      http(GetMyShopList, {ordernumber: ordernumber}, json => {
        if (json.code === 0) {
          this.data = json.response.list
        }
        //  console.log("111");
      })
    },
    ToGetShopDeatilLike (value) {
      // 1  未发货    2 配送中  3确认收货   4己完成
      http(GetShopDeatilLike, { key: value }, json => {
        if (json.code === 0) {
          console.log(json)
          this.data = json.response.list
          // this.username = json.response.nickname
        }
      })
    }
  },
  created () {
    this.ToGetShopList('')
  }
}
</script>
<style lang='less' scoped>

.shopListWrap {
  position: relative;
  .innerScroll {
  /deep/.wrapper {
    .bscroll-container {
      min-height: calc(100vh - 400px) !important;
    }
  }
}
   /deep/.toptab_search {
     margin-top: 80px;
   }
  .shop{
 padding: 0 58px;
  }
  .info {
    padding-top: 80px;
    li {
      height: 151px;
      background: #ffffff;
      border-radius: 30px;
      line-height: 151px;
      display: flex;
      margin-bottom: 20px;
      justify-content: space-between;
      padding: 0 40px;
      align-items: center;
      span {
        color: #767c8f;
        font-size: 51px;
        font-weight: bold;
        color: #767c8f;
      }
    }
  }
  .tag {
    width: 200px;
    height: 82px;
    line-height: 82px;
    border-radius: 40px;
    text-align: center;
    color: #fff;
    font-size: 38px;
  }
  .ytag {
    background: #efb618;
  }
  .gtag {
    background: #1b8710;
  }
  .rtag {
    background: #7d0d0d;
  }
  .graytag {
    background: #c6d0de;
  }
}
</style>
