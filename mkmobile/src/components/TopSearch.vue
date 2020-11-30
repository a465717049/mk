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
      default: '输入朋友ID进行查询 '
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
  height: 110px;
  font-size: 40px;
  color: #767C8F;
  position: relative;
  margin-top: 20px;
  input {
    height: 100%;
    background-color: #fff;
    width: 100%;
    padding: 30px 106px 30px 30px;
    border-radius: 20px;
  }
  .inputIcon {
    position: absolute;
    top: 10px;
    right: 40px;
    font-size: 65px;
    color: #4678BC;
  }
}
</style>
