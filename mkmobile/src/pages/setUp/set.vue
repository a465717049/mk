<template>
  <ScrollRefresh @getData="TogetUserInfo" :residualHeight="10" :isNeedUp="false">
  <div>
    <TopBar class="center-one-search" :option="topBarOption">設置</TopBar>

    <div class="set borderR bg-gray clearfix p-58">
      <div class="relative">
        <img :src="photo" class="img" alt />
        <van-cell class="cell-info borderR mb-100" v-bind:title="username" is-link to="./SetUp" />
      </div>
      <div class="relative">
        <img src="@/assets/imgs/set/B-1.png" class="img" alt />
        <van-cell class="cell-info borderR mb-40 cell-special" :title="userlv" />
        <van-progress class="process base-purple" :percentage="lv/5*100>100?100:lv/5*100" />
      </div>
       <div class="relative">
        <img src="@/assets/imgs/set/B-1.png" class="img" alt />
        <van-cell class="cell-info borderR mb-40" v-bind:title="this.createtime" />
      </div>
       <div class="relative">
        <img src="@/assets/imgs/set/B-1.png" class="img" alt />
        <van-cell class="cell-info borderR mb-40" v-bind:title="this.farmers"  />
      </div>
      <div class="relative">
        <img src="@/assets/imgs/set/B-2.png" class="img" alt />
        <van-cell class="cell-info borderR mb-40" title="认证" is-link @click="GoAuthenticator" />
      </div>
      <div class="relative">
        <img src="@/assets/imgs/set/b-3.png" class="img" alt />
        <van-cell class="cell-info borderR mb-40" title="密码" is-link to="./password" />
      </div>
      <div class="relative">
        <img src="@/assets/imgs/set/b-3.png" class="img" alt />
        <van-cell class="cell-info borderR mb-40" title="安全问题" is-link to="./SetAnswer" />
      </div>
      <div class="relative">
        <img src="@/assets/imgs/set/b-4.png" class="img" alt />
        <van-cell class="cell-info borderR mb-40" title="钱包" is-link to="./Wallet" />
      </div>
      <div class="relative">
        <img src="@/assets/imgs/set/B-5.png" class="img" alt />
        <van-cell class="cell-info borderR mb-40" title="语言" is-link to="./lanSet" />
      </div>
    </div>
  </div>
  </ScrollRefresh>
</template>
<script>
import TopBar from "components/TopBar";
import { http } from "util/request";
import { GetUserInfo } from "util/netApi";
import { storage } from "util/storage";
import { accessToken, photoList } from "util/const.js";
import ScrollRefresh from "components/ScrollRefresh";
export default {
  name: "Set",
  components: {
    TopBar,
    ScrollRefresh
  },
  data() {
    return {
      photo: "",
      userlv: "",
      lv: 0,
      createtime: 0,
      farmers:"",
      username: "",
      topBarOption: {
        iconLeft: "iconzhankai",
        iconRight: ""
      },
      isBindGoogle: false
    };
  },
  methods: {
    TogetUserInfo() {
      http(GetUserInfo, null, json => {
        if (json.code === 0) {
          this.username = json.response.nickname;
          this.isBindGoogle = json.response.isBindGoogle;
          if(json.response.lv_name==0)  this.userlv='普通玩家'
          if(json.response.lv_name==1)  this.userlv='達標社區'
          if(json.response.lv_name==2)  this.userlv='初級社區'
          if(json.response.lv_name==3)  this.userlv='中級社區'
          if(json.response.lv_name==4)  this.userlv='高級社區'
          if(json.response.lv_name==5)  this.userlv='超級社區'
          if(json.response.lv_name==6)  this.userlv='橘子派'
          if(json.response.lv_name==7)  this.userlv='香瓜派'
          if(json.response.lv_name==8)  this.userlv='菠蘿派'
          if(json.response.lv_name==9)  this.userlv='柚子派'
          if(json.response.lv_name==10)  this.userlv='蘋果派'
          this.lv = json.response.lv_name ;
          this.createtime ="注冊時間："+ this.formatDate(json.response.create_time) ;
          this.farmers ="農場級別："+(json.response.farmers==1?"高級農場":"中級農場");
          this.photo =photoList[json.response.photo];
        }
      })
    },
    formatDate(date) {
        var date = new Date(date*1000);
        var YY = date.getFullYear() + '-';
        var MM = (date.getMonth() + 1 < 10 ? '0' + (date.getMonth() + 1) : date.getMonth() + 1) + '-';
        var DD = (date.getDate() < 10 ? '0' + (date.getDate()) : date.getDate());
        var hh = (date.getHours() < 10 ? '0' + date.getHours() : date.getHours()) + ':';
        var mm = (date.getMinutes() < 10 ? '0' + date.getMinutes() : date.getMinutes()) ;
        return YY + MM + DD +" "+hh + mm;
     },
    GoAuthenticator() {
      console.log(this.isBindGoogle);
      if (this.isBindGoogle) {
        this.$router.push({
          name: "AuthenticatorThree"
        });
      } else {
        this.$router.push({
          name: "AuthenticatorOne"
        });
      }
    }
  },
  created() {
    this.TogetUserInfo();
  },
  mounted() {}
};
</script>
<style lang="less" scoped>
.set {
  // margin-top: -160px;
  border-radius: 40px 40px 0 0;
  .cell-info {
    height: 137px;
    line-height: 120px;
  }
  .img {
    position: absolute;
    left: 20px;
    top: 10px;
    width: 106px;
    height: 106px;
    z-index: 99;
  }
  /deep/.van-cell__right-icon {
    font-size: 60px;
    height: 137px;
    line-height: 120px;
  }
  /deep/.van-cell__title {
    font-size: 42px;
    margin-left: 140px;
    font-weight: bold;
    color: rgba(118, 124, 143, 1);
    position: relative;

  }
  .process {
    width: 60%;
    height: 54px;
    position: absolute;
    left: 160px;
    top: 40px;
    border-radius: 30px;
  }
  /deep/.van-progress {
    background: rgba(100, 24, 195, 1);
    border: 1px solid rgba(65, 3, 5, 1);
    box-shadow: 0px 5px 5px 0px rgba(0, 0, 0, 0.46);
  }
  /deep/.van-progress__portion {
    background: linear-gradient(
      0deg,
      rgba(79, 172, 254, 1),
      rgba(0, 242, 254, 1)
    );
    border: 1px solid rgba(65, 3, 5, 1);
    border-radius: 23px;
    height: 54px;
    line-height: 54px;
  }
  /deep/.van-progress__pivot {
    display: none;
  }
  .cell-special {
    /deep/.van-cell__title {
      margin-left: 85%;
    }
  }
}
</style>
