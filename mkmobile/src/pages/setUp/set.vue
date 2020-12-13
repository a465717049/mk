<template>
  <div>
    <TopBar class="center-one-search" >设置</TopBar>
    <ScrollRefresh @getData="TogetUserInfo" :residualHeight="160" :isNeedUp="false" class="innerScroll">
      <div class="set clearfix p-58">
        <div class="relative">
          <img :src="photo?photo:defaultImg" class="img" alt />
          <van-cell class="cell-info borderR mb-100" v-bind:title="username" is-link to="./SetUp" />
        </div>
        <div class="relative">
          <img src="@/assets/imgs/set/B-1.png" class="img" alt />
          <van-cell class="cell-info borderR mb-40 cell-special" :title="userlv" />
          <van-progress class="process base-purple" :percentage="lv/5*100>100?100:lv/5*100" />
        </div>
        <div class="relative">
          <img src="@/assets/imgs/set/B-1.png" class="img" alt />
          <van-cell class="cell-info borderR mb-40" :title="'注册时间:  '+registerTime" />
        </div>
        <div class="relative">
          <img src="@/assets/imgs/set/B-5.png" class="img" alt />
          <van-cell class="cell-info borderR mb-40" to="VipUpgrade">
            <template #title>
              <span class="custom-title">级别：{{level}}</span>
              <van-tag round class="tag">升级</van-tag>
            </template>
          </van-cell>
        </div>
        <div class="relative">
          <img src="@/assets/imgs/set/B-2.png" class="img" alt />
          <van-cell class="cell-info borderR mb-40" title="认证" is-link @click="GoAuthenticator" />
        </div>
        <div class="relative">
          <img src="@/assets/imgs/set/b-3.png" class="img" alt />
          <van-cell class="cell-info borderR mb-40" title="密码" is-link to="password" />
        </div>
        <!-- <div class="relative">
          <img src="@/assets/imgs/set/b-3.png" class="img" alt />
          <van-cell class="cell-info borderR mb-40" title="安全问题" is-link to="./SetAnswer" />
        </div> -->
        <div class="relative">
          <img src="@/assets/imgs/set/b-4.png" class="img" alt />
          <van-cell class="cell-info borderR mb-40" title="银行" is-link to="bank" />
        </div>
        <div class="relative">
          <img src="@/assets/imgs/set/b-6.png" class="img" alt />
          <van-cell class="cell-info borderR mb-40" title="个人资料" is-link to="additional" />
        </div>
        <div class="relative">
          <img src="@/assets/imgs/set/B-5.png" class="img" alt />
          <van-cell class="cell-info borderR mb-40" title="语言" is-link to="./lanSet" />
        </div>
      </div>
    </ScrollRefresh>
  </div>
</template>
<script>
import TopBar from "components/TopBar";
import { http } from "util/request";
import { GetUserInfo } from "util/netApi";
import { storage } from "util/storage";
import { accessToken, photoList } from "util/const.js";
import ScrollRefresh from "components/ScrollRefresh";
import defaultImg from "@/assets/imgs/set/head02.png";
export default {
  name: "Set",
  components: {
    TopBar,
    ScrollRefresh
  },
  data() {
    return {
      defaultImg,
      photo: "",
      userlv: "总监",
      lv: 0,
      username: "Totay cyels",
      isBindGoogle: false,
      level: "666会员",
      registerTime: "2020-09-10 15:04"
    };
  },
  methods: {
    TogetUserInfo() {
      http(GetUserInfo, null, json => {
        if (json.code === 0) {
          this.username = json.response.nickname;
          this.isBindGoogle = json.response.isBindGoogle;
          if (json.response.lv_name == 1) this.level = "666会员";
          if (json.response.lv_name == 2) this.level = "2000会员";
          if (json.response.lv_name == 3) this.level = "10000会员";

          if (json.response.lv_name == 0) this.userlv = "玩家";
          if (json.response.lv_name == 1) this.userlv = "達標社區";
          if (json.response.lv_name == 2) this.userlv = "初級社區";
          if (json.response.lv_name == 3) this.userlv = "中級社區";
          if (json.response.lv_name == 4) this.userlv = "高級社區";
          if (json.response.lv_name == 5) this.userlv = "超級社區";
          if (json.response.lv_name == 6) this.userlv = "橘子派";
          if (json.response.lv_name == 7) this.userlv = "香瓜派";
          if (json.response.lv_name == 8) this.userlv = "菠蘿派";
          if (json.response.lv_name == 9) this.userlv = "柚子派";
          if (json.response.lv_name == 10) this.userlv = "蘋果派";
          this.lv = json.response.lv_name;
          this.photo = photoList[json.response.photo];
        }
      });
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
.innerScroll{
  /deep/ .wrapper  .bscroll-container{
    min-height: calc(100vh - 420px);
  }
}
.set {
  // margin-top: -160px;
  // border-radius: 40px 40px 0 0;
  .cell-info {
    height: 130px;
    line-height: 120px;
  }
  .img {
    position: absolute;
    left: 20px;
    top: 13px;
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
  .tag {
    background-color: #6418c3;
    font-size: 42px;
    height: 66px;
    line-height: 66px;
    padding: 0 50px;
    position: absolute;
    right: 10px;
    top: 50%;
    transform: translateY(-50%);
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
