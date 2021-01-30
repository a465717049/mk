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
  <el-table-column label="商品图片" align="center" min-width="100" width="" sortable>
  　　　　<template slot-scope="data">
  　　　　　　<img v-if="data.row.pIcon" :src="getimgurl(data.row.pIcon)" height="80" width="80" />
  　　　　</template>
  　      　</el-table-column>
  <el-table-column label="商品详情图片" align="center" min-width="100" width="" sortable>
  　　　　<template slot-scope="data">
  　　　　　　<img v-if="data.row.pDetailIcon" :src="getimgurl(data.row.pDetailIcon)" height="80" width="80" />
  　　　　</template> 
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
          <el-table-column label="操作" align="center" width="" >
        <el-row class="edita" slot-scope="sholistinfo">  
             <a href="#" @click="groundingshop(sholistinfo.row.id)">
               <p v-if="sholistinfo.row.isgrounding">下架</p>
                <p v-else>上架</p>	
             </a>
        </el-row>
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
        <img v-if="shoptmpimg" :src="getimgurl(shoptmpimg)" height="128" width="128" />
        <div class="Thisform">
      <el-form ref="form" :model="form" label-width="80px">
          <input type="file" @change="getFile($event)">
      </el-form>
        
    </div>
        </el-form-item>
      <el-form-item label="商品详情" >
      <el-input v-model="Formtid.pDetailIcon" disabled auto-complete="off"></el-input>
      <img  v-if="shopdetailtmpimg" :src="getimgurl(shopdetailtmpimg)" height="128" width="128" />
      <div class="Thisform">
      <el-form ref="detailform" :model="detailform" label-width="80px">
      <input type="file" @change="getdetilFile($event)">
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

     <!--sku明细增加-->
    <el-dialog title="sku明细" :visible.sync="addskudtFormVisible" v-model="addskudtFormVisible" :close-on-click-modal="false">
      <el-form  label-width="80px" >
        <el-form-item label="id"  hidden>
        <el-input v-model="Formadddt.id" auto-complete="off"></el-input>
        </el-form-item>
         <el-form-item label="skuid" hidden >
        <el-input v-model="Formadddt.skuid" auto-complete="off"></el-input>
        </el-form-item>
        <el-form-item label="明细名称" >
        <el-input v-model="Formadddt.detailname" auto-complete="off"></el-input>
        </el-form-item>
        <el-form-item label="明细描述" >
        <el-input v-model="Formadddt.detaildesc" auto-complete="off"></el-input>
        </el-form-item>
            <el-form-item label="明细价格" >
        <el-input v-model="Formadddt.detailprice" auto-complete="off"></el-input>
        </el-form-item>
         <el-form-item label="明细数量" >
        <el-input v-model="Formadddt.detailnum" auto-complete="off"></el-input>
        </el-form-item>
        <el-form-item label="明细图片" >
        <el-input v-model="Formadddt.detailicon" disabled auto-complete="off"></el-input>
        <img  v-if="shopskudttmpimg" :src="getimgurl(shopskudttmpimg)" height="128" width="128" />
        <div class="Thisform">
      <el-form ref="skudtform" :model="skudtform" label-width="80px">
          <input type="file" @change="getskudtFile($event)">
      </el-form>
    </div>
  </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click.native="addskudtFormVisible = false">取消</el-button>
        <el-button type="primary" @click.native="addskudtSubmit" :loading="levelLoading">提交</el-button>
      </div>
    </el-dialog>

    <!--sku增加-->
    <el-dialog title="sku管理" :visible.sync="addskuFormVisible" v-model="addskuFormVisible" :close-on-click-modal="false">
      <div v-if="addeditshowsku">
      <el-button  @click="addskunamelist" >增加sku</el-button>
      <el-button  @click="plusskunamelist" >删除sku</el-button>
      </div>
      <el-form  label-width="80px" >
        <el-form-item label="id" hidden >
        <el-input v-model="Formadd.id" auto-complete="off"></el-input>
        </el-form-item>
         <el-form-item label="shopid" hidden  >
        <el-input v-model="Formadd.shopid" auto-complete="off"></el-input>
        </el-form-item>


        <el-form-item label="sku名称" > <!--  Formadd.skuname  -->
        <div v-if="addeditshowsku">
          <el-input  v-for="(el,idx) in skunamelist" :key="idx" v-model="el.skuname" auto-complete="off" style="width:200px"></el-input>
        </div>
           <el-input  v-if="!addeditshowsku" v-model="Formadd.skuname" auto-complete="off" ></el-input>
        </el-form-item>


        <el-form-item label="sku描述" >
        <el-input v-model="Formadd.skudesc" auto-complete="off" ></el-input>
        </el-form-item>
        <el-form-item label="sku图片" >
        <el-input v-model="Formadd.skuIcon" disabled auto-complete="off" ></el-input>
          <img v-if="shopskutmpimg" :src="getimgurl(shopskutmpimg)"  height="128" width="128" />
        <div class="Thisform">
        <el-form ref="skuform" :model="skuform" label-width="80px">
        <input type="file" @change="getskuFile($event)">
        </el-form>
        </div>
        </el-form-item>

         <div v-if="addeditshowsku">
         <el-button  @click="addskudtnamelist" >明细</el-button>
          <div v-if="addskudtshow">
          <el-button  @click="addskudtname" >增加明细</el-button>
          <el-button  @click="plusskudtname" >删除明细</el-button>
         <el-form  label-width="80px" >
        <el-form-item label="id"  hidden>
        <el-input v-model="Formadddt.id" auto-complete="off"></el-input>
        </el-form-item>
         <el-form-item label="skuid" hidden >
        <el-input v-model="Formadddt.skuid" auto-complete="off"></el-input>
        </el-form-item>
        <el-form-item label="明细名称" >
        <!--<el-input v-model="Formadddt.detailname" auto-complete="off"></el-input> -->
         <el-input  v-for="(el,idx) in skunamedtlist" :key="idx" v-model="el.skudtname" auto-complete="off" style="width:200px"></el-input>
        </el-form-item>
        
        <el-form-item label="明细描述" >
        <el-input v-model="Formadddt.detaildesc" auto-complete="off"></el-input>
        </el-form-item>
            <el-form-item label="明细价格" >
        <el-input v-model="Formadddt.detailprice" auto-complete="off"></el-input>
        </el-form-item>
         <el-form-item label="明细数量" >
        <el-input v-model="Formadddt.detailnum" auto-complete="off"></el-input>
        </el-form-item>
        <el-form-item label="明细图片" >
        <el-input v-model="Formadddt.detailicon" disabled auto-complete="off"></el-input>
        <img  v-if="shopskudttmpimg" :src="getimgurl(shopskudttmpimg)" height="128" width="128" />
        <div class="Thisform">
      <el-form ref="skudtform" :model="skudtform" label-width="80px">
          <input type="file" @change="getskudtFile($event)">
      </el-form>
    </div>
  </el-form-item>
      </el-form>
          </div>
      
      </div>
        </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click.native="addskuFormVisible = false">取消</el-button>
        <el-button type="primary" @click.native="addskuSubmit" :loading="levelLoading">提交</el-button>
      </div>
    </el-dialog>


    <el-dialog title="商品sku管理" :visible.sync="skuFormVisible" v-model="skuFormVisible" :close-on-click-modal="false">
     <el-button type="primary" @click="addskuinfo" >新增sku</el-button>
<el-table :data="skuinfolist" highlight-current-row @current-change="selectskuCurrentRow" v-loading="listLoading" @selection-change="selsskuChange" style="width: 100%;">
      <el-table-column prop="skuinfo.id" label="编号" width="80" sortable>
      </el-table-column>
      <el-table-column prop="skuinfo.skuname" label="sku名称" width="" sortable>
      </el-table-column>
      <el-table-column prop="skuinfo.skudesc" label="sku描述" width="" sortable>
      </el-table-column>
     <el-table-column prop="skuinfo.skuIcon" label="sku图片" width="" sortable>
      </el-table-column> 
      <el-table-column prop="skuinfo.createtime" label="创建时间" width="" sortable>
      </el-table-column>
       <el-table-column label="操作" width="" >
        <el-row class="edita" slot-scope="skuinfo">  
             <a href="#" @click="editsku(skuinfo.row.skuinfo)">编辑</a>
             <a href="#" @click="showeditskuinfo(skuinfo.row)">sku明细</a>
             <a href="#" @click="deleskuinfo(skuinfo.row.skuinfo.id)">删除</a>
        </el-row>
      </el-table-column>
    </el-table>
    <div slot="footer" class="dialog-footer">
    <el-button @click.native="skuFormVisible = false">关闭</el-button>
    </div>
    </el-dialog>

 <el-dialog title="sku明细" :visible.sync="skudtFormVisible" v-model="skudtFormVisible" :close-on-click-modal="false">
     <el-button type="primary" @click="addskudt" >新增sku明细</el-button>
<el-table :data="skudtinfo" highlight-current-row v-loading="listLoading" style="width: 100%;">
      <el-table-column prop="id" label="编号" width="80" sortable>
      </el-table-column>
      <el-table-column prop="detailname" label="名称" width="" sortable>
      </el-table-column>
      <el-table-column prop="detaildesc" label="描述" width="" sortable>
      </el-table-column>
      <el-table-column prop="detailnum" label="数量" width="" sortable>
      </el-table-column>
       <el-table-column prop="detailprice" label="价格" width="" sortable>
      </el-table-column>
      <el-table-column prop="detailicon" label="sku图片" width="" sortable>
      </el-table-column> 
      <el-table-column prop="createtime" label="创建时间" width="" sortable>
      </el-table-column>
       <el-table-column label="操作" width="" >
        <el-row class="edita" slot-scope="skuinfo">  
             <a href="#" @click="editskudtinfo(skuinfo.row)">编辑</a>
             <a href="#" @click="deleskudtinfo(skuinfo.row.id)">删除</a>
        </el-row> 
      </el-table-column>
    </el-table>
    <div slot="footer" class="dialog-footer">
    <el-button @click.native="skudtFormVisible = false">关闭</el-button>
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
  ChangeOrdersweb,GetShopListMyweb,
  DeleteShopListMyweb,AddShopListMyweb,uploadPicture,
  uploadPictureDetail,GetShopSkuInfoMyweb,DeleteSkudetailInfoMyweb,DeleteSkuInfoMyweb,
  uploadPictureSkuDetail ,AddSkuMyweb ,AddSkuDetailMyweb,uploadPictureSku,
  configimgurl ,AddSkuAndDetail,UpdateGrounding
} from "../../api/api";
import { getButtonList } from "../../promissionRouter";
import Toolbar from "../../components/Toolbar";

export default {
  components: { Toolbar },
  data() {
    return {
      statusvalue:0,
      shoptmpimg:"",
      shopdetailtmpimg:"",
      shopskutmpimg:"",
      shopskudttmpimg:"",

      skunamelist:[{skuname:""}],
      skunamedtlist:[{skudtname:""}],
      addeditshowsku:false,
      
     //推荐
      addskuFormVisible:false,
      addskudtFormVisible:false,
      tidFormVisible: false,
      tidLoading: false,
      form: {},
      skudtform:{},
      skuform:{},
      detailform: { },
      file: '',
       detailfile:'',
       skudetailfile:'',
       skufile:'',
      //detailname detaildesc detailprice detailnum detailicon
      Formadddt:
      {
        id:0,
        skuid:0,
        detailname :'',
        detaildesc:'',
        detailprice :0,
        detailnum :0,
        detailicon:'',
      },
      Formadd:
      {
      id:0,
      shopid:0,
      skuIcon:'',
      skuname:'',
      skudesc:'',
      },
      Formtid: {
        pDetailIcon:"",
        id: 0,
        minLevel: 0,
        price: 0,
        pNum: 1,
        pName: "",
        pDesc: "",
        pIcon: "",
        status: 1,
        ptype: 0,
        priceType: 1,
        Shopgroup: 1,
      },
      addskudtshow:false,
      skuFormVisible:false,
      skudtFormVisible:false,
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
      skuinfolist: [],
      skudtinfo: [],
      roles: [],
      total: 0,
      buttonList: [],
      currentRow: null,
      currentskuRow:null,
      currentskudtRow:null,
      page: 1,
      listLoading: false,
      sels: [], //列表选中列
    };
  },
  methods: {
    groundingshop(thisid)
    {
        this.$confirm("确认上下架该商品吗？", "提示", {}).then(() => {
            UpdateGrounding({id:thisid}).then((res) => {
              if (res.success) {
                this.tidFormVisible = false;
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
    getimgurl(imgurl)
    {   
    if(imgurl.length<200)
    {
    return configimgurl+imgurl;
    }else
    {
    return imgurl;
    }
    },
    addskuinfo()
    {
        this.Formadd.id=0;
        this.Formadd.shopid=this.currentRow.id;
        this.Formadd.skuIcon='';
        this.Formadd.skuname='';
        this.Formadd.skudesc='';
        this.shopskutmpimg='';
        this.addskuFormVisible=true;
        this.addeditshowsku=true;
        this.skunamelist=[{skuname:""}];
        this.addskudtshow=false;
    },
    addskudtnamelist()
    {
      this.skunamedtlist= [{skudtname:""}];
      this.Formadddt.detailname="";
      this.Formadddt.detaildesc="";
      this.Formadddt.detailprice=0;
      this.Formadddt.detailnum=0;
      if(this.addskudtshow)this.addskudtshow=false;else this.addskudtshow=true;
    },
    addskudt()
    {
       this.addskudtFormVisible=true;
       this.Formadddt.id=0;
       this.Formadddt.detailname='';
       this.Formadddt.detaildesc='';
       this.Formadddt.detailnum=0;
       this.Formadddt.detailicon='';
       this.Formadddt.detailprice=0;
       this.Formadddt.skudetailfile='';
       this.shopskudttmpimg='';
    },
    editsku(model)
    { 
      this.Formadd=model;
      this.shopskutmpimg=model.skuIcon;
      this.addskuFormVisible=true;
      this.addeditshowsku=false;
      this.addskudtshow=false;
    },
    editskudtinfo(model)
    {
      this.Formadddt=model;
      this.shopskudttmpimg=model.detailicon;
      this.addskudtFormVisible=true;
    },
    deleskudtinfo(dtid)
    {
       this.$confirm("确认删除吗？", "提示", {}).then(() => {
            DeleteSkudetailInfoMyweb({id:dtid}).then((res) => {
              if (res.success) {
                this.$message({
                  message: "操作成功",
                  type: "success",
                });
                this.editskuinfo();
                this.skudtFormVisible=false;
              } else {
                this.$message({
                  message: "操作失败请稍后再试！",
                  type: "error",
                });
              }
            });
          });
    },
    showeditskuinfo(model)
    {
      this.skudtFormVisible=true;
      this.skudtinfo=model.skudetailinfo;
      this.Formadddt.skuid=model.skuinfo.id;
    },
    getskudetail()
    {

    },
    deleskuinfo(skuid)
    {
      this.$confirm("确认删除吗？该明细下的商品也会跟着删除！", "提示", {}).then(() => {
            DeleteSkuInfoMyweb({id:skuid}).then((res) => {
              if (res.success) {
                this.tidFormVisible = false;
                this.$message({
                  message: "操作成功",
                  type: "success",
                });
                  this.editskuinfo();
              } else {
                this.$message({
                  message: res.msg,
                  type: "error",
                });
              }
            });
          });
    },
    editskuinfo()
    {
      let rows = this.currentRow;
      if (!rows) {
        this.$message({
          message: "请选择要操作的一行数据！",
          type: "error",
        });
        return;
      } 
       this.skuFormVisible=true;
       
       this.listLoading = true;
      //NProgress.start();
      GetShopSkuInfoMyweb({id:rows.id}).then((res) => {
        this.listLoading = false;
        this.skuinfolist=res.response.datainfo
        //NProgress.done();
      });

    },
    getFile(event) {
      this.file = event.target.files[0];

        let _this=this;
        if (!event || !window.FileReader) return  
        let reader = new FileReader()
        reader.readAsDataURL(this.file)
        reader.onloadend = function () {
        _this.shoptmpimg=this.result;
        }
    },
    getdetilFile(event) {
     this.detailfile = event.target.files[0];
      let _this=this;
        if (!event || !window.FileReader) return  
        let reader = new FileReader()
        reader.readAsDataURL(this.detailfile)
        reader.onloadend = function () {
        _this.shopdetailtmpimg=this.result;
        }
    },
    getskudtFile(event) {
      this.skudetailfile = event.target.files[0];
      console.log(this.skudetailfile )
       let _this=this;
        if (!event || !window.FileReader) return  
        let reader = new FileReader()
        reader.readAsDataURL(this.skudetailfile)
        reader.onloadend = function () {
        _this.shopskudttmpimg=this.result;
        }
    },
    getskuFile(event)
    {
     this.skufile = event.target.files[0];
       console.log(this.skufile )
        let _this=this;
        if (!event || !window.FileReader) return  
        let reader = new FileReader()
        reader.readAsDataURL(this.skufile)
        reader.onloadend = function () {
        _this.shopskutmpimg=this.result;
        }
    },
     onSubmit() {
      let that = this;
      event.preventDefault();//取消默认行为
      //创建 formData 对象
      let param = new FormData();
      param.append("file", this.file);
      param.append("id", this.Formtid.id);
      uploadPicture(param).then((res) => {
             this.file=""
            });
    },
     onSubmitdetail() {
      let that = this;
      event.preventDefault();//取消默认行为
      //创建 formData 对象
      let param = new FormData();
      param.append("file", this.detailfile);
      param.append("id", this.Formtid.id);
      uploadPictureDetail(param).then((res) => {
              this.detailfile=""
            });
    },
    onSubmitdetaillist(numlist)
    {
      let that = this;
      event.preventDefault();
      let param = new FormData();
      param.append("file", this.skudetailfile);
      param.append("id", numlist);
      uploadPictureSkuDetail(param).then((res) => {
            this.skudetailfile=""
      });
    },
    onskudtSubmit()
    {
      let that = this;
      event.preventDefault();
      let param = new FormData();
      param.append("file", this.skudetailfile);
      param.append("id", this.Formadddt.id);
      console.log(this.skudetailfile)
      console.log(this.Formadddt.id)
      uploadPictureSkuDetail(param).then((res) => {
               this.skudetailfile=""
      });
    },
     onskuSubmit()
    {
      let that = this;
      event.preventDefault();
      let param = new FormData();
      param.append("file", this.skufile);
      param.append("id", this.Formadd.id);
      uploadPictureSku(param).then((res) => {
             this.skufile=""
      });
    },
     onskuSubmitlist(numlist)
    {
      let that = this;
      event.preventDefault();
      let param = new FormData();
      param.append("file", this.skufile);
      param.append("id", numlist);
      uploadPictureSku(param).then((res) => {
            this.skufile=""
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
                  pDetailIcon:this.Formtid.pDetailIcon
            };
            let _this=this;
            AddShopListMyweb(para).then((res) => {
              if (res.success) {
                let resthis=res;
                this.levelFormVisible = false;
                if(this.file)
                {
                console.log(res.response); 
                this.Formtid.id=res.response;
                this.onSubmit();
                }
                if(this.detailfile)
                { 
                  console.log(res.response); 
                 this.Formtid.id=res.response; 
                 this.onSubmitdetail();
                 }
                this.$message({
                  message: "操作成功",
                  type: "success",
                });
                 setTimeout(function(){ _this.getUsers();},1000)
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
    addskunamelist()
    {
      this.skunamelist.push({skuname:""});
    },
    plusskunamelist()
    {
      if(this.skunamelist.length>1)
      {
        this.skunamelist.splice(this.skunamelist.length-1,1)
      }
    },
     addskudtname()
    {
      this.skunamedtlist.push({skudtname:""});
    },
    plusskudtname()
    {
      if(this.skunamedtlist.length>1)
      {
        this.skunamedtlist.splice(this.skunamedtlist.length-1,1)
      }
    },
    addskuSubmit:function()
    {  
        let _this=this;

        if(!this.addeditshowsku)
        {
        AddSkuMyweb(this.Formadd).then((res) => {
        if (res.success) {
        if(this.skufile)
        {
        this.Formadd.id=res.response;
        this.onskuSubmit();
        }
        this.$message({
        message: "操作成功",
        type: "success",
        });
        this.addskuFormVisible=false;
        this.editskuinfo();
        } else {
        this.$message({
        message: "操作失败请稍后再试！",
        type: "error",
        });
        }
        });
        }else
        {
          var  tmpskulist = [];
          this.skunamelist.forEach(element => {
              var ss = Object.assign({},this.Formadd);
              ss.skuname=element.skuname;
              tmpskulist.push(ss);
          });
        var  tmpskudtlist = [];
          if(this.addskudtshow)
          {
               this.skunamedtlist.forEach(element => {
              var ss = Object.assign({},this.Formadddt);
              ss.detailname=element.skudtname;
              tmpskudtlist.push(ss);
          });
          }

          AddSkuAndDetail({strskulist:JSON.stringify(tmpskulist) ,strskudtlist:JSON.stringify(tmpskudtlist)}).then((res) => {
               if(res.success)
               {
                    if(this.skufile && res.response.resultnum.length>0)
                    {
                        this.onskuSubmitlist(res.response.resultnum);
                    }
                    if(this.skudetailfile && res.response.resultdtnum.length>0)
                    {
                        this.onSubmitdetaillist(res.response.resultdtnum);
                    }
                      let _this=this;
                    setTimeout(function(){_this.editskuinfo(); _this.addskuFormVisible=false;},4000)
               }
            });
          

        this.$message({
        message: "操作成功",
        type: "success",
        });
      //  this.addskuFormVisible=false;
       // this.editskuinfo();
        }

       
    },
    addskudtSubmit:function()
    {
            let _this=this;
            AddSkuDetailMyweb(this.Formadddt).then((res) => {
              if (res.success) {
                let resthis=res;
                if(this.skudetailfile)
                {
                this.Formadddt.id=res.response;
                this.onskudtSubmit();
                }
                this.$message({
                  message: "操作成功",
                  type: "success",
                });
                 this.addskudtFormVisible=false;
                 this.skudtFormVisible=false;
                 this.editskuinfo();
              } else {
                this.$message({
                  message: "操作失败请稍后再试！",
                  type: "error",
                });
              }
            });
    },
    selectCurrentRow(val) {
      this.currentRow = val;
    },
    selectskuCurrentRow(val) {
      this.currentskuRow = val;
    },
    callFunction(item) {
      this.filters = {
        name: item.search,
      };
      this[item.Func].apply(this, item);
    },
    //修改状态
     updateshop(index, row) {
        this.detailfile="";
        this.detailform={};
        this.file="";
        this.form={};
       
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
       this.Formtid.pDetailIcon=rows.pDetailIcon;
       this.levelFormVisible=true;

       this.shoptmpimg=rows.pIcon;
       this.shopdetailtmpimg=rows.pDetailIcon;
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
        this.detailfile="";
        this.detailform={};
        this.file="";
        this.form={};
        this.Formtid.id=0;
        this.Formtid.minLevel=0;
        this.Formtid.price=0;
        this.Formtid.pNum=1;
        this.Formtid.pName="";
        this.Formtid.pDesc="";
        this.Formtid.pIcon="";
        this.Formtid.pDetailIcon="";
        this.Formtid.status=1;
        this.Formtid.ptype=0;
        this.Formtid.priceType=1;
        this.Formtid.Shopgroup=1;
        this.shoptmpimg="";
        this.shopdetailtmpimg="";
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
    selsskuChange: function(sels) {
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
.edita a
{
  margin-left:10px;

}
</style>
