<template>
  <div class="search">
    <input type="text" v-model="value" :placeholder="placeinputValue" />
    <div class="inputIcon iconfont iconsearch" @click="onSearch"></div>
  </div>
</template>
<script>
import { mapState } from 'vuex'
export default {
  name: 'TopSearch',
  props: {
    placeinputValue: {
      type: String,
      default: '搜索玩家ID'
    }
  },
  data() {
    return {
      value: ''
    }
  },
  methods: {
    onSearch() {
      this.$emit('onSearch', this.value)
    }
  }
}
</script>
<style lang="less" scoped>
.search {
  width: 100%;
  height: 130px;
  font-size: 46px;
  color: #4d4d4d;
  position: relative;
  margin-top: 20px;
  input {
    height: 100%;
    line-height: 60px;
    background-color: #f5f5f5;
    width: 100%;
    padding: 30px 106px 30px 30px;
    border-radius: 20px;
  }
  .inputIcon {
    position: absolute;
    top: 10px;
    right: 40px;
    font-size: 65px;
    color: #6318c3;
  }
}
</style>
