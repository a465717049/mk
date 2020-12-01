<template>
  <div id="app">
    <!-- <keep-alive :exclude="[]"> -->
      <router-view v-if="isRouterAlive"></router-view>
    <!-- </keep-alive> -->

    <van-overlay :show="globalLoadingShow" @click="show = false">
      <van-loading color="#efb618" size="300" v-show="globalLoadingShow" />
    </van-overlay>
  </div>
</template>
<script>
import { mapState } from "vuex";
export default {
  name: "App",
  data() {
    return {
      isRouterAlive: true
    };
  },
  computed: mapState({
    globalLoadingShow: state => state.globalLoadingShow
  }),
  methods: {
    reload() {
      this.isRouterAlive = false;
      this.$nextTick(() => (this.$nextTick = true));
    }
  }
};
</script>

<style lang="less">
@import "~vux/src/styles/reset.less";
#app {
  font-family: Adobe Heiti Std R;
  background: #4678BC;
  background: url('./assets/imgs/bj.png') no-repeat center center/100% 100%;
  height: 100vh;
  width: 100vw;
}

</style>
