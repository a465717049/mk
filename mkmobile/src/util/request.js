/**
 * 网络相关Api
 */
import { config } from "./config.js";
import { storage } from "./storage.js";
import { accessToken, IPHONE, Safari } from "./const.js";
import axios from "axios";
import notice from "./notice";
import router from "../router";

let baseUrl = config.baseUrl;

// 获取token;
let getAccessToken = () => {
  let token = storage.getLocalStorage(accessToken);
  return token;
};
// 请求头设置
let headerOption = () => {
  return {
    Authorization: getAccessToken(),
    "Access-Control-Allow-Origin": "https://app.snptw.cn/,https://api.snptw.cn/,*",
    "Access-Control-Allow-Credentials": "true",
    "content-type": "application/json;"
  };
};

console.log(headerOption.Authorizationm,'Authorization')
// http返回码状态判断
let state = (res, noLoading) => {
  if (noLoading) {
    // Loading隐藏
    notice.loadingHide();
  }

  if (!res.code&&res.code>0 ) { 
    notice.loadingHide();
  }
  switch (res.status) {
    case 302:
      notice.errorModal("302");
      break;
    case 400: 
      notice.errorModal("請求參數錯誤");
      break;
    case 401:
      notice.errorModal("未授權，請重新登陸", function() {
       router.push({ path: "/login" });
      });
      break;
    case 403:
      notice.errorModal("登錄超時，請重新登陸！", function() {
        notice.loadingHide();
        router.push({ path: "/login" });
       });
      break;
    case 404:
      notice.errorModal("非法請求！請重新登陸", function() {
        notice.loadingHide();
        router.push({ path: "/login" });
       });
      break;
    case 405:
      notice.errorModal("非法請求！請重新登陸！");
      break;
    case 408:
      notice.errorModal("非法請求！請重新登陸！");
      break;
    case 429:
      notice.errorModal("頻繁請求！請稍後重試！");
      break;
    case 500:
      notice.errorModal("服務器繁忙！請重試！");
      break;
    case 502:
      notice.errorModal("服務器繁忙！請重試！");
      break;
    case 505:
      notice.errorModal("服務器繁忙！請重試！");
      break;
    case 12000:
      notice.errorModal("参数错误！");
      break;
    case 10002:
      notice.errorModal("未授權，請重新登陸！", function() {
        router.push({ path: "/login" });
      });
      break;
  }
  notice.loadingHide();
};
/**
 * opts 包含url和method的对象
 * params 请求的参数
 * success 成功回調函數
 * noLoading 不顯示緩冲加載
 * error 異常
 */
export const http = (opts, params, success, noLoading, error) => {
  if (!noLoading || noLoading === "undefined") {
    notice.loading();
  }
  var xhr;
   if (IPHONE) {
     xhr = new plus.net.XMLHttpRequest();
     
    console.log(xhr,'xhr')
   }
  //  if (IPHONE && Safari) {
  //    if (!XMLHttpRequest) {
  //      return;
  //    }
  //   xhr = new XMLHttpRequest();
  //  }
  if (xhr) {
    error =
      error ||
      function(XMLHttpRequest, textStatus, errorThrown) {
        console.log("status:" + textStatus);
        console.log("data:" + XMLHttpRequest.responseText);
      };
      if(opts.url.substring(0,4)!=='http'){
        opts.url = baseUrl + opts.version + opts.url;
      }
   
  
    xhr.setRequestHeader(
      "Access-Control-Allow-Origin",
      "https://app.snptw.cn/,https://api.snptw.cn/,*"
    );
    xhr.setRequestHeader("Access-Control-Allow-Credentials", "true");
    xhr.onreadystatechange = function() {
      if (xhr.readyState === 4) {
        if (xhr.status === 200) {
          var json = JSON.parse(xhr.responseText || xhr.response);
          success(json);
          notice.loadingHide();
        } else {
          console.log(xhr.responseText)
          state(xhr, noLoading);
          error(xhr, xhr.status);
          notice.loadingHide();
        }
      }else{
        console.log(xhr.responseText)
      }
    };

    try {
      var postData = params;
      if (opts.string) {
        xhr.setRequestHeader(
          "Content-Type",
          "application/x-www-form-urlencoded;charset=utf-8"
        );
        var tempArr = [];
        for (var i in postData) {
          var key = encodeURIComponent(i);
          var value = encodeURIComponent(postData[i]);
          tempArr.push(key + "=" + value);
        }
        postData = tempArr.join("&");
      } else {
        xhr.setRequestHeader("Content-Type", "application/json;charset=utf-8");
      }
      xhr.open(opts.method, opts.url+'?'+postData, false);
      //console.log(opts.url,'URL')
      let token = getAccessToken();
      //console.log(token,'token')
      xhr.setRequestHeader("Authorization", token);
      xhr.send(postData);
     // console.log(postData)
    } catch (e) {
      console.log(e);
    }
  } else {
    // 请求默认配置
    let httpDefaultOptions = {};
    httpDefaultOptions = {
      url: baseUrl + opts.version + opts.url,
      method: opts.method,
      headers: headerOption(),
      timeout: 30000,
      params: Array.isArray(params) && opts.join ? {} : params,
      data: Array.isArray(params) && opts.join ? {} : params
    };
    if (
      opts.method === "GET" ||
      opts.method === "DELETE" ||
      opts.method === "HEAD" ||
      opts.string
    ) {
      delete httpDefaultOptions.data;
    } else {
      delete httpDefaultOptions.params;
    }
    // 如果参数是连接在url后面的形式
    if (opts.join && Array.isArray(params)) {
      params.forEach(c => {
        httpDefaultOptions.url += "/" + c;
      });
    }

    // 响应拦截器即异常处理
    axios.interceptors.request.use(
      data => {
        //notice.loadingHide()
        return data;
      },
      err => {
        console.log(err);
        return Promise.resolve(err);
      }
    );
    axios.interceptors.response.use(
      response => {
        if (response&&response.status && response.status === 200) {
          notice.loadingHide();
          state(response, ''); 
        if (response.data) {
          return response.data;
        } else {
          return response;
        }
       
        }else if(response&&response.status && response.status != 200){
          
            state(response, ''); 
            return response;
          }else if(response){
            state({code:403}, '');  
            return response;
        }
       
      },
      err => {
        state(err, ''); 
        return Promise.resolve(err.response);
      }
    );

    axios(httpDefaultOptions).then(success);
  }
};
