<template>
  <div class="xpIndex">
    <router-view />
    <TabBar />
  </div>
</template>

<script>
import TabBar from "components/TabBar";
export default {
  data() {
    return {};
  },
  components: {
    TabBar
  }
};
</script>
<style lang="less" scoped>
.xpIndex {
  height: 100%;
}
</style>
