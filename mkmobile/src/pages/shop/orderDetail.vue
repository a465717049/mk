<template>
  <div class="orderDetailWrap">
    <TopBar class="center-one-search" :option="topBarOption">订单详情</TopBar>
    <div>
      <div class="shop clearfix">
        <TopSearch @onSearch="search" :placeinputValue="''"></TopSearch>
        <ScrollRefresh
          @getData="ToGetShopList"
          :residualHeight="200"
          :isNeedUp="false"
          class="innerScroll"
        >
          <div class="innerWrap">
            <div class="goodsInfo">
              <span>{{orderNo}}</span>
              <div class="tag ytag" v-if="orderType===1">未发货</div>
              <div class="tag gtag" v-else-if="orderType===2">配送中</div>
              <div class="tag rtag" v-else-if="orderType===3">确认收货</div>
              <div class="tag graytag" v-else-if="orderType===4">己完成</div>
            </div>
            <div class="goods base-flex flex-start p-58 borderR mb-80">
              <img src="@/assets/imgs/tipimg.png" class="img" alt />
              <div class="goods-info">
                <div class="tip-titl">摩奇猴套装系列（A001)</div>
                <div>赠送：面膜+修复水</div>
              </div>
            </div>
            <div class="distribution">
              <div class="dTitle">配送信息：</div>
              <ul class="dInfo">
                <li>广东省深圳市龙华福龙大厦530室</li>
                <li>150****890 李红</li>
              </ul>
            </div>
            <div class="wuliu">
              <div class="title">
                配送公司：{{company}}
              </div>
              <van-steps direction="vertical" :active="0">
                <van-step>
                  <h3>【城市】物流状态1</h3>
                  <p>2016-07-12 12:40</p>
                </van-step>
                <van-step>
                  <h3>【城市】物流状态2</h3>
                  <p>2016-07-11 10:00</p>
                </van-step>
                <van-step>
                  <h3>快件已发货</h3>
                  <p>2016-07-10 09:30</p>
                </van-step>
              </van-steps>
            </div>
          </div>
        </ScrollRefresh>
      </div>
    </div>
    <YellowComfirm :show="showComfirm" :tipTitle="tips"></YellowComfirm>
  </div>
</template>
<script>
import TopBar from "components/TopBar";
import TopSearch from "components/TopSearch";
import { http } from "util/request";
import { GetShopList, GetUserInfo, GetShopDeatilLike } from "util/netApi";
import { storage } from "util/storage";
import { accessToken, loginPro } from "util/const.js";
import YellowComfirm from "components/YellowComfirm";
import ScrollRefresh from "components/ScrollRefresh";
export default {
  name: "orderDetail",
  components: {
    TopBar,
    TopSearch,
    YellowComfirm,
    ScrollRefresh
  },
  data() {
    return {
      showChat: false,
      placeinputValue: "",
      topBarOption: {
        iconLeft: "iconShapecopy",
        iconRight: "iconxiaoxi1"
      },
      showComfirm: true,
      tips: "即將更新！",
      orderNo: "MT000000043432",
      orderType: 2,
      col: 2,
      company:'顺丰快递',
    };
  },
  computed: {
    itemWidth() {
      return (document.documentElement.clientWidth - 60) / 2;
    },
    gutterWidth() {
      return 30 * 0.5 * (document.documentElement.clientWidth / 375);
    }
  },
  methods: {
    onSearch(value) {
      this.ToGetShopDeatilLike(value);
    },
    goDetail(shopid) {
      this.$router.push("./shopDetail?id=" + shopid);
    },
    scroll(scrollData) {
      console.log(scrollData);
    },
    loadmore(index) {
      this.data = this.data.concat(this.data);
    },
    ToGetShopList() {
      http(GetShopList, null, json => {
        if (json.code === 0) {
          this.data = json.response.list;
        }
      });
    },
    ToGetShopDeatilLike(value) {
      console.log(value);
      http(GetShopDeatilLike, { key: value }, json => {
        if (json.code === 0) {
          console.log(json);
          this.data = json.response.list;
          // this.username = json.response.nickname
        }
      });
    }
  },
  created() {
    this.ToGetShopList();
  }
};
</script>
<style lang='less' scoped>
.innerScroll {
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
      height: 184px;
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
    background: #FFFFFF;
    border-radius: 36px;
    padding: 30px 40px;
    margin-top: 40px;
    .title {
      font-weight: bold;
      color: #000000;
      line-height: 130px;
      opacity: 0.75;
    }
    /deep/ .van-step--vertical{
     line-height: 40px; 
    }
  }
}
</style>