<template>
  <div class="newsDetailsWrapper">
    <TopBar  class='center-one-search'  :option="topBarOption">
     詳情
    </TopBar>
    <div class="innerWrap">
      <div class="w90">
        <div class="top">
          <p class="detailsTile">{{detailsTile}}</p>
          <div class="bottom">
            <div class="times">{{timeZone}}</div>
            <!-- 
            <div class="ago">{{time}}</div>
            <div class="eyes">
              <i class="iconfont iconeye"></i>
              <span>{{seeNumber}}</span>
            </div>
            -->
          </div>
        </div>
        <div class="article" v-html='details' @click='ck'>
      
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
  data() {
    return {
      topBarOption: {
        iconLeft: 'back',
        iconRight: ''
      },
      detailsTile: '',
      timeZone: '',
      details: '',
      //  time: '13小时前',
      // seeNumber: '45,000',
      banner: banner,
      detailid: 0
    }
  },
  components: {
    TopBar
  },
  computed: {},
  methods: {
    ToGetNewsDeatilWeb(value) {
      http(GetNewsDeatilWeb, { id: value }, json => {
        if (json.code === 0) {
          this.detailsTile = json.response.title
          this.timeZone = json.response.date
          this.banner = json.response.img
          this.details = json.response.detail
        }
      })
    },
    ck(){
      return false;
    }
  },
  created() {
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
    height: calc(100vh - 400px);
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
      text-align: center!important;
      margin-top: 60px;
    }
    .bottom {
       text-align: center;
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
        float: right;
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
