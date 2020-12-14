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
  data () {
    return {
      value: ''
    }
  },
  methods: {
    onSearch () {
      this.$emit('onSearch', this.value)
    }
  }
}
</script>
<style lang="less" scoped>
.search {
  width: 100%;
  height: 110px;
  display: flex;
  margin-top: 20px;
  background-color: #fff;
  border-radius: 20px;
  padding: 30px ;
  align-items: center;
  input {
    height: 50px;
    line-height: 50px;
    width: 100%;
    font-size: 40px;
    color: #767c8f;
  }
  .inputIcon {
    font-size: 65px;
    color: #4678bc;
  }
}
</style>
