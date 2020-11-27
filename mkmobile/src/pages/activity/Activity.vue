<template>
  <div class="activityWrap">
    <TopBar class="center-one-search" :option="topBarOption">
      活動報名
    </TopBar>
    <div class="innerWrap">
      <ul>
        <li>
          <div class="photoWrap">
            <img class="photo" src="../../assets/imgs/forgot1-img1.png" alt />
            <span>
              提示：
              <br />報名成功后，請注意查收短信回執。
            </span>
          </div>
        </li>
        <li>
          <p class="activityName">{{ activityName }}</p>
        </li>
        <li>
          <div class="title">聯係人</div>
          <input type="text" :v-model="contacts" />
        </li>
        <li>
          <div class="title">手機</div>
          <input type="text" :v-model="phone" />
        </li>
        <li>
          <div class="title">人數</div>
          <input type="text" :v-model="peopleNumber" />
        </li>
        <li>
          <div class="title">費用</div>
          <input type="text" :v-model="cost" />
        </li>
      </ul>
      <button class="next">確定參加</button>
    </div>
  </div>
</template>

<script>
import TopBar from "components/TopBar";
import headerImg from "../../assets/imgs/headerImg.png";

export default {
  components: {
    TopBar
  },
  data() {
    return {
      topBarOption: {
        iconLeft: "iconzhankai",
        iconRight: ""
      },
      contacts: null,
      phone: null,
      peopleNumber: null,
      cost: null,
      activityName: "惠州超五星酒店"
    };
  },
  mounted() {},
  methods: {}
};
</script>

<style lang="less" scoped>
.activityWrap {
  .innerWrap {
    width: 100vw;
    padding-bottom: 300px;
    overflow-y: scroll;
    height: calc(100vh - 260px);
    background-color: #ebeaf0;
    border-radius: 40px 40px 0 0;
    margin-top: 0px;
    padding-top: 90px;
    .activityName {
      font-size: 56px;
      line-height: 220px;
      font-weight: 600;
    }
    ul {
      width: 90%;
      margin: 0 auto;
      color: #6318c3;
      font-weight: 800;
      li {
        .title {
          color: #757c8f;
          font-size: 42px;
          margin: 22px 0;
          font-weight: 800;
          letter-spacing: 10px;
        }
        input {
          height: 128px;
          line-height: 128px;
          color: #191719;
          width: 100%;
          padding: 0 30px;
          border-radius: 20px;
          font-size: 42px;
          font-weight: 600;
          letter-spacing: 10px;
        }
        /deep/ .van-dropdown-menu__bar {
          height: 148px;
          line-height: 148px;
          color: #6f6d72;
          width: 100%;
          padding: 0 20px;
          border-radius: 20px;
        }
        /deep/ .van-ellipsis {
          font-size: 42px;
          color: #9d9d9f;
          font-weight: 700;
        }
        /deep/ .van-dropdown-menu__title {
          height: 148px;
          line-height: 148px;
          display: inline-block;
          width: 98%;
        }
        /deep/ .van-dropdown-menu__title::after {
          border: 0.1467rem solid;
          border-color: transparent transparent #dcdee0 #dcdee0;
          margin-top: -10px;
          top: 37%;
        }
        /deep/ .van-dropdown-menu__title--down::after {
          top: 55% !important;
        }
        /deep/ .van-dropdown-item__content {
          // height: 148px;
          // line-height: 148px;
          display: inline-block;
          width: 90%;
          border-radius: 0 0 40px 40px;
        }
        /deep/ .van-dropdown-item__content {
          left: 5%;
        }
        /deep/ .van-dropdown-item__option {
          height: 148px;
          line-height: 148px;
          padding: 0 80px;
          font-size: 42px;
        }
        /deep/ .van-dropdown-item__option--active {
          color: #6e21d1;
        }
        /deep/ .van-icon-success::before {
          color: #6e21d1;
        }

        .verification {
          display: flex;
          border-radius: 40px;
          overflow: hidden;
          padding: 20px 0;
          background-color: #fff;
          height: 148px;
          input {
            display: inline-block;
            width: 60%;
            box-sizing: border-box;
            height: 110px;
            border-radius: 0;
          }
          img {
            display: inline-block;
            width: 39%;
          }
        }
      }
    }
    .photoWrap {
      background-color: #fff;
      border-radius: 40px;
      padding: 40px;
      margin-bottom: 50px;
      display: flex;
      align-items: center;
      img {
        width: 150px;
        height: 110px;
        vertical-align: middle;
        margin-right: 60px;
      }
      span {
        display: inline-block;
        vertical-align: middle;
        font-size: 38px;
        color: #343434;
        font-weight: 700;
        flex: 1;
      }
    }
  }
  .searchWrap {
    padding: 60px;
  }
  .next {
    display: block;
    width: 90%;
    margin: 0 auto;
    background: #f5c148;
    border-radius: 40px;
    height: 128px;
    line-height: 128px;
    font-size: 42px;
    color: #fff;
    margin-top: 100px;
    position: relative;
    font-weight: 800;
    letter-spacing: 10px;
    a {
      color: #fff;
      letter-spacing: 10px;
    }
  }
}
</style>
