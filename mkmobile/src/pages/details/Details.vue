<template>
  <div>
    <TopBar class="center-one-search"
            :option="topBarOption">SetUp</TopBar>

    <div class="set borderR bg-gray clearfix p-58">
      <div class="relative">
        <img src="@/assets/imgs/set/person.png"
             class="img"
             alt="" />
        <van-cell class="cell-info borderR mb-100"
                  title="Totay cyels" />
        <span class="date-info">20-06-22 12:45:01</span>
      </div>
      <div class="relative">
        <img src="@/assets/imgs/set/B-2.png"
             class="img"
             alt="" />
        <van-cell class="cell-info borderR mb-40"
                  title="中级农场" />
      </div>
      <div class="relative">
        <img src="@/assets/imgs/set/b-3.png"
             class="img"
             alt="" />
        <van-cell class="cell-info borderR mb-40"
                  title="100" />
      </div>
      <div class="relative">
        <img src="@/assets/imgs/set/b-4.png"
             class="img"
             alt="" />
        <van-cell class="cell-info borderR mb-40"
                  title="100" />
      </div>
      <div class="relative">
        <img src="@/assets/imgs/set/B-5.png"
             class="img"
             alt="" />
        <van-cell class="cell-info borderR mb-40"
                  title="0" />
      </div>
      <div class="relative">
        <img src="@/assets/imgs/set/B-5.png"
             class="img"
             alt="" />
        <van-cell class="cell-info borderR mb-40"
                  title="0" />
      </div>
      <div class="arrow center base-flex flex-center">
        <div class="arrow-show base-flex flex-center">
          <i class="iconfont iconarrow-left author font80"></i>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import TopBar from "components/TopBar";
export default {
  name: "Set",
  components: {
    TopBar
  },
  data () {
    return {
      topBarOption: {
        iconLeft: "iconlist2f",
        iconRight: ""
      }
    };
  },
  methods: {},
  created () { },
  mounted () { }
};
</script>
<style lang="less" scoped>
.set {
  margin-top: -160px;
  border-radius: 40px 40px 0 0;
  .cell-info {
    height: 137px;
    line-height: 120px;
  }
  .date-info {
    position: absolute;
    bottom: 30px;
    right: 30px;
    font-size: 33px;
    font-weight: bold;
    color: rgba(118, 124, 143, 1);
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
  .cell-special {
    /deep/.van-cell__title {
      margin-left: 600px;
    }
  }
  .arrow {
    margin-top: 50px;
    .arrow-show {
      width: 186px;
      height: 186px;
      align-items: center;
      background: url('../../assets/imgs/set/squer-img.png') center center /
        100% 100%;
      .author {
        color: #7ab4b4;
      }
    }
  }
}
</style>
