<template>
  <div>
    <div class="wrapper" ref="wrapper" :style="{height: scrollHeight}">
      <div class="bscroll-container">
        <!-- 刷新提示信息 -->
        <div class="top-tip">
          <span class="refresh-hook">{{pulldownMsg}}</span>
        </div>
        <!-- 滚动区域 -->
        <slot></slot>
        <!-- 底部提示信息 -->
        <div class="bottom-tip" v-show="isNone">
          <span class="loading-hook">{{pullupMsg}}</span>
        </div>
      </div>
    </div>

    <!-- alert提示刷新成功 -->
    <div class="alert-hook" :style="{display: alertHook}">刷新成功</div>
  </div>
</template>

<script>
import BScroll from 'better-scroll'

export default {
  mounted () {
    this.getHeight()
  },
  data () {
    return {
      pulldownMsg: '下拉刷新',
      pullupMsg: '加载更多',
      pullTopTip: false,
      pullBottomTip: false,
      alertHook: 'none',
      scrollHeight: 0, // 滚动区域的高度
      pageIndex: 1
    }
  },
  props: {
    residualHeight: {
      // 要减去的高度
      type: Number,
      required: true
    },
    active: {
      type: Number // 激活的tab栏
    },
    isNone: {
      type: Boolean, // 数据是否加载完毕
      default: false
    },
    isNeedDown: {
      type: Boolean, // 是否需要下拉刷新
      default: true
    },
    isNeedUp: {
      type: Boolean, // 是否需要上拉加载
      default: true
    }
  },
  methods: {
    getHeight () {
      let num = parseFloat(document.querySelector('html').style.fontSize)
      // console.log(num,'num')
      const windowHeight = document.body.clientHeight / num
      // console.log(windowHeight,9999)
      this.scrollHeight = windowHeight - this.residualHeight / num + 'rem'
      // console.log(this.scrollHeight,8888)
    },
    refreshalert () {
      // 刷新成功提示
      this.alertHook = 'block'
      setTimeout(() => {
        this.alertHook = 'none'
      }, 1000)
    }
  },
  watch: {
    active () {
      this.pageIndex = 1
    }
  },
  created () {
    const that = this
    this.$nextTick(() => {
      this.scroll = new BScroll(this.$refs.wrapper, {
        scrollY: true,
        click: true,
        pullUpLoad: this.isNeedUp, // 上拉加载更多
        bounce: {
          top: this.isNeedDown,
          bottom: this.isNeedUp
        }
      })

      // 滑动过程中事件
      this.scroll.on('scroll', pos => {
        if (pos.y > 30) {
          this.pulldownMsg = '释放立即刷新'
        }
      })
      // 滑动结束松开事件
      this.scroll.on('touchEnd', async pos => {
        // 上拉刷新
        if (pos.y > 30) {
          this.pageIndex = 1
          await this.$emit('getData', 1)
          that.pulldownMsg = '下拉刷新'
          // 刷新成功后提示
          that.refreshalert()
          // 刷新列表后，重新计算滚动区域高度
          that.scroll.refresh()
        } else if (pos.y < this.scroll.maxScrollY - 30) {
          // 下拉加载
          if (!this.isNone) {
            that.pullupMsg = '加载更多。。。'
            this.$emit('getData', ++this.pageIndex)
            // that.scroll.refresh();
          } else {
            that.pullupMsg = '没有更多了。。。'
          }
        }
      })
    })
  }
}
</script>
<style lang="less" scoped>
.wrapper {
  width: 100%;
  // height: 600px;
  overflow: hidden;
  position: relative;
  .bscroll-container {
    min-height: calc(100vh);
    // background-color: #c6d0de;
  }
  /* 下拉、上拉提示信息 */
  .top-tip {
    position: absolute;
    top: -100px;
    left: 0;
    z-index: 1;
    width: 100%;
    height: 100px;
    line-height: 100px;
    text-align: center;
    font-size: 42px;
    color: #555;
  }

  .bottom-tip {
    width: 100%;
    height: 100px;
    line-height: 100px;
    font-size: 42px;
    text-align: center;
    color: #777;
    position: absolute;
    bottom: -100px;
    left: 0;
  }
}
/* 全局提示信息 */
.alert-hook {
  display: none;
  position: fixed;
  top: 62px;
  left: 25vw;
  z-index: 9999;
  width: 100%;
  height: 100px;
  line-height: 100px;
  text-align: center;
  color: #fff;
  font-size: 30px;
  width: 50vw;
  border-radius: 20px;
  background: rgba(7, 17, 27, 0.5);
}
</style>
