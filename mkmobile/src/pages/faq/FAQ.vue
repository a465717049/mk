<template>
  <div class="faqWrap">
    <TopBar>
      <div class="center-one-search">FAQ</div>
    </TopBar>
    <div class="innerWrap">
      <div class="searchWrap">
        <TopSearch @onSearch="onSearch"></TopSearch>
      </div>
      <div class="outWrap">
        <ScrollRefresh
          @getData="getData()"
          :residualHeight="360"
          :isNeedUp="false"
          class="listWrap"
        >
          <ul>
            <li v-for="item in list" :key="item.id">
              <span class="dot"></span>
              <span class="text">{{ item.msg }}</span>
            </li>
          </ul>
        </ScrollRefresh>
      </div>
    </div>
  </div>
</template>

<script>
import TopBar from "components/TopBar";
import TopSearch from "components/TopSearch";
import ScrollRefresh from "components/ScrollRefresh";
export default {
  components: {
    TopBar,
    TopSearch,
    ScrollRefresh
  },
  data() {
    return {
      list: [
        {
          id: 1,
          msg: "USDT怎么购买？"
        },
        {
          id: 2,
          msg: "USDT怎么购买？"
        },
        {
          id: 3,
          msg: "USDT怎么购买？"
        },
        {
          id: 4,
          msg: "USDT怎么购买？"
        }
      ]
    };
  },
  mounted() {},
  methods: {
    onSearch(value) {
      console.log(value);
    },
    getData() {
      console.log(789);
    }
  }
};
</script>

<style lang="less" scoped>
.listWrap {
  /deep/.wrapper .bscroll-container {
    min-height: calc(100vh - 1020px);
  }
}
.faqWrap {
  .innerWrap {
    width: 100vw;
    background-color: #ebeaf0;
    border-radius: 40px 40px 0 0;
    margin-top: -20px;
  }
  .searchWrap {
    padding: 60px;
  }
  .outWrap {
    width: 90%;
    margin: 0 auto;

    box-sizing: border-box;
    padding: 40px 15px;
  }
  .listWrap {
    padding: 80px 15px 40px;
    border-radius: 20px;
    background-color: #fff;
    li {
      height: 134px;
      line-height: 134px;
      margin-bottom: 20px;
      background-color: #ececec;
      border-radius: 20px;
      padding: 0 30px;
      .dot {
        display: inline-block;
        width: 30px;
        height: 30px;
        background: rgba(151, 141, 171, 1);
        border-radius: 50%;
        margin-right: 60px;
      }
      .text {
        color: rgba(20, 13, 13, 1);
      }
    }
  }
}
</style>

<style lang="less">
.faqWrap .searchWrap input {
  background-color: #fff !important;
}
</style>
