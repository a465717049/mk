<template>
  <div class="homeWrapper bggray">
      <ScrollRefresh  
      @getData="re"
      :residualHeight="58"
      :isNeedUp="false"
      class="innerScroll">
    <TopBar :option="topBarOption" @clickR="goNews">
      <div class="center-four-search">
        <div class="four-tit-t">Hello,{{uid}}</div>
        <div class="four-tit-b">{{username}}</div>
        <img class="img-yueliang" src="@/assets/imgs/yueliang.png" alt />
        <img class="img-yueliang1" src="@/assets/imgs/yueliang1.png" alt />
        <img class="img-fangxing" src="@/assets/imgs/fangxing.png" alt />
       <div class='header-back'></div> <img  @click="goSetting" class="img-header" :src="headerimg" alt />
      </div>
    </TopBar>
  
    <div class="mainWrap">
      <div class="innerWrap">
        <van-swipe :autoplay="4000" class="sweiper1">
          <van-swipe-item v-for="(image, index) in images" :key="index">
            <img :src="image.image" />
          </van-swipe-item>
        </van-swipe>
        <div class="swiper2Box">
          <van-swipe :autoplay="400000" class="sweiper2" @change="onSwiperChange">
            <van-swipe-item v-for="(item, index) in balanceList"  :key="index">
              <img src="../../assets/imgs/yellowBg.png"  alt />
            </van-swipe-item>
          </van-swipe>
          <div class="countBox" @click='goinfo(type)'>
            <p>{{ balanceTile }}</p>
            <countTo
              ref="count"
              :startVal="0"
              :endVal="endVal"
              :duration="2000"
              separator=","
              prefix=""
              :useEasing="true"
              class="countNum"
            ></countTo>
          </div>
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
            <div class="title-re">近期收益</div>
            <div class="view"  ><router-link to="Exchangedata" class="router">更多</router-link></div>
          </div>
          <van-tabs v-model="active" @click="readloadinfo">
            <van-tab title="全部">
              <div class="detail" v-for="(item,index) in listOne" :key="'one'+index">
                <div class="info">
                  <img alt src='@/assets/imgs/EPimg.png' />
                  <!--:src="item.img"-->
                  <div class="num">
                    <div class="font50">{{ item.remark }}</div>
                    <div class="sub-num">{{ item.date }}</div>
                  </div>
                </div>
                <div class="price" :class='item.amount>0?"":"red"'>＄ {{ item.amount }}</div>
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
                <div class="price" :class='item.amount>0?"":"red"'>＄ {{ item.amount }}</div>
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
                <div class="price" :class='item.amount>0?"":"red"'>＄   {{ '   '+item.amount }}</div>
              </div>
            </van-tab>
            <!-- <van-tab class="no-area"></van-tab> -->
          </van-tabs>
        </div>
      </div>
    </div>
    </ScrollRefresh>
  </div>
</template>
<script type="text/javascript">
import TopBar from 'components/TopBar'
import ScrollRefresh from "components/ScrollRefresh";
import banner1 from '../../assets/imgs/banner-00.png'
import banner2 from '../../assets/imgs/banner-01.png'
import countTo from 'components/countTo'
import { http } from 'util/request'
import { GetBanner, GetUserInfo, getEpexchange } from 'util/netApi'
import { storage } from 'util/storage'
import { photoList } from 'util/const.js'
export default {
  data() {
    return {
      username: '',
      uid: '',
      topBarOption: {
        iconLeft: 'iconzhankai',
        iconRight: 'iconlingdang'
      },
      active: 0,
      startVal: 0,
      endVal: 0,
      type:"dpe",
      balanceTile: 'DPE Balance',
      images: [{image:banner1},{image:banner2}],
      headerimg:null,
      balanceList: [
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
    ScrollRefresh
  },
  computed: {},
  methods: {
    readloadinfo() {
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
    onSwiperChange(index) {
      this.endVal = this.balanceList[index].count
      this.balanceTile = this.balanceList[index].name
      this.type = this.balanceList[index].type

      return false;
    },
    
    ToGetBanner() {
      // http(GetBanner, { language: 'cn' }, json => {
      //   if (json.code === 0) {
      //     this.images = json.response.list
      //   }
      // })
    },
    goSetting() {
      this.$router.push({ name: "SetUp" });
    },
     goNews() {
      this.$router.push({ name: "News" });
    },
     goinfo(name) {
       if(name=='sum')
       {
              this.$router.push({ name: "myFamily" });
       }
       else
       {
        this.$router.push({ name: 'epList', params: { cktype: name } });
       }
    },
    TogetUserInfo() {
      var date=new Date();
      http(GetUserInfo, {date:date}, json => {
        if (json.code === 0) {
          this.username = json.response.nickname
          this.uid = json.response.uid
          this.endVal = json.response.apple
          this.headerimg=photoList[json.response.photo]
          storage.setLocalStorage("service",json.response.service)
          this.balanceList = [
             {
              name: 'DPE Balance',
              type:'dpe',
              count: json.response.apple
            }
            , 
            {
              name: 'EP Balance',  
               type:'ep',
              count: json.response.gold
            },
            {
              name: 'RP Balance',
               type:'rp',
              count: json.response.rp
            },
            {
              name: 'SP Balance',
               type:'sp',
              count: json.response.seed
            },
            {
              name: 'DPE(主+子) Balance',
               type:'sum',
              count: json.response.sum
            },
            {
              name: 'Dynamic Balance',
               type:'ep',
              count: json.response.dynamicTotal
            }
          ]
        }
        else{
           this.$router.push({ path: "/login" });
        }
      })
     
    },
    ToGetEPlist(cktype) {
      http(getEpexchange,{type: cktype, pageSize: 5, pageIndex: 1, cktype: 'ep' },
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
    re(){
        this.ToGetBanner()
         this.TogetUserInfo()
        this.readloadinfo()
    }
  },
  created() {
     this.ToGetBanner()
  },
  mounted(){
    this.ToGetBanner()
    this.TogetUserInfo()
    this.readloadinfo()
  }
}
</script>

<style lang="less" scoped>
.innerScroll {
  height: 100vh;
  /deep/.wrapper {
    .bscroll-container {
      min-height: calc(100vh + 450px) !important;
    }
  }
}
.homeWrapper {
  height: 100vh;
  overflow-y: scroll;
  .mainWrap {
    width: 95%;
    margin: -60px auto 0;
    img {
      width: 100%;
    }
  }
  .sweiper1 {
    margin-bottom: 20px;
    img {
      width: 100%;
      height: 360px;
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
      background-color: #6318c3;
    }
  }
  .swiper2Box {
    position: relative;
    margin-bottom: 20px;
    height: 390px;
    .sweiper2,
    .sweiper2 img {
      height: 100%;
      border-radius: 24px;
    }
    .countBox {
      position: absolute;
      top: 40px;
      color: #fff;
      left: 120px;
      p {
        font-size: 60px;
        margin-top:40px;
        margin-bottom: 50px;
      }
      .countNum {
        font-size: 106px;
        font-weight: 800;
        margin-top: 50px;
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
}
.four-tit-b{
   position:absolute;
    top:365px;
    font-size: 100px!important;
    z-index: 2;
}
.img-fangxing{
  animation: myfirst 3s 2s infinite;
}
.img-yueliang1{
  animation: myfirst 4s 3s infinite;
}
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
      font-size: 52px;
      font-weight: 600;
    }
    .view {
      margin-top: 10px;
      float: right;
      color: #6e21d1;
      font-size: 46px;
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
    color: #6e21d1;
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
    background: #6e21d1;
    width: 250px !important;
  }
  /deep/.van-tab__text {
    color: #c6c6c6;
    font-weight: 800;
  }
  /deep/.van-tab--active {
    .van-tab__text {
      color: #6e21d1;
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
      font-size: 70px;
      font-weight: 600;
      width: 40%;
      text-align: right;
      padding-right: 20px;
    }
     .price.red {
      color: #ff3300;
     }
    .info {
      width: 60%;
      display: flex;
      flex-direction: row;
      justify-content: space-aroud;
      .num {
        margin-top: 20px;
        font-weight: 600;
        opacity: 0.62;
        .sub-num {
           margin-top: 10px;
          color: #767c8f;
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
