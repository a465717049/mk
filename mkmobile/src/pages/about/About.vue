<template>
  <div class="aboutWrapper">
    <TopBar class="center-one-search">关于摩奇猴</TopBar>
    <div class="innerWrap">
      <!-- <div class="imgwrap"> -->
      <!-- <img src="@/assets/imgs/aboutImg.png" class="img" alt /> -->
      <!-- </div> -->
      <img src="@/assets/imgs/aboutHead.png" alt class="topHead" />
      <p class="info">用“大胆、开放、积极、有趣”的态度，帮助用户寻找人生的积极体验 、挖掘自我潜能、赋予生命能量，做“有趣的灵魂”，构建快乐、有趣、有意义、有价值的健康生活。</p>
      <p
        class="info"
      >健康不一定是沉闷的老年式养生，也可以是像孩子那样，对生活充满好奇，始终保持对生命生生不息的探索。MOKI摩奇猴用大胆配色突出健康，塑造有活力的生命感、年轻感，致力于成为大健康产业里一股年轻力量，致力于打造国民时尚健康潮牌。</p>
      <div class="qrwrap">
        <img src="@/assets/imgs/qr.png" alt class="qr" />
        <span>下载APP应用</span>
      </div>
    </div>
  </div>
</template>
<script type="text/javascript">
import TopBar from 'components/TopBar'
import headerImg from '../../assets/imgs/headerImg.png'
export default {
  data () {
    return {
      topBarOption: {
        iconLeft: 'back',
        iconRight: ''
        // image: headerImg
      },
      list: []
    }
  },
  components: {
    TopBar
  },
  mounted () {}
}
</script>

<style lang="less" scoped>
.aboutWrapper {
  .innerWrap {
    width: 100vw;
    height: calc(100vh - 300px);
    overflow: auto;
    padding: 0 54px;
    // .imgwrap{
    //   width: 100%;
    //   border-radius: 20px;
    //   margin-bottom:200px;
    //   overflow: hidden;
    //   img{
    //     width: 100%;
    //   }
    // }
    .topHead {
      height: 150px;
      margin-bottom: 86px;
    }
    .info {
      font-size: 40px;
      font-weight: bold;
      color: #ffffff;
      line-height: 60px;
      margin-bottom: 60px;
    }
    .qrwrap {
      display: flex;
      flex-direction: column;
      align-items: center;
      .qr {
        width: 354px;
        margin-bottom: 60px;
      }
      span {
        font-size: 38px;
        font-weight: bold;
        color: #ffffff;
      }
    }
  }
}
</style>
