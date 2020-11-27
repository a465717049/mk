<template>
  <div class="chargeAllWrapper">
    <TopBar class="center-one-search" :option="topBarOption">
      充值記錄
    </TopBar>
    <div class="innerWrap">
      <div class="chargeBox">
        <div class="listWrap" ref="listWrap">
          <ul>
            <li v-for="item in list" :key="item.id">
              <div class="left fl">{{ item.date }}</div>
              <div class="right fr">$ {{ item.price }}</div>
            </li>
          </ul>
        </div>
      </div>
    </div>
  </div>
</template>
<script type="text/javascript">
import TopBar from "components/TopBar";
import headerImg from "../../assets/imgs/headerImg.png";
import BScroll from "better-scroll";
export default {
  data() {
    return {
      topBarOption: {
        iconLeft: "back",
        iconRight: "",
        //image: headerImg
      },
      list: [
        
      ]
    };
  },
  components: {
    TopBar
  },
  mounted() {
    this.scrollInit();
  },
  computed: {},
  methods: {
    scrollInit() {
      if (!this.scroll) {
        this.scroll = new BScroll(this.$refs.listWrap, {
          scrollY: true,
          click: true,
          bounce: {
            top: true,
            bottom: true
          }
        });
      } else {
        this.scroll.refresh();
      }
    }
  }
};
</script>

<style lang="less" scoped>
.chargeAllWrapper {
  .innerWrap {
    width: 100vw;
    background-color: #ebeaf0;
    border-radius: 40px 40px 0 0;
    margin-top: -20px;
    padding-top: 30px;
  }

  .chargeBox {
    width: 96%;
    background-color: #fff;
    margin: 0 auto;
    margin-top: 16px;
    border-radius: 40px;
    padding-top: 30px;
    .header {
      height: 140px;
      line-height: 140px;
      padding: 0 52px;
      .left {
        color: #888;
        font-size: 40px;
        font-weight: 600;
      }
      .see {
        color: #fdae05;
      }
    }
    ul {
      padding: 0 52px 20px;
      li {
        height: 130px;
        line-height: 130px;
        background: #e8e8e8;
        margin-bottom: 20px;
        border-radius: 30px;
        padding: 0 20px;
        .left {
          font-size: 40px;
          color: #666;
        }
        .right {
          font-size: 50px;
          color: #23bb05;
          font-weight: 6000;
        }
      }
    }
    .listWrap {
      height: calc(100vh - 540px);
      overflow: hidden;
    }
  }
}
</style>
