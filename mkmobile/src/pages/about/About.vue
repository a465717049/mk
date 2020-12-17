<template>
  <div class="aboutWrapper">
    <TopBar class="center-one-search">
      关于摩奇猴
    </TopBar>
    <div class="innerWrap">
      <div class="imgwrap">
        <img src="@/assets/imgs/aboutImg.jpg" class="img" alt />
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
      list: [

      ]
    }
  },
  components: {
    TopBar
  },
  mounted () {
  }

}
</script>

<style lang="less" scoped>
.aboutWrapper {
  .innerWrap {
    width: 100vw;
    height: calc(100vh - 300px);
    overflow: scroll;
    padding: 0 40px;
    .imgwrap{
      width: 100%;
      border-radius: 20px;
      margin-bottom:200px;
      overflow: hidden;
      img{
        width: 100%;
      }
    }
  }
}
</style>
