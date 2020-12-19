<template>
  <div class="ep-list">
    <TopBar class="center-one-search">{{tmpcktype}}台账</TopBar>
    <div class="relative">
      <div class="trade clearfix">
        <van-tabs @click="readloadinfo" v-model="active" color="#EFB618" line-height="0">
          <van-tab title="所有台账">
            <div class="listWrap" ref="listWrap">
              <ScrollRefresh
                @getData="load"
                :residualHeight="topbarHeight+bottomTabBarHeight+60"
                :active="active"
                :isNone="isNone"
              >
                <div class="detailWrap">
                  <div class="detail" v-for="(item,index) in listOne" :key="'ONE'+index">
                    <div class="info">
                      <img :src="photoList[tmpcktype]" alt />
                      <div class="num">
                        <div class="font50">{{ item.remark }}</div>
                        <div class="sub-num">{{ item.date }}</div>
                      </div>
                    </div>
                    <div class="price">
                      <div :class="item.amount>0?'':'special-price'">{{ item.fromID }}</div>
                      <div :class="item.amount>0?'':'special-price'">{{ item.amount.toFixed(2) }}</div>
                    </div>
                  </div>
                </div>
              </ScrollRefresh>
            </div>
            <!-- <div class="listWrap" ref="listWrap">
              <div class="container">
                <div class="detail" v-for="(item,index) in listOne" :key="'ONE'+index">
                  <div class="info">
                    <img :src="photoList[tmpcktype]" alt />
                    <div class="num">
                      <div class="font50">{{ item.remark }}</div>
                      <div class="sub-num">{{ item.date }}</div>
                    </div>
                  </div>
                  <div class="price">
                       <div :class="item.amount>0?'':'special-price'">{{ item.fromID }}</div>
                       <div :class="item.amount>0?'':'special-price'">  {{ item.amount.toFixed(2) }}</div>
                    </div>
                </div>
              </div>
            </div>-->
          </van-tab>
          <van-tab title="转入">
            <div class="listWrap">
              <ScrollRefresh
                @getData="load"
                :residualHeight="topbarHeight+bottomTabBarHeight+60"
                :active="active"
                :isNone="isNone"
              >
                <div class="detailWrap">
                  <div class="detail" v-for="(item,index)  in listTwo" :key="'TWO'+index">
                    <div class="info">
                      <img :src="photoList[tmpcktype]" alt />
                      <div class="num">
                        <div class="font50">{{ item.remark }}</div>
                        <div class="sub-num">{{ item.date }}</div>
                      </div>
                    </div>
                    <div class="price">
                      <div>{{ item.fromID }}</div>
                      <div>{{ item.amount.toFixed(2) }}</div>
                    </div>
                  </div>
                </div>
              </ScrollRefresh>
              <!-- <div class="container">
                <div class="title font42"></div>
                <div class="detail" v-for="(item,index)  in listTwo" :key="'TWO'+index">
                  <div class="info">
                    <img :src="photoList[tmpcktype]" alt />
                    <div class="num">
                      <div class="font50">{{ item.remark }}</div>
                      <div class="sub-num">{{ item.date }}</div>
                    </div>
                  </div>
                  <div class="price">
                    <div>{{ item.fromID }}</div>
                    <div>{{ item.amount.toFixed(2) }}</div>
                  </div>
                </div>
              </div>-->
            </div>
          </van-tab>
          <van-tab title="转出">
            <div class="listWrap" ref="listWrap">
              <div class="container">
                <ScrollRefresh
                  @getData="load"
                  :residualHeight="topbarHeight+bottomTabBarHeight+60"
                  :active="active"
                  :isNone="isNone"
                >
                  <div class="detailWrap">
                    <div class="detail" v-for="(item,index) in listThree" :key="'THR'+index">
                      <div class="info">
                        <img :src="photoList[tmpcktype]" alt />
                        <div class="num">
                          <div class="font50">{{ item.remark }}</div>
                          <div class="sub-num">{{ item.date }}</div>
                        </div>
                      </div>

                      <div class="price special-price">
                        <div class="special-price">{{ item.fromID }}</div>
                        <div class="special-price">{{ item.amount.toFixed(2) }}</div>
                      </div>
                    </div>
                  </div>
                </ScrollRefresh>
              </div>
            </div>
          </van-tab>
        </van-tabs>
      </div>
    </div>
  </div>
</template>
<script>
import TopBar from 'components/TopBar'
import ScrollRefresh from 'components/ScrollRefresh'
import { http } from 'util/request'
import { storage } from 'util/storage'
import { accessToken, loginPro } from 'util/const.js'
import { getEpexchange } from 'util/netApi'
import EP from '@/assets/imgs/EPimg.png'
import SP from '@/assets/imgs/SPimg.png'
import RP from '@/assets/imgs/RPimg.png'
import DPE from '@/assets/imgs/DPimg.png'

export default {
  name: 'EpList',
  components: {
    TopBar,
    ScrollRefresh
  },
  data () {
    return {
      active: 0,
      showChat: true,
      photoList: {
        EP: EP,
        SP: SP,
        RP: RP,
        DPE: DPE
      },
      tmpcktype: 'EP',
      showLocation: true,
      listOne: [],
      listTwo: [],
      listThree: [],
      pageSize: 10,
      isNone: false // 数据是否加载完毕
    }
  },
  methods: {
    readloadinfo (pageIndex = 1) {
      // var choiceindex = 1;
      // if (this.active == 0) {
      //   choiceindex = 1;
      // } else if (this.active == 1) {
      //   choiceindex = 2;
      // } else if (this.active == 2) {
      //   choiceindex = 3;
      // }
      this.load(1)
    },
    load (pageIndex) {
      let stype = this.active + 1
      //  return
      console.log(pageIndex, ',pageSize,pageIndex')
      http(
        getEpexchange,
        {
          type: stype,
          pageSize: this.pageSize,
          pageIndex,
          cktype: this.tmpcktype
        },
        json => {
          if (json.response != 'undefined') {
            if (pageIndex !== 1 && json.response.length === 0) {
              this.isNone = true
            } else {
              console.log(777777)
              this.isNone = false
              if (pageIndex === 1) {
                if (stype == 1) {
                  this.listOne = json.response
                } else if (stype == 2) {
                  this.listTwo = json.response
                } else if (stype == 3) {
                  this.listThree = json.response
                }
              } else {
                if (stype == 1) {
                  this.listOne = [...this.listOne, ...json.response]
                } else if (stype == 2) {
                  this.listTwo = [...this.listTwo, ...json.response]
                } else if (stype == 3) {
                  this.listThree = [...this.listThree, ...json.response]
                }
              }
            }
          }
        }
      )
    }
  },
  created () {
    // console.log(this.$route.params);
    // this.tmpcktype = this.$route.params.cktype.toUpperCase();
    this.load(1)
  }
}
</script>
<style lang="less" scoped>
.ep-list {
  .trade {
    margin: 0;
    padding: 30px;
    .title {
      margin-top: 30px;
      font-weight: bold;
      color: rgba(52, 50, 55, 1);
      opacity: 0.75;
    }
    height: 76vh;
    margin-top: 10px;
    border-radius: 40px 40px 0 0;
    /deep/.van-tabs__wrap {
      height: 100px;
    }
    /deep/.van-tab {
      font-size: 0.48rem;
      font-weight: 600;
      border-bottom: 8px solid #ffffff;
      color: #191819;
      height: 90px;
      line-height: 90px;
      padding-bottom: 20px;
    }
    /deep/.van-tab__text {
      color: #191819;
    }
    /deep/.van-tab--active {
      border-bottom: 8px solid #efb618;
      /deep/.van-tab__text {
        color: #fff;

      }
    }
    .listWrap {
      // min-height: calc(100vh - 600px);
      padding-top: 20px;
      // overflow: scroll;
    }
    .detailWrap {
      padding-bottom: 80px;
    }
    .detail {
      display: flex;
      flex-direction: row;
      justify-content: space-aroud;
      padding: 58px 20px;
      background: #fff;
      margin-top: 20px;
      border-radius: 40px;
      height: 189px;
      .price {
        color: #09b216;
        font-size: 60px;
        font-weight: 700;
        width: 50%;
        text-align: right;
        &.special-price {
          color: #fe0000;
        }
      }
      .info {
        margin-top: -40px;
        margin-left: 0px;
        width: 80%;
        display: flex;
        flex-direction: row;
        justify-content: space-aroud;
        position: relative;
        .word {
          font-size: 100px;
          color: #fcc00b;
          position: absolute;
          top: 36%;
          left: 6%;
          font-weight: 900;
        }
        .num {
          font-weight: 600;
          margin-top: 20px;
          margin-left: 20px;
          width: 100%;
          .font50 {
            // font-size: 35px;
            font-weight: bold;
            color: #000000;
          }
          .sub-num {
            margin-top: 20px;
            font-size: 36px;
            font-weight: bold;
            color: #767c8f;
            opacity: 0.8;
          }
          > div {
            height: 50px;
            line-height: 50px;
          }
        }

        > img {
          margin-top: 20px;
          margin-left: 20px;
          width: 121px;
          height: 121px;
        }
      }
      .price {
        padding-right: 20px;
      }
      .special-price {
        color: #fe0000;
      }
    }
    /deep/ .van-tabs__nav {
      background: rgba(0, 0, 0, 0);
    }
    /deep/ .van-hairline--top-bottom::after {
      border-width: 0;
    }
  }
}
</style>
