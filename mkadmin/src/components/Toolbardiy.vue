<template>
  <el-col v-if="buttonList!=null&&buttonList.length>0" :span="24" class="toolbar" style="padding-bottom: 0px;">
    <el-form :inline="true" @submit.native.prevent>
      <el-form-item>
        <el-input style="width:50%" v-model="searchVal" placeholder="请输入编号/UID/buyId"></el-input>
        <el-select style="width:50%" v-model="searchstatusVal" placeholder="请选择EP状态">
          <el-option value="">全部</el-option>
          <el-option value="1">出售中</el-option>
          <el-option value="4">已支付</el-option>
          <el-option value="8">已完成</el-option>
        </el-select>
      </el-form-item>
      <!-- 这个就是当前页面内，所有的btn列表 -->
      <el-form-item v-for="item in buttonList" v-bind:key="item.id">
        <!-- 这里触发点击事件 -->
        <el-button :type="item.Func&&(item.Func.toLowerCase().indexOf('handledel')!= -1 ||item.Func.toLowerCase().indexOf('stop')!= -1 )? 'danger':'primary'" v-if="!item.IsHide" @click="callFunc(item)">{{item.name}}</el-button>
      </el-form-item>
    </el-form>
  </el-col>
</template>
<script>
export default {
  name: "Toolbar",
  data() {
    return {
      searchVal: "", //双向绑定搜索内容
      searchstatusVal: "",
    };
  },
  props: ["buttonList"], //接受父组件传值
  methods: {
    callFunc(item) {
      item.search = this.searchVal;
      item.status = this.searchstatusVal;
      this.$emit("callFunction", item); //将值传给父组件
    },
  },
};
</script>