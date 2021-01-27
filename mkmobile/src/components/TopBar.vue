<template>
  <div class="topbar-wrap">
    <div class="top-bar" ref="topBar">
      <i
        v-if="option.iconLeft!=='back'"
        class="iconfont icon-l"
        :class="option.iconLeft"
        @click="clickPopup"
      ></i>
      <i v-if="option.iconLeft=='back'" class="iconfont icon-l iconfanhui" @click="back"></i>
      <slot class="top-title"></slot>
      <!-- <img class="img-left" src="@/assets/imgs/header/left.png" alt />
      <img class="img-right" src="@/assets/imgs/header/right.png" alt />-->
      <i v-if="option.image" class="iconfont icon-r">
        <img class="img-header img-photo" :src="option.image" alt />
      </i>
      <i
        v-else-if="option.iconRight==='iconxinxi2'"
        class="iconfont img-r iconxinxi2"
        @click="goFeedBack"
      >
        <div v-if="hasNoRead" class="weidu"></div>
      </i>
      <i v-else class="iconfont img-r" :class="option.iconRight" @click="clickR">
        <!-- <img v-if="!option.iconRight" class="img-fangxing" src="@/assets/imgs/fangxing.png" alt /> -->
        <!-- 未读图标  -->
        <div v-if="badge" class="badge">{{badge}}</div>
      </i>
      <div class="clear"></div>
    </div>
    <van-popup v-model="menuShow" :position="'left'" :style="{ height: '100vh', width: '60%'}">
      <div class="menu-tree">
        <P class="menu-title">快速指引</P>
        <van-collapse v-model="activeNames" :accordion="true">
          <van-collapse-item name="1">
            <template #title>
              <div class="leftTreeTitle">
                <i class="iconfont iconfont2 iconcaiwutouzi"></i>
                <span>财务</span>
              </div>
            </template>
            <ul>
              <li>
                <router-link to="withdrawal" class="router">
                  <i class="iconfont iconarrow-right"></i>提现
                </router-link>
              </li>
               <li>
                <router-link to="withdrawalRecord" class="router">
                  <i class="iconfont iconarrow-right"></i>提现记录
                </router-link>
              </li>
              <li>
                <router-link to="transWithMe" class="router">
                  <i class="iconfont iconarrow-right"></i>转换
                </router-link>
              </li>
              <li>
                <router-link to="transRp" class="router">
                  <i class="iconfont iconarrow-right"></i>转分
                </router-link>
              </li>
              <li>
                <router-link to="VipUpgrade" class="router">
                  <i class="iconfont iconarrow-right"></i>升级
                </router-link>
              </li>
              <li>
                <router-link to="exChangeData" class="router">
                  <i class="iconfont iconarrow-right"></i>台帐
                </router-link>
              </li>
            </ul>
          </van-collapse-item>
          <van-collapse-item name="2">
            <template #title>
              <div class="leftTreeTitle">
                <i class="iconfont iconfont2 icontubiao-"></i>
                <span>社交</span>
              </div>
            </template>
            <ul>
              <li>
                <router-link to="friendsList" class="router">
                  <i class="iconfont iconarrow-right"></i>朋友
                </router-link>
              </li>
              <li>
                <router-link to="friendsListjid" class="router">
                  <i class="iconfont iconarrow-right"></i>安置
                </router-link>
              </li>
              <li>
                <router-link to="active" class="router">
                  <i class="iconfont iconarrow-right"></i>活动
                </router-link>
              </li>
              <li>
                <router-link to="task" class="router">
                  <i class="iconfont iconarrow-right"></i>任务
                </router-link>
              </li>
            </ul>
          </van-collapse-item>
          <van-collapse-item name="3">
            <template #title>
              <div class="leftTreeTitle">
                <i class="iconfont iconfont2 iconshangdian"></i>
                <span>商店</span>
              </div>
            </template>
            <ul>
              <li>
                <router-link to="shop" class="router">
                  <i class="iconfont iconarrow-right"></i>新品
                </router-link>
              </li>
              <li>
                <router-link to="shopCar" class="router">
                  <i class="iconfont iconarrow-right"></i>购物车
                </router-link>
              </li>
               <li>
                <router-link to="reshop" class="router">
                  <i class="iconfont iconarrow-right"></i>再次购买
                </router-link>
              </li>
              <li>
                <router-link to="shoplist" class="router">
                  <i class="iconfont iconarrow-right"></i>我的订单
                </router-link>
              </li>
              <li>
                <router-link to="orderDetail" class="router">
                  <i class="iconfont iconarrow-right"></i>订单详情
                </router-link>
              </li>
               <li>
                <router-link to="openShop" class="router">
                  <i class="iconfont iconarrow-right"></i>申请开店
                </router-link>
              </li>
            </ul>
          </van-collapse-item>
          <van-collapse-item name="4">
            <template #title>
              <div class="leftTreeTitle">
                <i class="iconfont iconfont2 iconxinxi1"></i>
                <span>信息</span>
              </div>
            </template>
            <ul>
              <li>
                <router-link to="news" class="router">
                  <i class="iconfont iconarrow-right"></i>公告
                </router-link>
              </li>
            </ul>
          </van-collapse-item>
          <div class="menu-item-self">
            <router-link to="feedback" class="router">
              <i class="iconfont iconyijianfankui"></i>
              <span>意见</span>
            </router-link>
          </div>
          <div class="menu-item-self">
            <router-link to="setting" class="router">
              <i class="iconfont iconshezhi1"></i>
              <span>设置</span>
            </router-link>
          </div>
          <div class="menu-item-self">
            <router-link to="login" class="router">
              <i class="iconfont icontuichu"></i>
              <span>退出</span>
            </router-link>
          </div>
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
import TopBar from 'components/TopBar'
import Store from '@/store'
import { mapState } from 'vuex'
import { http } from 'util/request'
import { GetReadUserFeedBack } from 'util/netApi'
import { storage } from '@/util/storage'
export default {
  components: {
    TopBar
  },
  props: {
    option: {
      type: Object,
      default: function () {
        return {
          iconLeft: 'iconmenu2',
          iconRight: 'iconxinxi2'
        }
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
    badge: {
      // 角标
      type: Number,
      default: 0
    }
  },
  data () {
    return {
      menuShow: false,
      service: false,
      activeNames: '1'
    }
  },
  computed: mapState({
    hasNoRead: state => state.hasNoRead
  }),
  methods: {
    clickPopup () {
      this.menuShow = true
    },
    clickR () {
      this.$emit('clickR', this)
    },
    back () {
      this.$router.go(-1)
    },
    goSetting () {
      this.$router.push({ name: 'setting' })
    },
    goFeedBack () {
      this.$router.push('Feedback')
    }
  },
  mounted () {
    this.service = storage.getLocalStorage('service')
    Store.commit('changeTopbarHeight', this.$refs.topBar.offsetHeight)

     http(GetReadUserFeedBack, null, json => {
        if (json.code === 0) {
          if(json.response>0)
          {
            Store.commit('changeRead',true)
          }else
          {
            Store.commit('changeRead',false)
          }
        }
      })
    //Store.commit('changeRead',false)
  }
}
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
.img-left {
  position: absolute;
  right: 0px;
  top: -50px;
  height: 70%;
  width: auto;
  z-index: -1;
}
.img-right {
  position: absolute;
  left: 0px;
  top: -50px;
  height: 80%;
  width: auto;
  z-index: -1;
}

.menu-title {
  font-size: 60px;
  font-weight: bold;
  color: #efb618;
}
.top-bar {
  padding: 60px;
  display: block;
  color: #fff !important;
  min-height: 240px;
  text-align: center;
  position: relative;
  // border-radius: 0 0 60px 60px;
  // overflow: hidden;

  .iconfont,
  .iconfont1 {
    font-size: 90px;
    position: absolute;
    z-index: 999;
    font-weight: normal;
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
      right: 0px;
      top: 0px;
      width: 30px;
      height: 30px;
      background: red;
      border-radius: 50%;
      z-index: 999;
    }
    .badge {
      display: block;
      position: absolute;
      right: -18px;
      top: 20px;
      width: 46px;
      height: 46px;
      line-height: 46px;
      text-align: center;
      background: red;
      border-radius: 50%;
      z-index: 999;
      font-size: 32px;
      font-weight: bold;
      color: #ffffff;
    }
  }
  .iconxinxi2{
    top: 88px;
    right: 50px;
    font-size: 100px;
    height: 100px;
    line-height: 100px;
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
  .iconmenu2 {
    // width: 100px;
    font-size: 90px;
    height: 90px;
    line-height: 90px;
    top: 100px;
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
.img-fangxing {
  animation: myfirst 3s 2s infinite;
}
.img-yueliang1 {
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
  // background: #6e21d1;
  display: block;
  color: #fff;
  min-height: 240px;
  text-align: center;
  position: relative;
  z-index: 999;
  border-radius: 0 0 60px 60px;
  // overflow: hidden;
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
  // overflow: hidden;
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
.center-two-search .img-fangxing {
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
  // overflow: hidden;
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
  height: 360px;
  position: relative;
  .four-tit-t {
    font-weight: normal;
    letter-spacing: 6px;
    padding-top: 40px;
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
  .header-back {
    position: absolute;
    right: -73px;
    top: 180px;
    width: 146px;
    height: 146px;
    background-color: #fff;
    border-radius: 100px;
    border: 8px solid #fff;
    z-index: 998;
    // box-sizing: border-box;
  }
  // .img-header {
  //   position: absolute;
  //   right: -65px;
  //   width: 140px;
  //   height: 140px;
  //   top: 267px;
  //   background-color: #fff;
  //   border-radius:140px;
  //   z-index: 999;
  // }
}
</style>
<style lang="less" scoped>
.topbar-wrap{
  position: relative;
  /deep/ .van-overlay {
    background-color: transparent !important;
  }
  /deep/ .van-collapse-item__title--expanded::after {
    display: none;
  }
  /deep/ .van-popup--left {
    opacity: 0.89;
    z-index: 9999998 !important;
  }
}
.menu-tree {
  height: 100vh;
  width: 60vw;
  background: #000;
  font-weight: normal;
  overflow: scroll;
  z-index: 9999999 !important;

  p {
    width: 60vw;
    text-align: left;
    font-weight: normal;
    padding-left: 50px;
    border-bottom: 0;
    padding-top: 210px;
  }
  /deep/ .van-collapse {
    width: 60vw;
    padding: 30px 20px 50px;
    padding-bottom: 350px;
    z-index: 9999999 !important;
    .van-collapse-item--border::after {
      border: 0;
    }
    .van-collapse-item {
      .van-cell {
        background: #000;
        color: #fff;
        padding: 30px 20px;
        text-align: left;
        color: #fff;
        border-bottom: 6px solid #fff;

        .van-cell__right-icon {
          // font-size: 60px;
          // color: #d9c704;
          display: none;
        }
        .van-cell__title {
          font-size: 50px;
        }
      }

      .van-collapse-item__content {
        background: #000;
        border: 0;
        border-width: 0 0;
        padding: 0;
      }
      ul {
        font-size: 50px;
        color: #fff;
        text-align: left;
        padding-left: 80px;
        padding-top: 30px;
        li {
          font-size: 48px;
          padding: 10px 0 10px 80px;

          .router {
            display: inline-block;
            position: relative;
            width: 100%;
            color: #0ef2bf !important;
          }
          i {
            position: absolute;
            left: -56px;
            font-size: 40px;
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
    font-size: 52px;
    color: #fff;
    position: relative;
    padding: 10px 0;
    height: 120px;
    line-height: 100px;
    text-align: left;
    display: flex;
    align-items: center;
    .router {
      color: #fff !important;
      display: flex;
      align-items: center;
    }
    .iconfont {
      font-size: 60px;
      margin-right: 36px;
      font-weight: normal;
    }
  }
  .iconfont2 {
    font-size: 60px;
    margin-right: 36px;
  }
  .icontubiao- {
    font-size: 80px;
    margin-right: 20px;
    margin-left: -8px;
  }
  .icontuichu {
    font-size: 50px !important;
    margin-left: 5px;
  }
  .iconshangdian{
    font-size: 80px;
    margin-left: -8px;
     margin-right: 20px;
  }
}
.leftTreeTitle {
  color: #fff;
  font-size: 52px;
  align-items: center;
  display: flex;
  height: 60px;
}
</style>
