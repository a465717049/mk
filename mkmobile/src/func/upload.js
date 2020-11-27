// import axios from 'axios'
import { storage } from 'util/storage.js'
import { accessToken } from 'util/const.js'
import { config } from 'util/config' // 图片路径
export const uploadPic = (e, fn) => {
  let files = e.target.files
  let formData = new FormData()
  formData.append('file', files[0])
  let header = {
    headers: {
      'Content-Type': 'multipart/form-data',
      'Authorization': storage.getLocalStorage(accessToken)
    }
  }
  return fn(config.baseUrl + 'file/upload', formData, header)
}
