import axios from 'axios';
import qs from 'qs';
// import router from '../routerManuaConfig'
import router from '../router/index'
import store from "../store";
import Vue from 'vue';

import applicationUserManager from "../Auth/applicationusermanager";

let base = 'https://api.a8dog.top'; //'https://api.snptw.cn' https://api.a8dog.top;

// 如果是IIS部署，用这个，因为 IIS 只能是 CORS 跨域，不能代理
// let base = process.env.NODE_ENV=="production"? 'http://localhost:8081':'';
export const configimgurl = 'https://api.a8dog.top/shopimg/';
export const dowmexcel = 'https://api.a8dog.top/shopimg/downinfo/';

// 请求延时
axios.defaults.timeout = 20000

var storeTemp = store;
axios.interceptors.request.use(
    config => {
        var curTime = new Date()
        var expiretime = new Date(Date.parse(storeTemp.state.tokenExpire))

        if (storeTemp.state.token && (curTime < expiretime && storeTemp.state.tokenExpire)) {
            // 判断是否存在token，如果存在的话，则每个http header都加上token
            config.headers.Authorization = "Bearer " + storeTemp.state.token;
        }

        saveRefreshtime();

        return config;
    },
    err => {
        return Promise.reject(err);
    }
);

// http response 拦截器
axios.interceptors.response.use(
    response => {
        return response;
    },
    error => {
        // 超时请求处理
        var originalRequest = error.config;
        if (error.code == 'ECONNABORTED' && error.message.indexOf('timeout') != -1 && !originalRequest._retry) {

            Vue.prototype.$message({
                message: '请求超时！',
                type: 'error'
            });

            originalRequest._retry = true
            return null;
        }

        if (error.response) {
            if (error.response.status == 401) {
                var curTime = new Date()
                var refreshtime = new Date(Date.parse(window.localStorage.refreshtime))
                // 在用户操作的活跃期内
                if (window.localStorage.refreshtime && (curTime <= refreshtime)) {
                    return refreshToken({
                        token: window.localStorage.Token
                    }).then((res) => {
                        if (res.success) {
                            Vue.prototype.$message({
                                message: 'refreshToken success! loading data...',
                                type: 'success'
                            });

                            store.commit("saveToken", res.response.token);

                            var curTime = new Date();
                            var expiredate = new Date(curTime.setSeconds(curTime.postSeconds() + res.response.expires_in));
                            store.commit("saveTokenExpire", expiredate);

                            error.config.__isRetryRequest = true;
                            error.config.headers.Authorization = 'Bearer ' + res.response.token;
                            return axios(error.config);
                        } else {
                            // 刷新token失败 清除token信息并跳转到登录页面
                            ToLogin()
                        }
                    });
                } else {
                    // 返回 401，并且不知用户操作活跃期内 清除token信息并跳转到登录页面
                    ToLogin()
                }

            }
            // 403 无权限
            if (error.response.status == 403) {
                Vue.prototype.$message({
                    message: '失败！该操作无权限',
                    type: 'error'
                });
                return null;
            }
            // 429 ip限流
            if (error.response.status == 429) {
                Vue.prototype.$message({
                    message: '刷新次数过多，请稍事休息重试！',
                    type: 'error'
                });
                return null;
            }
        }
        return ""; //返回接口返回的错误信息
    }
);


export const BaseApiUrl = '';

// 登录
export const requestLogin = params => {
    return axios.post(`${base}/api/login/login`, qs.stringify(params)).then(res => res.data);
};
export const requestLoginMock = params => {
    return axios.post(`${base}/login`, params).then(res => res.data);
};

export const refreshToken = params => {
    return axios.post(`${base}/api/login/RefreshToken`, {
        params: params
    }).then(res => res.data);
};

export const saveRefreshtime = params => {

    let nowtime = new Date();
    let lastRefreshtime = window.localStorage.refreshtime ? new Date(window.localStorage.refreshtime) : new Date(-1);
    let expiretime = new Date(Date.parse(window.localStorage.TokenExpire))

    let refreshCount = 1; //滑动系数
    if (lastRefreshtime >= nowtime) {
        lastRefreshtime = nowtime > expiretime ? nowtime : expiretime;
        lastRefreshtime.setMinutes(lastRefreshtime.postMinutes() + refreshCount);
        window.localStorage.refreshtime = lastRefreshtime;
    } else {
        window.localStorage.refreshtime = new Date(-1);
    }
};
const ToLogin = params => {

    store.commit("saveToken", "");
    store.commit("saveTokenExpire", "");
    store.commit("saveTagsData", "");
    window.localStorage.removeItem('user');
    window.localStorage.removeItem('NavigationBar');



    if (global.IS_IDS4) {
        applicationUserManager.login();
    } else {
        router.replace({
            path: "/login",
            query: {
                redirect: router.currentRoute.fullPath
            }
        });

        window.location.reload()
    }
};

export const getUserByToken = params => {
    return axios.get(`${base}/api/user/getInfoByToken`, {
        params: params
    }).then(res => res.data);
};




export function testapi2() {
    console.log('api is ok.')
}

export const testapi = pa => {
    console.log('api is ok.')
}

// 用户管理
export const getUserListPage = params => {
    return axios.get(`${base}/api/user/get`, {
        params: params
    });
};
export const removeUser = params => {
    return axios.delete(`${base}/api/user/delete`, {
        params: params
    });
};
export const editUser = params => {
    return axios.put(`${base}/api/user/put`, params);
};
export const addUser = params => {
    return axios.post(`${base}/api/user/post`, params);
};
export const batchRemoveUser = params => {
    return axios.delete(`${base}/api/Claims/BatchDelete`, {
        params: params
    }); //没做
};

// 角色管理
export const getRoleListPage = params => {
    return axios.get(`${base}/api/role/get`, {
        params: params
    });
};
export const removeRole = params => {
    return axios.delete(`${base}/api/role/delete`, {
        params: params
    });
};
export const editRole = params => {
    return axios.put(`${base}/api/role/put`, params);
};
export const addRole = params => {
    return axios.post(`${base}/api/role/post`, params);
};

// 接口模块管理
export const getModuleListPage = params => {
    return axios.get(`${base}/api/module/get`, {
        params: params
    });
};
export const removeModule = params => {
    return axios.delete(`${base}/api/module/delete`, {
        params: params
    });
};
export const editModule = params => {
    return axios.put(`${base}/api/module/put`, params);
};
export const addModule = params => {
    return axios.post(`${base}/api/module/post`, params);
};


// 菜单模块管理
export const getPermissionListPage = params => {
    return axios.post(`${base}/api/permission/get`, {
        params: params
    });
};
export const getPermissionTreeTable = params => {
    return axios.get(`${base}/api/permission/GetTreeTable`, {
        params: params
    });
};
export const removePermission = params => {
    return axios.delete(`${base}/api/permission/delete`, {
        params: params
    });
};
export const editPermission = params => {
    return axios.put(`${base}/api/permission/put`, params);
};
export const addPermission = params => {
    return axios.post(`${base}/api/permission/post`, params);
};
export const getPermissionTree = params => {
    return axios.get(`${base}/api/permission/getpermissiontree`, {
        params: params
    });
};
export const getPermissionIds = params => {
    return axios.get(`${base}/api/permission/GetPermissionIdByRoleId`, {
        params: params
    });
};

export const addRolePermission = params => {
    return axios.post(`${base}/api/permission/Assign`, params);
};
export const getNavigationBar = params => {
    return axios.get(`${base}/api/permission/GetNavigationBar`, {
        params: params
    }).then(res => res.data);
};

// Bug模块管理
export const getBugListPage = params => {
    return axios.post(`${base}/api/TopicDetail/get`, {
        params: params
    });
};
export const removeBug = params => {
    return axios.delete(`${base}/api/TopicDetail/delete`, {
        params: params
    });
};
export const editBug = params => {
    return axios.put(`${base}/api/TopicDetail/update`, params);
};
export const addBug = params => {
    return axios.post(`${base}/api/TopicDetail/post`, params);
};


// 博客模块管理
export const getBlogListPage = params => {
    return axios.post(`${base}/api/Blog`, {
        params: params
    });
};
export const getBlogDeatil = params => {
    return axios.post(`${base}/api/Blog/DetailNuxtNoPer`, {
        params: params
    });
};
export const editBlog = params => {
    return axios.put(`${base}/api/Blog/update`, params);
};
export const removeBlog = params => {
    return axios.delete(`${base}/api/Blog/delete`, {
        params: params
    });
};

// 日志
export const getLogs = params => {
    return axios.post(`${base}/api/Monitor/get`, {
        params: params
    });
};
export const getRequestApiinfoByWeek = params => {
    return axios.get(`${base}/api/Monitor/GetRequestApiinfoByWeek`, {
        params: params
    });
};
export const getAccessApiByDate = params => {
    return axios.get(`${base}/api/Monitor/GetAccessApiByDate`, {
        params: params
    });
};
export const getAccessApiByHour = params => {
    return axios.get(`${base}/api/Monitor/GetAccessApiByHour`, {
        params: params
    });
};
export const getServerInfo = params => {
    return axios.get(`${base}/api/Monitor/Server`, {
        params: params
    });
};
export const getAccessLogs = params => {
    return axios.post(`${base}/api/Monitor/GetAccessLogs`, {
        params: params
    });
};

//玩家資產 獎金管理
//新增業績
export const getRequestApiinfoByInManageWork = params => {
    return axios.get(`${base}/api/User/getRequestApiinfoByInManageWork`, {
        params: params
    });
};
//獎勵產出
export const getRequestApiinfoByInManageGold = params => {
    return axios.get(`${base}/api/User/getRequestApiinfoByInManageGold`, {
        params: params
    });
};



// Task管理
export const getTaskListPage = params => {
    return axios.post(`${base}/api/TasksQz/get`, {
        params: params
    });
};
export const removeTask = params => {
    return axios.delete(`${base}/api/TasksQz/delete`, {
        params: params
    });
};
export const editTask = params => {
    return axios.put(`${base}/api/TasksQz/put`, params);
};
export const addTask = params => {
    return axios.post(`${base}/api/TasksQz/post`, params);
};

export const startJob = params => {
    return axios.post(`${base}/api/TasksQz/StartJob`, {
        params: params
    });
};
export const stopJob = params => {
    return axios.post(`${base}/api/TasksQz/StopJob`, {
        params: params
    });
};
export const reCovery = params => {
    return axios.post(`${base}/api/TasksQz/ReCovery`, {
        params: params
    });
};

//EPSell
export const GetEPUserSell = params => {
    return axios.post(`${base}/api/EP/GetEPUserSell`, qs.stringify(params)).then(res => res.data);
};

//GetEPFinanceSell
export const GetEPFinanceSell = params => {
    return axios.post(`${base}/api/EP/GetEPFinanceSell`, qs.stringify(params)).then(res => res.data);
};
//GetDPERecord
export const GetDPERecord = params => {
    return axios.post(`${base}/api/DPE/GetDPERecord`, qs.stringify(params)).then(res => res.data);
};

//GetALLUserInfo
export const GetALLUserInfo = params => {
    return axios.post(`${base}/api/UserInfo/GetALLUserInfo`, qs.stringify(params)).then(res => res.data);
};

//GetUserInfoWeek
export const GetUserInfoWeek = params => {
    return axios.post(`${base}/api/UserInfo/GetUserInfoWeek`, qs.stringify(params)).then(res => res.data);
};
export const adminbofen = params => {
    return axios.post(`${base}/api/RP/adminbofen`, qs.stringify(params)).then(res => res.data);
};



export const GetUserInfo = params => {
    return axios.post(`${base}/api/UserInfo/GetUserInfo`, qs.stringify(params)).then(res => res.data);
};

//admin
//adminResetlock 
export const adminResetlock = params => {

    return axios.post(`${base}/api/User/adminResetlock`, qs.stringify(params)).then(res => res.data);
};
//adminResetpwd
export const adminResetpwd = params => {
    return axios.post(`${base}/api/User/adminResetpwd`, qs.stringify(params)).then(res => res.data);
};
//canceGoogle
export const admincanceGoogle = params => {
    return axios.post(`${base}/api/User/canceGoogle`, qs.stringify(params)).then(res => res.data);
};
export const admintidgooglekey = params => {
    return axios.post(`${base}/api/User/admintidgooglekey`, qs.stringify(params)).then(res => res.data);
};

//adminResetlevel
export const adminResetlevel = params => {
    console.log(params)
    return axios.post(`${base}/api/User/adminResetlevel`, qs.stringify(params)).then(res => res.data);
};
//adminResettid
export const adminResettid = params => {
    return axios.post(`${base}/api/User/adminResettid`, qs.stringify(params)).then(res => res.data);
};
//adminResetanswer
export const adminResetanswer = params => {
    return axios.post(`${base}/api/User/adminResetanswer`, qs.stringify(params)).then(res => res.data);
};

export const GetOpenShopMyweb = params => {
    return axios.post(`${base}/api/Shop/GetOpenShopMyweb`, qs.stringify(params)).then(res => res.data);
};

export const OrderOutAllPut = params => {
    return axios.post(`${base}/api/Shop/OrderOutAllPut`, qs.stringify(params)).then(res => res.data);
};
export const GetALLUserInfoExcel = params => {
    return axios.post(`${base}/api/UserInfo/GetALLUserInfoExcel`, qs.stringify(params)).then(res => res.data);
};

export const EPRecordOutAllPut = params => {
    return axios.post(`${base}/api/EP/EPRecordOutAllPut`, qs.stringify(params)).then(res => res.data);
};




export const GetDownExcelList = params => {
    return axios.post(`${base}/api/Shop/GetDownExcelList`, qs.stringify(params)).then(res => res.data);
};
export const DeleteDownExcelList = params => {
    return axios.post(`${base}/api/Shop/DeleteDownExcelList`, qs.stringify(params)).then(res => res.data);
};

//adminResetidcard
export const adminResetidcard = params => {
    return axios.post(`${base}/api/User/adminResetidcard`, qs.stringify(params)).then(res => res.data);
};

//GetAdminUserFeedBack
export const GetAdminUserFeedBack = params => {
    return axios.post(`${base}/api/User/GetAdminUserFeedBack`, qs.stringify(params)).then(res => res.data);
};

//AdminDisposeFeedBack
export const AdminDisposeFeedBack = params => {
    return axios.post(`${base}/api/User/AdminDisposeFeedBack`, qs.stringify(params)).then(res => res.data);
};

export const GetSearchRelation = params => {
    return axios.post(`${base}/api/Relation/GetRelationListadmin`, qs.stringify(params)).then(res => res.data);
};


//GetAdminAllUserFeedBack
export const GetAdminAllUserFeedBack = params => {
    return axios.post(`${base}/api/User/GetAdminAllUserFeedBack`, qs.stringify(params)).then(res => res.data);
};


//GetSPSearchServicInfo
export const GetSPSearchServicInfo = params => {
    return axios.post(`${base}/api/User/GetSPSearchServicInfo`, qs.stringify(params)).then(res => res.data);
};

//GetStockInfoDay
export const GetStockInfoDay = params => {
    return axios.post(`${base}/api/User/GetStockInfoDay`, qs.stringify(params)).then(res => res.data);
};

//GetAdminEPExchangeList
export const GetAdminEPExchangeList = params => {
    return axios.post(`${base}/api/EP/GetAdminEPExchangeList`, qs.stringify(params)).then(res => res.data);
};

//GetAdminRPExchangeList
export const GetAdminRPExchangeList = params => {
    return axios.post(`${base}/api/RP/GetAdminRPExchangeList`, qs.stringify(params)).then(res => res.data);
};

//RollBackTran
export const RollBackTran = params => {
    return axios.post(`${base}/api/EP/RollBackTran`, qs.stringify(params)).then(res => res.data);
};

export const GetAdminEPRecordList = params => {
    return axios.post(`${base}/api/EP/GetAdminEPRecordList`, qs.stringify(params)).then(res => res.data);
};

export const EPBuy = params => {
    return axios.post(`${base}/api/EP/EPBuy`, qs.stringify(params)).then(res => res.data);
};

export const EPPay = params => {
    return axios.post(`${base}/api/EP/EPPay`, qs.stringify(params)).then(res => res.data);
};

export const EPFinish = params => {
    return axios.post(`${base}/api/EP/EPFinish`, qs.stringify(params)).then(res => res.data);
};

export const GetAdminEPExchange = params => {
    return axios.post(`${base}/api/EP/GetAdminEPExchange`, qs.stringify(params)).then(res => res.data);
};

export const GetAdminSPExchange = params => {
    return axios.post(`${base}/api/SP/GetAdminSPExchange`, qs.stringify(params)).then(res => res.data);
};

export const GetAdminRPExchange = params => {
    return axios.post(`${base}/api/RP/GetAdminRPExchange`, qs.stringify(params)).then(res => res.data);
};

//GetAdminBuyShopList
export const GetAdminBuyShopList = params => {
    return axios.post(`${base}/api/Shop/GetAdminBuyShopList`, qs.stringify(params)).then(res => res.data);
};
export const ApplyOpenShopMyweb = params => {
    return axios.post(`${base}/api/Shop/ApplyOpenShopMyweb`, qs.stringify(params)).then(res => res.data);
};
export const GetShopListMyweb = params => {
    return axios.post(`${base}/api/Shop/GetShopListMyweb`, qs.stringify(params)).then(res => res.data);
};
export const DeleteShopListMyweb = params => {
    return axios.post(`${base}/api/Shop/DeleteShopListMyweb`, qs.stringify(params)).then(res => res.data);
};
export const GetShopSkuInfoMyweb = params => {
    return axios.post(`${base}/api/Shop/GetShopSkuInfoMyweb`, qs.stringify(params)).then(res => res.data);
};
export const DeleteSkudetailInfoMyweb = params => {
    return axios.post(`${base}/api/Shop/DeleteSkudetailInfoMyweb`, qs.stringify(params)).then(res => res.data);
};
export const DeleteSkuInfoMyweb = params => {
    return axios.post(`${base}/api/Shop/DeleteSkuInfoMyweb`, qs.stringify(params)).then(res => res.data);
};
export const AddSkuDetailMyweb = params => {
    return axios.post(`${base}/api/Shop/AddSkuDetailMyweb`, qs.stringify(params)).then(res => res.data);
};
export const AddSkuMyweb = params => {
    return axios.post(`${base}/api/Shop/AddSkuMyweb`, qs.stringify(params)).then(res => res.data);
};
export const AddSkuAndDetail = params => {
    return axios.post(`${base}/api/Shop/AddSkuAndDetail`, qs.stringify(params)).then(res => res.data);
};
export const UpdateGrounding = params => {
    return axios.post(`${base}/api/Shop/UpdateGrounding`, qs.stringify(params)).then(res => res.data);
};


export const uploadPictureSkuDetail = params => {
    return axios.post(`${base}/api/Shop/uploadPictureSkuDetail`, params).then(res => res.data);
};
export const uploadPictureSku = params => {
    return axios.post(`${base}/api/Shop/uploadPictureSku`, params).then(res => res.data);
};
export const uploadShopdetialexcel = params => {
    return axios.post(`${base}/api/Shop/uploadShopdetialexcel`, params).then(res => res.data);
};


export const AddShopListMyweb = params => {
    return axios.post(`${base}/api/Shop/AddShopListMyweb`, qs.stringify(params)).then(res => res.data);
};
export const uploadPicture = params => {
    return axios.post(`${base}/api/Shop/uploadPicture`, params).then(res => res.data);
};

export const uploadPictureDetail = params => {
    return axios.post(`${base}/api/Shop/uploadPictureDetail`, params).then(res => res.data);
};
//ChangeOrdersweb
export const ChangeOrdersweb = params => {
    return axios.post(`${base}/api/Shop/ChangeOrdersweb`, qs.stringify(params)).then(res => res.data);
};
//AddTruckOrdersweb ChangeOrdersweb
export const AddTruckOrdersweb = params => {
    return axios.post(`${base}/api/Shop/AddTruckOrdersweb`, qs.stringify(params)).then(res => res.data);
};

export const changepwdbyadmin = params => {
    return axios.post(`${base}/api/User/changepwdbyadmin`, qs.stringify(params)).then(res => res.data);
};

export const ckgoogle = params => {
    return axios.post(`${base}/api/User/ckgoogle`, qs.stringify(params)).then(res => res.data);
};

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

export const photoList = {
    head01: head01,
    head02: head02,
    head03: head03,
    head04: head04,
    head05: head05,
    head06: head06,
    head07: head07,
    head08: head08,
    head09: head09,

    head010: head010,
    head011: head011,
    head012: head012,
    head013: head013,
    head014: head014,
    head015: head015,
    head016: head016,
    head017: head017,
    head018: head018,
    head019: head019,

    head020: head020,
    head021: head021,
    head022: head022,
    head023: head023,
    head024: head024,
    head025: head025,
    head026: head026,
    head027: head027,
    head028: head028,
    head029: head029,

    head030: head030,
    head031: head031,
    head032: head032,
    head033: head033,
    head034: head034,
    head035: head035,
    head036: head036,
    head037: head037,
    head038: head038,
    head039: head039,

    head040: head040,
    head041: head041,
    head042: head042,
    head043: head043,
    head044: head044,
    head045: head045,
    head046: head046,
    head047: head047,
    head048: head048,
    head049: head049,

    head050: head050,
    head051: head051,
    head052: head052,
    head053: head053,
    head054: head054,
    head055: head055,
    head056: head056,
    head057: head057,
    head058: head058,
    head059: head059,
    head060: head060
}