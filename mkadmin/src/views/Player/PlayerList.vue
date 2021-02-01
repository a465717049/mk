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
    
   <el-col  :span="24" class="toolbar" style="padding-bottom: 0px;">
    <el-form :inline="true" >
      <el-form-item>
      <el-select v-model="ustatus" placeholder="订单状态">
      <el-option label="全部状态" value="">全部状态</el-option>
      <el-option label="正常" value="0">正常</el-option>
      <el-option label="锁定" value="1">锁定</el-option>
      </el-select>
      </el-form-item>  

      <el-form-item>
      <el-select v-model="ulevel" placeholder="选择职位">
      <el-option label="全部职位" value=""></el-option>
      <el-option label="会员" value="0"></el-option>
      <el-option label="经理" value="1"></el-option>
      <el-option label="总监" value="2"></el-option>
      <el-option label="总裁" value="3"></el-option>
      <el-option label="董事" value="4"></el-option>
      <el-option label="合伙人" value="5"></el-option>
      </el-select>
      </el-form-item> 

      <el-form-item>
      <el-select v-model="uhonur" placeholder="选择等级">
      <el-option label="全部等级" value=""></el-option>
      <el-option label="初级会员" value="1"></el-option>
      <el-option label="中级会员" value="2"></el-option>
      <el-option label="高级会员" value="3"></el-option>
      <el-option label="超级会员" value="4"></el-option>
      </el-select>
      </el-form-item> 

     <el-form-item>
      <el-input type="text" placeholder="输入推荐人" v-model="ujid"></el-input>
      </el-form-item>
      <el-form-item> 
        <el-input type="date" v-model="startdate"></el-input> 
      </el-form-item>
        <el-form-item>
      <el-input type="date" v-model="enddate"></el-input>
      </el-form-item>
    </el-form>
   </el-col>

     <el-col  :span="24" class="toolbar" style="padding-bottom: 0px;">
    <el-form :inline="true" >

     <el-form-item>
      <el-input type="text" placeholder="输入起始安置业绩" v-model="stid"></el-input>
      </el-form-item>
      <el-form-item>
      <el-input type="text" placeholder="输入截止安置业绩" v-model="etid"></el-input>
      </el-form-item>
      <el-form-item>
      <el-input type="text" placeholder="输入起始推荐业绩" v-model="sjid"></el-input>
      </el-form-item>
       <el-form-item>
      <el-input type="text" placeholder="输入截止推荐业绩" v-model="ejid"></el-input>
      </el-form-item>
    </el-form>
   </el-col>


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
           <el-tag type="success" @click="showrelation(scope.row.uid)" >关系图</el-tag>
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

    <el-dialog title="关系图" :visible.sync="relationFormVisible" v-model="relationFormVisible" :close-on-click-modal="false">
      <div class="relative">
    <div class="relation-box p-58 border-top-radius relative bg-gray">
      <div class="relation">
        <div class="concat">
          <div class="first-relation one base-flex around P-58 mt-100">
            <div class="opa-text Tright num1 font60 mt-100">{{First.LProfit}}</div>
            <div class="person center ver-center">
              <img :src="First.photo" class="author" alt />
              <div class="name">{{First.NickName+'('+First.uID+')'}}</div>
            </div>
            <div class="opa-text Tleft num2 font60 mt-100">{{First.RProfit}}</div>
          </div>
          <!-- 第二层 -->
          <div class="first-relation two base-flex around">
            <div v-for="(item,index) in SecList" :key="'s'+index">
              <div v-if="item.jid>0&&item.uID>0" @click="go(item.uID)" class="person1 center ver-center">
                <img :src="item.photo" class="author" alt />
                <div class="name">{{item.NickName+'('+item.uID+')'}}</div>
              </div>
              <div v-else-if="item.jid>0&&item.uID==0">
                <div class="person1 center ver-center" @click="goJoin(item.jid,item.isLeft)">
                  <i class="author author-icon iconfont icondingwei"></i>
                  <div class="name">--</div>
                </div>
              </div>
              <div v-else-if="item.jid==0&&item.uID==0">
                <div class="person1 center ver-center">
                  <i class="author author-icon iconfont icondingwei"></i>
                  <div class="name"></div>
                </div>
              </div>
            </div>
          </div>
          <!-- 第三层 -->
          <div class="first-relation thr base-flex around">
            <div v-for="(item,index) in ThrLeftList" :key="'tl'+index" class="base-flex around">
              <div v-if="item.jid>0&&item.uID>0" @click="go(item.uID)" class="person3 center ver-center">
                <img :src="item.photo" class="author" alt />
                <div class="name"  v-html="item.NickName+'<br/>('+item.uID+')'"></div>
              </div>
              <div v-if="item.jid>0&&item.uID==0">
                <div class="person3 center ver-center" @click="goJoin(item.jid,item.isLeft)">
                  <i class="author author-icon iconfont icondingwei"></i>
                  <div class="name">--</div>
                </div>
              </div>
              <div v-if="item.jid==0&&item.uID==0">
                <div class="person3 center ver-center">
                  <i class="author author-icon iconfont icondingwei"></i>
                  <div class="name"></div>
                </div>
              </div>
            </div>
            <div v-for="(item,index) in ThrRightList" :key="'tr'+index" class="base-flex around">
              <div v-if="item.jid>0&&item.uID>0" @click="go(item.uID)" class="person3 center ver-center">
                <img :src="item.photo" class="author" alt />
                <div class="name" v-html="item.NickName+'<br/>('+item.uID+')'" ></div>
              </div>
              <div v-if="item.jid>0&&item.uID==0">
                <div class="person3 center ver-center" @click="goJoin(item.jid,item.isLeft)">
                  <i class="author author-icon iconfont icondingwei"></i>
                  <div class="name">--</div>
                </div>
              </div>
              <div v-if="item.jid==0&&item.uID==0">
                <div class="person3 center ver-center">
                  <i class="author author-icon"></i>
                  <div class="name"></div>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div class="arrow base-flex flex-center">
          <div class="arrow-show center" @click="backUser()">
            <i class="iconfont iconarrow-left author font80"></i>
          </div>
          <div class="arrow-show center" @click="up()">
            <i class="iconfont iconarrowup author font80"></i>
          </div>
        </div>
      </div>
    </div>
  </div>
 
    <div class="relation-box p-58 border-top-radius relative bg-gray">
      <div class="relation">
        <div class="concat">
          <div class="first-relation one base-flex around P-58 mt-100">
            <div class="opa-text Tright num1 font60 mt-100">0</div>
            <div class="person center ver-center">
              <img  class="author" alt />
              <div class="name">0</div>
            </div>
            <div class="opa-text Tleft num2 font60 mt-100">0</div>
          </div>
          <!-- 第二层 -->
          <div class="first-relation two base-flex around">
            <div v-for="(item,index) in SecList" :key="'s'+index">
              <div v-if="item.jid>0&&item.uID>0" @click="go(item.uID)" class="person1 center ver-center">
                <img :src="item.photo" class="author" alt />
                <div class="name">{{item.NickName+'('+item.uID+')'}}</div>
              </div>
              <div v-else-if="item.jid>0&&item.uID==0">
                <div class="person1 center ver-center" @click="goJoin(item.jid,item.isLeft)">
                  <i class="author author-icon iconfont icondingwei"></i>
                  <div class="name">--</div>
                </div>
              </div>
              <div v-else-if="item.jid==0&&item.uID==0">
                <div class="person1 center ver-center">
                  <i class="author author-icon iconfont icondingwei"></i>
                  <div class="name"></div>
                </div>
              </div>
            </div>
          </div>
          <!-- 第三层 -->
          <div class="first-relation thr base-flex around">
            <div v-for="(item,index) in ThrLeftList" :key="'tl'+index" class="base-flex around">
              <div v-if="item.jid>0&&item.uID>0" @click="go(item.uID)" class="person3 center ver-center">
                <img :src="item.photo" class="author" alt />
                <div class="name"  v-html="item.NickName+'<br/>('+item.uID+')'"></div>
              </div>
              <div v-if="item.jid>0&&item.uID==0">
                <div class="person3 center ver-center" @click="goJoin(item.jid,item.isLeft)">
                  <i class="author author-icon iconfont icondingwei"></i>
                  <div class="name">--</div>
                </div>
              </div>
              <div v-if="item.jid==0&&item.uID==0">
                <div class="person3 center ver-center">
                  <i class="author author-icon iconfont icondingwei"></i>
                  <div class="name"></div>
                </div>
              </div>
            </div>
            <div v-for="(item,index) in ThrRightList" :key="'tr'+index" class="base-flex around">
              <div v-if="item.jid>0&&item.uID>0" @click="go(item.uID)" class="person3 center ver-center">
                <img :src="item.photo" class="author" alt />
                <div class="name" v-html="item.NickName+'<br/>('+item.uID+')'" ></div>
              </div>
              <div v-if="item.jid>0&&item.uID==0">
                <div class="person3 center ver-center" @click="goJoin(item.jid,item.isLeft)">
                  <i class="author author-icon iconfont icondingwei"></i>
                  <div class="name">--</div>
                </div>
              </div>
              <div v-if="item.jid==0&&item.uID==0">
                <div class="person3 center ver-center">
                  <i class="author author-icon"></i>
                  <div class="name"></div>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div class="arrow base-flex flex-center">
          <div class="arrow-show center" @click="backUser()">
            <i class="iconfont iconarrow-left author font80"></i>
          </div>
          <div class="arrow-show center" @click="up()">
            <i class="iconfont iconarrowup author font80"></i>
          </div>
        </div>
      </div>
    </div>
    </el-dialog>

    <!--导出记录-->
      <el-dialog title="导出记录" :visible.sync="outinfoVisible" v-model="outinfoVisible" :close-on-click-modal="false">
      <el-button type="primary" @click="sumbitoutput" >导出当前会员记录</el-button>
      <el-button type="primary" @click="sumbitazoutput" >导出当前安置记录</el-button>
      <el-button type="primary" @click="sumbittjoutput" >导出当前推荐记录</el-button>
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
  GetALLUserInfo,
  testapi,
  GetUserInfoWeek,
  adminResetlock,
  adminResetpwd,
  adminResetlevel,
  adminResettid,
  adminResetanswer,
  adminResetidcard,
  GetSearchRelation ,
  photoList,
  dowmexcel,GetALLUserInfoExcel,GetDownExcelList,DeleteDownExcelList
} from "../../api/api";
import { getButtonList } from "../../promissionRouter";
import Toolbar from "../../components/Toolbar";

export default {
  components: { Toolbar },
  data() {
    return {
      stid:"",
      etid:"",
      sjid:"",
      ejid:"",
       downinfo:
      [{
      id:0,
      downname:'',
      downdate:'',
      }],
        outinfoVisible:false,
      relationFormVisible:false,
       realtionid: 0,
      topBarOption: {
        iconLeft: "back",
        iconRight: ""
      },
      tipTitle: "xx",
      isEnter: false,
      upID: 0,
      parentId: 0,
      isLeft: 1,
      historyList: [],
      First: {},
      SecList: [],
      ThrLeftList: [],
      ThrRightList: [],
      ujid:"",
      ulevel:"",
      uhonur:"",
      ustatus:"",
      startdate:"",
      enddate:"",
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
     sumbitoutput()
     {
        let para = {
        pageindex: this.page,
        pagesize: 20,
        key: this.filters.name,
        ujid:this.ujid,
        ulevel:this.ulevel,
        uhonur:this.uhonur,
        ustatus:this.ustatus,
        startdate:this.startdate,
        enddate:this.enddate
      };
        this.$message({
        message: "已加入导出列表请稍后再来查看",
        type: "success",
        });
        GetALLUserInfoExcel(para).then((res) => {
        this.getorderoutput();
        this.listLoading = false;
        this.$message({
        message: "导出成功",
        type: "success",
        });
        });
     },
     sumbitazoutput()
     {
  let para = {
        pageindex: this.page,
        pagesize: 20,
        key: this.filters.name,
        ujid:this.ujid,
        ulevel:this.ulevel,
        uhonur:this.uhonur,
        ustatus:this.ustatus,
        startdate:this.startdate,
        enddate:this.enddate,
        stid:this.stid,
        etid:this.etid,
        sjid:this.sjid,
        ejid:this.ejid,
        type:"tid"
      };
        this.$message({
        message: "已加入导出列表请稍后再来查看",
        type: "success",
        });
        GetALLUserInfoExcel(para).then((res) => {
        this.getorderoutput();
        this.listLoading = false;
        this.$message({
        message: "导出成功",
        type: "success",
        });
        });

     },
     sumbittjoutput()
     {
  let para = {
        pageindex: this.page,
        pagesize: 20,
        key: this.filters.name,
        ujid:this.ujid,
        ulevel:this.ulevel,
        uhonur:this.uhonur,
        ustatus:this.ustatus,
        startdate:this.startdate,
        enddate:this.enddate,
        stid:this.stid,
        etid:this.etid,
        sjid:this.sjid,
        ejid:this.ejid,
        type:"jid"
      };
        this.$message({
        message: "已加入导出列表请稍后再来查看",
        type: "success",
        });
        GetALLUserInfoExcel(para).then((res) => {
        this.getorderoutput();
        this.listLoading = false;
        this.$message({
        message: "导出成功",
        type: "success",
        });
        });

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
      });
     },
    deleteexcel(thisid)
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
     downexcel(thisurl)
     {
      window.open(dowmexcel+thisurl)
     } ,
    getuserout()
    {
      this.outinfoVisible=true;
      this.getorderoutput();
    },
    showrelation(thisid)
    {
      this.realtionid=thisid;
      this.loadData();
      this.relationFormVisible=true;
      
    },
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
       return row.farmers == 1 ? "初级会员" : row.farmers == 2 ? "中级会员": row.farmers == 3 ? "高级会员"  : "超级会员";
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
     // ujid ulevel uhonur ustatus startdate enddate
      let para = {
        pageindex: this.page,
        pagesize: 20,
        key: this.filters.name,
        ujid:this.ujid,
        ulevel:this.ulevel,
        uhonur:this.uhonur,
        ustatus:this.ustatus,
        startdate:this.startdate,
        enddate:this.enddate,
        stid:this.stid,
        etid:this.etid,
        sjid:this.sjid,
        ejid:this.ejid,
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
      goJoin(parentId, isLeft) {
      this.isEnter = true;
      this.parentId = parentId;
      this.isLeft = isLeft;
    },
    clickOk() {
      this.isEnter = false;
      this.$router.push({
        name: "JoinUs",
        params: { uid: this.parentId, isLeft: this.isLeft }
      });
    },
    clickNo() {
      this.isEnter = false;
      this.$router.push({
        name: "JoinFamily",
        params: { uid: this.parentId, isLeft: this.isLeft }
      });
    },
    go(uid) {
      this.realtionid = uid;
      this.loadData();
    },
    up() {
      this.realtionid = this.upID;
      this.loadData();
    },
    backUser() {
      var that = this;
      for (var index = this.historyList.length; index >= 0; index--) {
        var element = this.historyList[index];
        if (element == this.realtionid) {
          let i = index > 0 ? index - 1 : index;
          this.realtionid = that.historyList[i];
          that.historyList.splice(index, 1);
          that.loadData(true);
          break;
        }
      }
    },
    Search(uid)
    {
      if(uid=="")
       uid=0
      this.realtionid =uid;
      this.loadData();
    },
    loadData(history) {
      var that = this;
      if (!history) {
        this.historyList.push(this.realtionid);
      }
      this.SecList = [];
      this.ThrLeftList = [];
      this.ThrRightList = [];

      GetSearchRelation({ uid: this.realtionid }).then((json) => {
       
        if (json.response) {
          that.friendsList = json.response;
          that.First = that.friendsList[0];
          that.upID = that.friendsList[0].jid;

          that.friendsList.forEach(element => {
            element.photo = photoList[element.photo];
            if (element.ceng == 1) {
              that.SecList.push(element);
            } else if (element.ceng == 2) {
              if (element.pos <= 2) {
                that.ThrLeftList.push(element);
              } else {
                that.ThrRightList.push(element);
              }
            }
          });

          if (that.SecList.length == 1) {
            that.SecList.push({
              honorLevel: 0,
              InvestmentLevel: 0,
              NickName: "",
              photo: "",
              jid: 0,
              L: 1,
              R: 0,
              subAccount: 0,
              friends: 0,
              LProfit: 0,
              RProfit: 0,
              LCount: 0,
              RCount: 0,
              uID: 0
            });
          }
          if (that.ThrLeftList.length < 2) {
            that.ThrLeftList.push({
              honorLevel: 0,
              InvestmentLevel: 0,
              NickName: "",
              photo: "",
              jid: 0,
              L: 1,
              R: 0,
              subAccount: 0,
              friends: 0,
              LProfit: 0,
              RProfit: 0,
              LCount: 0,
              RCount: 0,
              uID: 0
            });
            if (that.ThrLeftList.length == 1) {
              that.ThrLeftList.push({
                honorLevel: 0,
                InvestmentLevel: 0,
                NickName: "",
                photo: "",
                jid: 0,
                L: 1,
                R: 0,
                subAccount: 0,
                friends: 0,
                LProfit: 0,
                RProfit: 0,
                LCount: 0,
                RCount: 0,
                uID: 0
              });
            }
          }
          if (that.ThrRightList.length < 2) {
            that.ThrRightList.push({
              honorLevel: 0,
              InvestmentLevel: 0,
              NickName: "",
              photo: "",
              jid: 0,
              L: 1,
              R: 0,
              subAccount: 0,
              friends: 0,
              LProfit: 0,
              RProfit: 0,
              LCount: 0,
              RCount: 0,
              uID: 0
            });
            if (that.ThrRightList.length == 1) {
              that.ThrRightList.push({
                honorLevel: 0,
                InvestmentLevel: 0,
                NickName: "",
                photo: "",
                jid: 0,
                L: 1,
                R: 0,
                subAccount: 0,
                friends: 0,
                LProfit: 0,
                RProfit: 0,
                LCount: 0,
                RCount: 0,
                uID: 0
              });
            }
          }
        }
      });
    }
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

     this.realtionid = this.$route.params.uid;
  },
};
</script>
<style lang='less' scoped>

.relation-box {
  margin-top: -20px;
  // height: 75vh;
  // overflow: hidden;
}
.author-icon {
  font-size: 90px;
  opacity: 0.4;
}
.name {
  font-weight: bold;
  color: rgba(94, 91, 94, 0.877);
}
.relation {
  .concat {
    height: 1195px;
    padding: 20px 58px;
    background: url("../../assets/imgs/set/BG2.png") center center / 100% 100%;
    .title {
      color: rgba(25, 24, 25, 1);
      opacity: 0.52;
    }
    .first-relation {
      .num1 {
        width: 33%;
      }
      margin-top: 120px;
      .num2 {
        width: 33%;
      }
      .person {
        width: 234px;
        height: 229px;
        margin: 0 60px;
        background: url("../../assets/imgs/set/circle-img.png") center center /
          100% 100%;
        .author {
          width: 118px;
          height: 118px;
          margin-top: 40px;
          margin-left: 10px;
          display: inline-block;
        }
        .name {
          // width: 132px;
          height: 32px;
          margin: 62px auto 0;
          padding-left: 6px;
        }
      }
    }
     .first-relation.one {
       margin-top: 60px;
     }
      .first-relation.two {
       margin-top: 100px;
     }
      .first-relation.thr {
       margin-top: 160px;
     }
    .person1 {
      width: 263px;
      height: 239px;
      background: url("../../assets/imgs/set/triger-img.png") center center /
        100% 100%;
      .author {
        width: 122px;
        height: 122px;
        margin-left: 20px;
        margin-top: 62px;
      }
      i.author {
        width: 122px;
        height: 122px;
        margin-left: 20px;
        display: inline-block;
        margin-top: 50px;
      }
      .name {
        // width: 132px;
        height: 32px;
        margin: 62px auto 0;
        padding-left: 16px;
      }
    }
    .person2 {
      width: 263px;
      height: 239px;
      background: url("../../assets/imgs/set/triger-img.png") center center /
        100% 100%;
      .author {
        width: 122px;
        height: 122px;
        margin-left: 10px;
        margin-top: 60px;
      }

      .name {
        width: 132px;
        height: 32px;
        margin: 22px auto 0;
        padding-left: 6px;
      }
    }
    .person3 {
      width: 184px;
      height: 184px;
      margin: 0 20px;
      justify-content: center;
      align-items: center;
      background: url("../../assets/imgs/set/circle-img.png") center center /
        100% 100%;
      .author {
        width: 119px;
        height: 119px;
        margin-left: 12px;
        margin-top: 20px;
        display: inline-block;
      }

      .name {
        width: 132px;
        height: 32px;
        display: block;
        text-align: center;
        margin: 32px auto 0;
        padding-left: 6px;
      }
      .name.add {
        margin: 22px auto 0;
      }
    }
  }
}
.arrow {
  margin-top: 50px;
  .arrow-show {
    width: 143px;
    height: 140px;
    margin: 0 30px;
    background: url("../../assets/imgs/set/squer-img.png") center center / 100%
      100%;
    .author {
      width: 122px;
      height: 122px;
      margin-left: 10px;
      margin-top: 64px;
    }

    .name {
      width: 132px;
      height: 32px;
      margin: 32px auto 0;
      padding-left: 6px;
    }
  }
  .person3 {
    width: 184px;
    height: 184px;
    margin: 0 20px;
    justify-content: center;
    align-items: center;
    background: url("../../assets/imgs/set/circle-img.png") center center / 100%
      100%;
    .author {
      width: 119px;
      height: 119px;
      margin-left: 12px;
      margin-top: 20px;
      display: inline-block;
    }

    .name {
      width: 132px;
      height: 32px;
      display: block;
      text-align: center;
      margin: 32px auto 0;
      padding-left: 6px;
    }
    .name.add {
      margin: 22px auto 0;
    }
  }
}
.person2 {
  width: 263px;
  height: 239px;
  background: url("../../assets/imgs/set/triger-img.png") center center / 100%
    100%;
  .author {
    width: 122px;
    height: 122px;
    margin-left: 10px;
    margin-top: 60px;
  }

  .name {
    width: 132px;
    height: 32px;
    margin: 22px auto 0;
    padding-left: 6px;
  }
}
.person3 {
  width: 184px;
  height: 184px;
  margin: 0 20px;
  justify-content: center;
  align-items: center;
  background: url("../../assets/imgs/set/circle-img.png") center center / 100%
    100%;
  .author {
    width: 119px;
    height: 119px;
    margin-left: 12px;
    margin-top: 20px;
    display: inline-block;
  }

  .name {
    width: 132px;
    height: 32px;
    display: block;
    text-align: center;
    margin: 22px auto 0;
    padding-left: 6px;
  }
  .name.add {
    margin: 22px auto 0;
  }
}

.arrow {
  margin-top: 50px;
  .arrow-show {
    width: 143px;
    height: 140px;
    margin: 0 30px;
    background: url("../../assets/imgs/set/squer-img.png") center center / 100%
      100%;
    .author {
      color: #7ab4b4;
      margin-top: 35px;
      margin-left: 10px;
    }
  }
}
</style>