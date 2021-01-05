import axios from 'axios';
import qs from 'qs';
// import router from '../routerManuaConfig'
import router from '../router/index'
import store from "../store";
import Vue from 'vue';

import applicationUserManager from "../Auth/applicationusermanager";

let base = 'http://localhost:8083'; //'https://api.snptw.cn' https://api.a8dog.top;
// 如果是IIS部署，用这个，因为 IIS 只能是 CORS 跨域，不能代理
// let base = process.env.NODE_ENV=="production"? 'http://localhost:8081':'';


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

//admin
//adminResetlock 
export const adminResetlock = params => {

    return axios.post(`${base}/api/User/adminResetlock`, qs.stringify(params)).then(res => res.data);
};
//adminResetpwd
export const adminResetpwd = params => {
    return axios.post(`${base}/api/User/adminResetpwd`, qs.stringify(params)).then(res => res.data);
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
export const AddShopListMyweb = params => {
    return axios.post(`${base}/api/Shop/AddShopListMyweb`, qs.stringify(params)).then(res => res.data);
};
export const uploadPicture = params => {
    return axios.post(`${base}/api/Shop/uploadPicture`, params).then(res => res.data);
};

//ChangeOrdersweb
export const ChangeOrdersweb = params => {
    return axios.post(`${base}/api/Shop/ChangeOrdersweb`, qs.stringify(params)).then(res => res.data);
};
//AddTruckOrdersweb ChangeOrdersweb
export const AddTruckOrdersweb = params => {
    return axios.post(`${base}/api/Shop/AddTruckOrdersweb`, qs.stringify(params)).then(res => res.data);
};