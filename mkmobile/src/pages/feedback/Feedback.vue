<template>
  <div>
    <TopBar class="center-one-search" :option="topBarOption">反饋</TopBar>
    <ScrollRefresh
      @getData="ToGetUserFeedBack"
      :residualHeight="140"
      :isNeedUp="false"
      class="innerScroll"
    >
      <div class="feedback-body">
        <ul class="chat_bd">
          <li v-for="item in list" :key="item.id">
            <div class="chat_time" v-if="item.direction === 'center'">{{ item.time }}</div>
            <div v-else-if="item.direction === 'left'" class="chat_item">
              <img class="chat_left_img" :src="item.image" />
              <div class="chat_left_msg">{{ item.msg }}</div>
            </div>
            <div v-else class="chat_item chat-right">
              <img class="chat_right_img" :src="item.image" />
              <div class="chat_right_msg">{{ item.msg }}</div>
            </div>
          </li>
        </ul>
        <div class="send-msg">
          <i class="iconfont iconyuyin"></i>
          <input type="text" v-model="msg" />
          <button type="primary" @click="sendMessage">发送</button>
        </div>
      </div>
    </ScrollRefresh>
  </div>
</template>

<script>
import TopBar from "components/TopBar";
import { http } from "util/request";
import { GetUserFeedBack, AddUserFeedBack } from "util/netApi";
import { storage } from "util/storage";
import ScrollRefresh from "components/ScrollRefresh";
import { accessToken, loginPro } from "util/const.js";

export default {
  components: {
    TopBar,
    ScrollRefresh
  },
  data() {
    return {
      msg: "",
      topBarOption: {
        iconLeft: "iconzhankai",
        iconRight: ""
      },
      list: [
        //  {
        //    id: 1,
        //    direction: 'center',
        //    time: '昨天  22:18',
        //    image: ''
        //  },
        //  {
        //    id: 2,
        //    direction: 'left',
        //    date: '6mel,2020',
        //    image: require('@/assets/imgs/nick-1.png'),
        //    msg: '请问有什么可以帮到您'
        //   },
        //   {
        //     id: 3,
        //     image: require('@/assets/imgs/nick-1.png'),
        //    direction: 'right',
        //     msg:
        //      '请问最近的车奖是怎么一回事？？色极为关键是南方健身房是的封建时代和封建士大夫十分'
        //   }
      ]
    };
  },
  methods: {
    ToGetUserFeedBack() {
      http(GetUserFeedBack, null, json => {
        if (json.code === 0) {
          console.log(json.response);
          this.list = json.response;
        }
      });
    },
    sendMessage() {
      http(AddUserFeedBack, { content: this.msg }, json => {
        if (json.code == 0) {
          this.list.push({
            id: 2,
            direction: "right",
            date: "6mel,2020",
            image: require("@/assets/imgs/nick-1.png"),
            msg: this.msg
          });
          this.msg = "";
        } else {
          console.log("2");
        }
      });
    }
  },
  created() {
    this.ToGetUserFeedBack();
  }
};
</script>

<style lang="less" scope>
.innerScroll {
  /deep/.wrapper {
    background: #ece5cc;
    .bscroll-container {
      min-height: calc(100vh - 400px) !important;
    }
  }
}
.feedback-body {
  margin-top: 20px;
  width: 100%;
  // overflow: scroll;

  z-index: 999;
  border-radius: 50px 50px 0 0;
  padding: 98px 60px 60px 60px;
  min-height: calc(100vh - 400px);
  .chat_bd {
    height: calc(100vh - 850px);
    .chat_time {
      text-align: center;
      color: #6f6d72;
      font-size: 35px;
      margin-bottom: 20px;
    }
    .chat_item {
      overflow: hidden;
      font-size: 38px;
      img {
        width: 146px;
        height: 156px;
      }
      .chat_left_img {
        margin-right: 40px;
        float: left;
      }
      .chat_left_msg {
        margin-top: 30px;
        float: left;
        width: calc(100vw - 450px);
        min-height: 106px;
        background: #fff;
        border-radius: 20px;
        position: relative;
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
        margin-top: 30px;
        float: right;
        width: calc(100vw - 450px);
        min-height: 106px;
        background-color: #6318c3;
        color: #fff;
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
          border-color: transparent transparent transparent #6318c3;
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
      color: #6318c3;
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
      background: #6318c3;
      margin-left: 40px;
      border-radius: 20px;
      font-size: 40px;
    }
  }
}
</style>
