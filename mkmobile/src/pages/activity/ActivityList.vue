<!--  -->
<template>
  <div class="activity-list">
    <TopBar class="center-one-search" :option="option">
     活動
    </TopBar>
    <div class="scrollPart"
         ref="scrollPart">
      <div class="activity borderR bg-gray p-58">
        <div class="tips base-flex flex-start p-58 bg-white borderR mb-80">
          <img src="@/assets/imgs/forgot1-img1.png"
               class="img"
               alt />
          <div class="tips-part">
            <div class="tip-titl"></div>
            <!-- <div>任何活动您只需交纳EP后剩下的事情由官方 合作伙伴来解决。</div> -->
            <div>即將更新！</div> 
          </div>
        </div>
        <div class="ac-detail base-flex bg-darkgray p-58 borderR"
             v-for="(item,listIndex) in activityList"
             :key="listIndex">
          <div class="left-img">
            <van-swipe :autoplay="3000"
                       @change="onChange(item,$event)"
                       class="sweiper1">
              <van-swipe-item v-for="(image, imagesIndex) in item.images"
                              :key="imagesIndex">
                <img v-lazy="image" />
              </van-swipe-item>
              <template #indicator>
                <div class="custom-indicator">
                  <span class="current-index">{{ item.currentImg + 1 }}</span> /
                  <span>{{item.images.length}}</span>
                </div>
              </template>
            </van-swipe>
          </div>
          <div class="right-info Tleft">
            <div class="title base-purple mb-20">{{item.title}}</div>
            <div class="d-info">发起团队：{{item.team}}</div>
            <div class="d-info">地区：{{item.region}}</div>
            <div class="d-info">保险：{{item.isIncludEinsurance}}</div>
            <div class="d-info">住宿：{{item.isIncludAccommodation}}</div>
            <div class="d-info">时间：{{item.time}}</div>
            <div class="price-info base-purple">原价：{{item.originalCost}} 团队赞助：{{item.sponsor}}</div>
            <div class="base-purple d-pay">
              <span>{{item.currentPrice}}</span>
              <span>仅需支付</span>
            </div>
            <div class="btn-info base-flex around cWhite">
              <button>立即报名</button>
              <button>活动详情</button>
              <button>咨询客服</button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import TopBar from "components/TopBar";
import BScroll from "better-scroll";
export default {
  name: "Activity",
  components: {
    TopBar
  },
  data () {
    return {
      currentImg: 0,
      option: {
        iconLeft: "iconzhankai",
      },
      activityList: [
       
      ],
    };
  },
  methods: {
    onChange (item, index) {
      // 此时的index指的是当前images数组里面显示图片的index
      item.images.forEach(element => {
        this.$set(item, 'currentImg', index)
      });
    }
    // scrollInit() {
    //   if (!this.scroll) {
    //     this.scroll = new BScroll(this.$refs.scrollPart, {
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
    // }
  },
  created () { },
  mounted () {
    // this.scrollInit();
  }
};
</script>
<style lang='less' scoped>
.activity-list {
  .scrollPart {
    height: calc(100vh - 550px);
    overflow: auto;
    border-radius: 40px;
    margin-top: -20px;
    .activity {
      height: auto;
      overflow: scroll;
      background: #ebeaf0;
      .tips-part {
        font-weight: bold;
        color: rgba(52, 52, 52, 1);
      }
      .tips {
        min-height: 158px;
        align-items: center;
        padding: 30px 58px;
        .img {
          width: 148px;
          height: 97px;
          margin-right: 58px;
        }
      }
    }
    .ac-detail {
      // height: 720px;
      padding: 44px;
      margin-bottom: 46px;
      .left-img {
        width: 32%;
        img {
          width: 330px;
          height: 202px;
        }
        .custom-indicator {
          text-align: center;
          .current-index {
            color: rgba(100, 24, 195, 1);
          }
          font-weight: bold;
          line-height: 52px;
        }
      }
      .right-info {
        width: 66%;
        font-weight: bold;
        .title {
          margin-bottom: 20px;
        }
        .d-info {
          color: rgba(53, 53, 53, 1);
          line-height: 48px;
        }
        .price-info {
          margin-top: 50px;
          line-height: 58px;
          font-size: 28px;
          border-bottom: 5px solid #6e21d1;
        }
        .d-pay {
          font-weight: bold;
          span:first-child {
            font-size: 70px;
          }
        }
        .btn-info {
          margin-top: 30px;
          margin-bottom: 30px;
          button {
            width: 172px;
            height: 71px;
            background: rgba(100, 24, 195, 1);
            border-radius: 21px;
            font-size: 26px;
          }
        }
      }
    }
  }
}
</style>