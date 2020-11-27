<template>
  <section>
    <!--工具条-->
    <toolbar :buttonList="buttonList" @callFunction="callFunction"></toolbar>

    <!--列表-->
    <el-table :data="users" highlight-current-row @current-change="selectCurrentRow" v-loading="listLoading" @selection-change="selsChange" style="width: 100%;">
      <el-table-column type="selection" width="50">
      </el-table-column>
      <el-table-column type="index" width="40">
      </el-table-column>
      <el-table-column prop="uid" label="玩家" width="80" sortable>
      </el-table-column>
      <el-table-column prop="nickname" label="昵称" width="100" sortable>
      </el-table-column>
      <el-table-column prop="create_time" label="注册日期" width="150" sortable>
      </el-table-column>
      <el-table-column prop="ison" :formatter="formatSon" label="主/子" width="" sortable>
      </el-table-column>
      <el-table-column prop="lv_name" label="荣誉" width="" sortable>
      </el-table-column>
      <el-table-column prop="farmers" :formatter="formatfarmers" label="三星/二星" width="130" sortable>
      </el-table-column>
      <el-table-column prop="tid" label="推荐ID" width="100" sortable>
      </el-table-column>
      <el-table-column prop="jid" label="接点ID" width="100" sortable>
      </el-table-column>
      <el-table-column prop="isDelete" label="状态" width="" sortable>
        <template slot-scope="scope">
          <el-tag :type="!scope.row.isDelete  ? 'success' : 'danger'" disable-transitions>{{!scope.row.isDelete ? "正常":"锁定"}}</el-tag>
        </template>
      </el-table-column>
    </el-table>

    <!--工具条-->
    <el-col :span="24" class="toolbar">
      <el-pagination layout="prev, pager, next" @current-change="handleCurrentChange" :page-size="20" :total="total" style="float:right;padding: 8px 5px;">
      </el-pagination>
    </el-col>

    <!--荣誉界面-->
    <el-dialog title="更改荣誉" :visible.sync="levelFormVisible" v-model="levelFormVisible" :close-on-click-modal="false">
      <el-form :model="Formlevel" label-width="80px" :rules="Formlevels" ref="Formlevel">
        <el-form-item label="荣誉" prop="Name">
          <el-input v-model="Formlevel.Name" auto-complete="off"></el-input>
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click.native="levelFormVisible = false">取消</el-button>
        <el-button type="primary" @click.native="levelSubmit" :loading="levelLoading">提交</el-button>
      </div>
    </el-dialog>
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
      //荣誉
      levelFormVisible: false,
      levelLoading: false,
      Formlevels: {
        Name: [{ required: true, message: "请输入荣誉", trigger: "blur" }],
      },
      Formlevel: {
        Id: 0,
        Name: "",
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
    //荣誉
    levelSubmit: function () {
      this.$refs.Formlevel.validate((valid) => {
        if (valid) {
          this.$confirm("确认提交吗？", "提示", {}).then(() => {
            let para = {
              uid: this.Formlevel.Id,
              level: this.Formlevel.Name,
            };
            adminResetlevel(para).then((res) => {
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
        }
      });
    },
    changerlevel(index, row) {
      let rows = this.currentRow;
      if (!rows) {
        this.$message({
          message: "请选择要操作的一行数据！",
          type: "error",
        });
        return;
      }
      this.Formlevel.Id = rows.uid;
      this.levelFormVisible = true;
    },
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
      GetALLUserInfo(para).then((res) => {
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
    this.getUsers();
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
