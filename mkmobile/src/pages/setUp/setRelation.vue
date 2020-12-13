<template>
  <div>
    <TopBar class="center-one-search" :option="topBarOption">昵称</TopBar>

    <div class="listWrap" ref="listWrap">
      <div class="set borderR bg-gray clearfix p-58">
        <div class="title">设置昵称</div>
        <div class="relative rel-name">
          <img src="@/assets/imgs/set/person.png" class="img" alt />
          <input type="text" v-model="name" />
        </div>

        <div class="title">选择一个头像</div>
        <ScrollRefresh @getData="getData" :residualHeight="380" :isNeedUp="false" class="innerScroll">
          <div class="relative protainer-box">
            <div class="one-part" v-for="(item,index) in photoList" :key="index">
              <img :src="item" class="img" alt />
            </div>
          </div>
        </ScrollRefresh>
        <div class="submit cWhite">提交</div>
      </div>
    </div>
  </div>
</template>
<script>
import TopBar from "components/TopBar";
import { photoList } from "util/const.js";
import ScrollRefresh from "components/ScrollRefresh";
export default {
  name: "Set",
  components: {
    TopBar,
    ScrollRefresh
  },
  data() {
    return {
      topBarOption: {
        iconLeft: "iconShapecopy",
        iconRight: ""
      },
      name: "",
      photoList
    };
  },
  mounted() {},
  computed: {},
  methods: {
    getData() {}
  }
};
</script>
<style lang='less' scoped>
.innerScroll {
  /deep/ .wrapper {
       background: #fff;
    border-radius: 42px;
    .bscroll-container {
    min-height: calc(100vh - 1000px);
  }
  }
}
.submit {
  width: 100%;
  height: 134px;
  line-height: 134px;
  text-align: center;
  background-color: #eecb08;
  font-size: 42px;
  border-radius: 30px;
  margin-top: 60px;
}
.listWrap {
  border-radius: 42px;
}
.set {
  height: auto;
  overflow: scroll;
  margin-top: -160px;
  border-radius: 40px 40px 0 0;
  .cell-info {
    height: 137px;
    line-height: 120px;
  }
  .rel-name {
    height: 139px;
    line-height: 136px;
    background-color: #fff;
    border-radius: 20px;
    display: flex;
    justify-content: space-between;
    align-items: center;
    input {
      right: 0;
      flex: 1;
      height: 100%;
      padding: 20px 40px;
      font-size: 50px;
      font-weight: 700;
      color: #666;
    }
    .img {
      width: 106px;
      height: 106px;
      z-index: 99;
      margin-left: 40px;
    }
  }
  .title {
    margin-top: 30px;
    margin-bottom: 30px;
    color: rgba(52, 50, 55, 1);
    opacity: 0.75;
    font-size: 50px;
  }
  .protainer-box {
 
    height: 100%;
    display: flex;
    flex-direction: row;
    flex-wrap: wrap;
    justify-content: space-around;
    padding: 48px;
    // overflow: auto;
    .one-part {
      width: 21%;
      height: 21%;
      margin-bottom: 130px;
      img {
        width: 100%;
        height: 100%;
      }
    }
  }

  /deep/.van-icon-arrow {
    display: none;
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
  .process {
    width: 384px;
    height: 54px;
    position: absolute;
    left: 160px;
    top: 40px;
    border-radius: 30px;
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
}
</style>