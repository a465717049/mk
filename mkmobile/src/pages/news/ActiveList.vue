<template>
  <div class="newsIndexWrapper">
    <TopBar class="center-one-search">活动</TopBar>
     <ScrollRefresh
      @getData="ToGetNewsWeb"
      :residualHeight="topbarHeight+bottomTabBarHeight+10"
      :isNeedUp="false"
    >
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
              <div class="fl">
                <img src="@/assets/imgs/defaultNews.png" alt />
              </div>
              <div class="right fr">
                <p class="title">{{item.title}}</p>

                <!-- <div class="message">{{item.message}}</div> -->
                <div class="bottom">
                  <div class="name">{{item.name}}</div>
                  <div class="ago">{{item.time}}</div>
                  <div class="see">
                    <i class="iconfont iconeye"></i>
                    {{item.num}}
                  </div>
                </div>
              </div>
            </li>
          </ul>
        </div>
      </div>
    </div>
        </ScrollRefresh>
  </div>
</template>
<script type="text/javascript">
import TopBar from 'components/TopBar'
import { http } from 'util/request'
import { GetNewsWeb } from 'util/netApi'
import ScrollRefresh from 'components/ScrollRefresh'
import { storage } from 'util/storage'
import { accessToken, loginPro } from 'util/const.js'
export default {
  data () {
    return {
      topBarOption: {
        iconLeft: 'iconmenu2',
        iconRight: ''
      },
    
      newsList: [
      ]
    }
  },
  components: {
    TopBar,
    ScrollRefresh
  },
  computed: {},
  mounted () {
    this.ToGetNewsWeb()
  },
  methods: {
    onNext (value) {
      this.$router.push('./NewsDetail?type=1&id=' + value)
    },
    ToGetNewsWeb () {
      http(GetNewsWeb, { type:1 }, json => {
        if (json.code === 0) {
          this.newsList = json.response
        }
      })
    },

    getTabbar () {
      http(goodscollectionList).then(res => {
      })
    }
  },
  created () {
    this.ToGetNewsWeb()
  }
}
</script>

<style lang="less" scoped>
.newsIndexWrapper {
  /deep/.wrapper {
    padding-top: 20px;
    .bscroll-container {
      min-height: calc(100vh - 400px);
    }
  }
  .w90 {
    width: 90%;
    margin: 0 auto;
  }
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
      .title {
        font-size: 30px;
        color: #000;
        font-weight: 700;
        height: 120px;
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
      display: flex;
      align-items: center;
      justify-content: space-between;
      .see {
        display: flex;
        align-items: center;
        font-size: 26px;
        font-weight: 600;
      }
      .name {
        font-weight: 700;
      }
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
  img {
    border-radius: 20px !important;
    overflow: hidden;
  }
}
// .right {
//   .title {
//     font-weight: 700;
//     font-size: 30px;
//     overflow: hidden;
//     text-overflow: ellipsis;
//     display: -webkit-box;
//     -webkit-line-clamp: 3;
//     -webkit-box-orient: vertical;
//   }
//   .message {
//     font-size: 32px;
//     margin-top: 15px;
//     margin-left: 15px;
//     color: #666;
//   }
//   .bottom {
//     display: flex;
//   }
//   .ago {
//     margin-left: 15px;
//     font-size: 38px;
//     color: #666;
//   }
// }
</style>
