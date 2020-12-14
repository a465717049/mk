import Vue from 'vue'
import Vuex from 'vuex'
import home from './modules/home'
Vue.use(Vuex)
export default new Vuex.Store({
  state: {
    globalLoadingShow: false,
    topbarHeight: 0,
    bottomTabBarHeight: 0
  },

  mutations: {
    changeGlobalLoadingShow (state, flag) {
      state.globalLoadingShow = flag
    },
    changeTopbarHeight (state, height) {
      // console.log(height, 8989)
      state.topbarHeight = height
    },
    changeBottomTabBarHeight (state, height) {
      // console.log(height, 7777)
      state.bottomTabBarHeight = height
    }
  },
  modules: {
    home
  }
})
