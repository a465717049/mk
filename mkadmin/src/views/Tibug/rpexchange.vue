<template>
  <section>
    <!--工具条-->
    <toolbar :buttonList="buttonList" @callFunction="callFunction"></toolbar>
    <el-select style="width:202px" v-model="searchstatusVal" placeholder="类型">
      <el-option value="" label="全部">全部</el-option>
      <el-option value="7" label="創建新賬號">創建新賬號</el-option>
      <el-option value="8" label="RP轉SP">RP轉SP</el-option>
      <el-option value="9" label="EP轉RP">EP轉RP</el-option>
      <el-option value="13" label="转换">转换</el-option>
      <el-option value="17" label="歸縂">歸縂</el-option>
    </el-select>

    <!--列表-->
    <el-table :data="users" highlight-current-row @current-change="selectCurrentRow" v-loading="listLoading" @selection-change="selsChange" style="width: 100%;">
      <el-table-column type="selection" width="50">
      </el-table-column>
      <el-table-column type="index" width="80">
      </el-table-column>
      <el-table-column prop="id" label="编号" width="" sortable>
      </el-table-column>
      <el-table-column prop="fromID" label="来源ID" width="" sortable>
      </el-table-column>
      <el-table-column prop="uID" label="UID" width="" sortable>
      </el-table-column>
      <el-table-column prop="amount" label="金额" width="" sortable>
      </el-table-column>
      <el-table-column prop="lastTotal" label="最后金额" width="" sortable>
      </el-table-column>
      <el-table-column prop="remark" label="备注" width="" sortable>
      </el-table-column>
      <el-table-column prop="createTime" label="日期" width="" sortable>
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
  GetAdminRPExchange,
  RollBackTran,
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
      searchstatusVal: "",
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
        pageindex: this.page,
        pagesize: 20,
        key: this.filters.name,
        stype: this.searchstatusVal,
      };
      this.listLoading = true;
      //NProgress.start();
      GetAdminRPExchange(para).then((res) => {
        this.total = res.response.dataCount;
        this.users = res.response.data;
        this.listLoading = false;
        //NProgress.done();
      });
    },
    rollbackthis() {
      let rows = this.currentRow;
      if (!rows) {
        this.$message({
          message: "请选择要操作的一行数据！",
          type: "error",
        });
        return;
      }
      this.$confirm("是否更改当前状态？", "提示", {}).then(() => {
        let para = {
          id: rows.id,
          type: "EP",
        };
        RollBackTran(para).then((res) => {
          if (res.success) {
            this.$message({
              message: "操作成功",
              type: "success",
            });
            this.getUsers();
          } else {
            this.$message({
              message: "操作失败请稍后再试！",
              type: "error",
            });
          }
        });
      });
    },
    selsChange: function (sels) {
      this.sels = sels;
    },
  },
  mounted() {
   // this.getUsers();
    let routers = window.localStorage.router
      ? JSON.parse(window.localStorage.router)
      : [];
    this.buttonList = getButtonList(this.$route.path, routers);
  },
};
</script>

<style scoped>
</style>
