<template>
  <div>
    <TopBar class="center-one-search" :option="topBarOption">反 馈</TopBar>

    <div class="feedback-body">
      <!-- <div class="innerScroll"></div> -->
      <ScrollRefresh
        @getData="ToGetUserFeedBack"
        :residualHeight="topbarHeight+bottomTabBarHeight+85"
        :isNeedUp="false"
        class="innerScroll"
      >
        <ul class="chat_bd">
          <li v-for="item in list" :key="item.id">
            <div class="chat_time" v-if="item.direction === 'center'">{{ item.time }}</div>
            <div v-else-if="item.direction === 'left'" class="chat_item">
              <img class="chat_left_img" :src="item.image" />
              <div class="chat_left_msg">{{ item.msg }}</div>
            </div>
            <div v-else class="chat_item chat-right">
              <img class="chat_right_img" :src="defaultimg" />
              <div class="chat_right_msg">{{ item.msg }}</div>
            </div>
          </li>
        </ul>
      </ScrollRefresh>
      <div class="send-msg">
        <i class="iconfont iconyuyin"></i>
        <input type="text" v-model="msg" />
        <button type="primary" @click="sendMessage">发送</button>
      </div>
    </div>
  </div>
</template>

<script>
import TopBar from 'components/TopBar'
import { http } from 'util/request'
import { GetUserFeedBack, AddUserFeedBack, GetUserInfo, SetReadUserFeedBack } from 'util/netApi'
import { storage } from 'util/storage'
import ScrollRefresh from 'components/ScrollRefresh'
import defaultImg from '@/assets/imgs/set/head03.png'
import { accessToken, loginPro, photoList } from 'util/const.js'
import Store from '@/store'

export default {
  components: {
    TopBar,
    ScrollRefresh
  },
  data () {
    return {
      msg: '',
      topBarOption: {
        iconLeft: 'iconmenu2',
        iconRight: false
      },
      defaultimg: '',
      list: [
        // {
        //   id: 1,
        //   direction: "center",
        //   time: "昨天  22:18",
        //   image: defaultImg
        // },
        // {
        //   id: 2,
        //   direction: "left",
        //   date: "6mel,2020",
        //   image: defaultImg,
        //   msg: "请问有什么可以帮到您"
        // },
        // {
        //   id: 3,
        //   image: require("@/assets/imgs/nick-1.png"),
        //   direction: "right",
        //   msg:
        //     "请问最近的车奖是怎么一回事？？色极为关键是南方健身房是的封建时代和封建士大夫十分"
        // },
        // {
        //   id: 4,
        //   direction: "left",
        //   date: "6mel,2020",
        //   image: defaultImg,
        //   msg: "请问有什么可以帮到您"
        // },
        // {
        //   id: 5,
        //   image: require("@/assets/imgs/nick-1.png"),
        //   direction: "right",
        //   msg: "请问最近的车奖是怎么一回事？"
        // },
        // {
        //   id: 6,
        //   direction: "left",
        //   date: "6mel,2020",
        //   image: defaultImg,
        //   msg: "请问有什么可以帮到您"
        // },
        // {
        //   id: 7,
        //   image: require("@/assets/imgs/nick-1.png"),
        //   direction: "right",
        //   msg: "请问最近的车奖是怎么一回事？"
        // },
        // {
        //   id: 8,
        //   direction: "left",
        //   date: "6mel,2020",
        //   image: defaultImg,
        //   msg: "请问有什么可以帮到您"
        // },
        // {
        //   id: 9,
        //   image: require("@/assets/imgs/nick-1.png"),
        //   direction: "right",
        //   msg: "请问最近的车奖是怎么一回事？"
        // },
        // {
        //   id: 10,
        //   direction: "left",
        //   date: "6mel,2020",
        //   image: defaultImg,
        //   msg: "请问有什么可以帮到您"
        // },
        // {
        //   id: 11,
        //   image: require("@/assets/imgs/nick-1.png"),
        //   direction: "right",
        //   msg: "请问最近的车奖是怎么一回事？"
        // }
      ]
    }
  },
  methods: {
    TogetUserInfo () {
      http(GetUserInfo, null, json => {
        if (json.code === 0) {
          console.log(json)
          this.defaultimg = photoList[json.response.photo]
        }
      })
    },
    ToGetUserFeedBack () {
      http(GetUserFeedBack, null, json => {
        if (json.code === 0) {
        //  console.log(json.response)
          this.list = json.response
        }
      })
    },
    sendMessage () {
      http(AddUserFeedBack, { content: this.msg }, json => {
        if (json.code == 0) {
          this.list.push({
            id: 2,
            direction: 'right',
            date: '6mel,2020',
            image: require('@/assets/imgs/nick-1.png'),
            msg: this.msg
          })
          this.msg = ''
        } else {
          console.log('2')
        }
      })
    }
  },
  created () {
    this.TogetUserInfo()
    this.ToGetUserFeedBack()
    http(SetReadUserFeedBack, null, json => {
      if (json.code === 0) {

      }
    })
  }
}
</script>

<style lang="less" scope>
.feedback-body .innerScroll {
  /deep/.wrapper {
    // background: #ece5cc;
    max-height: calc(100vh - 700px) !important;;
    .bscroll-container {
      min-height: calc(100vh - 400px) !important;
    }
  }
}
.feedback-body {
  margin-top: 20px;
  width: 100%;
  z-index: 999;
  border-radius: 50px 50px 0 0;
  padding: 0px 30px 60px 30px;
  min-height: calc(100vh - 400px);
  .chat_bd {
    // padding-bottom: 200px;
    .chat_time {
      text-align: center;
      color: #fff;
      font-size: 35px;
      margin-bottom: 20px;
    }
    .chat_item {
      overflow: hidden;
      font-size: 38px;
      padding: 10px 0;
      img {
        width: 132px;
        // height: 154px;
      }
      .chat_left_img {
        margin-right: 40px;
        float: left;
      }
      .chat_left_msg {
        margin-top: 30px;
        float: left;
        width: calc(100vw - 400px);
        line-height: 54px;
        background: #fff;
        border-radius: 20px;
        position: relative;
        color: #6f6d72;
        padding: 20px;

        &::before {
          content: "";
          display: block;
          position: absolute;
          left: -30px;
          top: 32px;
          width: 0;
          border: 15px solid;
          border-color: transparent #fff transparent transparent;
        }
      }
      .chat_right_img {
        margin-left: 40px;
        float: right;
      }
      .chat_right_msg {
        padding: 20px;
        float: right;
        width: calc(100vw - 400px);
        margin-top: 30px;
        line-height: 54px;
        background-color: #77d9d3;
        color: #040000;
        border-radius: 20px;
        position: relative;
        &::before {
          content: "";
          display: block;
          position: absolute;
          right: -30px;
          top: 32px;
          width: 0;
          border: 15px solid;
          border-color: transparent transparent transparent #77d9d3;
        }
      }
    }
  }
  .send-msg {
    display: flex;
    justify-content: center;
    align-items: center;
    padding-top: 20px;

    i {
      font-size: 96px;
      color: #fff;
      margin-right: 40px;
    }
    input {
      flex: 1;
      padding-left: 20px;
      border-radius: 20px;
      height: 108px;
      box-shadow: 1px 1px 1px #666;
    }
    button {
      width: 178px;
      height: 108px;
      text-align: center;
      color: #fff;
      background: #efb618;
      margin-left: 40px;
      border-radius: 20px;
      font-size: 40px;
    }
  }
}
</style>
