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
        <div class="item-r">
          <h3 class="item-r-tit">请输入密码：</h3>
          <!-- <div class="item-r-tip">{{tipTitle}}</div> -->
          <input type="password" v-model="password" />
        </div>
      </div>
      <div class="btn" @click="clickOk">OK</div>
      <!-- <div v-if="showConfirmBtn" class="confirm-btn">
        <div class="first-btn" @click="clickOk">主農場</div>
        <div class="second-btn" @click="clickNo">子農場</div>
      </div> -->
    </div>
  </van-popup>
</template>

<script>

export default {
  name: "PasswordConfirm",
  components: {},
  props: {
    show: {
      type: Boolean,
      default: false
    },
    initPassword:{
      type: String
    }
    // showConfirmBtn: {
    //   type: Boolean,
    //   default: false
    // },
    // first: {
    //   type: String,
    //   default: "主農場"
    // },
    // second: {
    //   type: String,
    //   default: "子農場"
    // }
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
  computed: {
    password: {
      get: function() {
        return this.initPassword;
      },
      set: function(newValue) {
        this.$emit("change", newValue);
      }
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
    // clickNo() {
    //   this.$emit("clickNo");
    // }
  }
};
</script>

<style lang="less" scoped>
.yellow-wrap {
  background: #facc5d;
  width: 76vw;
  min-height: 300px;
  border-radius: 30px;
  padding: 5px;
  .brown-border {
    padding: 40px;
    border: 10px solid #3f1008;
    width: 100%;
    min-height: 290px;
    border-radius: 30px;
  }
  .btn {
    color: #fff;
    font-weight: bold;
    background: #3f1008;
    font-size: 46px;
    line-height: 70px;
    float: right;
    width: 140px;
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
  width: 80vw;
  display: flex;
  .item-l {
    width: 98px;
    height: 118px;
    margin-right: 60px;
  }
  .item-r {
    flex: 1;
    color: #3f1008;
    .item-r-tit {
      font-size: 40px;
      font-weight: bold;
      line-height: 60px;
    }
    .item-r-tip {
      font-size: 36px;
      line-height: 58px;
    }
    input{
      height: 80px;
      line-height: 80px;
      font-size: 42px;
      border-radius: 20px;
      border: 1px solid #ccc;
      margin: 20px 0;
      padding: 0 20px;
    }
  }
}
</style>