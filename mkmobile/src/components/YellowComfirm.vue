<template>
  <van-popup
    v-model="modelShow"
    :close-on-click-overlay="true"
    @click-overlay="clickOver"
    class="yellow-wrap"
  >
    <div class="brown-border clearfix">
      <div class="yellow-bg">
        <div class="imgCont">
          <img class="item-l" src="@/assets/imgs/login/head.png" alt />
          <!-- <img class="eye" src="@/assets/imgs/login/eye2.png" alt /> -->
        </div>
        <i class="iconfont iconguanbi" @click="clickNo"></i>
        <div class="item-r">
          <h3 class="item-r-tit">提示：</h3>
          <div class="item-r-tip" v-html="tipTitle"></div>
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
  name: 'YellowComfirm',
  components: {},
  props: {
    show: {
      type: Boolean,
      default: false
    },
    tipTitle: {
      type: String,
      default: '操作成功'
    },
    showConfirmBtn: {
      type: Boolean,
      default: false
    },
    first: {
      type: String,
      default: '主農場'
    },
    second: {
      type: String,
      default: '子農場'
    }
  },
  data () {
    return {
      modelShow: false
    }
  },
  watch: {
    show (newV) {
      this.modelShow = newV
    },
    modelShow (newV) {
      this.$emit('changeModel', newV)
    }
  },
  methods: {
    clickOver () {
      this.$emit('clickOver', false)
    },
    // 输入支付密码
    clickOk () {
      this.$emit('clickOk')
    },
    clickNo () {
      this.modelShow = false
      this.$emit('clickNo')
    }
  }
}
</script>

<style lang="less" scoped>
.yellow-wrap {
  background: #efb618;
  width: 76vw;
  border-radius: 50px;
  // padding: 4px;
  .brown-border {
    padding: 50px 40px;
    border: 12px solid #fff;
    width: 100%;
    border-radius: 50px;
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
  .imgCont {
    width: 155px;
    height: 155px;
    margin-right: 60px;
    position: relative;
    .item-l {
      width: 100%;
      height: 100%;
    }
    .eye{
      position: absolute;
      width: 30%;
      top: 34px;
      left: 50%;
      transform: translateX(-50%);
    }
  }

  .item-r {
    flex: 1;
    color: #3f1008;
    .item-r-tit {
      font-size: 36px;
      font-weight: bold;
      line-height: 60px;
      color: #410305;
      margin-top: 10px;
      margin-bottom: 20px;
    }
    .item-r-tip {
      font-size: 36px;
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
