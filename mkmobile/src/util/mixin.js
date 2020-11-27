import Vue from 'vue'
Vue.mixin({
  data () {
    return {
      isEnter: false,
      showComfirm: false
    }
  },
  methods: {
    clickOverpay (val) {
      this.isEnter = val
      this.showComfirm = val
    }
  }
})