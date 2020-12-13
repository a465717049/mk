<template>
  <div>
    <TopBar class="center-one-search">設置語言</TopBar>
    <div class="lan-set border-top-radius relative">
      <div class="set borderR clearfix p-58">
        <div class="relative special">
          <img src="@/assets/imgs/set/B-5.png" class="img" alt />
          <van-cell class="cell-info borderR mb-40" title="语言" arrow-direction="down" is-link url />
        </div>
        <div class="relative cell-special">
          <van-cell class="cell-info borderR mb-40 active" title="中文繁体" is-link url >
            <i class="iconfont icondui"></i>
          </van-cell>
              <van-cell class="cell-info borderR mb-40" title="ENGLISH" is-link url >
            <i class="iconfont icondui"></i>
          </van-cell>
              <van-cell class="cell-info borderR mb-40" title="日文" is-link url >
            <i class="iconfont icondui"></i>
          </van-cell>
              <van-cell class="cell-info borderR mb-40" title="韩文" is-link url >
            <i class="iconfont icondui"></i>
          </van-cell>
        </div>
        <!-- <div class="relative cell-special">
          <van-cell class="cell-info borderR mb-40"
                    title="ENGLISH"
                    is-link
                    url="" />
        </div>
        <div class="relative cell-special">
          <van-cell class="cell-info borderR mb-40"
                    title="JAPAN"
                    is-link
                    url=" " />
        </div>
        <div class="relative cell-special">
          <van-cell class="cell-info borderR mb-40"
                    title="KOREA"
                    is-link
                    url=" " />
        </div>-->
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
  data() {
    return {};
  },
  methods: {},
  created() {},
  mounted() {}
};
</script>
<style lang='less' scoped>
.set {
  // margin-top: -160px;
  border-radius: 40px 40px 0 0;
  .cell-info {
    height: 140px;
    line-height: 120px;
    .iconfont{
      opacity: 0;
    }
  }
  .cell-info.active {
    border: 8px solid #494b63;
           .iconfont {
        position: absolute;
        font-size: 60px;
        color: #4493d5;
        opacity: 1;
        top: -6px;
        right: 20px;
      }
  }
  .img {
    position: absolute;
    left: 20px;
    top: 10px;
    width: 106px;
    height: 106px;
    z-index: 99;
  }
  /deep/.van-cell__title {
    font-size: 42px;
    margin-left: 40px;
    font-weight: bold;
    color: rgba(118, 124, 143, 1);
    position: relative;
  }
  .special {
    /deep/.van-cell__title {
      margin-left: 140px;
    }
  }
  /deep/.van-cell__right-icon {
    font-size: 60px;
    height: 137px;
    line-height: 120px;
    margin-right: 15px;
  }

  .cell-special {
    /deep/.van-icon-arrow {
      display: none;
    }
  }
}
</style>