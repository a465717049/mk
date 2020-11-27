import Vue from 'vue'
import Store from '../store'
import { Dialog  } from 'vant';

//console.log(Store,'Store')
const notice = {
  errorModal: (message, hideFn) => {
    Dialog.alert({
      message,
    }).then(() => {
      hideFn()
    });
  },
  loading: (text = 'loading') => {
    Store.commit("changeGlobalLoadingShow",true)
  },
  loadingHide: () => {
    setTimeout(() => {
      Store.commit("changeGlobalLoadingShow",false)
    }, 300);
    
  }, 
}

export default notice
