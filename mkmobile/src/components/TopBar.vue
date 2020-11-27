<template>
  <div>
  <div class="top-bar">
    <i v-if="option.iconLeft!=='back'" class="iconfont1 icon-l" :class="option.iconLeft" @click="clickPopup"></i>
    <i v-if="option.iconLeft=='back'" class="iconfont1 icon-l fanhui1" @click="back"></i>
    <slot class='top-title'></slot>
      <img class="img-left" src="@/assets/imgs/header/left.png" alt />
    <img class="img-right" src="@/assets/imgs/header/right.png" alt />
    <i v-if="option.image" class="iconfont icon-r ">
      <img class="img-header img-photo" :src="option.image" alt />
    </i>
  
    <i v-else class="iconfont img-r" :class="option.iconRight" @click="clickR">
      <img v-if="!option.iconRight" class="img-fangxing" src="@/assets/imgs/fangxing.png" alt />
      <!-- 未读图标  -->
      <div v-if="hasNoRead" class="weidu"></div>
    </i>
    <div class='clear'></div>
    </div>
    <van-popup v-model="menuShow" :position="'left'" :style="{ height: '100vh', width: '60%'}">
      <div class="menu-tree">
        <P class='menu-title'>快速指引</P>
        <van-collapse v-model="activeNames">
          <van-collapse-item title="仓库" name="1">
            <ul>
              <li>
                <router-link to="Config" class="router">
                  <i class="iconfont iconshezhi"></i>資產
                </router-link>
              </li>
              <!-- <li>
                <router-link to="Config"
                             class="router">
                  <i class="iconfont iconxiaoshou"></i>出售
                </router-link>
              </li>-->
              <li>
                <!--   <router-link to="epList" class="router"> -->
                <router-link to="Exchangedata" class="router">
                  <i class="iconfont iconfeiyongtaizhang"></i>臺賬
                </router-link>
              </li>
              <!-- <li v-if="service">
                <router-link to="Charge" class="router">
                  <i class="iconfont iconjiayou"></i>充值
                </router-link>
              </li> -->
            </ul>
          </van-collapse-item>
          <van-collapse-item title="信息" name="2">
            <ul>
              <li>
                <router-link to="News" class="router">
                  <i class="iconfont iconxinxi"></i>公告
                </router-link>
              </li>
              <li>
                <router-link to="FX" class="router">
                  <i class="iconfont iconfenxi"></i>分析
                </router-link>
              </li>
            </ul>
          </van-collapse-item>
          <div class="menu-item-self">
            <router-link to="shop" class="router">商店</router-link>
          </div>
          <van-collapse-item title="社交" name="4">
            <ul>
              <li>
                <router-link to="friends" class="router">
                  <i class="iconfont iconpengyou"></i>朋友
                </router-link>
              </li>
              <li>
                <router-link to="activityList" class="router">
                  <i class="iconfont iconhuodong"></i>活動
                </router-link>
              </li>
              <li>
                <router-link to="task" class="router">
                  <i class="iconfont iconrenwu"></i>任務
                </router-link>
              </li>
            </ul>
          </van-collapse-item>
          <div class="menu-item-self">
            <router-link to="feedback" class="router">意見反饋</router-link>
          </div>
          <div class="menu-item-self" @click="goSetting">設置</div>
          <div class="menu-item-self"> <router-link to="login" class="router">退出  </router-link></div>
        </van-collapse>
      </div>
    </van-popup>
    <img src="@/assets/imgs/s-person.png" alt v-if="showChat" class="loc-p" />
    <!-- <i v-if="showLocation"
       class="iconfont iconaui-icon-location loc-p"></i>
    <i v-if="showChat"
    class="iconfont iconchat chat"></i>-->
</div>
</template>

<script>
import TopBar from "components/TopBar";

import { storage } from "@/util/storage";
export default {
  components: {
    TopBar
  },
  props: {
    option: {
      type: Object,
      default: function() {
        return {};
      }
    },
    showLocation: {
      type: Boolean,
      default: false
    },
    showChat: {
      type: Boolean,
      default: false
    },
    hasNoRead: {
      type: Boolean,
      default: false
    }
  },
  data() {
    return {
      menuShow: false,
      service: false,
      activeNames: ["1"]
    };
  },
  methods: {
    clickPopup() {
      this.menuShow = true;
    },
    clickR(){
       this.$emit('clickR',this)
    },
    back(){
      this.$router.go(-1)
    },
    goSetting() {
      this.$router.push({ name: "setting" });
    }
  },
  mounted() {
    this.service = storage.getLocalStorage("service");
  }
};
</script>
<style lang="less" scoped>
@keyframes myfirst {
  from {
    transform: rotate(0deg);
  }
  to {
    transform: rotate(360deg);
  }
}
.img-left{
  position:absolute;
  right: 0px;
  top:0px;
  height: 70%;
  width: auto;
  z-index: -1;
}
.img-right{
  position:absolute;
  left: 0px;
  top:0px;
  height: 80%;
  width: auto;
  z-index: -1;
}
.menu-title{
  font-size: 50px;
  font-weight: bold;
  color:#fff;
}
.top-bar {
  padding: 60px;
  background: #6e21d1;
  display: block;
  color: #fff!important;
  opacity: 0.9;
  min-height: 240px;
  text-align: center;
  position: relative;
  border-radius: 0 0 60px 60px;

  .iconfont,.iconfont1 {
    font-size: 90px;
    position: absolute;
    z-index: 999;
  }
  .icon-r {
    right: 60px;
    color: #fff;
    img {
      width: 156px;
      height: 156px;
    }
  }
  .img-r {
    right: 60px;
    color: #fff;
    top: 58px;
    font-size: 80px;
    display: block;
    img {
      width: 56px;
      height: 56px;
      animation: myfirst 4s 2s infinite;
    }
    .weidu {
      display: block;
      position: absolute;
      right: 8px;
      top: 30px;
      width: 30px;
      height: 30px;
      background: red;
      border-radius: 50%;
      z-index: 999;
    }
  }

  @keyframes myfirst {
    from {
      transform: rotate(0deg);
    }
    to {
      transform: rotate(360deg);
    }
  }
  .icon-l {
    left: 60px;
  }
  .loc-p {
    font-size: 80px;
    color: #62bcdb;
    position: absolute;
    top: 80px;
    right: 30px;
    width: 120px;
    height: 110px;
  }
  .chat {
    font-size: 50px;
    color: #62bcdb;
    position: absolute;
    top: 20px;
    right: 30px;
  }
}
.img-fangxing{
  animation: myfirst 3s 2s infinite;
}
.img-yueliang1{
  animation: myfirst 4s 3s infinite;
  z-index: 2;
}
</style>
<style lang="less" scoped>
// 1
.center-one-search {
  width: 100vw;
  line-height: 150px;
  padding: 0px;
  font-size: 69px;
  font-weight: bold;
  justify-content: center;
  background: #6e21d1;
  display: block;
  color: #fff;
  opacity: 0.9;
  min-height: 240px;
  text-align: center;
  position: relative;
  z-index: 999;
  border-radius: 0 0 60px 60px;
}
</style>
<style lang="less" scoped>
// 2
.center-two-search {
  text-align: center;
  width: calc(100vw - 340px);
  margin: 0 auto;
  padding-top: 100px;
  font-size: 69px;
  height: 500px;
  font-weight: bold;
  position: relative;
  .two-tit-t {
    font-weight: normal;
    letter-spacing: 6px;
  }
  .two-tit-b {
    font-size: 88px;
    letter-spacing: 6px;
  }
  .img-yueliang {
    position: absolute;
    left: 60px;
    width: 128px;
    top: 30px;
    height: 158px;
  }
  .img-fangxing {
    position: absolute;
    left: 150px;
    width: 58px;
    top: 257px;
    height: 58px;
  }
 
}
 .center-two-search .img-fangxing{
     left: 650px;
      top: 127px;
  }
</style>
<style lang="less" scoped>
// 3
.center-three-search {
  text-align: center;
  width: calc(100vw - 340px);
  margin: 0 auto;
  padding-top: 100px;
  font-size: 69px;
  height: 580px;
  font-weight: bold;
  position: relative;
  .three-tit-t {
    font-weight: normal;
    letter-spacing: 6px;
    margin-bottom: 40px;
  }
  .three-tit-b {
    font-size: 88px;
    letter-spacing: 6px;
  }
}
</style>

<style lang="less" scoped>
// 4
.center-four-search {
  width: calc(100vw - 240px);
  margin: 0 auto;
  padding-top: 100px;
  font-size: 60px;
  height: 580px;
  position: relative;
  .four-tit-t {
    font-weight: normal;
    letter-spacing: 6px;
    padding-top: 140px;
    padding-left: 30px;
    color: #ccc;
    margin-left: -80px;
  }
  .four-tit-b {
    font-size: 66px;
    letter-spacing: 6px;
    padding-left: 30px;
    margin-left: -80px;
  }
  .img-yueliang {
    position: absolute;
    left: 140px;
    width: 128px;
    top: 10px;
    height: 158px;
  }
  .img-yueliang1 {
    position: absolute;
    right: 230px;
    width: 128px;
    top: 180px;
    height: 158px;
  }
  .img-fangxing {
    position: absolute;
    left: 360px;
    width: 58px;
    top: 140px;
    height: 58px;
  }
  .header-back{
     position: absolute;
    right: -73px;
     top: 320px;
    width: 186px;
    height: 186px;
    background-color: #fff;
    border-radius: 100px;
    z-index: 998;  
  }
  .img-header {
    position: absolute;
    right: -60px;
    width: 164px;
    height: 164px;
    top: 330px;
    background-color: #fff;
    border-radius: 80px;
    z-index: 999;  
  }
}
</style>
<style lang="less" scoped>
/deep/ .van-overlay {
  background-color: transparent !important;
}
/deep/ .van-collapse-item__title--expanded::after {
  display: none;
}
.menu-tree {
  height: 100vh;
  width: 60vw;
  background: rgba(27, 25, 30, 1);
  font-weight: normal;
  overflow: scroll;
  z-index: 99999;
  p {
    width: 60vw;
    text-align: left;
    font-weight: normal;
    padding-left: 50px;
    border-bottom: 0;
    padding-top: 150px;
  }
  /deep/ .van-collapse {
    width: 60vw;
    padding: 30px 50px 50px;
    padding-bottom: 350px;
     z-index: 9999;
    .van-collapse-item--border::after {
      border: 0;
    }
    .van-collapse-item {
      .van-cell {
        background: rgba(27, 25, 30, 1);
        color: #fff;
        padding: 50px 0;
        text-align: left;
        color: #d9c704;
        border-bottom: 4px solid #fff;
        .van-cell__right-icon {
          font-size: 60px;
          color: #d9c704;
        }
        .van-cell__title {
          font-size: 50px;
        }
      }

      .van-collapse-item__content {
        background: rgba(27, 25, 30, 1);
        border: 0;
        border-width: 0 0;
        padding: 0;
      }
      ul {
        font-size: 50px;
        color: #fff;
        text-align: left;
        padding-left: 80px;
        li {
          padding: 20px;
          position: relative;
          i {
            position: absolute;
            left: -70px;
            font-size: 55px;
          }
          .router {
            color: #fff!important;
          }
        }
      }
    }
    .no-arrow {
      .van-cell {
        .van-cell__right-icon {
          display: none;
        }
      }
    }
  }
  /deep/ .van-hairline--top-bottom::after,
  .van-hairline-unset--top-bottom::after {
    border-width: 0 0;
  }
  .menu-item-self {
    font-size: 50px;
    color: #d9c704;
    position: relative;
    border-bottom: 4px solid #fff;
    padding: 20px 0 20px 0;
    height: 140px;
    line-height: 100px;
    text-align: left;
    .router {
      color: #d9c704!important;
    }
    &::after {
      content: "";
      display: block;
      position: absolute;
      right: 30px;
    }
  }
}
</style>
