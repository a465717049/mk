<template>
  <section>
    <el-scrollbar class="default-scrollbar" wrap-class="default-scrollbar__wrap" view-class="p20-scrollbar__view">
      <el-row :gutter="20">
        <el-col :span="12" class="echarts-item">
          <div class="content-title">服务中心统计数据</div>
          <ve-histogram :data="histogramChartDataWeek" :extend="extendweek" :settings="histogramChartSettingsWeek" :mark-line="histogramChartMarkLine"></ve-histogram>
        </el-col>
        <el-col :span="12" class="echarts-item">
          <div class="content-title">出售/购买</div>
          <ve-line :data="lineChartData7Day" :extend="extend" :settings="lineChartSettings7Day"></ve-line>
        </el-col>
      </el-row>
      <el-row :gutter="20">
              <div style="margin-left:20px;">
          <el-date-picker         v-model="date1"         type="daterange"         align="right" value-format="yyyy-MM-dd"         unlink-panels         range-separator="至"         start-placeholder="开始日期"         end-placeholder="结束日期" @change="onPickdate"         :picker-options="pickerOptions2">
                    </el-date-picker>
        </div>
        <el-col :span="12" class="echarts-item">
            
          <div class="content-title">服务中心统计数据</div>
          <ve-histogram :data="histogramChartDataWeekdate" :extend="extendweek" :settings="histogramChartSettingsWeek" :mark-line="histogramChartMarkLine"></ve-histogram>
        </el-col>
        <el-col :span="12" class="echarts-item">
          <div class="content-title">出售/购买</div>
          <ve-line :data="lineChartData7Daydate" :extend="extend" :settings="lineChartSettings7Day"></ve-line>
        </el-col>
      </el-row>
    </el-scrollbar>
  </section>
</template>

<script>
import Vue from "vue";
import VCharts from "v-charts";
Vue.use(VCharts);
import { GetSPSearchServicInfo } from "../../api/api";

export default {
  name: "AdminDashboard",
  data() {
    return {
      date1: "",
      date2: "",
      pickerOptions2: {
        shortcuts: [
          {
            text: "最近一周",
            onClick(picker) {
              const end = new Date();
              const start = new Date();
              start.setTime(start.getTime() - 3600 * 1000 * 24 * 7);
              picker.$emit("pick", [start, end]);
            },
          },
          {
            text: "最近一个月",
            onClick(picker) {
              const end = new Date();
              const start = new Date();
              start.setTime(start.getTime() - 3600 * 1000 * 24 * 30);
              picker.$emit("pick", [start, end]);
            },
          },
          {
            text: "最近三个月",
            onClick(picker) {
              const end = new Date();
              const start = new Date();
              start.setTime(start.getTime() - 3600 * 1000 * 24 * 90);
              picker.$emit("pick", [start, end]);
            },
          },
        ],
      },
      histogramChartDataWeek: {
        columns: [],
        rows: [],
      },
      histogramChartDataWeekdate: {
        columns: [],
        rows: [],
      },
      histogramChartSettingsWeek: {},
      histogramChartMarkLine: {},
      lineChartData7Day: {
        columns: [],
        rows: [],
      },
      lineChartData7Daydate: {
        columns: [],
        rows: [],
      },
      lineChartSettings7Day: {
        metrics: ["sell", "buy"],
        dimension: ["ServiceId"],
      },
      extend: {
        tooltip: {
          show: true,
          formatter: function (datas) {
            var res = datas[0].name + "<br/>",
              val;
            for (var i = 0, length = datas.length; i < length; i++) {
              val = datas[i].value[1];
              var value = datas[i].seriesName;
              if (value === "sell") {
                value = "出售";
              } else if (value === "buy") {
                value = "购买";
              }
              res += value + "：" + val + "<br/>";
            }
            return res;
            //    debugger;
          },
        },
        legend: {
          formatter: function (value) {
            if (value === "sell") {
              return "出售";
            } else if (value === "buy") {
              return "购买";
            }
          },
        },
        series: {
          label: {
            normal: {
              show: true,
            },
          },
        },
      },
      extendweek: {
        tooltip: {
          show: true,
          formatter: function (datas) {
            var res = datas[0].name + "<br/>",
              val;
            for (var i = 0, length = datas.length; i < length; i++) {
              val = datas[i].value;
              var value = datas[i].seriesName;
              if (value === "Total") {
                value = "总计";
              } else if (value === "gaoji") {
                value = "高级";
              } else if (value === "zhongji") {
                value = "中级";
              } else if (value === "zhu") {
                value = "主";
              } else if (value === "zi") {
                value = "子";
              }
              res += value + "：" + val + "<br/>";
            }
            return res;
            //    debugger;
          },
        },
        legend: {
          formatter: function (value) {
            if (value === "Total") {
              return "总计";
            } else if (value === "gaoji") {
              return "高级";
            } else if (value === "zhongji") {
              return "中级";
            } else if (value === "zhu") {
              return "主";
            } else if (value === "zi") {
              return "子";
            }
          },
        },
      },
    };
  },
  created: function () {},
  methods: {
    onPickdate(pickdate) {
      var para = { beginTime: pickdate[0], endTime: pickdate[1] };
      this.getServiceInfo(para);
    },
    getServiceInfo(para) {
      GetSPSearchServicInfo(para).then((res) => {
        console.log(res);
        var tmpcolum = new Array();
        try {
          res.response.Table1.forEach((e) => {
            for (var name in e) {
              tmpcolum.push(name);
            }
            throw new Error("1");
          });
        } catch {}

        var lineServiceData = new Array();
        //start 删除
        var indexsell = tmpcolum.indexOf("sell");
        tmpcolum.splice(indexsell, 1);
        var indexbuy = tmpcolum.indexOf("buy");
        tmpcolum.splice(indexbuy, 1);
        //end
        this.histogramChartDataWeek.columns = tmpcolum;
        this.histogramChartDataWeek.rows = res.response.Table1;

        this.lineChartData7Day.columns = ["ServiceId", "sell", "buy"];
        this.lineChartData7Day.rows = res.response.Table1;

        //date
        this.histogramChartDataWeekdate.columns = tmpcolum;
        this.histogramChartDataWeekdate.rows = res.response.Table2;
        this.lineChartData7Daydate.columns = ["ServiceId", "sell", "buy"];
        this.lineChartData7Daydate.rows = res.response.Table2;
      });
    },
  },
  mounted() {
    this.getServiceInfo({});
  },
};
</script>

<style scoped>
.content-title {
  clear: both;
  font-weight: 400;
  line-height: 50px;
  padding: 10px 10px;
  font-size: 22px;
  color: #1f2f3d;
}
</style>
