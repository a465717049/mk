<template>
  <section>
    <!--工具条-->
    <toolbar :buttonList="buttonList" @callFunction="callFunction"></toolbar>

    <!--列表-->
    <el-table :data="users" highlight-current-row @current-change="selectCurrentRow" v-loading="listLoading" @selection-change="selsChange" style="width: 100%;">
      <el-table-column type="index" width="40">
      </el-table-column>
      <el-table-column prop="uid" label="玩家" width="" sortable>
      </el-table-column>
      <el-table-column prop="nickname" label="昵称" width="" sortable>
      </el-table-column>
      <el-table-column prop="create_time" label="注册日期" width="" sortable>
      </el-table-column>
      <el-table-column prop="lv_name" :formatter="formatlv" label="职位" width="" sortable>
      </el-table-column>
      <el-table-column prop="farmers" :formatter="formatfarmers" label="等级" width="" sortable>
      </el-table-column>
      <el-table-column prop="tid" label="推荐ID" width="" sortable>
      </el-table-column>
      <el-table-column prop="jid" label="接点ID" width="" sortable>
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
    <el-dialog title="更改" :visible.sync="levelFormVisible" v-model="levelFormVisible" :close-on-click-modal="false">
      <el-form :model="Formlevel" label-width="80px" :rules="Formlevels" ref="Formlevel">
        <el-form-item v-if="showzw"  label="职位">
          <el-select v-model="Formlevel.Name" placeholder="请选择职位">
          <el-option label="会员" :value="0"></el-option>
          <el-option label="经理" :value="1"></el-option>
          <el-option label="总监" :value="2"></el-option>
          <el-option label="总裁" :value="3"></el-option>
          <el-option label="董事" :value="4"></el-option>
          <el-option label="合伙人" :value="5"></el-option>
          </el-select>
        </el-form-item>
        <el-form-item v-if="showlv" label="等级" >
        <el-select v-model="Formlevel.honur" placeholder="请选择等级">
        <el-option label="初级会员" :value="1"></el-option>
        <el-option label="中级会员" :value="2"></el-option>
        <el-option label="高级会员" :value="3"></el-option>
        <el-option label="超级会员" :value="4"></el-option>
         </el-select>
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
      showlv:false,
      showzw:false,
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
        Name: [{ required: true, message: "请输入职位", trigger: "blur" }],
      },
      Formlevel: {
        Id: 0,
        Name: "",
        honur:"",
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
    levelSubmit: function () {
      this.$refs.Formlevel.validate((valid) => {
        if (valid) {
          this.$confirm("确认提交吗？", "提示", {}).then(() => {
            var para = {
              uid: this.Formlevel.Id,
              level: this.Formlevel.Name,
            };
            if(this.showlv)
            {
               para = {
              uid: this.Formlevel.Id,
              level: this.Formlevel.honur,
            };
            }else
            {
               para = {
               uid: this.Formlevel.Id,
               zw: this.Formlevel.Name,
            };
            }
            adminResetlevel(para).then((res) => {
              if (res.success) {
                this.$message({
                  message: "操作成功",
                  type: "success",
                });
                this.levelFormVisible=false;
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
      this.showzw=false;
      this.showlv=true;
      
      this.Formlevel.Id = rows.uid;
      this.Formlevel.Name = '';
      this.Formlevel.honur='';
       this.levelFormVisible = true;
     
    },
    selectCurrentRow(val) {
      this.currentRow = val;
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
      return row.farmers == 1 ? "初级会员" : row.farmers == 2 ? "中级会员": row.farmers == 3 ? "高级会员"  : "超级会员";
    },
    callFunction(item) {
      this.filters = {
        name: item.search,
      };
      this[item.Func].apply(this, item);
    },
    changerfarmaer(index, row)
    {
      let rows = this.currentRow;
      if (!rows) {
        this.$message({
          message: "请选择要操作的一行数据！",
          type: "error",
        });
        return;
      }
      this.showlv=false;
      this.showzw=true;
      this.Formlevel.Id = rows.uid;
      this.Formlevel.Name ='';
      this.Formlevel.honur='';
      this.levelFormVisible = true;
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
