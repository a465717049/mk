<template>
  <section>
    <!--工具条-->
    <toolbar :buttonList="buttonList" @callFunction="callFunction"></toolbar>

    <!--列表-->
    <el-table :data="users" highlight-current-row @current-change="selectCurrentRow" v-loading="listLoading" @selection-change="selsChange" style="width: 100%;">
      <el-table-column type="index" width="80">
      </el-table-column>
         <el-table-column prop="id" label="编号" width="" sortable>
      </el-table-column>
      <el-table-column prop="pName" label="商品名" width="" sortable>
      </el-table-column>
      <el-table-column prop="pDesc" label="商品描述" width="" sortable>
      </el-table-column>
        <el-table-column prop="pIcon" label="商品图片" width="" sortable>
      </el-table-column>
       <el-table-column prop="pNum" label="数量" width="" sortable>
      </el-table-column>
       <el-table-column prop="price" label="价格" width="" sortable>
      </el-table-column>
      <el-table-column prop="ptype"  :formatter="formattype"  label="商品类型" width="" sortable>
      </el-table-column>
           <el-table-column prop="minLevel"  :formatter="formatlv"  label="购买级别" width="" sortable>
      </el-table-column>
        <!-- <el-table-column prop="priceType" label="价格类型" width="" sortable>
      </el-table-column>
      <el-table-column prop="status" label="状态" width="" sortable>
      </el-table-column> -->
         <el-table-column prop="createTime" label="创建时间" width="" sortable>
      </el-table-column>
    </el-table>

 <!--工具条-->
    <el-col :span="24" class="toolbar">
      <el-pagination layout="prev, pager, next" @current-change="handleCurrentChange" :page-size="20" :total="total" style="float:right;padding: 8px 5px;">
      </el-pagination>
    </el-col>

     <!--修改状态-->
    <el-dialog title="商品管理" :visible.sync="levelFormVisible" v-model="levelFormVisible" :close-on-click-modal="false">
      <!-- id minLevel price pNum pName pDesc pIcon status ptype priceType Shopgroup -->
      <el-form :model="Formlevel" label-width="80px"  ref="Formlevel">
        <el-form-item label="商品名" >
        <el-input v-model="Formtid.pName" auto-complete="off"></el-input>
        </el-form-item>
        <el-form-item label="商品描述" >
        <el-input v-model="Formtid.pDesc" auto-complete="off"></el-input>
        </el-form-item>
        <el-form-item label="商品图片" >
        <el-input v-model="Formtid.pIcon" disabled auto-complete="off"></el-input>
        <div class="Thisform">
      <el-form ref="form" :model="form" label-width="80px">
          <input type="file" @change="getFile($event)">
      </el-form>
    </div>
        </el-form-item>
        <el-form-item label="商品数量" >
        <el-input v-model="Formtid.pNum" auto-complete="off"></el-input>
        </el-form-item>
        <el-form-item label="商品价格" >
        <el-input v-model="Formtid.price" auto-complete="off"></el-input>
        </el-form-item>
        <el-form-item label="最低职位" >
        <el-select v-model="Formtid.minLevel" placeholder="请选择职位">
        <el-option label="会员" :value="0"></el-option>
        <el-option label="经理" :value="1"></el-option>
        <el-option label="总监" :value="2"></el-option>
        <el-option label="总裁" :value="3"></el-option>
        <el-option label="董事" :value="4"></el-option>
        <el-option label="合伙人" :value="5"></el-option>
        </el-select>
        </el-form-item>
        <el-form-item label="商品类型" >
        <el-select v-model="Formtid.ptype">
        <el-option label="普通" :value="0">普通</el-option>
        <el-option  label="再次购买" :value="1">再次购买</el-option>
        </el-select>
         </el-form-item>
      <!--  <el-form-item label="priceType" >
        <el-select v-model="Formtid.priceType">
        <el-option label="1" value="1">1</el-option>
        </el-select>
         </el-form-item>
        <el-form-item label="状态" >
        <el-select v-model="Formtid.status">
        <el-option label="1" value="1">正常</el-option>
        </el-select>
         </el-form-item>  -->
        <el-form-item label="商品分组" >
        <el-select v-model="Formtid.Shopgroup">
        <el-option label="默认" :value="1">默认</el-option>
        <el-option  label="商品2" :value="2">商品2</el-option>
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
  GetEPUserSell,
  testapi,
  GetAdminBuyShopList ,AddTruckOrdersweb, 
  ChangeOrdersweb,GetShopListMyweb,DeleteShopListMyweb,AddShopListMyweb,uploadPicture
} from "../../api/api";
import { getButtonList } from "../../promissionRouter";
import Toolbar from "../../components/Toolbar";

export default {
  components: { Toolbar },
  data() {
    return {
      statusvalue:0,
     //推荐
      tidFormVisible: false,
      tidLoading: false,
       form: {
      },
       file: '',
      //id minLevel price pNum pName pDesc pIcon status ptype priceType Shopgroup
      Formtid: {
        id: 0,
        minLevel: 0,
        price: 0,
        pNum: 1,
        pName: "",
        pDesc: "",
        pIcon: "",
        status: 1,
        ptype: 1,
        priceType: 1,
        Shopgroup: 1,
      },
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
     getFile(event) {
      this.file = event.target.files[0];
    },
     onSubmit() {
      let that = this;
      event.preventDefault();//取消默认行为
      //创建 formData 对象
      let param = new FormData();
      param.append("file", this.file);
       param.append("id", this.Formtid.id);
      
   uploadPicture(param).then((res) => {
              this.files="";
            });
    },
    formatlv: function (row, column) {
          var tmpname="";
          if (row.minLevel == 0) tmpname= '会员'
          if (row.minLevel == 1) tmpname= '经理'
          if (row.minLevel == 2) tmpname= '总监'
          if (row.minLevel == 3) tmpname= '总裁'
          if (row.minLevel == 4) tmpname= '董事'
          if (row.minLevel == 5) tmpname= '合伙人'
      return tmpname;
    },
    formattype: function (row, column) {
          var tmpname="";
          if (row.ptype == 0) tmpname= '普通'
          if (row.ptype == 1) tmpname= '再次购买'
      return tmpname;
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
           // id minLevel price pNum pName pDesc pIcon status ptype priceType Shopgroup 
            let para = {
                  id: this.Formtid.id,
                  minLevel:this.Formtid.minLevel,
                  price:this.Formtid.price,
                  pNum:this.Formtid.pNum,
                  pName:this.Formtid.pName,
                  pDesc:this.Formtid.pDesc,
                  pIcon:this.Formtid.pIcon,
                  status:this.Formtid.status,
                  ptype:this.Formtid.ptype,
                  priceType:this.Formtid.priceType,
                  Shopgroup:this.Formtid.Shopgroup,
            };
            AddShopListMyweb(para).then((res) => {
              if (res.success) {
                this.levelFormVisible = false;
                if(this.file){ this.Formtid.id=res.response; this.onSubmit();}
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
     updateshop(index, row) {
      let rows = this.currentRow;
      if (!rows) {
        this.$message({
          message: "请选择要操作的一行数据！",
          type: "error",
        });
        return;
      }
       this.Formtid.id=rows.id;
       this.Formtid.minLevel =rows.minLevel;
       this.Formtid.price=rows.price;
       this.Formtid.pNum=rows.pNum;
       this.Formtid.pName=rows.pName;
       this.Formtid.pDesc=rows.pDesc;
       this.Formtid.pIcon=rows.pIcon;
       this.Formtid.status=rows.status;
       this.Formtid.ptype=rows.ptype;
       this.Formtid.priceType=rows.priceType;
       this.Formtid.Shopgroup=rows.Shopgroup;
      this.levelFormVisible=true;
     
    },
      deleteshop(index, row) {
      let rows = this.currentRow;
      if (!rows) {
        this.$message({
          message: "请选择要操作的一行数据！",
          type: "error",
        });
        return;
      }

          this.$confirm("确认删除吗？", "提示", {}).then(() => {
            let para = {
              id: rows.id,
            };
            DeleteShopListMyweb(para).then((res) => {
              if (res.success) {
                this.levelFormVisible = false;
                this.$message({
                  message: "删除成功",
                  type: "success",
                });
                 
                this.getUsers();
              } else {
                this.$message({
                  message: "删除失败请稍后再试！",
                  type: "error",
                });
              }
            });
          });
     
    },
      addshop(index, row) {
        this.Formtid.id=0;
        this.Formtid.minLevel=0;
        this.Formtid.price=0;
        this.Formtid.pNum=1;
        this.Formtid.pName="";
        this.Formtid.pDesc="";
        this.Formtid.pIcon="";
        this.Formtid.status=1;
        this.Formtid.ptype=1;
        this.Formtid.priceType=1;
        this.Formtid.Shopgroup=1;
      this.levelFormVisible=true;
    },
    handleCurrentChange(val) {
      this.page = val;
      this.getUsers();
    },
    //获取Sell列表
    getUsers() {
      let para = {
        pageSize: 20,
        pageIndex: this.page,
        key:this.filters.name,
      };
      this.listLoading = true;
      //NProgress.start();
      GetShopListMyweb(para).then((res) => {
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
