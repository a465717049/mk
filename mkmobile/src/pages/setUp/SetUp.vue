<template>
  <div>
    <TopBar class="center-one-search">头像设置</TopBar>
    <ScrollRefresh @getData="TogetUserInfo" :residualHeight="160" :isNeedUp="false">
      <div class="setUp-body">
        <h5>设置昵称</h5>
        <div class="t-img">
          <img :src="headurl" alt />
          <input type="text" v-model="username" />
        </div>
        <h5>选择一个头像</h5>
        <ul class="setUp-content">
          <li
            v-for="(item,index) in addList"
            :class="flag==index?'choosed':'choosed1'"
            @click="choose_head(index,item.headimg,item.img)"
            :key="index"
          >
            <img :src="item.img" alt />
            <i class="iconfont icondui "></i>
          </li>
        </ul>
        <button class="button" @click="goNext">提交</button>
      </div>
    </ScrollRefresh>
  </div>
</template>

<script>
import TopBar from "components/TopBar";
import { http } from "util/request";
import { GetUpdateHeadImageAndNickName, GetUserInfo } from "util/netApi";
import ScrollRefresh from "components/ScrollRefresh";
import { storage } from "util/storage";
import { accessToken, photoList } from "util/const.js";
export default {
  name: "AuthenticatorOne",
  components: {
    TopBar,
    ScrollRefresh
  },
  data() {
    return {
      username: "",
      flag: -1,
      address: "",
      headimg: "",
      headurl: "",
      addList: [
        { img: photoList.head01, headimg: "head01" },
        { img: photoList.head02, headimg: "head02" },
        { img: photoList.head03, headimg: "head03" },
        { img: photoList.head04, headimg: "head04" },
        { img: photoList.head05, headimg: "head05" },
        { img: photoList.head06, headimg: "head06" },
        { img: photoList.head07, headimg: "head07" },
        { img: photoList.head08, headimg: "head08" },
        { img: photoList.head09, headimg: "head09" },
        { img: photoList.head010, headimg: "head010" },
        { img: photoList.head011, headimg: "head011" },
        { img: photoList.head012, headimg: "head012" },
        { img: photoList.head013, headimg: "head013" },
        { img: photoList.head014, headimg: "head014" },
        { img: photoList.head015, headimg: "head015" },
        { img: photoList.head016, headimg: "head016" },
        { img: photoList.head017, headimg: "head017" },
        { img: photoList.head018, headimg: "head018" },
        { img: photoList.head019, headimg: "head019" },
        { img: photoList.head020, headimg: "head020" },
        { img: photoList.head021, headimg: "head021" },
        { img: photoList.head022, headimg: "head022" },
        { img: photoList.head023, headimg: "head023" },
        { img: photoList.head024, headimg: "head024" },
        { img: photoList.head025, headimg: "head025" },
        { img: photoList.head026, headimg: "head026" },
        { img: photoList.head027, headimg: "head027" },
        { img: photoList.head028, headimg: "head028" },
        { img: photoList.head029, headimg: "head029" },
        { img: photoList.head030, headimg: "head030" },
        { img: photoList.head031, headimg: "head031" },
        { img: photoList.head032, headimg: "head032" },
        { img: photoList.head033, headimg: "head033" },
        { img: photoList.head034, headimg: "head034" },
        { img: photoList.head035, headimg: "head035" },
        { img: photoList.head036, headimg: "head036" },
        { img: photoList.head037, headimg: "head037" },
        { img: photoList.head038, headimg: "head038" },
        { img: photoList.head039, headimg: "head039" },
        { img: photoList.head040, headimg: "head040" },
        { img: photoList.head041, headimg: "head041" },
        { img: photoList.head042, headimg: "head042" },
        { img: photoList.head043, headimg: "head043" },
        { img: photoList.head044, headimg: "head044" },
        { img: photoList.head045, headimg: "head045" },
        { img: photoList.head046, headimg: "head046" },
        { img: photoList.head047, headimg: "head047" },
        { img: photoList.head048, headimg: "head048" },
        { img: photoList.head049, headimg: "head049" },
        { img: photoList.head050, headimg: "head050" },
        { img: photoList.head051, headimg: "head051" },
        { img: photoList.head052, headimg: "head052" },
        { img: photoList.head053, headimg: "head053" },
        { img: photoList.head054, headimg: "head054" },
        { img: photoList.head055, headimg: "head055" },
        { img: photoList.head056, headimg: "head056" },
        { img: photoList.head057, headimg: "head057" },
        { img: photoList.head058, headimg: "head058" },
        { img: photoList.head059, headimg: "head059" },
        { img: photoList.head060, headimg: "head060" }
      ]
    };
  },
  methods: {
    onSubmit(values) {
      console.log("submit", values);
    },
    goNext() {
      http(
        GetUpdateHeadImageAndNickName,
        { nickname: this.username, uHeadImgUrl: this.headimg },
        json => {
          if (json.code === 0) {
            this.$router.push({ name: "setting" });
          }
        }
      );
    },
    choose_head(index, headimg, url) {
      this.flag = index;
      this.headimg = headimg;
      this.headurl = url;
    },
    TogetUserInfo() {
      http(GetUserInfo, null, json => {
        if (json.code === 0) {
          this.username = json.response.nickname;
          this.headurl =photoList[json.response.photo];
        }
      });
    }
  },
  created() {
    this.TogetUserInfo();
  }
};
</script>

<style lang="less" scope>
.setUp-body {
  top: 260px;
  width: 100%;
  z-index: 999;
  border-radius: 50px 50px 0 0;
  padding: 4vw 4vw  100px 4vw;
  h5 {
    font-size: 44px;
    margin: 42px 0 42px 20px;
    margin-bottom: 20px;
    color: #fff;
    font-weight: bold;
  }
  .t-img {
    position: relative;
    img {
      width: 80px;
      height: 80px;
      position: absolute;
      left: 30px;
      top: 25px;
      z-index: 999;
    }
    input {
      height: 130px;
      font-size: 42px;
      width: 100%;
      padding: 30px 20px;
      border-radius: 20px;
      padding-left: 140px;
    }
  }
  .setUp-content {
    padding: 6vw;
    overflow: hidden;
    padding-right: 20px;
    background-color: #fff;
    border-radius: 20px;
    li {
      width: 20vw;
      height: 20vw;
      float: left;
      border-radius: 43px;
      padding: 50px;
      position: relative;
      img {
        width: 100%;
        height: 100%;
        overflow: hidden;
      }
      .iconfont{
        position: absolute;
        font-size: 60px;
        color: #4493D5;
        top: 11vw;
        left: 11vw;
        opacity: 0;
      }
    }
    .choosed {
      background-color: rgba(239,182,24,0.22);
      .iconfont{
        opacity: 1;
      }
    }
  }
  .button {
    display: block;
    width: 100%;
    background: #edca08;
    border-radius: 20px;
    height: 130px;
    line-height: 130px;
    font-size: 42px;
    letter-spacing: 10px;
    color: #fff;
    margin-top: 100px;
  }
}
</style>
