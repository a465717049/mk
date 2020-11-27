import {
  config
} from 'util/config.js'
export const getList = (list) => {
  let listArr = []
  for (let i = 0; i < list.length; i++) {
    let arr = []
    let imgs = list[i].commentsPics.split(',')
    for (let j = 0; j < imgs.length; j++) {
      arr.push({
        msrc: config.imageUrl + imgs[j],
        src: config.imageUrl + imgs[j]
      })
      listArr[i] = arr
    }
  }
  return listArr
}
