<template>
  <section>
    <!--工具条-->
    <toolbar :buttonList="buttonList" @callFunction="callFunction"></toolbar>

    <!--列表-->
    <el-table :data="users" highlight-current-row @current-change="selectCurrentRow" v-loading="listLoading" @selection-change="selsChange" @row-dblclick="handledbClick" style="width: 100%;">
      <el-table-column prop="id" label="编号" width="" sortable>
      </el-table-column>
      <el-table-column prop="uID" label="UID" width="" sortable>
      </el-table-column>
      <el-table-column prop="amount" label="ep金额" width="" sortable>
      </el-table-column>
      <el-table-column prop="statusName" label="状态" width="" sortable>
      </el-table-column>
      <el-table-column prop="usdtAddress" label="usdt地址" width="" sortable>
      </el-table-column>
      <el-table-column prop="alipayaccount" label="支付宝账号" width="" sortable>
      </el-table-column>
       <el-table-column prop="alipayname" label="支付宝姓名" width="" sortable>
      </el-table-column>
        <el-table-column prop="bankname" label="银行卡姓名" width="" sortable>
      </el-table-column>
       <el-table-column prop="bankaddr" label="开户行" width="" sortable>
      </el-table-column>
    <el-table-column prop="bankidcard" label="银行卡号" width="" sortable>
      </el-table-column>
      <el-table-column prop="createTime" label="创建时间" width="" sortable>
      </el-table-column>
    </el-table>

    <!--工具条-->
    <el-col :span="24" class="toolbar">
      <el-pagination layout="prev, pager, next" @current-change="handleCurrentChange" :page-size="20" :total="total" style="float:right;padding: 8px 5px;">
      </el-pagination>
    </el-col>

    <el-dialog title="usdt/trc地址" :visible.sync="tidFormVisible" v-model="tidFormVisible" :close-on-click-modal="false">
      <div>
        <div class="qrcode" id="qrcode" ref="qrCodeUrl">usdt</div>
        <br />
        <br />
        <div class="qrcode" id="trcAddress" ref="trcAddress">trc</div>
      </div>

    </el-dialog>

    <!--导出记录-->
      <el-dialog title="导出记录" :visible.sync="outinfoVisible" v-model="outinfoVisible" :close-on-click-modal="false">
      <el-button type="primary" @click="sumbitoutput" >导出</el-button>
      <el-table :data="downinfo" highlight-current-row v-loading="listLoading" style="width: 100%;">
      <el-table-column prop="id" label="编号" width="80" sortable>
      </el-table-column>
      <el-table-column prop="downname" label="下载名称" width="" sortable>
      </el-table-column>
      <el-table-column prop="downdate" label="下载时间" width="" sortable>
      </el-table-column>
      <el-table-column label="操作" width="" >
      <el-row class="edita" slot-scope="downinfo">  
      <a href="#" @click="downexcel(downinfo.row.downname)">下载</a>   
      <a style="margin-left:20px" href="#" @click="deleteexcel(downinfo.row.id)">删除</a>
      </el-row> 
      </el-table-column>
      </el-table>
      <div slot="footer" class="dialog-footer">
      <el-button @click.native="outinfoVisible = false">关闭</el-button>
      </div>
      </el-dialog>


  </section>
</template>
<script>
import QRCode from "qrcodejs2";
import util from "../../../util/date";
import { dowmexcel,GetAdminEPRecordList,
 EPBuy, EPPay, EPFinish,
 GetDownExcelList,DeleteDownExcelList,EPRecordOutAllPut } from "../../api/api";
import { getButtonList } from "../../promissionRouter";
import Toolbar from "../../components/Toolbardiy";
export default {
  components: { Toolbar },
  data() {
    return {
      downinfo: [
        {
          id: 0,
          downname: "",
          downdate: "",
        },
      ],
      outinfoVisible:false,
      filters: {
        name: "",
        status: "",
      },
      tidFormVisible: true,
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
    downexcel(thisurl)
    {
       window.open(dowmexcel + thisurl);
    },
    deleteexcel(thisid)
    {
      this.$confirm("确认删除吗？", "提示", {}).then(() => {
        DeleteDownExcelList({ id: thisid }).then((res) => {
          if (res.success) {
            this.tidFormVisible = false;
            this.$message({
              message: "操作成功",
              type: "success",
            });
            this.getorderoutput();
          } else {
            this.$message({
              message: "操作失败请稍后再试！",
              type: "error",
            });
          }
        });
      });
    },
    sumbitoutput()
    {
        let para = {
        pageindex: this.page,
        pagesize: 20,
        key: this.filters.name,
        status: this.filters.status,
      };
      this.listLoading = true;
      this.$message({
        message: "已加入导出列表请稍后再来查看",
        type: "success",
      });
      EPRecordOutAllPut(para).then((res) => {
        this.getorderoutput();
        this.listLoading = false;
        this.$message({
          message: "导出成功",
          type: "success",
        });
      });
    },
    epinput(){

       this.outinfoVisible = true;
      this.getorderoutput();
    },
      getorderoutput() {
      let para = {
        pageSize: 20,
        pageIndex: 1,
        key: "",
      };
      //NProgress.start();
      GetDownExcelList(para).then((res) => {
        this.downinfo = res.response.data;
      });
    },
    formattdDetail: function (row, column) {
      console.log(row.amount * row.rate);
      return row.amount * row.rate;
    },
    selectCurrentRow(val) {
      this.currentRow = val;
    },
    callFunction(item) {
      this.filters = {
        name: item.search,
        status: item.status,
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
        status: this.filters.status,
      };
      this.listLoading = true;
      GetAdminEPRecordList(para).then((res) => {
        this.total = res.response.dataCount;
        this.users = res.response.data;
        this.listLoading = false;
        //NProgress.done();
      });
    },
    selsChange: function (sels) {
      this.sels = sels;
    },
    epbuy() {
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
          eid: rows.id,
        };
        EPBuy(para).then((res) => {
          if (res.success) {
            this.$message({
              message: "操作成功",
              type: "success",
            });
            this.getUsers();
          } else {
            this.$message({
              message: res.msg,
              type: "error",
            });
          }
        });
      });
    },
    eppay() {
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
          eid: rows.id,
        };
        EPPay(para).then((res) => {
          if (res.success) {
            this.$message({
              message: "操作成功",
              type: "success",
            });
            this.getUsers();
          } else {
            this.$message({
              message: res.msg,
              type: "error",
            });
          }
        });
      });
    },
    epfinish() {
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
          eid: rows.id,
        };
        EPFinish(para).then((res) => {
          if (res.success) {
            this.$message({
              message: "操作成功",
              type: "success",
            });
            this.getUsers();
          } else {
            this.$message({
              message: res.msg,
              type: "error",
            });
          }
        });
      });
    },
    handledbClick(row) {
      this.tidFormVisible = true;
      document.getElementById("qrcode").innerHTML = "usdt";
      document.getElementById("trcAddress").innerHTML = "trc";
      console.log(row.usdtAddress);
      this.creatQrCode(row.usdtAddress, row.trcAddress);
    },
    creatQrCode(value, trcAddress) {
      var qrcode = new QRCode(this.$refs.qrCodeUrl, {
        text: value,
        width: 150,
        height: 150,
        colorDark: "#000000",
        colorLight: "#ffffff",
        correctLevel: QRCode.CorrectLevel.H,
      });

      var qrcodetrcAddress = new QRCode(this.$refs.trcAddress, {
        text: trcAddress,
        width: 150,
        height: 150,
        colorDark: "#000000",
        colorLight: "#ffffff",
        correctLevel: QRCode.CorrectLevel.H,
      });
    },
  },
  mounted() {
    this.tidFormVisible = false;
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
