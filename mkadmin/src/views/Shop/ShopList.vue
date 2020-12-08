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
         <el-table-column prop="item.id" label="编号" width="" sortable>
      </el-table-column>
      <el-table-column prop="item.buyuid" label="购买者" width="" sortable>
      </el-table-column>
      <el-table-column prop="item.shopordernumber" label="购买订单号" width="" sortable>
      </el-table-column>
        <el-table-column prop="shopname" label="购买商品" width="" sortable>
      </el-table-column>
            <el-table-column prop="item.buyNum" label="购买数量" width="" sortable>
      </el-table-column>
            <el-table-column prop="item.price" label="购买价格" width="" sortable>
      </el-table-column>
       <el-table-column prop="item.status" :formatter="formatStatus"  label="购买状态" width="" sortable>
      </el-table-column>
         <el-table-column prop="item.buyname" label="购买名" width="" sortable>
      </el-table-column>
      <el-table-column prop="item.buyphone" label="购买手机" width="" sortable>
      </el-table-column>
      <el-table-column prop="item.buyaddr" label="购买地址" width="" sortable>
      </el-table-column>
         <el-table-column prop="item.createTime" label="购买时间" width="" sortable>
      </el-table-column>
         <el-table-column prop="item.trackingnumber" label="快递单号" width="" sortable>
      </el-table-column>
         <el-table-column prop="item.reamrk" label="购买备注" width="" sortable>
      </el-table-column>
    </el-table>

 <!--工具条-->
    <el-col :span="24" class="toolbar">
      <el-pagination layout="prev, pager, next" @current-change="handleCurrentChange" :page-size="20" :total="total" style="float:right;padding: 8px 5px;">
      </el-pagination>
    </el-col>

     <!--修改状态-->
    <el-dialog title="修改状态" :visible.sync="levelFormVisible" v-model="levelFormVisible" :close-on-click-modal="false">
      <el-form :model="Formlevel" label-width="80px"  ref="Formlevel">
        <el-form-item label="状态" prop="Name">
          <el-select v-model="statusvalue">
          <el-option label="未发货" value="1">未发货</el-option>
          <el-option  label="配送中" value="2">配送中</el-option>
          <el-option  label="确认收货" value="3">确认收货</el-option>
          <el-option label="己完成" value="4">己完成</el-option>
    </el-select>
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click.native="levelFormVisible = false">取消</el-button>
        <el-button type="primary" @click.native="levelSubmit" :loading="levelLoading">提交</el-button>
      </div>
    </el-dialog>

      <el-dialog title="快递单号" :visible.sync="tidFormVisible" v-model="tidFormVisible" :close-on-click-modal="false">
      <el-form :model="Formtid" label-width="80px" :rules="Formtids" ref="Formtid">
        <el-form-item label="快递单号" prop="Name">
          <el-input v-model="Formtid.Name" auto-complete="off"></el-input>
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click.native="tidFormVisible = false">取消</el-button>
        <el-button type="primary" @click.native="tidSubmit" :loading="tidLoading">提交</el-button>
      </div>
    </el-dialog>

  </section>
</template>

<script>
import util from "../../../util/date";
import {
  GetEPUserSell,
  testapi,
  GetAdminBuyShopList ,AddTruckOrdersweb, ChangeOrdersweb
} from "../../api/api";
import { getButtonList } from "../../promissionRouter";
import Toolbar from "../../components/Toolbar";

export default {
  components: { Toolbar },
  data() {
    return {
      statusvalue:0,
     formatStatus: function (row, column) {
       var tmps="";
       // 未发货 配送中 确认收货 己完成
       if(row.item.status ==1)
       {
         tmps="未发货";
       }else if(row.item.status ==2)
       {
         tmps="配送中";
       }else if(row.item.status ==3)
       {
         tmps="确认收货";
       }else
       {
          tmps="己完成";
       }
      return tmps;
    },
     //推荐
      tidFormVisible: false,
      tidLoading: false,
      Formtids: {
        Name: [{ required: true, message: "请输入快递单号", trigger: "blur" }],
      },
      Formtid: {
        Id: 0,
        Name: "",
      },
    //状态
      levelFormVisible: false,
      levelLoading: false,
      Formlevel: {
        detailid: 0,
        status: 0,
      },

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
      tidSubmit: function () {
      this.$refs.Formtid.validate((valid) => {
        if (valid) {
          this.$confirm("确认提交吗？", "提示", {}).then(() => {
            let para = {
              detailid: this.Formlevel.detailid,
              order: this.Formtid.Name,
            };
            AddTruckOrdersweb(para).then((res) => {
              if (res.success) {
                this.tidFormVisible = false;
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
 //状态
    levelSubmit: function () {
      this.$refs.Formlevel.validate((valid) => {
        if (valid) {
          this.$confirm("确认提交吗？", "提示", {}).then(() => {
            let para = {
              detailid: this.Formlevel.detailid,
              status: this.statusvalue,
            };
            ChangeOrdersweb(para).then((res) => {
              if (res.success) {
                this.levelFormVisible = false;
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
    selectCurrentRow(val) {
      this.currentRow = val;
    },
    callFunction(item) {
      this.filters = {
        name: item.search,
      };
      this[item.Func].apply(this, item);
    },
    //修改状态
     changeorder(index, row) {
      let rows = this.currentRow;
      if (!rows) {
        this.$message({
          message: "请选择要操作的一行数据！",
          type: "error",
        });
        return;
      }
      this.Formlevel.detailid=rows.item.id;
      this.statusvalue = rows.item.status;
      this.statusvalue=this.statusvalue.toString();
      this.levelFormVisible = true;
    },
    //快递单号
    addtrack(index, row) {
      let rows = this.currentRow;
      if (!rows) {
        this.$message({
          message: "请选择要操作的一行数据！",
          type: "error",
        });
        return;
      }
      this.Formlevel.detailid=rows.item.id;
      this.tidFormVisible = true;
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
      //NProgress.start();
      GetAdminBuyShopList(para).then((res) => {
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
