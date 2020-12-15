<template>
  <div class="newsDetailsWrapper">
    <TopBar :option="topBarOption" class="center-one-search">
      活动详情
    </TopBar>
    <div class="innerWrap">
      <div class="w90">
        <div class="top">
          <p class="detailsTile">{{detailsTile}}</p>
          <div class="bottom">
            <div class="times">{{timeZone}} ·</div>
            <div class="ago">{{time}}</div>
            <div class="eyes">
              <i class="iconfont iconeye"></i>
              <span>{{seeNumber}}</span>
            </div>
          </div>
        </div>
        <div class="banner">
          <img :src="banner" alt />
        </div>
        <div class="article">
          <p>And the students are moving ahead: They're building a business portal and have designed an online toy shopping mall on an infant education forum.</p>
          <p>And the students are moving ahead: They're building a business portal and have designed an online toy shopping mall on an infant education forum.</p>
          <p>And the students are moving ahead: They're building a business portal and have designed an online toy shopping mall on an infant education forum.</p>
          <p>And the students are moving ahead: They're building a business portal and have designed an online toy shopping mall on an infant education forum.</p>
          <p>And the students are moving ahead: They're building a business portal and have designed an online toy shopping mall on an infant education forum.</p>
          <p>And the students are moving ahead: They're building a business portal and have designed an online toy shopping mall on an infant education forum.</p>
          <p>And the students are moving ahead: They're building a business portal and have designed an online toy shopping mall on an infant education forum.</p>
        </div>
      </div>
    </div>
  </div>
</template>
<script type="text/javascript">
import TopBar from "components/TopBar";
import banner from "../../assets/imgs/banner.png"

export default {
  data() {
    return {
      topBarOption: {
        iconLeft: "iconmenu2",
        iconRight: ""
      },
      detailsTile:"惠州超五星酒店周末极奢晚宴1晚惠州超五星酒店周末极奢晚宴1晚惠州超五星酒店周末极奢晚宴1晚",
      timeZone:"The Seattle Times",
      time:"13小时前",
      seeNumber:"45,000",
      banner:banner,
    };
  },
  components: {
    TopBar
  },
  computed: {},
  methods: {}
};
</script>

<style lang="less" scoped>
.newsDetailsWrapper {
  height: 100vh;
  .innerWrap {
    width: 100vw;
    background-color: #ebeaf0;
    border-radius: 40px 40px 0 0;
    // margin-top: -160px;
    padding-top: 90px;
    height: calc(100vh - 500px);
    overflow: auto;
  }
  .w90 {
    width: 90%;
    margin: 0 auto;
  }
  .banner {
    width: 100%;
    margin: 80px 0;
    img {
      width: 100%;
    }
  }
  .top {
    p {
      color: #6318C3;
      font-size: 50px;
      font-weight: 800;
      height: 120px;
      line-height: 60px;
      overflow: hidden;
      text-overflow: ellipsis;
      display: -webkit-box;
      -webkit-line-clamp: 2;
      -webkit-box-orient: vertical;
    }
    .bottom {
      margin-top: 40px;
      .iconfont {
        color: #ff780b;
        vertical-align: middle;
      }
      .iconeye:before {
        font-size: 50px;
      }

      div {
        display: inline-block;
        font-size: 24px;
      }
      .eyes {
        float: right;
        color: #000;
        font-weight: 600;
      }
      .times {
        color: #000;
        font-weight: 600;
      }
    }
  }
  .article {
    p {
      font-size: 34px;
      color: #000;
      font-weight: 500;
      margin-bottom: 60px;
    }
  }
}
</style>
