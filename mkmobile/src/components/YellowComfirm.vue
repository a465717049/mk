<template>
  <van-popup
    v-model="modelShow"
    :close-on-click-overlay="true"
    @click-overlay="clickOver"
    class="yellow-wrap"
  >
    <div class="brown-border clearfix">
      <div class="yellow-bg">
        <img class="item-l" src="@/assets/imgs/googl-header.png" alt />
        <i class="iconfont iconguanbi" @click="clickNo"></i>
        <div class="item-r">
          <h3 class="item-r-tit">提示：</h3>
          <div class="item-r-tip">{{tipTitle}}</div>
        </div>
      </div>
      <!-- <div class="btn" v-if="!showConfirmBtn" @click="clickOk">OK</div> -->
      <div v-if="showConfirmBtn" class="confirm-btn">
        <div class="first-btn" @click="clickOk">主農場</div>
        <div class="second-btn" @click="clickNo">子農場</div>
      </div>
      <div></div>
    </div>
  </van-popup>
</template>

<script>
export default {
  name: "YellowComfirm",
  components: {},
  props: {
    show: {
      type: Boolean,
      default: false
    },
    tipTitle: {
      type: String,
      default: "操作成功"
    },
    showConfirmBtn: {
      type: Boolean,
      default: false
    },
    first: {
      type: String,
      default: "主農場"
    },
    second: {
      type: String,
      default: "子農場"
    }
  },
  data() {
    return {
      modelShow: false
    };
  },
  watch: {
    show(newV) {
      this.modelShow = newV;
    },
    modelShow(newV) {
      this.$emit("changeModel", newV);
    }
  },
  methods: {
    clickOver() {
      this.$emit("clickOver", false);
    },
    //输入支付密码
    clickOk() {
      this.$emit("clickOk");
    },
    clickNo() {
       this.modelShow=false
      this.$emit("clickNo");
    }
  }
};
</script>

<style lang="less" scoped>
.yellow-wrap {
  background: #efb618;
  width: 76vw;
  border-radius: 30px;
  padding: 5px;
  .brown-border {
    padding: 40px 40px 20px 40px;
    border: 12px solid #fff;
    width: 100%;
    border-radius: 30px;
  }
  .btn {
    color: #fff;
    font-weight: bold;
    background: #3f1008;
    font-size: 46px;
    line-height: 70px;
    float: right;
    width: 180px;
    text-align: center;
    border-radius: 10px;
  }
  .confirm-btn {
    width: 70%;
    margin: 0 auto;
    margin-bottom: 30px;
    margin-top: 30px;
    display: flex;
    justify-content: space-around;
    .first-btn {
      display: inline-block;
      width: 200px;
      height: 57px;
      margin-right: 20px;
      line-height: 57px;
      background: rgba(65, 3, 5, 1);
      border-radius: 15px;
      color: #ffffff;
      font-size: 40px;
      text-align: center;
    }
    .second-btn {
      display: inline-block;
      width: 200px;
      height: 57px;
      line-height: 57px;
      background: rgba(65, 3, 5, 1);
      border-radius: 15px;
      color: #ffffff;
      font-size: 40px;
      text-align: center;
    }
  }
}
.yellow-bg {
  width: 100%;
  display: flex;
  position: relative;
  .item-l {
    width: 98px;
    height: 118px;
    margin-right: 60px;
  }
  .item-r {
    flex: 1;
    color: #3f1008;
    .item-r-tit {
      font-size: 33px;
      font-weight: bold;
      line-height: 60px;
      color: #410305;
    }
    .item-r-tip {
      font-size: 33px;
      line-height: 60px;
      color: #410305;
      display: block;
    }
  }
  .iconfont {
    position: absolute;
    font-size: 60px;
    color: #fff;
    top: -30px;
    right: -20px;
  }
}
</style>