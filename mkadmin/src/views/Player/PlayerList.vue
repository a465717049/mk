<template>
  <section>

    <el-card class="welcome-card note" style="width: calc(49% - 10px);margin-right: 10px;">
      <div slot="header" class="clearfix">
        <span>玩家数据</span>
      </div>
      <div class="text item"><i class="el-icon-edit"></i>会员总数：{{dayinfo.total}}</div><br />
      <div class="text item"><i class="el-icon-edit"></i>本周新增：{{dayinfo.weektotal}}  </div><br />
      <div class="text item"><i class="el-icon-edit"></i>今天新增：{{dayinfo.daytotal}} </div>

    </el-card>
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
      <el-table-column prop="lv_name" :formatter="formatlv" label="职位" width="" sortable>
      </el-table-column>
      <el-table-column prop="farmers" :formatter="formatfarmers" label="等级" width="" sortable>
      </el-table-column>
      <el-table-column prop="tid" label="推荐ID" width="100" sortable>
      </el-table-column>
      <el-table-column prop="jid" label="安置ID" width="100" sortable>
      </el-table-column>
      <el-table-column prop="gold" label="EP" width="100" sortable>
      </el-table-column>
      <el-table-column prop="rp" label="RP" width="100" sortable>
      </el-table-column>
      <el-table-column prop="seed" label="SP" width="100" sortable>
      </el-table-column>
      <el-table-column prop="apple" label="DPE" width="100" sortable>
      </el-table-column>
      <el-table-column prop="friend" label="好友" width="" sortable>
      </el-table-column>
      <el-table-column prop="lprofit" label="L" width="120" sortable>
      </el-table-column>
      <el-table-column prop="rprofit" label="R" width="120" sortable>
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

    <!--推荐界面-->
    <el-dialog title="更改推荐人" :visible.sync="tidFormVisible" v-model="tidFormVisible" :close-on-click-modal="false">
      <el-form :model="Formtid" label-width="80px" :rules="Formtids" ref="Formtid">
        <el-form-item v-if="showtid" label="推荐人" >
          <el-input v-model="Formtid.Name" auto-complete="off"></el-input>
        </el-form-item>
          <el-form-item v-if="showjid" label="安置ID" >
          <el-input v-model="Formtid.jid" auto-complete="off"></el-input>
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click.native="tidFormVisible = false">取消</el-button>
        <el-button type="primary" @click.native="tidSubmit" :loading="tidLoading">提交</el-button>
      </div>
    </el-dialog>

    <!--重置密码界面-->
    <el-dialog title="重置密码" :visible.sync="pwdFormVisible" v-model="pwdFormVisible" :close-on-click-modal="false">
      <el-form :model="Formpwd" label-width="80px" :rules="Formpwds" ref="Formpwd">
        <el-form-item label="密码" prop="Name">
          <el-input v-model="Formpwd.Name" auto-complete="off"></el-input>
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click.native="pwdFormVisible = false">取消</el-button>
        <el-button type="primary" @click.native="pwdSubmit" :loading="pwdLoading">提交</el-button>
      </div>
    </el-dialog>

    <!--重置密保界面-->
    <el-dialog title="重置密保" :visible.sync="answerFormVisible" v-model="answerFormVisible" :close-on-click-modal="false">
      <el-form :model="Formanswer" label-width="80px" :rules="Formanswers" ref="Formanswer">
        <el-form-item label="密保" prop="Name">
          <el-input v-model="Formanswer.Name" auto-complete="off"></el-input>
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click.native="answerFormVisible = false">取消</el-button>
        <el-button type="primary" @click.native="answerSubmit" :loading="answerLoading">提交</el-button>
      </div>
    </el-dialog>

    <!--更改身份证界面-->
    <el-dialog title="更改身份证" :visible.sync="idcardFormVisible" v-model="idcardFormVisible" :close-on-click-modal="false">
      <el-form :model="Formidcard" label-width="80px" :rules="Formidcards" ref="Formidcard">
        <el-form-item label="姓名" prop="Name">
          <el-input v-model="Formidcard.Name" auto-complete="off"></el-input>
        </el-form-item>
        <el-form-item label="身份证号码" prop="Name">
          <el-input v-model="Formidcard.idcard" auto-complete="off"></el-input>
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click.native="idcardFormVisible = false">取消</el-button>
        <el-button type="primary" @click.native="idcardSubmit" :loading="idcardLoading">提交</el-button>
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
      showjid:false,
      showtid:false,
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
      //推荐
      tidFormVisible: false,
      tidLoading: false,
      Formtids: {
       // Name: [{ required: true, message: "请输入推荐人", trigger: "blur" }],
       // JID: [{ required: true, message: "请输入安置ID", trigger: "blur" }],
      },
      Formtid: {
        Id: 0,
        Name: "",
        jid:"",
      },
      //密码
      pwdFormVisible: false,
      pwdLoading: false,
      Formpwds: {
        Name: [{ required: true, message: "请输入密码", trigger: "blur" }],
      },
      Formpwd: {
        Id: 0,
        Name: "",
      },
      //密保
      answerFormVisible: false,
      answerLoading: false,
      Formanswers: {
        Name: [{ required: true, message: "请输入密保", trigger: "blur" }],
      },
      Formanswer: {
        Id: 0,
        Name: "",
      },
      //身份证
      idcardFormVisible: false,
      idcardLoading: false,
      Formidcards: {
        Name: [{ required: true, message: "请输入身份证", trigger: "blur" }],
      },
      Formidcard: {
        Id: 0,
        Name: "",
        idcard: "",
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
              console.log(res);
            });
          });
        }
      });
    },
    //tid
    tidSubmit: function () {
      this.$refs.Formtid.validate((valid) => {
        if (valid) {
          this.$confirm("确认提交吗？", "提示", {}).then(() => {
            var para = {}
            if(this.showtid)
            {
              para={
              uid: this.Formtid.Id,
              tid: this.Formtid.Name,
              };
            }else
            {
               para={
              uid: this.Formtid.Id,
              jid: this.Formtid.jid,
              };
            }
            console.log(para)
            adminResettid(para).then((res) => {
               if (res.success) {
                this.$message({
                  message: "操作成功",
                  type: "success",
                });
                this.tidFormVisible=false;
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
    //pwd
    pwdSubmit: function () {
      this.$refs.Formpwd.validate((valid) => {
        if (valid) {
          this.$confirm("确认提交吗？", "提示", {}).then(() => {
            let para = {
              uid: this.Formpwd.Id,
              pwd: this.Formpwd.Name,
            };
            adminResetpwd(para).then((res) => {
              console.log(res);
            });
          });
        }
      });
    },
    //answer
    answerSubmit: function () {
      this.$refs.Formanswer.validate((valid) => {
        if (valid) {
          this.$confirm("确认提交吗？", "提示", {}).then(() => {
            let para = {
              uid: this.Formanswer.Id,
              answer: this.Formanswer.Name,
            };
            adminResetanswer(para).then((res) => {
              console.log(res);
            });
          });
        }
      });
    },
    //idcard
    idcardSubmit: function () {
      this.$refs.Formidcard.validate((valid) => {
        if (valid) {
          this.$confirm("确认提交吗？", "提示", {}).then(() => {
            let para = {
              uid: this.Formidcard.Id,
              realname: this.Formidcard.Name,
              idcard: this.Formidcard.idcard,
            };
            adminResetidcard(para).then((res) => {
              console.log(res);
            });
          });
        }
      });
    },
    //锁定
    lockUser(index, row) {
      this.$confirm("是否更改当前状态？", "提示", {}).then(() => {
        let para = {
          uid: row.uid,
        };
        adminResetlock(para).then((res) => {});
      });
    },
    changerlevel(index, row) {
      this.Formlevel.Id = row.uid;
      this.levelFormVisible = true;
    },
    changetid(index, row) {
      let rows = this.currentRow;
      if (!rows) {
        this.$message({
          message: "请选择要操作的一行数据！",
          type: "error",
        });
        return;
      }
        this.showtid=true;
      this.showjid=false;
    
      this.Formtid.Id = rows.uid;
      this.tidFormVisible = true;
    },
     changejid(index, row) {
      let rows = this.currentRow;
      if (!rows) {
        this.$message({
          message: "请选择要操作的一行数据！",
          type: "error",
        });
        return;
      }
      this.showjid=true;
      this.showtid=false;
      this.Formtid.jid = rows.jid;
      this.tidFormVisible = true;
    },
    resetpwd(index, row) {
      let rows = this.currentRow;
      if (!rows) {
        this.$message({
          message: "请选择要操作的一行数据！",
          type: "error",
        });
        return;
      }
      this.Formpwd.Id = rows.uid;
      this.pwdFormVisible = true;
    },
    resetquestion(index, row) {
      let rows = this.currentRow;
      if (!rows) {
        this.$message({
          message: "请选择要操作的一行数据！",
          type: "error",
        });
        return;
      }
      this.Formanswer.Id = rows.uid;
      this.answerFormVisible = true;
    },
    resetidcard(index, row) {
      let rows = this.currentRow;
      if (!rows) {
        this.$message({
          message: "请选择要操作的一行数据！",
          type: "error",
        });
        return;
      }
      this.Formidcard.Id = rows.uid;
      this.idcardFormVisible = true;
    },
    selectCurrentRow(val) {
      this.currentRow = val;
    },

    formatSon: function (row, column) {
      return row.ison == 1 ? "子" : row.ison == 0 ? "主" : "未知";
    },
    formatlv: function (row, column) {
          var tmpname="";
          if (row.lv_name == 0) tmpname= '会员'
          if (row.lv_name == 1) tmpname= '经理'
          if (row.lv_name == 2) tmpname= '总监'
          if (row.lv_name == 3) tmpname= '总裁'
          if (row.lv_name == 4) tmpname= '董事'
          if (row.lv_name == 5) tmpname= '合伙人'
      return tmpname;
    },
    formatfarmers: function (row, column) {
      return row.farmers == 1 ? "初级会员" : row.farmers == 2 ? "中级会员" : "高级会员";
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
