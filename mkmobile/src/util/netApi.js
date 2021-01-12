// 版本号
const v = "";
/**
 * 接口文件，按需加载某个/多个接口
 * url     接口连接
 * method  调用方法
 * version 接口版本号
 * join    是否将参数连接到url上，参数是字符串数组
 */
// 登录
export const getLogin = {
  url: "Login/Login",
  method: "POST",
  version: v,
  string: true
};
// ep记录
export const getEpexchange = {
  url: "ep/GetEPExchangeList",
  method: "POST",
  version: v,
  string: true
};

export const GetEPRecordLists = {
  url: "ep/GetEPRecordLists",
  method: "POST",
  version: v,
  string: true
};
// ep转出
export const EPToEexchange = {
  url: "ep/EPToEexchange",
  method: "POST",
  version: v,
  string: true
};
export const checkEPUser = {
  url: "userinfo/checkEPUser",
  method: "POST",
  version: v,
  string: true
};
// rp转出
export const RPToEexchange = {
  url: "RP/RPToEexchange",
  method: "POST",
  version: v,
  string: true
};
// 互转
export const TransOtherType = {
  url: "EP/TransOtherType",
  method: "POST",
  version: v,
  string: true
};
// 出售EP
export const EPSell = {
  contentType: "application/json",
  url: "EP/EPSell",
  method: "POST",
  version: v,
  string: true
};

export const GetEpSellWeb = {
  contentType: "application/json",
  url: "EP/GetEpSellWeb",
  method: "POST",
  version: v,
  string: true
};

// 出售股票
export const SellStock = {
  contentType: "application/json",
  url: "manure/SellStock",
  method: "POST",
  version: v,
  string: true
};
// 获取EP交易
export const GetEPRecordList = {
  contentType: "application/json",
  url: "EP/GetEPRecordList",
  method: "POST",
  version: v,
  string: true
};
// 任务领取
export const GetDPETask = {
  contentType: "application/json",
  url: "DPETask/GetDPETask",
  method: "POST",
  version: v,
  string: true
};
// 获取用户信息
export const GetUserInfo = {
  contentType: "application/json",
  url: "UserInfo/GetUserInfo",
  method: "POST",
  version: v
};
// 设置钱包地址
export const SetUserlocationweb = {
  contentType: "application/json",
  url: "User/SetUserlocationweb",
  method: "POST",
  version: v,
  string: true
};

export const UpdateLevelWeb = {
  contentType: "application/json",
  url: "User/UpdateLevelWeb",
  method: "POST",
  version: v,
  string: true
};

// 修改密码
export const SetUpdatePassword = {
  contentType: "application/json",
  url: "User/UpdatePassword",
  method: "POST",
  version: v,
  string: true
};
// 滚屏公告
export const GetBanner = {
  contentType: "application/json",
  url: "Banner/GetBanner",
  method: "POST",
  version: v,
  string: true
};
// 商品列表
export const GetShopList = {
  contentType: "application/json",
  url: "Shop/GetShopList",
  method: "POST",
  version: v,
  string: true
};
//商品详细
export const GetShopDeatilList = {
  contentType: "application/json",
  url: "Shop/GetShopDeatilList",
  method: "POST",
  version: v,
  string: true
};
//商品模糊查询
export const GetShopDeatilLike = {
  contentType: "application/json",
  url: "Shop/GetShopDeatilLike",
  method: "POST",
  version: v,
  string: true
};
//购买商品
export const BuyGoodsweb = {
  contentType: "application/json",
  url: "Shop/BuyGoodsweb",
  method: "POST",
  version: v,
  string: true
};

export const BuyGoodsbyweb = {
  contentType: "application/json",
  url: "Shop/BuyGoodsbyweb",
  method: "POST",
  version: v,
  string: true
};


//查找好友
export const GetFriendsList = {
  contentType: "application/json",
  url: "Relation/GetFriendsList",
  method: "POST",
  version: v,
  string: true
};
export const GetFriendsListJid = {
  contentType: "application/json",
  url: "Relation/GetFriendsListJid",
  method: "POST",
  version: v,
  string: true
};
//查看某人资料
export const GetFriendsListbyId = {
  contentType: "application/json",
  url: "Relation/GetFriendsListbyId",
  method: "POST",
  version: v,
  string: true
};
//查找家人
export const GetSearchFimaly = {
  contentType: "application/json",
  url: "Relation/GetFimalyList",
  method: "POST",
  version: v,
  string: true
};

//朋友關係
export const GetSearchRelation = {
  contentType: "application/json",
  url: "Relation/GetRelationList",
  method: "POST",
  version: v,
  string: true
};
//用户反馈
export const GetUserFeedBack = {
  contentType: "application/json",
  url: "User/GetUserFeedBack",
  method: "POST",
  version: v,
  string: true
};
//新增用户反馈
export const AddUserFeedBack = {
  contentType: "application/json",
  url: "User/AddUserFeedBack",
  method: "POST",
  version: v,
  string: true
};
//SetReadUserFeedBack GetReadUserFeedBack
export const SetReadUserFeedBack = {
  contentType: "application/json",
  url: "User/SetReadUserFeedBack",
  method: "POST",
  version: v,
  string: true
};

export const GetReadUserFeedBack = {
  contentType: "application/json",
  url: "User/GetReadUserFeedBack",
  method: "POST",
  version: v,
  string: true
};
//价格曲线
export const GetStockPriceTrend = {
  contentType: "application/json",
  url: "StockPriceTrend/GetStockPriceTrend",
  method: "POST",
  version: v,
  string: true
};
//股票记录
export const GetBuyDPEHistory = {
  contentType: "application/json",
  url: "DPE/GetBuyDPEHistory",
  method: "POST",
  version: v,
  string: true
};

//排队记录
export const GetBuyDPEList = {
  contentType: "application/json",
  url: "DPE/GetBuyDPEList",
  method: "POST",
  version: v,
  string: true
};
//谷歌验证
export const SetAuthenticator = {
  contentType: "application/json",
  url: "User/SetAuthenticator",
  method: "POST",
  version: v,
  string: true
};
//请求apple数据
export const GetSerarchApple = {
  contentType: "application/json",
  url: "DPE/GetSerarchApple",
  method: "POST",
  version: v,
  string: true
};
//dpelist
export const GetDPEEexchange = {
  contentType: "application/json",
  url: "DPE/GetDPEEexchange",
  method: "POST",
  version: v,
  string: true
};
//split记录
export const GetSplitRecords = {
  contentType: "application/json",
  url: "SP/GetSplitRecords",
  method: "POST",
  version: v,
  string: true
};
//谷歌生成base32
export const SetAuthenticatorGooglekey = {
  contentType: "application/json",
  url: "User/SetAuthenticatorGooglekey",
  method: "POST",
  version: v
};
//谷歌校验确认
export const CheckGoogleKey = {
  contentType: "application/json",
  url: "User/CheckGoogleKey",
  method: "POST",
  version: v,
  string: true
};
//公告消息
export const GetNewsWeb = {
  contentType: "application/json",
  url: "News/GetNewsWeb",
  method: "POST",
  version: v,
  string: true
};
//公告消息byid
export const GetNewsDeatilWeb = {
  contentType: "application/json",
  url: "News/GetNewsDeatilWeb",
  method: "POST",
  version: v,
  string: true
};
//创建新账户
export const CreateNewAccount = {
  contentType: "application/json",
  url: "User/CreateNewAccount",
  method: "POST",
  version: v,
  string: true
};
//创建新账户
export const CreateSubAccount = {
  contentType: "application/json",
  url: "User/CreateSubAccount",
  method: "POST",
  version: v,
  string: true
};
//交易密碼確認
export const CheckUpwd = {
  contentType: "application/json",
  url: "User/CheckUpwd",
  method: "POST",
  version: v,
  string: true
};
//獲取待售信息
export const GetEPExchangeById = {
  contentType: "application/json",
  url: "EP/GetEPExchangeById",
  method: "POST",
  version: v,
  string: true
};
//購買EP
export const EPBuy = {
  contentType: "application/json",
  url: "EP/EPBuy",
  method: "POST",
  version: v,
  string: true
};
//支付EP
export const EPPay = {
  contentType: "application/json",
  url: "EP/EPPay",
  method: "POST",
  version: v,
  string: true
};
//支付EP
export const EPFinish = {
  contentType: "application/json",
  url: "EP/EPFinish",
  method: "POST",
  version: v,
  string: true
};
//获得我的问题
export const GetMyQuestion = {
  contentType: "application/json",
  url: "Question/GetMyQuestion",
  method: "POST",
  version: v,
  string: true
};
export const GetShouru = {
  contentType: "application/json",
  url: "UserInfo/GetShouru",
  method: "POST",
  version: v,
  string: true
};
export const getVerificationCode = {
  contentType: "application/json",
  url: "Update/getVerificationCode",
  method: "POST",
  version: v,
  string: true
};
export const checkUser = {
  contentType: "application/json",
  url: "UserInfo/checkUser",
  method: "POST",
  version: v,
  string: true
};
export const GetQuestionListByID = {
  contentType: "application/json",
  url: "Question/GetQuestionListByID",
  method: "POST",
  version: v,
  string: true
};
export const GetCheckQuestionAll = {
  contentType: "application/json",
  url: "Question/CheckQuestionAll",
  method: "POST",
  version: v,
  string: true
};
export const GetUpdateHeadImageAndNickName = {
  contentType: "application/json",
  url: "User/UpdateHeadImageAndNickName",
  method: "POST",
  version: v,
  string: true
};

export const Getupdatebankinfo = {
  contentType: "application/json",
  url: "User/updatebankinfo",
  method: "POST",
  version: v,
  string: true
};

export const GetAllQuestionList = {
  contentType: "application/json",
  url: "Question/GetAllQuestionList",
  method: "POST",
  version: v
};
export const GetSetQuestionList = {
  contentType: "application/json",
  url: "Question/SetQuestionList",
  method: "POST",
  version: v,
  string: true
};
//一键归集
export const GetOneKeyReturn = {
  contentType: "application/json",
  url: "User/OneKeyReturn",
  method: "POST",
  version: v,
  string: true
};

//购物车
export const AddGoodsweb = {
  contentType: "application/json",
  url: "Shop/AddGoodsweb",
  method: "POST",
  version: v,
  string: true
};

export const GetShopSkuList = {
  contentType: "application/json",
  url: "Shop/GetShopSkuList",
  method: "POST",
  version: v,
  string: true
};


export const GetShopSkuDetailList = {
  contentType: "application/json",
  url: "Shop/GetShopSkuDetailList",
  method: "POST",
  version: v,
  string: true
};


export const ApplyOpenShop = {
  contentType: "application/json",
  url: "Shop/ApplyOpenShop",
  method: "POST",
  version: v,
  string: true
};

//购物车数量
export const GetShopCartsweb = {
  url: "Shop/GetShopCartsweb",
  method: "POST",
  version: v,
  string: true
};
export const GetShopCartsbyweb = {
  url: "Shop/GetShopCartsbyweb",
  method: "POST",
  version: v,
  string: true
};


//购物车数量
export const GetShopaddr = {
  contentType: "application/json",
  url: "Shop/GetShopaddr",
  method: "POST",
  version: v,
  string: true
};
//GetMyShopList
export const GetMyShopList = {
  contentType: "application/json",
  url: "Shop/GetMyShopList",
  method: "POST",
  version: v,
  string: true
};
export const GetShopDetailsMyweb = {
  contentType: "application/json",
  url: "Shop/GetShopDetailsMyweb",
  method: "POST",
  version: v,
  string: true
};
