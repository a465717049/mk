<template>
  <div>
    <ScrollRefresh
      @getData="getInfo()"
      :residualHeight="60"
      :isNeedUp="false"
      class="relativeScroll"
    >
      <TopBar class="center-one-search" :option="topBarOption">查詢</TopBar>
      <div class="search-body">
        <img class="search-bd-img" src="@/assets/imgs/three.png" alt />
        <TopSearch class="search-input" @onSearch="onSearch"></TopSearch>
        <div class="auth-card">
          <div :style="{ flex: 1 }">
            <div class="auth-top-tip">用 户 I D&nbsp;：{{ bNum }}</div>
            <div class="auth-top-tip">持有股票：{{ sNum }}</div>
          </div>
        </div>
        <div class="arrow base-flex">
          <div class="arrow-show arrow-one center" @click="onButton('back')">
            <i class="iconfont iconarrow-left author font80"></i>
          </div>
          <div class="arrow-r">
            <div class="arrow-show center" @click="onButton('up')">
              <i class="iconfont iconarrowup author font80"></i>
            </div>
            <div class="arrow-show arrow-last center" @click="onButton('down')">
              <i class="iconfont iconarrow author font80"></i>
            </div>
          </div>
        </div>
        <button class="button" @click="onPeoPle">
          <div>
            <p class="num">{{ queueNumder }}</p>
            <p class="pai-title">正在排队</p>
          </div>

          <i class="jiantou iconfont iconarrow-right"></i>
        </button>
      </div>
      <YellowComfirm :show="showComfirm" :tipTitle="tips" @clickOk="clickOk()" @changeModel="changeModel"></YellowComfirm>
    </ScrollRefresh>
  </div>
</template>

<script>
import TopBar from 'components/TopBar'
import TopSearch from 'components/TopSearch'
import ScrollRefresh from 'components/ScrollRefresh'
import { http } from 'util/request'
import { GetBuyDPEList, GetSerarchApple } from 'util/netApi'
import { storage } from 'util/storage'
import { accessToken, loginPro } from 'util/const.js'
import YellowComfirm from 'components/YellowComfirm'
export default {
  name: 'searchOne',
  components: {
    TopBar,
    TopSearch,
    YellowComfirm,
    ScrollRefresh
  },
  data () {
    return {
      topBarOption: {
        iconLeft: 'back',
        iconRight: ''
      },
      bNum: 0,
      sNum: 0,
      queueNumder: 0,
      showComfirm: false,
      tips: '',
      option: null
    }
  },
  methods: {
    onPeoPle () {
      this.$router.push('./Queue')
    },
    clickOk () {
      this.showComfirm = false
    },
    changeModel (v) {
      this.showComfirm = v
    },
    onSearch (value) {
      this.bNum = value
      this.ToGetSerarchApple()
      // this.$router.push('./Queue?id=' + value)
    },
    onButton (option) {
      this.option = option
      if (option === 'back') {
       // this.$router.push('./Analyse?')
      } else if (option == 'down') {
        this.ToGetSerarchApple('down')
      } else if (option == 'up') {
        this.ToGetSerarchApple('up')
      }
    },
    ToGetSerarchApple () {
      http(GetSerarchApple, { uid: this.bNum, option: this.option }, json => {
        if (json.code === 0) {
          // bNum sNum
          if (json.response.data.length > 0) {
            this.bNum = json.response.data[0].uID
            this.sNum = json.response.data[0].amount
          } else {
            this.showComfirm = true
            this.tips = '沒有查詢到此ID'
            this.bNum = 0
            this.sNum = 0
          }
        }
      })
    },
    ToGetBuyDPEList () {
      http(GetBuyDPEList, null, json => {
        if (json.code === 0) {
          this.queueNumder = json.response.length
        }
      })
    },
    getInfo () {
      this.ToGetSerarchApple()
      this.ToGetBuyDPEList()
    }
  },
  created () {
    this.ToGetSerarchApple()
    this.ToGetBuyDPEList()
  }
}
</script>

<style lang="less" scope>
.search-body {
  background: #ebeaf0;
  position: absolute;
  top: 280px;
  width: 100%;
  overflow: scroll;
  z-index: 999;
  border-radius: 50px 50px 0 0;
  padding: 98px 60px 300px 60px;
  height: calc(100vh - 260px);
  .search-bd-img {
    width: 100%;
    height: auto;
    margin-bottom: 80px;
  }
  .search-input {
    margin-bottom: 50px;
  }
  .auth-card {
    background: #fff;
    display: flex;
    padding: 70px 50px 50px 50px;
    margin-bottom: 50px;
    border-radius: 20px;
    img {
      width: 200px;
      height: 200px;
      margin-right: 20px;
      margin-top: 20px;
    }
    .auth-top-tit {
      font-size: 60px;
      line-height: 60px;
      margin-bottom: 25px;
      color: #6a22c5;
      font-weight: bold;
    }
    .auth-top-tip {
      font-size: 60px;
      color: #8d8d8d;
    }
  }
  .arrow {
    // margin-top: 50px;
    .arrow-r {
      display: flex;
    }
    .arrow-show {
      width: 143px;
      height: 140px;
      margin: 0 30px;
      background: url("../../assets/imgs/set/squer-img.png") center center /
        100% 100%;
      .author {
        color: #7ab4b4;
        margin-top: 35px;
        margin-left: 10px;
      }
    }
    .arrow-one,
    .arrow-last {
      margin: 0;
    }
  }
  .button {
    width: 100%;
    background: #7533c9;
    color: #fff;
    border-radius: 40px;
    position: relative;
    margin-top: 120px;
    padding: 40px;
    div {
      text-align: right;
      font-size: 92px;
      width: 400px;
      line-height: 90px;
      .pai-title {
        font-size: 40px;
      }
    }
    .jiantou {
      position: absolute;
      right: 30px;
      top: 100px;
      font-size: 60px;
      color: #ebeaf0;
    }
    .num {
      font-weight: bold;
    }
    p {
      text-align: center;
    }
  }
}
</style>
