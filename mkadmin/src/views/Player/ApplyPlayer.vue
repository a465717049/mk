<template>
  <section>
    <!--工具条-->
    <toolbar :buttonList="buttonList" @callFunction="callFunction"></toolbar>

    <!--列表-->
    <el-table :data="users" highlight-current-row @current-change="selectCurrentRow" v-loading="listLoading" @selection-change="selsChange" style="width: 100%;">
      <el-table-column type="index" label="序号" width="60">
      </el-table-column>
      <el-table-column prop="applyid" label="申请id" sortable>
      </el-table-column>
      <el-table-column prop="nickname" label="昵称"  sortable>
      </el-table-column>
      <el-table-column prop="username" label="用户名"  sortable>
      </el-table-column>
      <el-table-column prop="userphone" label="手机号" sortable>
      </el-table-column>
       <el-table-column prop="createtime" label="注册日期" sortable>
      </el-table-column>
      <el-table-column prop="openstatus" label="状态" sortable>
        <template slot-scope="scope">
          <el-tag :type="scope.row.openstatus  ? 'success' : 'danger'" disable-transitions>{{scope.row.openstatus ? "通过":"未通过"}}</el-tag>
        </template>
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
  GetALLUserInfo,
  testapi,
  GetUserInfoWeek,
  adminResetlock,
  adminResetpwd,
  adminResetlevel,
  adminResettid,
  adminResetanswer,
  adminResetidcard,
  GetOpenShopMyweb,
  ApplyOpenShopMyweb
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
      dayinfo: {
        total: 0,
        maintotal: 0,
        sontotal: 0,
        daytotal: 0,
        daymaintotal: 0,
        daysontotal: 0,
        weektotal: 0,
        weekmaintotal: 0,
        weeksontotal: 0,
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
    formatSon: function (row, column) {
      return row.ison == 1 ? "子" : row.ison == 0 ? "主" : "未知";
    },
    formatfarmers: function (row, column) {
      return row.farmers == 1 ? "三星" : row.farmers == 0 ? "二星" : "未知";
    },
    callFunction(item) {
      this.filters = {
        name: item.search,
      };
      this[item.Func].apply(this, item);
    },
    passinfo(index,row)
    {
      let rows = this.currentRow;
      if (!rows) {
      this.$message({
      message: "请选择要操作的一行数据！",
      type: "error",
      });
      return;
      }
          this.$confirm("是否通过当前数据？", "提示", {}).then(() => {
        let para = {
          openid: rows.openid,
        };
        console.log(para)
        ApplyOpenShopMyweb(para).then((res) => {
          if (res.success) {
            this.$message({
              message: "操作成功",
              type: "success",
            });
            this.getUsers();
          } else {
            this.$message({
              message: "该数据已通过 请勿重新通过！",
              type: "error",
            });
          }
        });
      });


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
      };
      this.listLoading = true;
      //NProgress.start();
      GetOpenShopMyweb(para).then((res) => {
        this.total = res.response.datacount;
        this.users = res.response.data;
        this.listLoading = false;
        //NProgress.done();
      });
    },
    selsChange: function (sels) {
      this.sels = sels;
    },
  },
  mounted() {
    //this.getUsers();
    let routers = window.localStorage.router
      ? JSON.parse(window.localStorage.router)
      : [];
    this.buttonList = getButtonList(this.$route.path, routers);

    GetUserInfoWeek(null).then((res) => {
      this.dayinfo = res.response.list;
      this.dayinfo.weektotal =
        this.dayinfo.weekmaintotal + this.dayinfo.weeksontotal;
      this.dayinfo.daytotal =
        this.dayinfo.daymaintotal + this.dayinfo.daysontotal;
    });
  },
};
</script>

<style scoped>
</style>
