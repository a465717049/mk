<template>
  <div>
    <TopBar :option="topBarOption">
      <!-- 1  -->
      <div class="center-one-search">Search</div>
      <!-- 2 有星星背景 -->
      <!-- <div class="center-two-search">
          <div class="two-tit-t">Total Buy</div>
          <div class="two-tit-b">$ 624,271</div>
          <img class="img-yueliang" src="@/assets/imgs/yueliang.png" alt="">
          <img class="img-fangxing" src="@/assets/imgs/fangxing.png" alt="">
      </div>-->
      <!-- 3 有搜索框 -->
      <!-- <div class="center-three-search">
          <div class="three-tit-t">Queue</div>
          <div class="three-tit-b">4,271</div>
          <TopSearch></TopSearch>
      </div>-->
      <!-- 4 首页头部 -->
      <!-- <div class="center-four-search">
          <div class="four-tit-t">Hello,</div>
          <div class="four-tit-b">Chelsea lalan</div>
          <img class="img-yueliang" src="@/assets/imgs/yueliang.png" alt="">
          <img class="img-yueliang1" src="@/assets/imgs/yueliang1.png" alt="">
          <img class="img-fangxing" src="@/assets/imgs/fangxing.png" alt="">
          <img class="img-header" src="@/assets/imgs/headerImg.png" alt="">
      </div>-->
    </TopBar>
    <!-- <PasswordConfirm
      :show="showComfirm"
      :initPassword="password"
      @clickOver="clickOverpay"
      @clickOk="clickOk()"
      @changeModel="changeModel"
    /> -->
    <!-- <scrollRefresh :residualHeight="100">
    </scrollRefresh> -->
  </div>
</template>

<script>
import TopBar from "components/TopBar";
import TopSearch from "components/TopSearch";
import PasswordConfirm from "components/PasswordConfirm";
// import scrollRefresh from "components/scrollRefresh";
export default {
  components: {
    TopBar,
    TopSearch,
    PasswordConfirm,
    // scrollRefresh
  },
  mounted(){
    this.changeModel(true)
  },
  data() {
    return {
      topBarOption: {
        // 右边展开： iconlist2f
        // 人物头像： iconchangyonglianxiren
        // 铃铛： iconlingdang
        // 不要就传空就行
        iconLeft: "iconlist2f",
        iconRight: ""
      },
      showComfirm: true,
      password: "123"
    };
  },
  methods: {
    clickOk() {
      this.showComfirm = false;
    },
    changeModel(v) {
      this.showComfirm = v;
    },
    changePassword(v) {
      this.password = v;
    }
  }
};
</script>

