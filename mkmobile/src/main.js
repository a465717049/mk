// The Vue build version to load with the `import` command
// (runtime-only or standalone) has been set in webpack.base.conf with an alias.
import Vue from 'vue'
import App from './App'
import router from './router'
import fastClick from 'fastclick'
import VueLazyLoad from 'vue-lazyload'
import store from './store'
import 'util/mixin'
import 'lib-flexible/flexible'
import 'styles/reset.css'
import 'styles/common.css'
import 'styles/border.css'
import "components/vant/index"
import "styles/iconfont/iconfont.css"
import "styles/iconfont/iconfont1.css"

import waterfall from 'vue-waterfall2'

Vue.use(waterfall)
Vue.config.productionTip = false
fastClick.attach(document.body)
/* eslint-disable no-new */
new Vue({
  el: '#app',
  router,
  store,
  components: { App },
  template: '<App/>'
})
