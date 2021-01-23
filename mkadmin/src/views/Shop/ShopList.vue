<template>
  <section>
    <!--工具条-->
    
    <toolbar :buttonList="buttonList" @callFunction="callFunction">
    </toolbar>

   <el-col  :span="24" class="toolbar" style="padding-bottom: 0px;">
    <el-form :inline="true" >
    <el-form-item>
       <el-select v-model="cktype" placeholder="订单状态">
      <el-option label="全部状态" value="">全部状态</el-option>
      <el-option label="未发货" value="1">未发货</el-option>
      <el-option  label="配送中" value="2">配送中</el-option>
      <el-option  label="确认收货" value="3">确认收货</el-option>
      <el-option label="己完成" value="4">己完成</el-option>
        </el-select>
      </el-form-item>
       <!--   <el-form-item>
       <el-select v-model="ordertype" placeholder="订单类型">
      <el-option label="全部类型" value="">全部类型</el-option>
      <el-option label="普通商品" value="0">普通商品</el-option>
      <el-option  label="复购商品" value="1">复购商品</el-option>
        </el-select>
      </el-form-item> -->
      <el-form-item>
        <el-input type="date" v-model="startdate"></el-input> 
      </el-form-item>
        <el-form-item>
      <el-input type="date" v-model="enddate"></el-input>
      </el-form-item>
    </el-form>
  </el-col>
       

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
        <el-table-column prop="shopinfo.pName" label="购买商品" width="" sortable>
      </el-table-column>
      <el-table-column prop="shopinfo.ptype" label="订单类型" :formatter="formatptype" width="" sortable>
      </el-table-column>
        <el-table-column prop="shopsku.skuname" label="商品规格" width="" sortable>
      </el-table-column>
        <el-table-column prop="shopskudt.detailname" label="商品尺码" width="" sortable>
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


     <!--导出记录-->
      <el-dialog title="导出记录" :visible.sync="outinfoVisible" v-model="outinfoVisible" :close-on-click-modal="false">
      <el-button type="primary" @click="sumbitoutput" >新增当前查询记录</el-button>
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
import util from "../../../util/date";
import {
  GetEPUserSell,
  testapi,
  GetAdminBuyShopList ,AddTruckOrdersweb, ChangeOrdersweb,OrderOutAllPut,GetDownExcelList,dowmexcel,DeleteDownExcelList
} from "../../api/api";
import { getButtonList } from "../../promissionRouter";
import Toolbar from "../../components/Toolbar";

export default {
  components: { Toolbar },
  data() {
    return {
      downinfo:
      [{
      id:0,
      downname:'',
      downdate:'',
      }],
      outinfoVisible:false,
      cktype:"",
      startdate:"",
      enddate:"", 
      statusvalue:0,
      ordertype:"",
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
    formatptype: function (row, column) {
       var tmps="";
       if(row.shopinfo.ptype ==1)
       {
         tmps="复购商品";
       }else
       {
          tmps="普通商品";
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
     downexcel(thisurl)
     {
      window.open(dowmexcel+thisurl)
     }, deleteexcel(thisid)
     {
        this.$confirm("确认删除吗？", "提示", {}).then(() => {

            DeleteDownExcelList({id:thisid}).then((res) => {
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
    orderoutput()
    {
      this.outinfoVisible=true;
      this.getorderoutput();
     },
     getorderoutput()
     {
        let para = {
        pageSize: 20,
        pageIndex: 1,
        key: "",
      };
      //NProgress.start();
      GetDownExcelList(para).then((res) => {
        this.downinfo=res.response.data;
        
        //NProgress.done();
      });
     },
     sumbitoutput()
     {
        let para = {
        pageSize: 20,
        pageIndex: this.page,
        stype: this.cktype,
        startdt:this.startdate,
        enddt:this.enddate,
        ordertype:this.ordertype,
      };
        this.listLoading = true;
        OrderOutAllPut(para).then((res) => {
        this.getorderoutput();
        this.listLoading = false;
        this.$message({
        message: "操作成功",
        type: "success",
        });
        });
     },
      setQueryConfig (queryConfig)
      {
      var _str = "?";
      for(var o in queryConfig){
      if(queryConfig[o] != -1){
      _str += o + "=" + queryConfig[o] + "&";
      }
      }
      var _str = _str.substring(0, _str.length-1);
      return _str;
      },
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
        stype: this.cktype,
        startdt:this.startdate,
        enddt:this.enddate,
        ordertype:this.ordertype,
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
