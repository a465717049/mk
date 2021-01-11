<template>
  <section>
    <!--工具条-->
    <toolbar :buttonList="buttonList" @callFunction="callFunction"></toolbar>

    <!--列表-->
    <el-table :data="users" highlight-current-row @row-dblclick="dbSelected" @current-change="selectCurrentRow" v-loading="listLoading" @selection-change="selsChange" style="width: 100%;">
      <el-table-column type="selection" width="50">
      </el-table-column>
      <el-table-column type="index" width="40">
      </el-table-column>
      <el-table-column prop="Id" label="内容编号" width="120" sortable>
      </el-table-column>
      <el-table-column prop="uId" label="玩家" width="120" sortable>
      </el-table-column>
      <el-table-column prop="Message" label="消息内容" width="500" sortable>
      </el-table-column>
      <el-table-column prop="CreateTime" label="创建日期" width="200" sortable>
      </el-table-column>
      <el-table-column prop="isDelete" label="是否回复" width="" sortable>
        <template slot-scope="scope">
          <el-tag :type="!scope.row.IsReply  ? 'danger' : 'success'" disable-transitions>{{!scope.row.IsReply ? "未回复":"已回复"}}</el-tag>
        </template>
      </el-table-column>
    </el-table>

    <!--工具条-->
    <el-col :span="24" class="toolbar">
      <el-pagination layout="prev, pager, next" @current-change="handleCurrentChange" :page-size="20" :total="total" style="float:right;padding: 8px 5px;">
      </el-pagination>
    </el-col>

    <!--回复界面-->
    <el-dialog title="回复" :visible.sync="msgFormVisible" v-model="msgFormVisible" :close-on-click-modal="false">
      <el-form :model="Formmsg" label-width="80px" :rules="Formmsgs" ref="Formmsg">
        <el-form-item label="回复内容" prop="Name">
          <el-input v-model="Formmsg.Name" auto-complete="off"></el-input>
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click.native="msgFormVisible = false">取消</el-button>
        <el-button type="primary" @click.native="msgSubmit" :loading="msgLoading">提交</el-button>
      </div>
    </el-dialog>

    <el-dialog style="width:130%;margin-left:-10%;" title="消息内容" :visible.sync="msgVisible" v-model="msgVisible" :close-on-click-modal="false">
      <el-table :data="msgdata" highlight-current-row>
        <el-table-column type="selection" width="50">
        </el-table-column>
        <!--   <el-table-column type="index" width="40">
        </el-table-column>
        <el-table-column prop="id" label="内容编号" width="120" sortable>
        </el-table-column> -->
        <el-table-column prop="isadmin" label="管理员" width="100">
          <template slot-scope="scope">
            <el-tag :type="!scope.row.isadmin  ? '' : 'success'" disable-transitions>{{!scope.row.isadmin ? "":"管理员"}}</el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="msg" label="消息内容" width="400">
        </el-table-column>
        <el-table-column prop="date" label="创建时间" width="150" sortable>
        </el-table-column>

      </el-table>

    </el-dialog>
  </section>
</template>

<script>
import util from "../../../util/date";
import {
  GetAdminUserFeedBack,
  testapi,
  AdminDisposeFeedBack,
  GetAdminAllUserFeedBack,
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
      msgVisible: false,
      //消息
      msgFormVisible: false,
      msgLoading: false,
      Formmsgs: {
        Name: [{ required: true, message: "请输入荣誉", trigger: "blur" }],
      },
      Formmsg: {
        Id: 0,
        Name: "",
      },
      msgid: 0,
      users: [],
      msgdata: [],
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
    msgSubmit: function () {
      let that = this;
      this.$refs.Formmsg.validate((valid) => {
        if (valid) {
          this.$confirm("确认提交吗？", "提示", {}).then(() => {
            let para = {
              Id: this.Formmsg.Id,
              msg: this.Formmsg.Name,
            };
            AdminDisposeFeedBack(para).then((res) => {
              if (res.success) {
                this.$message({
                  message: "操作成功",
                  type: "success",
                });
                setTimeout(function () {
                  that.getUsers();
                }, 2000);
              } else {
                this.$message({
                  message: "操作失败请稍后再  试！",
                  type: "error",
                });
              }
            });
          });
        }
      });
    },
    operate(id) {
      let rows = this.currentRow;
      this.msgid = rows.Id;
      this.msgVisible = true;
    },
    replymsg(index, row) {
      let rows = this.currentRow;
      if (!rows) {
        this.$message({
          message: "请选择要操作的一行数据！",
          type: "error",
        });
        return;
      }
      this.Formmsg.Id = rows.Id;
      this.msgFormVisible = true;
    },
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
      };
      this.listLoading = true;
      //NProgress.start();
      GetAdminUserFeedBack(para).then((res) => {
        this.total = res.response.dataCount;
        this.users = res.response.data;
        this.listLoading = false;
        //NProgress.done();
      });
    },
    deletemsg() {},
    dbSelected() {
      let rows = this.currentRow;
      this.msgid = rows.uId;
      this.msgVisible = true;

      let para = {
        uid: this.msgid,
      };
      GetAdminAllUserFeedBack(para).then((res) => {
        this.msgdata = res.response;
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
  },
};
</script>

<style scoped>
</style>
