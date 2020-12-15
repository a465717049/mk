import Vue from 'vue'
import Vuex from 'vuex'
import home from './modules/home'
Vue.use(Vuex)
export default new Vuex.Store({
  state: {
    globalLoadingShow: false,
    topbarHeight: 0,
    bottomTabBarHeight: 0,
    hasNoRead: true// 是否有未读消息
  },

  mutations: {
    changeGlobalLoadingShow (state, flag) {
      state.globalLoadingShow = flag
    },
    changeTopbarHeight (state, height) {
      state.topbarHeight = height
    },
    changeBottomTabBarHeight (state, height) {
      state.bottomTabBarHeight = height
    },
    changeRead (state, bool) {
      state.hasNoRead = bool
    }
  },
  modules: {
    home
  }
})
