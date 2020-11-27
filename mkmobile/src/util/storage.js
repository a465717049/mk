import {
  IPHONE
} from './const.js' 
var st
 if(IPHONE){
    st=plus.storage
    // console.log("IPHONE storage")
  }else{
    st=localStorage
  }
const storage = {
 
  // 设置本地存储
  setLocalStorage (name, value) {
    const curTime = new Date().getTime() + 1000 * 60 * 60 * 24 * 1
    // console.log("IPHONE storage:"+name+"----------   "+value)
    st.setItem(name, JSON.stringify({
      data: value,
      time: curTime
    }))
  },
  // 获取本地存储
  getLocalStorage (name) {
    const data = st.getItem(name)
    const dataObj = JSON.parse(data)
    // console.log("IPHONE storage:"+name+"----------   "+data)
    if (dataObj) {
      // if (new Date().getTime() - dataObj.time > 0) {
      //   st.removeItem(name)
      //   return null
      // } else {
        return dataObj.data
     // }
    }
  },
  // 获取本地存储没有指定存储时间
  getLocalStorageLong (name) {
    const data = st.getItem(name)
    const dataObj = JSON.parse(data)
    if (dataObj) {
      return dataObj.data
    } else {
      return null
    }
  },
  // 删除本地存储
  delLocalStorage (name) {
    st.removeItem(name)
  }
}

export { storage }
