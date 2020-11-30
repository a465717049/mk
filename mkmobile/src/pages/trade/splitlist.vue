<template>
  <div>
    <TopBar class="center-one-search" :option="topBarOption" >拆分紀錄</TopBar>
    
    <div class="bg-gray p-58">
      <div class="queue relative">
        <div class="queue-name">Split</div>
        <div class="listWrap" ref="listWrap">
          <ul>
            <li class="title base-info">
              <span class="number">uid</span>
              <span class="date-info">Count</span>
              <span class="date-info">Rate</span>
              <span class="date-info">Before</span>
              <span class="date-info">Later</span>
              <span class="date-info">DATE</span>
            </li>
            <li class="content base-info" v-for="(item) in dataList" :key="item.uid">
              <span class="number">{{ item.uid }}</span>
              <span class="date-info">{{ item.splitcount }}</span>
              <span class="date-info">{{ item.splitrate }}</span>
              <span class="date-info">{{ item.splitbefore }}</span>
              <span class="date-info">{{ item.splitlater }}</span>
              <span class="date-info">{{ item.date }}</span>
            </li>
          </ul>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import TopBar from 'components/TopBar'
import TopSearch from 'components/TopSearch'
import BScroll from 'better-scroll'
import { http } from 'util/request'
import { GetSplitRecords } from 'util/netApi'
import { storage } from 'util/storage'
import { accessToken, loginPro } from 'util/const.js'
export default {
  components: {
    TopBar,
    TopSearch,
    BScroll
  },
  data() {
    return {
      searchvalue: 0,
      showLocation: true,
      topBarOption: {
        iconLeft: 'iconShapecopy',
        iconRight: ''
      },
      count: 0,
      dataList: []
    }
  },
  mounted() {
    this.scrollInit()
  },
  computed: {},
  methods: {
    onSearch(value) {
      this.ToGetSplitRecords(value)
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
        })
      } else {
        this.scroll.refresh()
      }
    },
    ToGetSplitRecords(value) {
      http(GetSplitRecords, { uid: value }, json => {
        if (json.code === 0) {
          this.dataList = json.response
          this.count = json.response.length
        }
      })
    }
  },
  created() {
    if (this.$route.query.id != 'undefined') {
      this.ToGetSplitRecords(this.$route.query.id)
    } else {
      this.ToGetSplitRecords(0)
    }
  }
}
</script>
<style lang="less" scoped>
.search-pos {
  height: 129px;
  position: absolute;
  width: 100%;
  top: 330px;
  left: 50%;
  padding: 0 58px;
  transform: translate(-50%);
}
.count {
  text-align: center;
  color: #fff;
  font-size: 100px;
  position: absolute;
  width: 100%;
  top: 180px;
  left: 50%;
  font-weight: 600;
  padding: 0 58px;
  transform: translate(-50%);
}
.center-one-search {
  height: 600px;
}
.queue {
  margin-top: -20px;
  border-radius: 40px;
  padding-left: 52px;
  padding-right: 59px;

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
    height: calc(100vh - 980px);
    overflow: scroll;
  }
  .base-info {
    display: flex;
    flex-direction: row;
    justify-content: flex-start;
    height: 130px;
    line-height: 100px;
    padding: 21px 59px 21px 58px;
    box-shadow: 0px 4px 2px 0px rgba(0, 0, 0, 0.15);
    border-radius: 21px;
    margin-bottom: 21px;
    .number {
      width: 17%;
    }
    .id-info {
      width: 23%;
    }
    .invest-info {
      width: 22%;
    }
    .date-info {
      width: 40%;
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
