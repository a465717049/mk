<template>
  <div class="secretQuestionWrap">
    <TopBar class="center-one-search" :option="topBarOption">
     安全问题
    </TopBar>
    <div class="innerWrap">
      <ul>
        <li>
          <div class="photoWrap">
            <img class="photo" src="../../assets/imgs/forgot1-img1.png" alt />
            <span>
              提示：
              <br />您将设置三个问题及答案，以后在多场合会 涉及到密保验证。
            </span>
          </div>
        </li>
        <li>
          <div class="inputWrap">
            <van-dropdown-menu>
              <van-dropdown-item v-model="question1" :options="questionList" />
            </van-dropdown-menu>
            <input type="text" />
          </div>
        </li>
        <li>
          <div class="inputWrap">
            <van-dropdown-menu>
              <van-dropdown-item v-model="question2" :options="questionList" />
            </van-dropdown-menu>
            <input type="text" />
          </div>
        </li>
        <li>
        <div class="inputWrap">
            <van-dropdown-menu>
              <van-dropdown-item v-model="question3" :options="questionList" />
            </van-dropdown-menu>
            <input type="text" />
          </div>
        </li>
      </ul>

      <button class="bgPurple">
        <router-link to="verification" class="router">提交</router-link>
      </button>
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
        iconLeft: "back",
        iconRight: "",
       // image: headerImg
      },
      question1: 0,
      question2: 1,
      question3: 2,
      questionList: [
        { text: "您的出生地？", value: 0 },
        { text: "您小学时候班主任姓名？", value: 1 },
        { text: "您最喜欢的一部电影？", value: 2 }
      ]
    };
  },
  mounted() {},
  methods: {}
};
</script>

<style lang="less" scoped>
.secretQuestionWrap {
  .innerWrap {
    width: 100vw;
    padding-bottom: 300px;
    overflow-y: scroll;
    height: calc(100vh - 260px);
    background-color: #ebeaf0;
    border-radius: 40px 40px 0 0;
    margin-top: -20px;
    padding-top: 90px;
    ul {
      width: 90%;
      margin: 0 auto;
      li {
        margin-bottom: 26px;
        .title {
          color: #333;
          font-size: 42px;
          margin: 42px 0;
          font-weight: 800;
          letter-spacing: 10px;
        }

        .inputWrap {
          padding: 20px;
          border-radius: 20px;
          background-color: #fff;
          input {
            height: 128px;
            line-height: 128px;

            color: #9d9d9f;
            width: 100%;
            padding: 0 30px;
            border-radius: 30px;
            font-size: 42px;
            font-weight: 600;
            letter-spacing: 10px;
            box-shadow: 0px 0px 5px 0px rgba(0, 0, 0, 0.24);
            border-radius: 30px;
            margin-top: 20px;
          }
        }
        /deep/ .van-dropdown-menu__bar {
          height: 128px;
          line-height: 128px;
          color: #6f6d72;
          box-shadow: 0px 0px 5px 0px rgba(0, 0, 0, 0.24);
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
      // height: 222px;
      background-color: #fff;
      border-radius: 40px;
      padding: 30px;
      display: flex;
      align-items: center;
      margin-bottom: 60px;
      img {
        width: 150px;
        height: 110px;
        vertical-align: middle;
        margin-right: 60px;
      }
      span {
        flex: 1;
        display: inline-block;
        vertical-align: middle;
        font-size: 38px;
        color: #343434;
        font-weight: 700;
      }
    }
  }
  .searchWrap {
    padding: 60px;
  }
  .bgPurple {
    display: block;
    width: 90%;
    margin: 0 auto;
    background: #6317c3;
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
      letter-spacing: 30px;
    }
  }
}
</style>
