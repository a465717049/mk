// 常量相关
export const accessToken = 'ACCESSTOKEN'
export const searchHistory = 'SEARCHHISTORY'// 搜索历史
export const experience = 'EXPERIENCE' // 体验馆
export const comment = 'COMMENT' // comment
export const createOrderFrom = 'CREATEORDERFROM'
export const goodsInfo = 'GOODSINFO' // 创建订单时商品详情需传的goodsItems
export const orderInfo = 'ORDERINFO' // 创建订单时商品的配送和优惠券及发票相关信息
export const invoiceInfo = 'INVOICEINFO' // 发票信息
export const goodsListData = 'GOODSLISTDATA'
export const cartGoods = 'CARTGOODS'// 在页面跳转之前购物车的数据（分页原因）
export const couponByGoods = 'COUPONBYGOODS' // 创建订单选择相关优惠券
export const logistics = 'LOGISTICS' // 物流订单
export const aftersale = 'AFTERSALE' // 售后
export const orderSn = 'ORDERSN' // 申请开票需要的订单号
export const searchorder = 'SEARCHORDER' // 订单搜索
export const retrunLogistics = 'RETRUNLOGISTICS' // 退货物流
export const immedPaymentMony = 'IMMEDPAYMENTMONY' // 支付金额
export const activityCategory = 'ACTIVITYCATEGORY' // 品类秒杀查看详情
export const fromRoute = 'FROMROUTE' // 编辑收入地址的地址来自哪里
export const invoiceAmount = 'INVOICEAMOUNT' // 发票金额
export const loginPro = 'LOGINPRO' // 登录页面之前的页面
export const IPHONE = /iphone/gi.test(window.navigator.userAgent)&&/html5plus/gi.test(window.navigator.userAgent)
export const Safari = /iphone/gi.test(window.navigator.userAgent)&&/safari/gi.test(window.navigator.userAgent)
export const IPHONEX = {
    // iPhone X、iPhone XS
    isIPhoneX: /iphone/gi.test(window.navigator.userAgent) && window.devicePixelRatio && window.devicePixelRatio === 3 && window.screen.width === 375 && window.screen.height === 812,
    // iPhone XS Max
    isIPhoneXSMax: /iphone/gi.test(window.navigator.userAgent) && window.devicePixelRatio && window.devicePixelRatio === 3 && window.screen.width === 414 && window.screen.height === 896,
    // iPhone XR
    isIPhoneXR: /iphone/gi.test(window.navigator.userAgent) && window.devicePixelRatio && window.devicePixelRatio === 2 && window.screen.width === 414 && window.screen.height === 896
}
import head01 from '@/assets/imgs/set/head01.png'
import head02 from '@/assets/imgs/set/head02.png'
import head03 from '@/assets/imgs/set/head03.png'
import head04 from '@/assets/imgs/set/head04.png'
import head05 from '@/assets/imgs/set/head05.png'
import head06 from '@/assets/imgs/set/head06.png'
import head07 from '@/assets/imgs/set/head07.png'
import head08 from '@/assets/imgs/set/head08.png'
import head09 from '@/assets/imgs/set/head09.png'

import head010 from '@/assets/imgs/set/head10.png'
import head011 from '@/assets/imgs/set/head11.png'
import head012 from '@/assets/imgs/set/head12.png'
import head013 from '@/assets/imgs/set/head13.png'
import head014 from '@/assets/imgs/set/head14.png'
import head015 from '@/assets/imgs/set/head15.png'
import head016 from '@/assets/imgs/set/head16.png'
import head017 from '@/assets/imgs/set/head17.png'
import head018 from '@/assets/imgs/set/head18.png'
import head019 from '@/assets/imgs/set/head19.png'

import head020 from '@/assets/imgs/set/head20.png'
import head021 from '@/assets/imgs/set/head21.png'
import head022 from '@/assets/imgs/set/head22.png'
import head023 from '@/assets/imgs/set/head23.png'
import head024 from '@/assets/imgs/set/head24.png'
import head025 from '@/assets/imgs/set/head25.png'
import head026 from '@/assets/imgs/set/head26.png'
import head027 from '@/assets/imgs/set/head27.png'
import head028 from '@/assets/imgs/set/head28.png'
import head029 from '@/assets/imgs/set/head29.png'

import head030 from '@/assets/imgs/set/head30.png'
import head031 from '@/assets/imgs/set/head31.png'
import head032 from '@/assets/imgs/set/head32.png'
import head033 from '@/assets/imgs/set/head33.png'
import head034 from '@/assets/imgs/set/head34.png'
import head035 from '@/assets/imgs/set/head35.png'
import head036 from '@/assets/imgs/set/head36.png'
import head037 from '@/assets/imgs/set/head37.png'
import head038 from '@/assets/imgs/set/head38.png'
import head039 from '@/assets/imgs/set/head39.png'

import head040 from '@/assets/imgs/set/head40.png'
import head041 from '@/assets/imgs/set/head41.png'
import head042 from '@/assets/imgs/set/head42.png'
import head043 from '@/assets/imgs/set/head43.png'
import head044 from '@/assets/imgs/set/head44.png'
import head045 from '@/assets/imgs/set/head45.png'
import head046 from '@/assets/imgs/set/head46.png'
import head047 from '@/assets/imgs/set/head47.png'
import head048 from '@/assets/imgs/set/head48.png'
import head049 from '@/assets/imgs/set/head49.png'

import head050 from '@/assets/imgs/set/head50.png'
import head051 from '@/assets/imgs/set/head51.png'
import head052 from '@/assets/imgs/set/head52.png'
import head053 from '@/assets/imgs/set/head53.png'
import head054 from '@/assets/imgs/set/head54.png'
import head055 from '@/assets/imgs/set/head55.png'
import head056 from '@/assets/imgs/set/head56.png'
import head057 from '@/assets/imgs/set/head57.png'
import head058 from '@/assets/imgs/set/head58.png'
import head059 from '@/assets/imgs/set/head59.png'
import head060 from '@/assets/imgs/set/head60.png'

export const photoList=
{
    head01:head01,
    head02:head02,
    head03:head03,
    head04:head04,
    head05:head05,
    head06:head06,
    head07:head07,
    head08:head08,
    head09:head09,

    head010:head010,
    head011:head011,
    head012:head012,
    head013:head013,
    head014:head014,
    head015:head015,
    head016:head016,
    head017:head017,
    head018:head018,
    head019:head019,

    head020:head020,
    head021:head021,
    head022:head022,
    head023:head023,
    head024:head024,
    head025:head025,
    head026:head026,
    head027:head027,
    head028:head028,
    head029:head029,

    head030:head030,
    head031:head031,
    head032:head032,
    head033:head033,
    head034:head034,
    head035:head035,
    head036:head036,
    head037:head037,
    head038:head038,
    head039:head039,

    head040:head040,
    head041:head041,
    head042:head042,
    head043:head043,
    head044:head044,
    head045:head045,
    head046:head046,
    head047:head047,
    head048:head048,
    head049:head049,

    head050:head050,
    head051:head051,
    head052:head052,
    head053:head053,
    head054:head054,
    head055:head055,
    head056:head056,
    head057:head057,
    head058:head058,
    head059:head059,
    head060:head060




}