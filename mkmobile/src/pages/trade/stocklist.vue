<template>
  <div class="relative">
    <TopBar class="center-one-search" :option="topBarOption">
      股票交易
      <!-- <TopSearch @onSearch="onSearch"></TopSearch> -->
    </TopBar>

    <div class="bg-gray p-58">
      <div class="queue relative">
        <div class="queue-name"></div>
        <ul>
          <li class="title base-info">
            <span class="number">ID</span>
            <span class="id-info">数量</span>
            <span class="invest-info">金额</span>
            <span class="date-info">时间</span>
          </li>
        </ul>
        <ScrollRefresh @getData="ToGetDPEEexchange" :residualHeight="250">
          <ul>
            <li class="content base-info" v-for="(item,index) in dataList" :key="index">
              <span class="number">{{ item.uid }}</span>
              <span class="id-info">{{ item.amount.toFixed(0) }}</span>
              <span class="invest-info">{{ item.lasttotal.toFixed(2) }}</span>
              <span class="date-info">{{ item.date }}</span>
            </li>
          </ul>
        </ScrollRefresh>
        <!-- <div class="listWrap" ref="listWrap">
          <ul>
            <li class="content base-info" v-for="(item,index) in dataList" :key="index">
              <span class="number">{{ item.uid }}</span>
              <span class="id-info">{{ item.amount.toFixed(0) }}</span>
              <span class="invest-info">{{ item.lasttotal.toFixed(2) }}</span>
              <span class="date-info">{{ item.date }}</span>
            </li>
          </ul>
        </div> -->
      </div>
    </div>
  </div>
</template>
<script>
import TopBar from "components/TopBar";
import TopSearch from "components/TopSearch";
import ScrollRefresh from "components/ScrollRefresh";
import BScroll from "better-scroll";
import { http } from "util/request";
import { GetDPEEexchange } from "util/netApi";
import { storage } from "util/storage";
import { accessToken, loginPro } from "util/const.js";

export default {
  components: {
    TopBar,
    TopSearch,
    ScrollRefresh,
    BScroll
  },
  data() {
    return {
      searchvalue: 0,
      showLocation: true,
      topBarOption: {
        iconLeft: "back",
        iconRight: ""
      },
      count: 0,
      dataList: []
    };
  },
  mounted() {
    // this.scrollInit();
  },
  computed: {},
  methods: {
    onSearch(value) {
      this.ToGetDPEEexchange(value);
    },
    scrollInit() {
      if (!this.scroll) {
        this.scroll = new BScroll(this.$refs.listWrap, {
          scrollY: true,
          click: true,
          bounce: {
            top: true,
            bottom: true
          }
        });
      } else {
        this.scroll.refresh();
      }
    },
    ToGetDPEEexchange(value) {
      http(GetDPEEexchange, { uid: value }, json => {
        if (json.code === 0) {
          this.dataList = json.response;
          this.count = json.response.length;
        }
      });
    }
  },
  created() {
    if (this.$route.query.id != "undefined") {
      this.ToGetDPEEexchange(this.$route.query.id);
    } else {
      this.ToGetDPEEexchange(0);
    }
  }
};
</script>
<style lang="less" scoped>
.search-pos {
  height: 129px;
  position: absolute;
  width: 100%;
  top: 260px;
  left: 50%;
  padding: 0 58px;
  transform: translate(-50%);
}

.queue {
  margin-top: -20px;
  border-radius: 40px;
  padding-left: 32px;
  padding-right: 32px;

  padding-bottom: 60px;
  background: #fff;
  border: 2px solid #e5e5e5;
  .queue-name {
    margin-top: 34px;
    margin-bottom: 34px;
    font-size: 60px;
    color: #6e21d1;
  }
  .listWrap {
    height: calc(100vh - 760px);
    overflow: hidden;
  }
  .base-info {
    display: flex;
    flex-direction: row;
    justify-content: flex-start;
    height: 130px;
    line-height: 100px;
    padding: 21px 29px 21px 28px;
    box-shadow: 0px 4px 2px 0px rgba(0, 0, 0, 0.15);
    border-radius: 21px;
    margin-bottom: 21px;
    .number {
      width: 20%;
      text-align: center;
    }
    .id-info {
      width: 20%;
      text-align: center;
    }
    .invest-info {
      width: 20%;
      text-align: center;
    }
    .date-info {
      width: 40%;
      text-align: center;
    }
  }
  .title {
    margin-top: 21px;
    background: #e5e5e5;
    font-size: 38px;
  }
  .content {
    background: #ececec;
    font-size: 38px;
    color: rgba(41, 9, 82, 1);
    opacity: 0.65;
  }
}
</style>
