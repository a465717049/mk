<template>
  <section>
    <el-scrollbar
      class="default-scrollbar"
      wrap-class="default-scrollbar__wrap"
      view-class="p20-scrollbar__view"
    >
      <el-row :gutter="20" >
       <el-col  class="toolbar"> 
        <el-col :span="15">
        <el-date-picker
        v-model="date1"
        type="daterange"
        align="right"
        value-format="yyyy-MM-dd"
        unlink-panels
        range-separator="至"
        start-placeholder="开始日期"
        end-placeholder="结束日期"
        @change="onPickdate"
        :picker-options="pickerOptions2" >
        </el-date-picker>
        <el-button type="primary" @click="btnclick('day')">日拨比</el-button>
        <el-button type="primary" @click="btnclick('month')">月拨比</el-button>
        <el-button type="primary" @click="btnclick('year')">年拨比</el-button>
        </el-col>
        </el-col>
   <el-col :span="12" class="echarts-item"  >
          <div class="content-title">新增業績/獎勵產出</div>
          <ve-line
            :data="lineChartData7Day"
            :extend="extend"
            :settings="lineChartSettings7Day"
          ></ve-line>
        </el-col>
      </el-row>
     
    </el-scrollbar>
      </el-row>
  </section>
</template>

<script>
import Vue from "vue";
import VCharts from "v-charts";
Vue.use(VCharts);
import {  getAccessApiByDate ,getRequestApiinfoByInManageWork} from "../../api/api";

export default {
  name: "AdminDashboard",
  data() {
    return {
    date1 :"",
      extend: {
        series: {
          label: {
            normal: {
              show: true
            }
          }
        }
      },
       pickerOptions2: {
          shortcuts: [{
            text: '最近一周',
            onClick(picker) {
              const end = new Date();
              const start = new Date();
              start.setTime(start.getTime() - 3600 * 1000 * 24 * 7);
              picker.$emit('pick', [start, end]);
            }
          }, {
            text: '最近一个月',
            onClick(picker) {
              const end = new Date();
              const start = new Date();
              start.setTime(start.getTime() - 3600 * 1000 * 24 * 30);
              picker.$emit('pick', [start, end]);
            }
          }, {
            text: '最近三个月',
            onClick(picker) {
              const end = new Date();
              const start = new Date();
              start.setTime(start.getTime() - 3600 * 1000 * 24 * 90);
              picker.$emit('pick', [start, end]);
            }
          }]
        },
      lineChartData7Day: {
        columns: [],
        rows: []
      },
      lineChartSettings7Day: {
        metrics: ["count","epcount"],
        dimension: ["date"]
      }
    };
  },
  created: function() {},
  methods: {
  onPickdate(pickdate){ 
     var para={date1:pickdate[0],date2:pickdate[1]};
        getRequestApiinfoByInManageWork(para).then(res => {
        this.lineChartData7Day.rows=res.data.response.list;
    });
  },
  btnclick(options)
  {
  //    var now = new Date();
  //    var nowMonth = now.getMonth()+1<10?"0"+(now.getMonth()+1):now.getMonth()+1;
  //    var nowYear = now.getFullYear();
  //    var nowdate = now.getDate()<10?"0"+now.getDate():now.getDate();
  //   var tmpdateday =nowYear+"-"+nowMonth+"-"+nowdate;
  //    var tmpstartmonth =nowYear+"-"+nowMonth+"-"+"01";
  //    var tmpstartyear =nowYear+"-01"+"-01";
  //     debugger;
   this.loadInfo({date1:"",date2:"",options:options});
  },
  loadInfo(para)
  {
        getRequestApiinfoByInManageWork(para).then(res => {
        this.lineChartData7Day.columns=["date","count","epcount"];
        this.lineChartData7Day.rows=res.data.response.list;
    });
      
  }
  },
  mounted() {
    this.loadInfo({});
  }
};
</script>

<style scoped>
.content-title {
  text-align: center;
  clear: both;
  font-weight: 400;
  line-height: 50px;
  padding: 10px 10px;
  font-size: 22px;
  color: #1f2f3d;
}
</style>
