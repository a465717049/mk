<template>
  <div>
    <TopBar class="center-one-search" :option="topBarOption">臺賬</TopBar>
    <div class="config-body">
     
      <div class="group">
        <div class="group-item" @click="routerChagne('epList','ep')">
          <img src="@/assets/imgs/EPimg.png" alt />
             <span>EP记录</span>
        </div>
        <div class="group-item" @click="routerChagne('epList','rp')">
          <img src="@/assets/imgs/RPimg.png" alt />
           <span>RP记录</span>
        </div>
        <div class="group-item" @click="routerChagne('epList','sp')">
          <img src="@/assets/imgs/SPimg.png" alt />
           <span>SP记录</span>
        </div>
      </div>
      <div class="group">
       
        <div class="group-item" @click="routerChagne('epList','dpe')">
          <img src="@/assets/imgs/DPimg.png" alt />
          <span>DP股票</span>
        </div> <div class="group-item none" >
         
          <span></span>
        </div>
        <div class="group-item none">
         
          <span></span>
        </div>
      </div>
   
    </div>
  </div>
</template>

<script>
import TopBar from "components/TopBar";
import headerImg from "@/assets/imgs/headerImg.png";

export default {
  name: "Config",
  components: {
    TopBar
  },
  data() {
    return {
      topBarOption: {
        iconLeft: "iconlist2f",
        iconRight: ""
      }
    };
  },
  methods: {
    routerChagne(routeName, cktype) {
      this.$router.push({ name: routeName, params: { cktype: cktype } });
    }
  }
};
</script>

<style lang="less" scope>
.config-body {
  background: #ebeaf0;
  margin-top: -20px;
  width: 100%;
  overflow: scroll;
  z-index: 0;
  border-radius: 50px 50px 0 0;
  padding: 98px 60px 60px 60px;
  height: calc(100vh - 260px);
  .cf-bd-item {
    display: flex;
    background: #6e21d1;
    height: 180px;
    line-height: 180px;
    border-radius: 40px;
    margin-bottom: 32px;
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
    margin-bottom: 80px;
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
      box-shadow: 3px 3px 3px #666;
      &:last-child {
        margin: 0;
      }
      img {
        width: 170px;
        height: 170px;
        &.yellow-img {
          width: 280px;
          height: 190px;
        }
      }
      span{
          color:#666;
        margin-top:20px;
        font-weight: 700;
      }
    }
     .none{
    background: none;
      box-shadow:0px 0px 0px #666;
  }
  }
  .group-one {
    //  margin-bottom: 250px;
    .group-item {
      img {
        width: 170px;
        height: 170px;
      }
      span {
        color: #41a3e0;
        font-size: 40px;
        font-weight: bold;
      }

    }
  .none{
    background: none;
  }
  }
  .auth-card {
    background: #fff;
    display: flex;
    padding: 40px 50px;
    margin-bottom: 370px;
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
</style>
