<template>
  <div class="homeWrapper">
    <TopBar >
      <div class="center-four-search">
        <div class="four-tit-t">Hello,{{uid}}</div>
        <div class="four-tit-b">{{username}}</div>
        <!-- <img class="img-yueliang" src="@/assets/imgs/yueliang.png" alt />
        <img class="img-yueliang1" src="@/assets/imgs/yueliang1.png" alt />
        <img class="img-fangxing" src="@/assets/imgs/fangxing.png" alt /> -->
        <img  @click="goSetting" class="header-back" :src="headerimg?headerimg:defaultImg" alt />
      </div>
    </TopBar>
    <ScrollRefresh
      @getData="getData"
      :residualHeight="bottomTabBarHeight+topbarHeight+10"
      :isNeedUp="false"
      class="innerScroll">

    <div class="mainWrap">
      <div class="innerWrap">
        <van-swipe :autoplay="4000" class="sweiper1" :show-indicators="false" >
          <van-swipe-item v-for="(image, index) in images" :key="index">
            <img :src="image.image" />
          </van-swipe-item>
        </van-swipe>
        <div class="swiper2Box">
          <van-swipe  class="sweiper2" @change="onSwiperChange" :stop-propagation="false" >
            <van-swipe-item v-for="(item, index) in balanceList" :key="index" >
              <!-- @click='goinfo(item.type)' -->
              <!-- <img src="../../assets/imgs/yellowBg.png" alt /> -->
               <div class="countBox"  >
                <p>{{ balanceTile }}</p>
                <countTo
                  ref="count"
                  :startVal="0"
                  :endVal="endVal"
                  :duration="1000"
                  separator=","
                  prefix=""
                  :useEasing="true"
                  class="countNum"
                  v-show="showMount"
                ></countTo>
                <div class="countNum"  v-show="!showMount">{{ new Array((endVal+'').length+1).join("*")}}</div>
                 <i class="iconfont iconyanjing1 eye" @click="showMount=!showMount" v-show="showMount"></i>
           <i class="iconfont iconyincang eye" @click="showMount=!showMount" v-show="!showMount"></i>
              </div>
            </van-swipe-item>
          </van-swipe>

          <!-- <countTo
              ref="count"
              :startVal="0"
              :endVal="endVal"
              :duration="3000"
              separator=","
              prefix="$"
              class="countNum"
          ></countTo>-->
        </div>
        <div class="home-list">
          <div class="recent">
            <div class="title-re">最近交易</div>
            <div class="view"  ><router-link to="epList" class="router">查看所有</router-link></div>
          </div>
          <van-tabs v-model="active" @click="readloadinfo">
            <van-tab title="所有">
              <div class="detail" v-for="(item,index) in listOne" :key="'one'+index">
                <div class="info">
                  <img alt src='@/assets/imgs/EPimg.png' />
                  <!--:src="item.img"-->
                  <div class="num">
                    <div class="font50">{{ item.remark }}</div>
                    <div class="sub-num">{{ item.date }}</div>
                  </div>
                </div>
                <div class="price" :class='item.amount>0?"":"red"'> {{ item.amount }}</div>
              </div>
            </van-tab>
            <van-tab title="收入">
                <div class="detail" v-for="(item,index) in listTwo" :key="'two'+index">
                <div class="info">
                  <img alt src='@/assets/imgs/EPimg.png' />
                  <!--:src="item.img"-->
                  <div class="num">
                    <div class="font50">{{ item.remark }}</div>
                    <div class="sub-num">{{ item.date }}</div>
                  </div>
                </div>
                <div class="price" :class='item.amount>0?"":"red"'> {{ item.amount }}</div>
              </div>
            </van-tab>
            <van-tab title="支出">
                <div class="detail" v-for="(item,index) in listThree" :key="'three'+index">
                <div class="info">
                  <img alt='' src='@/assets/imgs/EPimg.png' />
                  <!--:src="item.img"-->
                  <div class="num">
                    <div class="font50">{{ item.remark }}</div>
                    <div class="sub-num">{{ item.date }}</div>
                  </div>
                </div>
                <div class="price" :class='item.amount>0?"":"red"'>   {{ '   '+item.amount }}</div>
              </div>
            </van-tab>
            <!-- <van-tab class="no-area"></van-tab> -->
          </van-tabs>
        </div>
      </div>
    </div>
    <YellowComfirm :show="isEnter" :tipTitle="tips"  @clickNo="clickNo()" @clickOk="clickOk()" @changeModel="changeModel"></YellowComfirm>
    </ScrollRefresh>

  </div>
</template>
<script type="text/javascript">
import YellowComfirm from 'components/YellowComfirm'
import TopBar from 'components/TopBar'
import banner1 from '../../assets/imgs/banner-00.png'
import banner2 from '../../assets/imgs/banner-01.png'
import banner3 from '../../assets/imgs/banner-02.png'
import defaultImg from '@/assets/imgs/set/head02.png'
import countTo from 'components/countTo'
import { http } from 'util/request'
import { GetBanner, GetUserInfo, getEpexchange } from 'util/netApi'
import ScrollRefresh from 'components/ScrollRefresh'
import { storage } from 'util/storage'
import { photoList } from 'util/const.js'

export default {

  data () {
    return {
      tips: '',
      isEnter: false,
      defaultImg,
      username: '',
      uid: '',
      showMount: false,
      // topBarOption: {
      //   iconLeft: 'iconmenu2',
      //   iconRight: 'iconxinxi2'
      // },
      active: 0,
      startVal: 0,
      endVal: 0,
      balanceTile: '奖金',
      images: [{image: banner1}, {image: banner2}, {image: banner3}],
      headerimg: null,
      balanceList: [
        {
          name: '奖金',
          type: 'ep',
          count: 0
        },
        {
          name: '注册分',
          type: 'rp',
          count: 0
        }
      ],
      listOne: [
      ],
      listTwo: [

      ],
      listThree: [
      ]
    }
  },
  components: {
    TopBar,
    countTo,
    ScrollRefresh,
    YellowComfirm
  },
  computed: {},
  methods: {
    clickNo () {
      this.$router.push({name: 'setting'})
    },
    readloadinfo () {
      var choiceindex = 1
      if (this.active == 0) {
        choiceindex = 1
      } else if (this.active == 1) {
        choiceindex = 2
      } else if (this.active == 2) {
        choiceindex = 3
      }
      this.ToGetEPlist(choiceindex)
    },
    onSwiperChange (index) {
      this.endVal = this.balanceList[index].count
      this.balanceTile = this.balanceList[index].name
    },

    ToGetBanner () {
      // http(GetBanner, { language: 'cn' }, json => {
      //   if (json.code === 0) {
      //     this.images = json.response.list
      //   }
      // })
    },
    goSetting () {
      this.$router.push({ name: 'SetUp' })
    },
    goNews () {
      this.$router.push({ name: 'News' })
    },
    goinfo (name) {
      if (name == 'sum') {
        this.$router.push({ name: 'myFamily' })
      } else {
        this.$router.push({ name: 'epList', params: { cktype: name } })
      }
    },
    TogetUserInfo () {
      http(GetUserInfo, null, json => {
        if (json.code === 0) {
          this.username = json.response.nickname
          this.uid = json.response.uid
          this.endVal = json.response.gold
          this.headerimg = photoList[json.response.photo]
          storage.setLocalStorage('service', json.response.service)
          this.balanceList = [
            {
              name: '奖金',
              type: 'ep',
              count: json.response.gold
            },
            {
              name: '注册分',
              type: 'rp',
              count: json.response.rp
            }
          ]

          if (!json.response.isSetIDNumber) {
            this.tips = '您还未完善资料，请前去完善资料!'
            this.isEnter = true
          }
        } else {
          //  this.$router.push({ path: "/login" });
        }
      })
    },
    changeModel (v) {
      this.isEnter = v
    },
    ToGetEPlist (cktype) {
      http(getEpexchange, {type: cktype, pageSize: 5, pageIndex: 1, cktype: 'ep' },
        json => {
          if (json.response) {
            if (cktype == 1) {
              this.listOne = json.response
            } else if (cktype == 2) {
              this.listTwo = json.response
            } else if (cktype == 3) {
              this.listThree = json.response
            }
          }
        }
      )
    },
    getData () {
      this.ToGetBanner()
      this.TogetUserInfo()
      this.readloadinfo()
    }
  },
  mounted () {
    // this.ToGetBanner()
    this.TogetUserInfo()
    this.readloadinfo()
  }
}
</script>

<style lang="less" scoped>
.homeWrapper .innerScroll {
  /deep/.wrapper {
    .bscroll-container {
      min-height: calc(100vh + 10px) !important;
    }
  }
}
.homeWrapper {
  height: 100vh;
  // overflow-y: scroll;
  .mainWrap {
    width: 95%;
    margin: 0 auto;
    img {
      width: 100%;
    }
  }
  .sweiper1 {
    margin-bottom: 42px;
    img {
      width: 100%;
      height: 354px;
    }
    /deep/ .van-swipe-item {
      border-radius: 24px;
      overflow: hidden;
      border: 10px solid #fff;
    }
    /deep/ .van-swipe__indicator {
      width: 20px;
      height: 20px;
    }
    /deep/ .van-swipe__indicator--active {
      background-color: #efb618;
    }
  }
  .swiper2Box {
    position: relative;
    margin-bottom: 42px;
    height: 340px;
    .sweiper2{
      background: url('../../assets/imgs/blueBj.png') no-repeat center center/100% 100%;
    }
    .sweiper2,
    .sweiper2 img {
      height: 100%;
      border-radius: 24px;
      background-color: #4678BC;
    }
    .countBox {
      position: absolute;
      top:0;
      height: 340px;
      width: 80vw;
      display: flex;
      left: 100px;
      align-items: center;
      justify-content: space-between;
     .eye {
          position: absolute;
          right: 0px;
          bottom: 30px;
          font-size: 60px;
          color: #0C0100;
          font-weight: bold;
        }
        .eye.iconyanjing1{
          font-size: 70px;
          bottom:20px;
          width: 70px;
        }
      p {
        font-size: 50px;
        margin-top:40px;
        font-weight: 600;
        color: #191819;
        margin-bottom: 50px;
      }
      .countNum {
        font-size: 106px;
        font-weight: 800;
        color: #0C0100;
        line-height: 120px;
        padding-right: 100px;
      }
    }
    /deep/ .van-swipe__indicator {
      display: none;
    }
  }
}
</style>
<style lang="less" scoped>
@keyframes myfirst {
  from {
    transform: rotate(0deg);
  }
  to {
    transform: rotate(360deg);
  }
}

.four-tit-t{
   position:absolute;
   top:120px;
   left: 10px;
   z-index: 2;
   font-size: 64px;
   color: #fff !important;
   margin-left: -100px !important;
}
.four-tit-b{
  position:absolute;
  color: #0C0100;
  top:260px;
  font-size: 80px!important;
  z-index: 2;
  margin-left: -100px !important;
}
// .img-fangxing{
//   animation: myfirst 3s 2s infinite;
// }
// .img-yueliang1{
//   animation: myfirst 4s 3s infinite;
// }
.home-list {
  border-radius: 24px;
  background: #f6f7f9;
  padding: 30px;
  .recent {
    display: flex;
    flex-direction: row;
    justify-content: space-between;
    margin-bottom: 60px;
    border-bottom: none;
    .title-re {
      margin-top: 10px;
      font-size: 54px;
      font-weight: 600;
    }
    .view {
      margin-top: 10px;
      float: right;
      color: #EFB618;
      font-size: 44px;
      font-weight: 800;
    }
  }
  .title {
    margin-top: 20px;
    font-weight: bold;
    // color: rgba(52, 50, 55, 1);
    opacity: 0.75;
  }
  /deep/ .van-tabs {
    background: #f6f7f9;
  }
  /deep/ .van-tabs__wrap {
    background: #f6f7f9;
    font-size: 60px;
    height: 100px;
    padding-right: 240px;
  }
  /deep/ .van-tab {
    font-size: 42px;
    font-weight: 600;
    color: #EFB618;
    background: #f6f7f9 !important;
  }
  /deep/ .van-tabs__nav--line::after {
    content: '';
    display: block;
    position: absolute;
    width: 1025px;
    height: 5px;
    background: #c5c5c5;
    bottom: 14px;
  }
  /deep/ .van-hairline--top-bottom::after {
    border: none;
  }
  /deep/ .van-tabs__line {
    background: #EFB618;
    width: 250px !important;
    height: 6px;
  }
  /deep/.van-tab__text {
    color: #C5C5C5;
    font-size: 54px;
    display: block;
    height: 100%;
    line-height: 100%;
    // font-weight: 600;
  }
  /deep/.van-tab--active {
    .van-tab__text {
      color: #EFB618;
    }
  }
  /deep/ .van-tabs__content {
    overflow: auto;
    padding-top: 30px;
  }
  .detail {
    display: flex;
    flex-direction: row;
    justify-content: space-aroud;
    padding: 28px;
    background: #fff;
    margin-top: 30px;
    border-radius: 40px;
    min-height: 200px;
    box-shadow: #ddd 2px;
    .price {
      color: #09b216;
      margin-top: 30px;
      font-size: 54px;
      font-weight: 600;
      width: 40%;
      text-align: right;
      padding-right: 20px;
    }
     .price.red {
      color: #FB3D42;
     }
    .info {
      width: 60%;
      display: flex;
      flex-direction: row;
      justify-content: space-aroud;
      .num {
        margin-top: 20px;
        font-weight: 600;
        font-size: 46px;
        color: #212227;
        // opacity: 0.62;
        .sub-num {
          margin-top: 20px;
          color: #777E8F;
          font-size: 40px;
        }
        > div {
          height: 50px;
          line-height: 50px;
        }
      }
      > img {
        margin-right: 20px;
        margin-top: 10px;
        width: 123px;
        height: 123px;
      }
    }
  }
}
</style>
