<template>
  <div class="newsIndexWrapper">
    <TopBar class='center-one-search' :option="topBarOption">
      分析
    </TopBar>
    <div class="innerWrap">
      <div class="w90">
        <!-- <div class="banner">
          <img src="../../assets/imgs/banner.png" alt />
        </div>-->
        <div class="newsList" ref="listWrap">
          <ul>
            <li
              @click="onNext(item.id)"
              class="clearfix"
              v-for="(item,index) in newsList"
              :key="index"
            >
             <div class="fl" > 
               <img src="@/assets/imgs/NewsImg2.png" alt/>
               </div>
              <div class="right fr">
                <div class='title'>{{item.title}}</div>
               
                
                 <div class='message'>{{item.message}}</div><div class="bottom">
                  <div class="ago">{{item.date}}</div>
    
                </div>
              </div>
            </li>
          </ul>
        </div>
      </div>
    </div>
  </div>
</template>
<script type="text/javascript">
import TopBar from 'components/TopBar'
import BScroll from 'better-scroll'
import { http } from 'util/request'
import { GetNewsWeb } from 'util/netApi'
import { storage } from 'util/storage'
import { accessToken, loginPro } from 'util/const.js'
export default {
  data() {
    return {
      topBarOption: {
        iconLeft: 'iconlist2f',
        iconRight: ''
      },
      ntype:null,
      newsList: []
    }
  },
   watch: {
      ntype(val) {
        this.value = this.ntype;
         
      this.ToGetNewsWeb()
      }
    },
  components: {
    TopBar
  },
  computed: {},
  mounted() {
    ntype=this.$route.query.type
      this.ToGetNewsWeb()
  },
  methods: {
    onNext(value) {
      this.$router.push('./NewsDetail?id=' + value)
    },
    ToGetNewsWeb() {
      http(GetNewsWeb, {type:1}, json => {
        if (json.code === 0) {
          this.newsList = json.response
        }
      })
    },
    // scrollInit() {
    //   if (!this.scroll) {
    //     this.scroll = new BScroll(this.$refs.listWrap, {
    //       scrollY: true,
    //       click: true,
    //       bounce: {
    //         top: true,
    //         bottom: true
    //       }
    //     });
    //   } else {
    //     this.scroll.refresh();
    //   }
    // },
    getTabbar() {
      http(goodscollectionList).then(res => {
        console.log('11')
      })
    }
  },
  created() {
    this.ToGetNewsWeb()
  }
}
</script>

<style lang="less" scoped>

.newsIndexWrapper {
  height: 100vh;
  .innerWrap {
    width: 100vw;
    background-color: #ebeaf0;
    border-radius: 40px 40px 0 0;
    margin-top: -20px;
    padding-top: 30px;
    z-index: 1000;
    height: calc(100vh - 460px);
    overflow: auto;
  }
  .w90 {
    width: 90%;
    margin: 0 auto;
  }
  // .banner {
  //   width: 100%;
  //   height: 380px;
  //   border-radius: 30px;
  //   overflow: hidden;
  //   margin-bottom: 30px;
  //   img {
  //     width: 100%;
  //     height: 100%;
  //   }
  // }
  .newsList {
    padding-top: 50px;
    li {
      background-color: #fff;
      margin-bottom: 40px;
      border-radius: 20px;
      padding: 30px;
    }
    img {
      height: 257px;
      width: 385px;
    }
    .right {
      width: 54%;
      p {
        font-size: 30px;
        color: #000;
        font-weight: 700;
        height: 126px;
        line-height: 42px;
        overflow: hidden;
        text-overflow: ellipsis;
        display: -webkit-box;
        -webkit-line-clamp: 3;
        -webkit-box-orient: vertical;
        margin-bottom: 60px;
      }
    }
    .bottom {
      .iconfont {
        color: #ff780b;
        vertical-align: middle;
      }
      .iconeye:before {
        font-size: 46px;
      }

      div {
        display: inline-block;
        font-size: 24px;
      }
      
    }
  }
}
  .f1 {
      border-radius: 20px !important;
      overflow: hidden;
      img{
           border-radius: 20px !important;
          overflow: hidden;
      }
      
    }
    .right{
      .title{
        font-weight: 700;
        font-size: 35px;
      }
      .message{
        font-size: 32px;
         margin-top: 15px;
         margin-left: 15px;
          color: #666;
      }
      .ago{
        margin-left: 15px;
         font-size: 38px;
         color: #666;
      }
    }
</style>
