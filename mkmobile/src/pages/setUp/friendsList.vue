<template>
  <div class="relative">
    <TopBar class="center-one-search">
      <div>
        <div class="three-tit-t">我的朋友</div>
        <div class="three-tit-b">{{friendsList.length}}</div>
        <TopSearch @onSearch="search"></TopSearch>
      </div>
    </TopBar>
    <ScrollRefresh
      @getData="loadinfo"
      :residualHeight="topbarHeight+bottomTabBarHeight+10"
      :isNeedUp="false"
      class="relativeScroll"
    >
      <div class="friendlistWrap borderR" ref="listWrap">
        <div class="friendsList">
          <van-collapse v-model="activeName" accordion>
            <van-collapse-item
              :name="index"
              v-for="(item,index) in friendsList"
              :key="index"
              class="friendWrap"
            >
              <template #title>
                <div class="friendTop">
                  <img :src="item.photo" class="img" alt />
                  <div class="data-title">
                    <div class="friendsName">{{item.NickName+'('+item.uID+')'}}</div>
                    <!-- <div class="friendsNum" v-if="item.friendsNum">{{item.friendsNum}}</div> -->
                  </div>
                </div>
              </template>
              <ul>
                <li v-if="item.friendsNum">
                  <span class="title">朋友：</span>
                  <dl>
                    <dt>{{item.friendsNum}}个</dt>
                  </dl>
                </li>
                <li v-if="item.performance">
                  <span class="title">业绩：</span>
                  <dl>
                    <dt>{{item.performance}}</dt>
                  </dl>
                </li>
                <li v-if="item.honors&&item.honors.length">
                  <span class="title">荣誉：</span>
                  <dl>
                    <dt v-for="(honor,j) in item.honors" :key="index+j">{{honor}}</dt>
                  </dl>
                </li>
              </ul>
            </van-collapse-item>
          </van-collapse>
          <!-- <div class="list">
            <div class="relative" v-for="(item,index) in friendsList" :key="index">
              <img :src="item.photo" class="img" alt />
              <van-cell class="cell-info borderR mb-40" is-link @click="gofriend(item.uID)">
                <div slot="title" class="data-title">
                  <div class="friendsName">{{item.NickName+'('+item.uID+')'}}</div>
                  <div class="friendsNum" v-if="item.friendsNum">{{item.friendsNum}}</div>
                </div>
              </van-cell>
            </div>
          </div>-->
        </div>
      </div>
    </ScrollRefresh>
  </div>
</template>
<script>
import TopBar from 'components/TopBar'
import { http } from 'util/request'
import { photoList } from 'util/const.js'
import TopSearch from 'components/TopSearch'
import ScrollRefresh from 'components/ScrollRefresh'
import defaultImg from '@/assets/imgs/set/head02.png'
import { GetFriendsList, GetSearchFimaly, GetUserInfo } from 'util/netApi'
// import getHeightPX from "../../mixins/getHeightPX";
export default {
  components: {
    TopBar,
    TopSearch,
    ScrollRefresh
  },
  data () {
    return {
      friendsList: [
        {
          photo: defaultImg,
          NickName: 'Totay cyels',
          uID: '10001',
          friendsNum: 34,
          performance: 6000,
          honors: ['经理2个', ' 总监0个', ' 总裁0个', ' 董事0个', '合伙人0个']
        },
        {
          photo: defaultImg,
          NickName: 'Totay cyels',
          uID: '10002',
          friendsNum: 34,
          performance: 6000,
          honors: ['经理2个', ' 总监0个', ' 总裁0个', ' 董事0个', '合伙人0个']
        }
      ],
      uid: 0,
      activeName: 0
    }
  },
  computed: {},
  methods: {
    loadinfo () {
      var _this = this
      http(GetFriendsList, { uid: this.uid }, json => {
        if (json.response) {
          _this.friendsList = json.response
          _this.friendsList.forEach(element => {
            element.photo = photoList[element.photo]
          })
        }
      })
    },
    search (uid) {
      if (uid == '') {
        this.uid = 0
      } else {
        this.uid = uid
      }
      this.loadinfo()
    },
    gofriend (uID) {
      // this.$router.push({ name: "relation", params: { uid: uID } });
    }
  },
  created () {
    this.loadinfo()
  }
}
</script>
<style lang='less' scoped>
.relativeScroll {
  /deep/.wrapper {
    padding-top: 20px;
    .bscroll-container {
      min-height: calc(100vh - 620px);
    }
  }
}

.friendlistWrap {
  position: relative;
  width: 100%;
  margin-top: -40px;
  padding: 40px 60px 0;
  .friendsList {
    margin-top: 60px;
    padding-bottom: 50px;
  }
  .friendWrap {
    background-color: #fff;
    padding: 10px 30px;
    border-radius: 30px;
    margin-bottom: 33px;

    .friendTop {
      display: flex;
      justify-content: space-between;
    }
    img {
      width: 120px;
      height: 120px;
      margin-right: 60px;
    }
  }
  /deep/ .van-collapse {
    .van-collapse-item--border::after {
      border: 0;
    }
    .van-collapse-item {
      .van-icon-arrow::before {
        color: #767c8f;
        font-size: 60px;
        transform: rotate(0);
      }
      .van-collapse-item__title--expanded .van-cell__right-icon::before {
        transform: rotate(-90deg);
      }
      .van-collapse-item__title--expanded::after {
        display: none;
      }
      .van-cell {
        align-items: center;
        .van-cell__right-icon {
          font-size: 60px;
          color: #d9c704;
        }
      }
      .van-collapse-item__content {
        padding: 0;
      }
      ul {
        width: 100%;
        padding-left: 180px;
        li {
          font-size: 40px;
          color: #767c8f;
          font-weight: bold;
          display: flex;
          margin-bottom: 10px;
          .title {
            width: 18%;
          }
          dl {
            flex: 1;
            display: flex;
            flex-wrap: wrap;
            dt {
              margin-right: 10px;
            }
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

  .data-title {
    flex: 1;
    div {
      height: 60px;
      line-height: 60px;
    }
    div.friendsName {
      white-space: nowrap;
      height: 100px;
      line-height: 100px;
      color: #767c8f;
      font-size: 50px;
      font-weight: bold;
    }
    div.friendsNum {
      margin-left: 0px;
      font-size: 40px;
    }
  }
}
</style>
