import Vue from 'vue'
import Vuex from 'vuex'
import home from './modules/home'
Vue.use(Vuex)
export default new Vuex.Store({
  state:{
    globalLoadingShow:false,
  },

  mutations:{
    changeGlobalLoadingShow (state, flag) {
      //console.log(flag,1212)
      state.globalLoadingShow = flag
    }
  },
  modules: {
    home
  },
})
