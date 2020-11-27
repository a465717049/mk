<template>
  <ScrollRefresh @getData="getData" :residualHeight="0" :isNeedUp="false" class="chartsScroll">
    <div>
      <TopBar :option="topBarOption" :hasNoRead="false" @clickR="goNews">
        <div class="center-two-search">
          <div class="two-tit-t">排隊購買</div>
          <div @click="goBuy()" class="two-tit-b">＄{{ totalData }}</div>
          <img class="img-yueliang" src="@/assets/imgs/yueliang.png" alt />
        </div>
      </TopBar>
      <div class="analyse-body">
        <h4 class="an-bd-tit">
          行情
          <div class="an-btn">
            <router-link to="search" class="router">
              <i class="iconfont iconsearch"></i> 查詢
            </router-link>
          </div>
        </h4>
        <div class="an-bd-chart">
          <v-chart
            id="lineChart"
            ref="lineChart"
            :options="this.drawLine()"
            @mouseleave="mouseleave"
            @blur="mouseleave"
          />
        </div>
        <h4 class="an-bd-tit">概覽</h4>
        <div class="categories-bd">
          <div class="cg-item">
            <p>回報率</p>
            <v-chart id="binChart" ref="binChart" :options="this.drawPie()" />
          </div>
          <div class="cg-item cg-item-1">
            <div class="top">$</div>
            <p>初始成本</p>
            <span>＄ {{ initialVal || 0 }}</span>
          </div>
          <div class="cg-item cg-item-1 cg-item-2">
            <div class="top">+</div>
            <p>纍計收益</p>
            <span class="green">＄{{ withdrawVal || 0 }}</span>
          </div>
        </div>
      </div>
    </div>
  </ScrollRefresh>
</template>

<script>
import ECharts from "vue-echarts";
import TopBar from "components/TopBar";
import ScrollRefresh from "components/ScrollRefresh";
import { http } from "util/request";
import "echarts/lib/chart/bar";
import "echarts/lib/chart/pie";
import "echarts/lib/chart/line";
import "echarts/lib/component/tooltip";
import "echarts/lib/component/legend";
import "echarts/lib/component/polar";
import "echarts/lib/component/title";
import "echarts/lib/component/legendScroll";
import "echarts/lib/component/dataZoom";
import { GetStockPriceTrend, GetUserInfo, GetShouru } from "util/netApi";
import { storage } from "util/storage";
import { accessToken, loginPro } from "util/const.js";

export default {
  name: "Analyse",
  components: {
    TopBar,
    ScrollRefresh,
    "v-chart": ECharts
  },
  data() {
    return {
      topBarOption: {
        iconLeft: "iconzhankai",
        iconRight: ""
      },
      totalData: 0,
      lineData: [900, 3000, 400, 1800, 900, 1330, 1320],
      datearray: ["1", "2", "3", "4", "5", "6", "7"],
      binVal: 56,
      initialVal: 500,
      withdrawVal: 500
    };
  },
  mounted() {
    var v = this;
    this.$nextTick(() => {
      setTimeout(() => {
        this.mouseleave();
      }, 100);
    });
    window.addEventListener("resize", () => {
      v.$refs.lineChart.resize();
    });
    this.ToGetStockPriceTrend();
    this.TogetUserInfo();
    this.getShouru();
    this.ToGetStockPriceTrend();
  },
  methods: {
    getData() {
      this.ToGetStockPriceTrend();
      this.TogetUserInfo();
      this.getShouru();
    },
    goNews() {
      this.$router.push({ name: "News" });
    },
    goBuy() {
      this.$router.push({ name: "Queue" });
    },
    drawPie() {
      return {
        title: {
          text: this.binVal + "%",
          textStyle: {
            color: "#000",
            fontSize: 20
          },
          left: "center",
          top: "center"
        },
        angleAxis: {
          max: 800, // 满分
          axisLine: {
            show: false
          },
          axisTick: {
            show: false
          },
          axisLabel: {
            show: false
          },
          splitLine: {
            show: false
          }
        },
        radiusAxis: {
          type: "category",
          axisLine: {
            show: false
          },
          axisTick: {
            show: false
          },
          axisLabel: {
            show: false
          },
          splitLine: {
            show: false
          }
        },
        polar: {
          center: ["50%", "50%"],
          radius: "125%" //图形大小
        },
        series: [
          {
            type: "bar",
            data: [
              {
                value: this.binVal,
                itemStyle: {
                  normal: {
                    color: "#6318C3"
                  }
                }
              }
            ],
            coordinateSystem: "polar",
            roundCap: true,
            barWidth: 8,
            barGap: "-100%", // 两环重叠
            z: 2
          },
          {
            // 灰色环
            type: "bar",
            data: [
              {
                value: 800,
                itemStyle: {
                  color: "#facc5d",
                  shadowColor: "rgba(0, 0, 0, 0.1)",
                  shadowBlur: 5,
                  shadowOffsetY: 2
                }
              }
            ],
            coordinateSystem: "polar",
            roundCap: true,
            barWidth: 8,
            barGap: "-100%", // 两环重叠
            z: 1
          }
        ]
      };
    },

    drawLine() {
      var base = +new Date(1968, 9, 3);
      var oneDay = 24 * 3600 * 1000;
      var date = [];

      var data = [Math.random() * 300];

      for (var i = 1; i < 20000; i++) {
        var now = new Date((base += oneDay));
        date.push(
          [now.getFullYear(), now.getMonth() + 1, now.getDate()].join("/")
        );
        data.push(Math.round((Math.random() - 0.5) * 20 + data[i - 1]));
      }
      return {
        tooltip: {
          trigger: "axis",
          backgroundColor: "#fff",
          borderColor: "#fff",
          extraCssText: "box-shadow: 0 0 8px 3px #eee",
          axisPointer: {
            type: "line",
            snap: true,
            lineStyle: {
              type: "dashed",
              color: "#783cc7"
            }
          },
          textStyle: {
            color: "#783cc7",
            fontSize: "16",
            fontWeight: "bold"
          },
          formatter: "$ {c}"
        },
        xAxis: {
          type: "category",
          boundaryGap: false,
          axisTick: {
            show: false
          },
          axisLine: {
            lineStyle: {
              color: "#ccc"
            }
          },
          axisLabel: {
              interval:0,  
              rotate:-30,
            textStyle: {
              color: "#76787d", // x轴文字颜色
              fontSize:9
            }
           
          },
          data: this.datearray
        },
         grid: {
            left: 0,
            right: 0,
            // bottom: 30
          },
        yAxis: {
          type: "value",
          show: false,
          axisPointer: {
            type: "none" // 是否有坐标指示器
          }
        },
        dataZoom: [
          {
            type: "slider",
            start: 60,
            end: 100
          },
          {
            start: 60,
            end: 100,
             handleIcon:
              "M10.7,11.9v-1.3H9.3v1.3c-4.9,0.3-8.8,4.4-8.8,9.4c0,5,3.9,9.1,8.8,9.4v1.3h1.3v-1.3c4.9-0.3,8.8-4.4,8.8-9.4C19.5,16.3,15.6,12.2,10.7,11.9z M13.3,24.4H6.7V23h6.6V24.4z M13.3,19.6H6.7v-1.4h6.6V19.6z",
            handleSize: "96%",
            handleStyle: {
              color: "#fff",
              shadowBlur: 3,
              shadowColor: "rgba(0, 0, 0, 0.6)",
              shadowOffsetX: 2,
              shadowOffsetY: 2
            }
          }
        ],
        series: [
          {
            data: this.lineData,
            type: "line",
            smooth: false, // 曲线是否圆润
            symbol: (value, params) => {
              // 折线原点显示样式
              return "circle";
            }, // 是否去掉原点
            showSymbol: true, // 是否显示原点
            symbolSize: 10,
            itemStyle: {
              // 折现样式
              normal: {
                borderColor: "#fff",
                borderWidth: 1,
                color: "#783cc7", //改变折线点的颜色
                lineStyle: {
                  color: "#783cc7" //改变折线颜色
                },
                label:{
                  show:true,
                  position:'top',
                  textStyle: {
                                fontSize: 20
                            },
                  formatter: (params) => {
                                    if ( this.lineData.length - 2 == params.dataIndex) {
                                        return params.value
                                    } else {
                                        return ""
                                    }
                                }
                }
              }
            },
            areaStyle: {
              // 选中点及以下阴影
              color: {
                type: "linear",
                x: 0,
                y: 0,
                x2: 0,
                y2: 1,
                colorStops: [
                  {
                    offset: 0,
                    color: "rgba(100, 24, 195, 0.8)" // 0% 处的颜色
                  },
                  {
                    offset: 1,
                    color: "rgba(100, 24, 195, 0.02)" // 100% 处的颜色
                  }
                ]
              }
            }
          }
        ]
      };
    },
    mouseleave() {
      this.$refs.lineChart.dispatchAction({
        type: "showTip",
        seriesIndex: 0, //这行不能省
        dataIndex: 1
      });
    },
    ToGetStockPriceTrend() {
      http(GetStockPriceTrend, null, json => {
        if (json.code === 0) {
          this.totalData = json.response.totalamount.toString();
          let temp = "";
          for (let i = this.totalData.length; i > -3; i = i - 3) {
            let str = this.totalData.substring(i - 3, i);
            if (temp == "") {
              temp = str;
            } else {
              if (str != "") temp = str + "," + temp;
            }
          }
          this.totalData = temp;
          var tmpresult = new Array();
          var tmpdate = new Array();
      
          json.response.list.forEach(model => {
            tmpresult.unshift(model.price);
            var tmpyue = new Date(model.createTime.replace(/\-/g, "/")).getMonth()+1;
            var tmpcreat = new Date(model.createTime.replace(/\-/g, "/")).getDate();
               tmpdate.unshift(tmpyue+'-'+tmpcreat);
           
          });
          tmpresult.push(null)
          tmpdate.push('')
          //console.log(tmpresult, tmpdate, "tmpdate");
          this.lineData = tmpresult;
          this.datearray = tmpdate;
        }
      });
    },
    TogetUserInfo() {
      http(GetUserInfo, null, json => {
        if (json.code === 0) {
          this.initialVal = json.response.farmers == 1 ? 500 : 245;
          //this.withdrawVal = json.response.weekly
        }
      });
    },
    getShouru() {
      http(GetShouru, null, json => {
        if (json.code === 0) {
          this.withdrawVal = json.response.sum;
          this.binVal = ((json.response.sum / this.initialVal) * 100).toFixed(
            0
          );
        }
      });
    },
    go() {
      this.$router.push({ name: "exChangeData" });
    }
  }
};
</script>

<style lang="less" scope>
.chartsScroll {
  /deep/.wrapper .bscroll-container {
    height: calc(100vh + 400px);
  }
}
.two-tit-b {
  margin-top: 88px;
  font-size: 120px !important;
}
.analyse-body {
  background: #ebeaf0;
  position: absolute;
  top: 500px;
  width: 100%;
  // overflow: scroll;
  z-index: 999;
  border-radius: 100px 100px 0 0;
  // padding: 98px 60px 60px 20px;
  // padding-bottom: 400px;
  // height: calc(100vh - 60px);
  .an-bd-tit {
    padding: 98px 60px 60px 20px;
    display: flex;
    margin-left: 30px;
    justify-content: space-between;
    font-size: 64px;
    font-weight: bold;
    //border-bottom: 1px solid #ccc;
    padding-bottom: 20px;
    span {
      color: #6318c3;
      font-size: 50px;

      line-height: 110px;
    }
    .an-btn {
      display: inline-block;
      width: 220px;
      height: 80px;
      font-size: 50px;
      border-radius: 10px;
      margin-top: 20px;
      line-height: 66px;
      text-align: center;
      i {
        font-size: 50px;
      }
    }
  }

  .an-bd-chart {
    height: 720px;
  }
  .an-tab {
    display: flex;
  }
  .categories-bd {
    padding: 38px 20px 20px 20px;
    height: 500px;
    display: flex;
    padding-left: 40px;
    justify-content: space-between;
    .cg-item {
      width: 31%;
      background: #fff;
      margin-right: 6px;
      padding: 20px;
      padding-bottom: 20px;
      box-shadow: 4px 4px 1px #dddddd;
      border-radius: 30px 80px 30px 30px;
      &:last-child {
        margin-right: 0;
      }
      p {
        font-size: 46px;
        padding-top: 20px;
        color: #666;
      }
    }
    .cg-item-1 {
      padding-top: 190px;
      position: relative;
      .top {
        position: absolute;
        left: 30px;
        top: 30px;
        width: 100px;
        height: 100px;
        border-radius: 18px 8px 8px 8px;
        background: #ffcb2d;
        font-size: 62px;
        font-weight: bold;
        line-height: 90px;
        text-align: center;
        color: #fff;
      }
      span {
        color: #6318c3;
        font-size: 50px;
        font-weight: bold;
        padding-top: 20px;
        display: inline-block;
      }
      .green {
        color: #80b234;
      }
    }
    .cg-item-2 {
      .top {
        font-size: 86px;
      }
    }
  }
}
</style>
<style lang="less">
.an-bd-chart {
  .echarts {
    width: 100vw !important;
    height: 100% !important;
  }
}
.cg-item {
  position: relative;
  .echarts {
    position: absolute;
    left: -4px;
    top: 50px;
  }
  canvas,
  .echarts,
  .echarts div {
    height: 100% !important;
    width: 100% !important;
  }
}
</style>
