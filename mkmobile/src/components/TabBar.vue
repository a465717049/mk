<template>
  <div class="xptabbar">
    <span
      v-for="item in tabItemData"
      @click="link(item)"
      :key="item.id"
      :class="[$route.path === item.to ?'active':'','tabItem']"
    >
      <i :class="[item.className,'tabIcon']"></i>
      <span :class="{active: $route.path === item.to}">{{item.text}}</span>
    </span>
    <van-action-sheet v-model="sheetShow" class="expressWrap">
      <ul class="box">
        <li @click="goPage('/friendsList')">
          <i class="iconfont iconzhucehuiyuan img1"></i>
          <p>新增</p>
        </li>
        <li @click="goPage('/transEp')">
          <i class="iconfont icontubiaozhizuomoban img2"></i>
          <p>提现</p>
        </li>
        <li @click="goPage('/feedback')">
          <i class="iconfont iconyouxiang img3"></i>
          <p>反馈</p>
        </li>
        <li @click="goPage('/')">
          <i class="iconfont icontuichu img4"></i>
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
          className: "iconcl-icon-Home iconfont",
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
          to: "/friendsList",
          className: "icontuandui iconfont",
          text: "朋友",
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
    logout() {
      this.sheetShow = false;
    }
  }
};
</script>
<style lang="less" scoped>
.xptabbar {
  position: fixed;
  bottom: 0;
  z-index: 9999997;
  background-color: #113d79;
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
    .tabIcon::before {
      color: #efb618;
    }
    span {
      color: #efb618;
    }
  }
  .tabIcon {
    width: 82px;
    height: 82px;
    display: block;
    margin: 0 auto;
    line-height: 82px;
  }
  .tabIcon::before {
    font-size: 90px;
    color: #fff;
  }
  span {
    display: block;
    text-align: center;
    font-size: 40px;
    color: #fff;
    height: 56px;
    line-height: 56px;
  }
  &.active {
    color: #ba825a;
  }
  .tabItem:nth-child(2) {
    .tabIcon {
      width: 216px;
      height: 216px;
      line-height: 190px;
      margin: -100px 0 0 0;
      border-radius: 50%;
      background-color: #efb618;
      box-shadow: 0px -4px 2px #4678bc;
      border: 12px solid #4678bc;
    }
    .tabIcon::before {
      font-size: 90px;
      color: #fff;
    }
  }
  .van-popup--bottom.van-popup--round {
    background: rgba(1, 11, 11, 0.8);
    border-radius: 40px 40px 0 0;
    z-index: 9999999;
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
      padding-top: 60px;
      display: flex;
      flex-direction: column;
      p {
        letter-spacing: 6px;
        height: 100px;
        line-height: 100px;
        margin-bottom: 20px;
      }
      .iconfont {
        height: 120px;
        font-weight: normal;
      }
      .img1::before {
        font-size: 100px;
      }
      .img2::before {
        font-size: 100px;
      }
      .img3::before {
        font-size: 74px;
      }
      .img4::before {
        font-size: 90px;
      }
    }
  }
}
</style>
