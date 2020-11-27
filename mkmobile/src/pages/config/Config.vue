<template>
  <div>
    <TopBar class="center-one-search" :option="topBarOption">資產</TopBar>
    <div class="config-body">
        
      <div class="cf-bd-item">
        <img src="@/assets/imgs/config/gg_1.png" alt />
        <span>自由調配自己的資產</span>
      </div>
      <div class="group">
        <div class="group-item" @click="routerChagne('TransEP')">
          <img src="@/assets/imgs/EPimg.png" alt />
           <span>EP轉出</span>
        </div>
        <div class="group-item" @click="routerChagne('TransRP')">
          <img src="@/assets/imgs/RPimg.png" alt />
           <span>RP轉出</span>
        </div>
        <div class="group-item" @click="routerChagne('TransWithMe')">
          <img src="@/assets/imgs/TRimg.png" alt />
           <span>資產轉換</span>
        </div>
      </div>
      <div class="cf-bd-item">
        <img src="@/assets/imgs/config/gg_2.png" alt />
        <span>畅通便捷的处理方式</span>
      </div>
      <div class="group group-one">
        <div class="group-item" @click="routerChagne('SellStock')">
          <img src="@/assets/imgs/config/ss_1.png" alt />
          <span>出售股票</span>
        </div>
        <div class="group-item" @click="routerChagne('SellEp')">
          <img src="@/assets/imgs/config/ss_2.png" alt />
          <span>出售EP</span>
        </div>
        <div class="group-item" @click="routerChagne('epTrade')">
          <img src="@/assets/imgs/config/WS.png" alt />
          <span>EP交易</span>
        </div>
      </div>
      <div class="group group-one">
        <div class="group-item" @click="routerChagne('stockList')">
          <img src="@/assets/imgs/config/c4.png" alt />
          <span>股票交易</span>
        </div>
        <div class="group-item" @click="routerChagne('Split')">
          <img src="@/assets/imgs/config/c5.png" alt />
          <span>拆配記錄</span>
        </div>
         <div class="group-item none">
        
          <span></span>
        </div>
      </div>
    
    </div>
  </div>
</template>

<script>
import TopBar from 'components/TopBar'
import headerImg from '@/assets/imgs/headerImg.png'

export default {
  name: 'Config',
  components: {
    TopBar
  },
  data() {
    return {
      topBarOption: {
       // image: headerImg,
        iconLeft: 'iconzhankai',
        iconRight: ''
      }
    }
  },
  methods: {
    routerChagne(routeName) {
      this.$router.push({ name: routeName }) 
      
    }
  }
}
</script>

<style lang="less" scope>
.config-body {
  background: #ebeaf0;
  margin-top: -50px;
  width: 100%;
  overflow: scroll;
  z-index: 2;
  border-radius: 50px 50px 0 0;
  padding: 98px 60px 60px 60px;
  height: calc(100vh - 260px);
  .cf-bd-item {
    display: flex;
    background: #6e21d1;
    height: 180px;
    line-height: 180px;
    border-radius: 40px;
    margin-bottom: 22px;
    img {
      width: 104px;
      height: 104px;
      margin: 30px 60px 0 60px;
    }
    span {
      font-size: 44px;
      color: #fff;
    }
  }
  
  .group {
    display: flex;
      margin-top: 50px;
    margin-bottom: 60px;
    .group-item {
      flex: 1;
      height: 304px;
      margin-right: 40px;
      background: #fff;
      border-radius: 40px;
      display: flex;
      justify-content: center;
      align-items: center;
      flex-direction: column;
      &:last-child {
        margin: 0;
      }
      span{
        color:#666;
        margin-top:20px;
        font-weight: 700;
      }
      img {
        width: 170px;
        height: 170px;
        &.yellow-img {
          width: 280px;
          height: 190px;
        }
      }
    }
  }
  .group-one {
      margin-top: 50px;
    .group-item {
      img {
        width: 170px;
        height: 170px;
      }
        span{
        color:#666;
        margin-top:20px;
        font-weight: 700;
      }
    }
  }
  .auth-card {
    background: #fff;
    display: flex;
    padding: 40px 50px;
    margin-bottom: 50px;
    border-radius: 20px;
    img {
      width: 148px;
      height: 98px;
      margin-right: 60px;
      margin-top: 20px;
    }
    .auth-top-tit {
      font-size: 55px;
      line-height: 60px;
      margin-bottom: 18px;
    }
    .auth-top-tip {
      font-size: 30px;
    }
  }
}
.none{
    background: none!important;
  }
</style>
