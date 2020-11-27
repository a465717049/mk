const home = {
  state: {
    searchActive: false,
    tabbarIsFixed: false
  },
  mutations: {
    changeSearchActive (state, searchActive) {
      state.searchActive = searchActive
    },
    changeTabbarFixed (state, tabbarIsFixed) {
      state.tabbarIsFixed = tabbarIsFixed
    }
  }
}
export default home
