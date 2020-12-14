<template>
  <div class="newsDetailsWrapper">
    <TopBar  class='center-one-search' >
     资 讯
    </TopBar>
    <div class="innerWrap">
      <div class="w90">
        <div class="top">
          <p class="detailsTile">{{detailsTile}}</p>
          <div class="bottom">
            <div class="times">{{timeZone}}</div>

            <div class="ago">{{time}}</div>
            <div class="eyes">
              <i class="iconfont iconeye"></i>
              <span>{{seeNumber}}</span>
            </div>

          </div>
        </div>
        <div class="article" v-html='details'>

        </div>
      </div>
    </div>
  </div>
</template>
<script type="text/javascript">
import TopBar from 'components/TopBar'
import banner from '../../assets/imgs/banner.png'
import { http } from 'util/request'
import { GetNewsDeatilWeb } from 'util/netApi'
import { storage } from 'util/storage'
import { accessToken, loginPro } from 'util/const.js'
export default {
  data () {
    return {
      topBarOption: {
        iconLeft: 'back',
        iconRight: ''
      },
      detailsTile: '年假很低级啊很大声',
      timeZone: '方式方式的',
      details: 'Money is like water, and precision control means not only "injecting fresh water", but also "repairing drainage channels". In the guarantee "current" abundant at the same time, the diversion water into the canal, can "fish" raise, raise well Recently, on the basis of the previous structural monetary policy tools, the Peoples Bank of China has created two more monetary policy tools that are direct to the real economy -- the loan extension support Tool for inclusive small and micro businesses and the Credit loan Support Plan, so as to continuously enhance the targeted and valuable policies serving small and micro businesses. These two innovative policy tools directly link the operation of monetary policy with the financial support provided by financial institutions to small and micro enterprises, so as to ensure the continuous and accurate flow of financial water to the real economy, especially to small and micro enterprises, along the "canal". Precision regulation has been a major feature of recent macro policies. Not only monetary policy, but fiscal policy also emphasizes precision.',
      time: '13小时前',
      seeNumber: '45,000',
      banner: banner,
      detailid: 0
    }
  },
  components: {
    TopBar
  },
  computed: {},
  methods: {
    ToGetNewsDeatilWeb (value) {
      http(GetNewsDeatilWeb, { id: value }, json => {
        if (json.code === 0) {
          this.detailsTile = json.response.title
          this.timeZone = json.response.date
          this.banner = json.response.img
          this.details = json.response.detail
        }
      })
    }
  },
  created () {
    if (this.$route.query.id) {
      this.detailid = this.$route.query.id
      this.ToGetNewsDeatilWeb(this.detailid)
    }
  }
}
</script>

<style lang="less" scoped>
.newsDetailsWrapper {
  height: 100vh;
  .innerWrap {
    width: 100vw;
    background-color: #ebeaf0;
    border-radius: 40px 40px 0 0;
    margin-top: -40px;
    padding-top: 30px;
    height: calc(100vh - 360px);
    overflow: auto;
  }
  .w90 {
    width: 90%;
    margin: 0 auto;
  }
  .banner {
    width: 100%;
    margin: 80px 0;
    img {
      width: 100%;
    }
  }
  .top {
    .detailsTile {
      color: #6318c3;
      font-size: 50px;
      font-weight: 800;
      height: 80px;
      line-height: 60px;
      overflow: hidden;
      text-overflow: ellipsis;
      text-align: left!important;
      margin-top: 60px;
    }
    .bottom {
       text-align: left;
       font-size: 40px;
       line-height: 40px;
       margin: 0;
      .iconfont {
        color: #ff780b;
        vertical-align: middle;
      }
      .iconeye:before {
        font-size: 50px;
      }

      div {
        display: inline-block;
        font-size: 24px;
      }
      .eyes {
        // float: right;
        margin-left: 100px;
        color: #000;
        font-weight: 600;
      }
      .times {
        color: #666;
          font-size: 35px;
        font-weight: 600;
      }
    }
  }
  .article {

      margin-top:80px;
      font-size: 40px;
      line-height: 60px;
      color: #111;
      margin-bottom: 60px;

  }
}
</style>
