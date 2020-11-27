<template>
  <div class="xptabbar">
    <span
      v-for="item in tabItemData"
      @click="link(item)"
      :key="item.id"
      :class="[$route.path === item.to ?'active':'','tabItem']"
    >
      <i :class="item.className"></i>
      <span :class="{active: $route.path === item.to}">{{item.text}}</span>
    </span>
    <van-action-sheet v-model="sheetShow" class="expressWrap">
      <div class="title">Express</div>
      <ul class="box">
        <li @click="goPage('/friends')">
          <span>
            <img src="../assets/imgs/bottom_icon1.png" alt class="img1" />
          </span>
          <p>朋友</p>
        </li>
        <li @click="goPage('/feedback')">
          <span>
            <img src="../assets/imgs/bottom_icon2.png" alt class="img2" />
          </span>
          <p>反饋</p>
        </li>
        <li @click="goPage('/config')">
          <span>
            <img src="../assets/imgs/bottom_icon3.png" alt class="img3" />
          </span>
          <p>交易</p>
        </li>
        <li @click="goPage('/')">
          <span>
            <img src="../assets/imgs/bottom_icon4.png" alt class="img4" />
          </span>
          <p>退出</p>
        </li>
      </ul>
    </van-action-sheet>
  </div>
</template>

<script>
export default {
  name: "Tabbar",
  data() {
    return {
      tabItemData: [
        {
          id: "tabbar1",
          to: "/home",
          className: "home1 iconfont1",
          text: "首頁",
          selected: true
        },
        {
          id: "tabbar2",
          to: "/",
          className: "icon21 iconfont",
          text: "",
          selected: false
        },
        {
          id: "tabbar3",
          to: "/Analyse",
          className: "fenxi1 iconfont1",
          text: "報表",
          selected: false
        }
      ],
      sheetShow: false
    };
  },
  methods: {
    link(item) {
      if (item.id !== "tabbar2") {
        this.$router.push(item.to);
      } else {
        this.sheetShow = true;
      }
    },
    goPage(href) {
      this.sheetShow = false;
      this.$router.push(href);
    },
    logout(){
      this.sheetShow = false;
      
    }
  }
};
</script>
<style lang="less" scoped>
.xptabbar {
  position: fixed;
  bottom: 0;
  z-index: 9997;
  background-color: #fff;
  height: 170px;
  line-height: 170px;
  display: flex;
  width: 100%;
  justify-content: space-around;
  padding-top: 16px;
  border-radius: 20px 20px 0 0;

  .tabItem {
    display: inline-block;
    width: 160px;
    text-align: center;
  }
  .tabItem.active {
    i::before {
      color: #6318c3;
    }
    span {
      color: #6318c3;
    }
  }
  i {
    width: 82px;
    height: 82px;
    display: block;
    margin: 0 auto;
    line-height: 82px;
  }
  i::before {
    font-size: 90px;
    color: #94999e;
  }
  span {
    display: block;
    text-align: center;
    font-size: 40px;
    color: #94999e;
    height: 56px;
    line-height: 56px;
  }
  &.active {
    color: #ba825a;
  }
  .tabItem:nth-child(2) {
    i {
      width: 200px;
      height: 200px;
      line-height: 190px;
      margin: -100px 0 0 0;
      border-radius: 50%;
      background-color: #6318c3;
      box-shadow: 0px -4px 2px  #dddddd;
      border: 8px solid #ebeaf0;
    }
    i::before {
      font-size: 90px;
      color: #fff;
    }
  }
  .van-popup--bottom.van-popup--round {
    background: rgba(100, 24, 195, 0.9);
    border-radius: 40px 40px 0 0;
    z-index: 9999999;
    .title {
      height: 220px;
      line-height: 220px;
      font-size: 76px;
      color: #fff;
      text-align: center;
    }
  }
  .iconzhexiantu:before {
    font-size: 80px;
  }
  .box {
    display: flex;
    justify-content: space-around;
    li {
      width: 180px;
      color: #fff;
      font-size: 40px;
      font-weight: 600;
      text-align: center;
      p {
        letter-spacing: 20px;
        padding-left: 20px;
      }
      span {
        width: 180px;
        height: 148px;
        display: inline-block;
        // background-color: rgba(255, 255, 255, 0.5);
        border-radius: 20px;
      }
      .img1 {
        width: 124px;
        height: 99px;
        margin-top: 20px;
      }
      .img2 {
        width: 124px;
        height: 99px;
        margin-top: 20px;
      }
      .img3 {
        width: 114px;
        height: 99px;
        margin-top: 20px;
      }
      .img4 {
        width: 114px;
        height: 99px;
        margin-top: 20px;
      }
    }
  }
}
</style>
