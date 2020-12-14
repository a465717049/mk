import Vue from 'vue'
import { mapState } from 'vuex'
Vue.mixin({
  data () {
    return {
      isEnter: false,
      showComfirm: false
    }
  },
  computed: mapState({
    topbarHeight: state => state.topbarHeight,
    bottomTabBarHeight: state => state.bottomTabBarHeight
  }),
  methods: {
    clickOverpay (val) {
      this.isEnter = val
      this.showComfirm = val
    }
  }
})
