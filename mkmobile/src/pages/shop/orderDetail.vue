<template>
  <div class="orderDetailWrap">
    <TopBar class="center-one-search" :option="topBarOption">订单详情</TopBar>
    <div>
      <div class="shop clearfix">
        <!--<TopSearch @onSearch="search" placeinputValue="输入订单号查询"></TopSearch>-->
        <ScrollRefresh
          @getData="ToGetShopList"
          :residualHeight="topbarHeight+bottomTabBarHeight+10"
          :isNeedUp="false"
          class="innerScroll"
        >
          <div class="innerWrap">
            <div class="goodsInfo">
              <span>{{data.shopordernumber}}</span>
              <div class="tag ytag" v-if="data.status===1">未发货</div>
              <div class="tag gtag" v-else-if="data.status===2">配送中</div>
              <div class="tag rtag" v-else-if="data.status===3">确认收货</div>
              <div class="tag graytag" v-else-if="data.status===4">己完成</div>
            </div>
            <div class="goods base-flex flex-start p-58 borderR mb-80">
              <img :src="getimgurl(data.shopsku.detailicon)" class="img" alt />
              <div class="goods-info">
                <div class="tip-titl">{{data.shopdetail.shopname}} : {{data.shopsku.detaildesc}}</div>
                <div>数量:{{data.shopnum}}    总价:{{data.shopprice}}</div>

              </div>
            </div>
            <div class="distribution">
              <div class="dTitle">配送信息：</div>
              <ul class="dInfo">
                <li>{{data.buyaddr}}</li>
                <li>{{data.phonename}}</li>
                 <li>备注:{{data.remark}}</li>
              </ul>
            </div>
            <div class="wuliu">
              <div class="title">配送公司：{{data.company}} 快递单号:{{data.trackingnumber}}</div>
              <ul class="steps">
            <!--    <li v-for="(item,index) in steps" :key="index">
                  <span class="time_steps">{{item.time}}</span>
                  <i :class="['iconfont', icons[item.type]]"></i>
                  <div class="info_steps">
                    <span class="title_steps">{{item.title}}</span>
                    <span class="subInfo_steps">{{item.subInfo}}</span>
                  </div>
                  <div class="stepLine"></div>
                </li> -->
              </ul>
            </div>
          </div>
        </ScrollRefresh>
      </div>
    </div>
    <YellowComfirm :show="showComfirm" :tipTitle="data.tips"></YellowComfirm>
  </div>
</template>
<script>
import TopBar from 'components/TopBar'
import TopSearch from 'components/TopSearch'
import { http } from 'util/request'
import { config } from 'util/config'
import { GetShopList, GetUserInfo, GetShopDeatilLike, GetShopDetailsMyweb } from 'util/netApi'
import { storage } from 'util/storage'
import { accessToken, loginPro } from 'util/const.js'
import YellowComfirm from 'components/YellowComfirm'
import ScrollRefresh from 'components/ScrollRefresh'
export default {
  name: 'orderDetail',
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
      data:
      {
        shopdetail:{
        Shopgroup: 1,
        createTime: "2020-12-03 16:21",
        id: 1,
        isDelete: false,
        minLevel: 1,
        pDesc: "摩奇猴666套装",
        pDetailIcon: "",
        pIcon: "shopimg_1.png",
        pName: "摩奇猴666套装",
        pNum: 9993,
        price: 666,
        priceType: 1,
        ptype: 0,
        status: 1,
        },
        shopsku:
        {
        createtime: "2021-01-06 00:00",
        detaildesc: "黑色-S码",
        detailicon: "shopimg_1.png",
        detailname: "S",
        detailnum: 199,
        detailprice: 300,
        id: 1,
        skuid: 1
        },
      shopskudetail:{
      createtime: "2021-01-06 00:00",
      id: 1,
      shopid: 1,
      skuIcon: "shopimg_1.png",
      skudesc: "黑色的东西",
      skuname: "黑色",},
        shopidid: 0,
        showComfirm: true,
        tips: '即將更新！',
        trackingnumber: '',
        buyaddr: '',
        phonename: '',
        shopname: '',
        shopnum: 0,
        shopprice: 0,
        shopordernumber: 'MT000000043432',
        status: 2,
        remark: ''
      },
      imgicon: '',
      col: 2,
      company: '顺丰快递',
      icons: ['dot', 'iconlist', 'iconcangku', 'iconjiantoushang', 'icongoodswhoutStock', 'iconfahuo', 'iconcar', 'iconyunshu', 'iconperson', 'iconchaibaoguoqujian-xianxing', 'iconright'],
      steps: [
        // type:0 点  type:1 已下单
        {
          time: '11-12 12:11',
          title: '已签收',
          subInfo: '店完成取件，感谢使用菜鸟驿站，期待再次为您服务。',
          type: 10
        },
        {
          time: '11-12 12:11',
          title: '代取件',
          subInfo: '请凭取货码及时领取。如有疑问请联系19925322056',
          type: 9
        },
        {
          time: '11-12 12:11',
          title: '派送中',
          subInfo:
            '深圳观兰D站派件员：葛细球 电话：18575590297 当前正在为您派件',
          type: 8
        },
        {
          time: '11-12 12:11',
          title: '运输中',
          subInfo:
            '深圳观兰D站派件员：葛细球 电话：18575590297 当前正在为您派件',
          type: 7
        },
        {
          time: '11-12 12:11',
          subInfo: '您的包裹已到达【深圳观兰D站】，准备分配派送员',
          type: 0
        },
        {
          time: '11-12 12:11',
          subInfo: '您的包裹已到达1',
          type: 0
        },
        {
          time: '11-12 11:11',
          title: '已揽件',
          subInfo: '您的包裹已由物流公司揽收',
          type: 6
        },
        {
          time: '11-12 12:11',
          title: '已发货',
          subInfo: '包裹正在等待揽收',
          type: 5
        },
        {
          time: '11-12 12:11',
          title: '已出库',
          subInfo: '包裹已出库',
          type: 4
        },
        {
          time: '11-12 11:11',
          title: '仓库处理中',
          subInfo: '商品已打包',
          type: 3
        },
        {
          time: '11-12 12:11',
          subInfo: '订单已打印',
          type: 0
        },
        {
          time: '11-12 12:11',
          title: '仓库已接单',
          subInfo: '订单已入库',
          type: 2
        },
        {
          time: '11-12 11:11',
          title: '已下单',
          subInfo: '商品已经下单',
          type: 1
        }
      ]
    }
  },
  computed: {
    itemWidth () {
      return (document.documentElement.clientWidth - 60) / 2
    },
    gutterWidth () {
      return 30 * 0.5 * (document.documentElement.clientWidth / 375)
    }
  },
  methods: {
    getimgurl(imgurl)
    {
      return config.shopimgUrl+imgurl;
    },
    onSearch (value) {
      this.ToGetShopDeatilLike(value)
    },
    goDetail (shopid) {
      this.$router.push('./shopDetail?id=' + shopid)
    },
    scroll (scrollData) {
      console.log(scrollData)
    },
    loadmore (index) {
      this.data = this.data.concat(this.data)
    },
    ToGetShopList () {
      http(GetShopDetailsMyweb, {id: this.shopid}, json => {
        if (json.code === 0) {
          this.data = json.response.datainfo[0]
        }
      })
    }
  },
  created () {
    this.shopid = this.$route.query.id
    this.ToGetShopList()
  }
}
</script>
<style lang='less' scoped>
.orderDetailWrap .innerScroll {
  /deep/.wrapper {
    padding-top: 80px;
    .bscroll-container {
      min-height: calc(100vh - 400px) !important;
    }
  }
}

.orderDetailWrap {
  position: relative;
  .innerWrap {
    padding-top: 80px;
  }
  .shop {
    padding: 0 58px;
  }
  .goodsInfo {
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
  .goods-info {
    font-weight: bold;
    font-size: 33px;
    color: #000000;
    opacity: 0.75;
    flex: 1;
  }
  .goods {
    min-height: 158px;
    align-items: center;
    padding: 30px;
    margin: 56px auto 50px;
    background-color: #dee3ee;
    border-radius: 36px;
    .img {
      width: 151px;
      // height: 184px;
      margin-right: 70px;
    }
  }
  .distribution {
    background: #dee3ee;
    border-radius: 36px;
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
  .wuliu {
    background: #ffffff;
    border-radius: 36px;
    padding: 30px 40px;
    margin-top: 40px;
    .title {
      font-weight: bold;
      color: #000000;
      line-height: 130px;
      opacity: 0.75;
    }
  }
  .steps {
    li {
      display: flex;
      align-items: initial;
      position: relative;
      width: 100%;
      padding-bottom: 30px;
    }
    .iconfont {
      font-size: 40px;
      position: absolute;
      top: 0px;
      left: 125px;
      z-index: 2;
      width: 50px;
      height: 50px;
      background-color: #ccc;
      color: #fff;
      border-radius: 50%;
      text-align: center;
      line-height: 50px;
    }
    .iconfont.dot {
      top: 0px;
      left: 142px;
      width: 16px;
      height: 16px;
    }
    .time_steps {
      width: 100px;
      margin-right: 100px;
      text-align: center;
      font-size: 30px;
    }
    .info_steps {
      flex: 1;
      display: flex;
      flex-direction: column;
      .title_steps {
        font-weight: bold;
        color: #000000;
        opacity: 0.75;
        font-size: 36px;
      }
      .subInfo_steps {
        opacity: 0.75;
        font-size: 30px;
      }
    }
  }
  .stepLine {
    position: absolute;
    background-color: #ebedf0;
    -webkit-transition: background-color 0.3s;
    transition: background-color 0.3s;
    top: 16px;
    left: 150px;
    width: 1px;
    height: 100%;
  }
  .steps li:last-child .stepLine {
    width: 0;
  }
  .steps li:first-child .iconfont {
    color: #fff;
    background-color: #e53d26;
  }
}
</style>
