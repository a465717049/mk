<template>
  <section>
    <!--工具条-->
    <toolbar :buttonList="buttonList" @callFunction="callFunction"></toolbar>

    <!--列表-->
    <el-table :data="users" highlight-current-row @current-change="selectCurrentRow" v-loading="listLoading" @selection-change="selsChange" style="width: 100%;">
      <el-table-column type="selection" width="50">
      </el-table-column>
      <el-table-column type="index" width="80">
      </el-table-column>
      <el-table-column prop="id" label="玩家" width="" sortable>
      </el-table-column>
      <el-table-column prop="lastTotal" label="最后余额" width="" sortable>
      </el-table-column>
         <el-table-column prop="amount" label="金额" width="" sortable>
      </el-table-column>
      <!--<el-table-column prop="name" label="姓名" width="" sortable>-->
      <!--</el-table-column>-->
      <el-table-column prop="remark" label="备注" width="" sortable>
      </el-table-column>
      <el-table-column prop="date" label="创建时间" width="" sortable>
      </el-table-column>
    </el-table>

 <!--工具条-->
    <el-col :span="24" class="toolbar">
      <el-pagination layout="prev, pager, next" @current-change="handleCurrentChange" :page-size="20" :total="total" style="float:right;padding: 8px 5px;">
      </el-pagination>
    </el-col>

  </section>
</template>

<script>
import util from "../../../util/date";
import {
  GetEPUserSell,
  testapi,
  getUserListPage,
  getRoleListPage,
  removeUser,
  batchRemoveUser,
  editUser,
  addUser,
} from "../../api/api";
import { getButtonList } from "../../promissionRouter";
import Toolbar from "../../components/Toolbar";

export default {
  components: { Toolbar },
  data() {
    return {
      filters: {
        name: "",
      },
      users: [],
      roles: [],
      total: 0,
      buttonList: [],
      currentRow: null,
      page: 1,
      listLoading: false,
      sels: [], //列表选中列
    };
  },
  methods: {
    selectCurrentRow(val) {
      this.currentRow = val;
    },
    callFunction(item) {
      this.filters = {
        name: item.search,
      };
      this[item.Func].apply(this, item);
    },
    handleCurrentChange(val) {
      this.page = val;
      this.getUsers();
    },
    //获取Sell列表
    getUsers() {
      let para = {
        type: 1,
        pageSize: 20,
        pageIndex: this.page,
        cktype: "",
      };
      this.listLoading = true;

      testapi();
      //NProgress.start();
      GetEPUserSell(para).then((res) => {
        this.total = res.response.datacount;
        this.users = res.response.data;
        this.listLoading = false;
        //NProgress.done();
      });
    },
    selsChange: function(sels) {
      this.sels = sels;
    },
  },
  mounted() {
    this.getUsers();
    let routers = window.localStorage.router
      ? JSON.parse(window.localStorage.router)
      : [];
    this.buttonList = getButtonList(this.$route.path, routers);
  },
};
</script>

<style scoped>
</style>
