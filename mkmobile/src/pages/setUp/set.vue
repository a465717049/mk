<template>
  <div class="set-wrap">
    <TopBar class="center-one-search" >设置</TopBar>
    <ScrollRefresh @getData="TogetUserInfo" :residualHeight="topbarHeight+bottomTabBarHeight+10" :isNeedUp="false" class="innerScroll">
      <div class="set clearfix p-58">
        <div class="relative">
          <img :src="photo?photo:defaultImg" class="img" alt />
          <van-cell class="cell-info borderR mb-40" v-bind:title="username" is-link to="./SetUp" />
        </div>
        <div class="relative">
          <img src="@/assets/imgs/set/B-1.png" class="img" alt />
          <van-cell class="cell-info borderR cell-special" :title="userlv" />
          <van-progress class="process base-purple" :percentage="Lvname/5*100>100?100:Lvname/5*100" />
        </div>
        <div class="relative">
          <img src="@/assets/imgs/set/B-1.png" class="img" alt />
          <van-cell class="cell-info borderR" :title="'注册时间:  '+registerTime" />
        </div>
        <div class="relative">
          <img src="@/assets/imgs/set/B-5.png" class="img" alt />
          <van-cell class="cell-info borderR" to="VipUpgrade">
            <template #title>
              <span class="custom-title">级别：{{level}}</span>
              <van-tag round class="tag" v-if="lv!=3">升级</van-tag>
            </template>
          </van-cell>
        </div>
        <div class="relative">
          <img src="@/assets/imgs/set/B-2.png" class="img" alt />
          <van-cell class="cell-info borderR" title="认证" is-link @click="GoAuthenticator" />
        </div>
        <div class="relative">
          <img src="@/assets/imgs/set/b-3.png" class="img" alt />
          <van-cell class="cell-info borderR" title="密码" is-link to="password" />
        </div>
        <div class="relative">
          <img src="@/assets/imgs/set/b-3.png" class="img" alt />
          <van-cell class="cell-info borderR" title="安全问题" is-link to="./SetAnswer" />
        </div>
        <div class="relative">
          <img src="@/assets/imgs/set/b-4.png" class="img" alt />
          <van-cell class="cell-info borderR" title="银行" is-link to="bank" />
        </div>
        <div class="relative">
          <img src="@/assets/imgs/set/b-6.png" class="img" alt />
          <van-cell class="cell-info borderR" title="个人资料" is-link  @click="Goadditional" />
        </div>
        <div class="relative">
          <img src="@/assets/imgs/set/B-5.png" class="img" alt />
          <van-cell class="cell-info borderR" title="语言" is-link to="./lanSet" />
        </div>
        <div class="relative">
          <img src="@/assets/imgs/set/B-7.png" class="img" alt />
          <van-cell class="cell-info borderR" title="关于摩奇猴" is-link to="about" />
        </div>
      </div>
    </ScrollRefresh>
    <YellowComfirm :show="showComfirm" :tipTitle="tips" @clickOk="clickOk()" @changeModel="changeModel"></YellowComfirm>

  </div>
</template>
<script>
import YellowComfirm from 'components/YellowComfirm'
import TopBar from 'components/TopBar'
import { http } from 'util/request'
import { GetUserInfo } from 'util/netApi'
import { storage } from 'util/storage'
import { accessToken, photoList } from 'util/const.js'
import ScrollRefresh from 'components/ScrollRefresh'
import defaultImg from '@/assets/imgs/set/head02.png'
export default {
  name: 'Set',
  components: {
    TopBar,
    YellowComfirm,
    ScrollRefresh
  },
  data () {
    return {
      tips: '',
      isEnter: false,
      defaultImg,
      photo: '',
      userlv: '',
      lv: 0,
      username: '',
      isBindGoogle: false,
      isSetIDNumber: false,
      level: '',
      registerTime: ''
    }
  },
  methods: {
    TogetUserInfo () {
      http(GetUserInfo, null, json => {
        if (json.code === 0) {
          this.username = json.response.nickname
          this.isBindGoogle = json.response.isBindGoogle
          this.isSetIDNumber = json.response.isSetIDNumber
          if (json.response.farmers == 1) this.level = '初级会员'
          if (json.response.farmers == 2) this.level = '中级会员'
          if (json.response.farmers == 3) this.level = '高级会员'

          if (json.response.lv_name == 0) this.userlv = '会员'
          if (json.response.lv_name == 1) this.userlv = '经理'
          if (json.response.lv_name == 2) this.userlv = '总监'
          if (json.response.lv_name == 3) this.userlv = '总裁'
          if (json.response.lv_name == 4) this.userlv = '董事'
          if (json.response.lv_name == 5) this.userlv = '合伙人'
          this.Lvname = json.response.lv_name
          this.lv = json.response.farmers
          this.photo = photoList[json.response.photo]
          this.username = json.response.nickname
          this.isBindGoogle = json.response.isBindGoogle
        }
      })
    },
    GoAuthenticator () {
      if (this.isBindGoogle) {
        this.$router.push({
          name: 'AuthenticatorThree'
        })
      } else {
        this.$router.push({
          name: 'AuthenticatorOne'
        })
      }
    },
    changeModel (v) {
      this.isEnter = v
    },
    Goadditional () {
      if (!this.isSetIDNumber) {
        this.$router.push({
          name: 'additional'
        })
      } else {
        this.showComfirm = true
        this.tips = '资料已经完善，无法修改'
      }
    }
  },
  created () {
    this.TogetUserInfo()
  },
  mounted () {}
}
</script>
<style lang="less" scoped>
.set-wrap .innerScroll {
  /deep/ .wrapper .bscroll-container {
    min-height: calc(100vh - 420px);
  }
}
.set {
  // margin-top: -160px;
  padding-top: 0;
  // border-radius: 40px 40px 0 0;
  .cell-info {
    height: 130px;
    line-height: 120px;
    margin-bottom: 30px;
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
