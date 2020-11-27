<template>
  <div class="splitWrapper">
    <TopBar class="center-one-search" :option="topBarOption">
     拆配記錄
    </TopBar>
    <div class="innerWrap">
      <ul>
        <li v-for="(item,index) in list" :key="index">
          <div class="time">{{item.date}}</div>
          <div class="whiteWrap">
            <div class="top clearfix">
              <span class="before fl">
                {{item.splitbefore}}
                <em>（前）</em>
              </span>
              <i class="arrow fl">→</i>
              <span class="after fr">
                {{item.splitlater}}
                <em>（后）</em>
              </span>
            </div>
            <div class="bottom clearfix">
              <span class="batch fl">
                {{item.splitcount}}
                <em>（批次）</em>
              </span>
              <span class="rate fr">
                {{item.splitrate}}
                <em>（倍率）</em>
              </span>
            </div>
          </div>
        </li>
      </ul>
    </div>
  </div>
</template>
<script type="text/javascript">
import TopBar from "components/TopBar";
import { http } from 'util/request'
import { GetSplitRecords } from 'util/netApi'
export default {
  data() {
    return {
      topBarOption: {
        iconLeft: 'back'
      },
      list:[]
    };
  },
  components: {
    TopBar
  },
  mounted() {},
  computed: {},
  methods: {
    ToGetSplitRecords() {
      http(GetSplitRecords,{}, json => {
        if (json.code === 0) {
          this.list = json.response
          this.count = json.response.length
        }
      })
    }
  },
  created() {
      this.ToGetSplitRecords()
    
  }
};
</script>

<style lang="less" scoped>
.splitWrapper {
  .innerWrap {
    width: 100vw;
    border-radius: 40px 40px 0 0;
    margin-top: -10px;
    // padding-top: 30px;
    background-color: #ebeaf0;
    ul {
      height: calc(100vh - 300px);
      width: 90%;
      overflow: auto;
      margin: 0 auto;
      padding: 30px 0 90px;
    }
    ul::-webkit-scrollbar {
      background-color: rgba(0, 0, 0, 0);
    }
    li {
      margin-bottom: 80px;
      color: #353535;
      font-size: 54px;
      .time {
        font-size: 36px;
        line-height: 80px;
        margin-left: 10px;
      }
      .whiteWrap {
        border-radius: 34px;
        background-color: #fff;
        font-weight: 700;
        em {
          font-weight: 700;
          font-size: 32px;
        }
      }
      .top {
        border-bottom: 4px solid #ebeaf0;
        line-height: 120px;

        .arrow {
          color: #6418c3;
          width: 30%;
          text-align: center;
          font-size: 100px;
          //  transform: scaleX(2);
        }
        .after {
          color: #6418c3;
        }
        .em {
          color: #353535;
        }
      }
      .bottom {
        line-height: 120px;
      }
      .before,
      .batch {
        padding-left: 40px;
        width: 33%;
      }
      .after,
      .rate {
        padding-right: 40px;
        width: 33%;
        text-align: right;
      }
    }
  }
}
</style>
