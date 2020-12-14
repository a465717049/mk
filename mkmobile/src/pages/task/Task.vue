<template>
  <div class="task">
    <TopBar class="center-one-search" :option="option">
      任务
    </TopBar>
    <div class="scrollPart" ref="scrollPart">
      <div class="content-part borderR bg-gray p-58">
        <div class="tips base-flex flex-start p-58 bg-white borderR mb-80">
          <img src="@/assets/imgs/forgot1-img1.png" class="img" alt />
         
             <div class="tips-part">
            <div class="tip-titl"></div>
            <!-- <div>任何活动您只需交纳EP后剩下的事情由官方 合作伙伴来解决。</div> -->
            <div>即將更新！</div> 
          </div>
        </div>
        <!-- <div
          class="task-info bg-darkgray borderR mb-80"
          :class="activatedIndex===index?'':'task-info-hide'"
          v-for="(item,index) in taskList"
          :key="index"
          @click="changeActivated(index)"
        >
          <div class="tips-part">
            任务：粉丝：{{item.fansNum}}
            <span>农场规模：{{item.scale}}</span>
          </div>
          <div class="relative mb-40">
            <img src="@/assets/imgs/set/B-1.png" class="img" alt />
            <van-cell class="cell-info borderR mb-40 cell-special" title="L4" />
            <van-progress class="process base-purple" :percentage="item.FProgress" />
          </div>
          <div class="relative">
            <img src="@/assets/imgs/set/B-1.png" class="img" alt />
            <van-cell class="cell-info borderR mb-40 cell-special" title="L4" />
            <van-progress class="process base-purple" :percentage="item.TProgress" />
          </div>
          <div class="desc tips-part">说明：{{item.explain}}</div>
          <div class="task-btn around base-flex">
            <button class="t-yellow cWhite s-btn">状态：未接受挑战</button>
            <button class="base-purple cWhite s-btn">接受挑战</button>
          </div>
        </div> -->
      
      </div>
    </div>
  </div>
</template>

<script>
import TopBar from "components/TopBar";

export default {
  name: "Task",
  components: {
    TopBar
  },
  data() {
    return {
      activatedIndex:0,
      option: {
        iconLeft: "iconlist2f",
       // image: require("@/assets/imgs/set/person.png")
      },
      taskList: [
        {
          fansNum: 111,
          scale: 12334,
          FProgress: 30,
          TProgress: 50,
          explain: "完成此任务可领取华为P40手机一台，详 情请咨询社区服务站。"
        },
        {
          fansNum: 111,
          scale: 12334,
          FProgress: 30,
          TProgress: 50,
          explain: "完成此任务可领取华为P40手机一台，详 情请咨询社区服务站。"
        },
        {
          fansNum: 111,
          scale: 12334,
          FProgress: 30,
          TProgress: 50,
          explain: "完成此任务可领取华为P40手机一台，详 情请咨询社区服务站。"
        }
      ]
    };
  },
  methods: {
    changeActivated(index){
      console.log(index,'index')
      this.activatedIndex = index
    }
  },
  created() {},
  mounted() {}
};
</script>
<style lang='less' scoped>
.task {
  .scrollPart {
    height: calc(100vh - 550px);
    overflow: auto;
    border-radius: 40px;
    margin-top: -20px;
  }

  .content-part {
    height: auto;
    overflow: scroll;

    background: #ebeaf0;
    .tips-part {
      font-weight: bold;
      color: rgba(52, 52, 52, 1);
      margin-bottom: 40px;
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
    .task-info {
      padding: 32px;
      .process {
        width: 760px;
        height: 54px;
        position: absolute;
        left: 160px;
        top: 40px;
        border-radius: 30px;
      }
      .cell-info {
        height: 137px;
        line-height: 120px;
      }
      .img {
        position: absolute;
        left: 20px;
        top: 10px;
        width: 106px;
        height: 106px;
        z-index: 99;
      }
      /deep/.van-cell__right-icon {
        font-size: 60px;
        height: 137px;
        line-height: 120px;
      }
      /deep/.van-cell__title {
        font-size: 42px;
        margin-left: 140px;
        font-weight: bold;
        color: rgba(118, 124, 143, 1);
        position: relative;
      }
      /deep/.van-progress {
        background: rgba(100, 24, 195, 1);
        border: 1px solid rgba(65, 3, 5, 1);
        box-shadow: 0px 5px 5px 0px rgba(0, 0, 0, 0.46);
      }
      /deep/.van-progress__portion {
        background: linear-gradient(
          0deg,
          rgba(79, 172, 254, 1),
          rgba(0, 242, 254, 1)
        );
        border: 1px solid rgba(65, 3, 5, 1);
        border-radius: 23px;
        height: 54px;
        line-height: 54px;
      }
      /deep/.van-progress__pivot {
        display: none;
      }
      .cell-special {
        /deep/.van-cell__title {
          margin-left: 600px;
        }
      }
      .desc {
        margin-top: 47px 0;
      }
      .task-btn {
        padding: 58px 0;
        .s-btn {
          margin-top: -30px;
          height: 160px;
          // text-align: center;
          border-radius: 21px;
          background: #ffbb18;
          font-size: 55px;
        }
        .s-btn:first-child {
          width: 60%;
        }
        .s-btn:last-child {
          width: 35%;
          background: #6e21d1;
        }
      }
    }
    .task-info.task-info-hide {
      height: 158px;
    
      padding: 30px 58px;
      background-color: #fff;
      .tips-part {
        font-weight: bold;
        color: rgba(52, 52, 52, 1);
        margin-bottom: 0px;
        font-size: 44px;
      }
      .relative,.desc,.task-btn{
        display: none;
      }
    }
  }
}
</style>