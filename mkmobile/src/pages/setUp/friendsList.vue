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
      <div class="listWrap borderR" ref="listWrap">
        <div class="friendsList">
          <div class="list">
            <div class="relative" v-for="(item,index) in friendsList" :key="index">
              <img :src="item.photo" class="img" alt />
              <van-cell class="cell-info borderR mb-40" is-link @click="toGetFriendsListbyId(item.uID)">
                <div slot="title" class="data-title">
                  <div class="friendsName">{{item.NickName+'('+item.uID+')'}}</div>
                  <div class="friendsNum" v-if="item.friendsNum">{{item.friendsNum}}</div>
                </div>
              </van-cell>
            </div>
          </div>
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
import { GetFriendsList, GetSearchFimaly, GetUserInfo,GetFriendsListbyId } from 'util/netApi'
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
      ],
      uid: 0
    }
  },
  computed: {},
  methods: {
    toGetFriendsListbyId(id)
    {
      http(GetFriendsListbyId, { uid: id }, json => {
        if (json.response) {
            console.log(json)
          }
        })
      
    },
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

.listWrap {
  position: relative;
  width: 100%;
  margin-top: -40px;
  padding: 40px 60px 0;
}
.friendsList {
  margin-top: 60px;
  padding-bottom: 50px;
  .list {
    // background: #f2f3f7;
    .cell-info {
      // line-height: 120px;
      min-height: 150px;
      align-items: center;
    }
    .img {
      position: absolute;
      left: 20px;
      top: 50%;
      transform: translateY(-50%);
      width: 120px;
      height: 120px;
      z-index: 99;
    }
    /deep/.van-cell__right-icon {
      font-size: 60px;
      // height: 137px;
      // margin-top: 30px;
      // line-height: 120px;
    }
    /deep/.van-cell__title {
      margin-left: 160px;
      font-weight: bold;
      color: #767c8f;
      position: relative;
    }
    /deep/.van-cell__value {
      font-size: 35px;
      margin-left: 140px;
      font-weight: bold;
      color: rgba(118, 124, 143, 1);
      position: relative;
    }
    /deep/.van-progress__pivot {
      display: none;
    }
    .cell-special {
      /deep/.van-cell__title {
        margin-left: 600px;
      }
    }
  }
}

.data-info {
  height: 60px;
  line-height: 60px;
  div {
    display: inline-flex;
    width: 50%;
    color: #e28b07;
    span {
      color: #660a79;
      text-align: right;
      display: inline-block;
      width: 140px;
    }
  }
}
.data-title {
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
  }
  div.friendsNum {
    margin-left: 0px;
    font-size: 40px;
  }
}
</style>
