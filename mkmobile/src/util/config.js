 let baseUrl = "https://api.a8dog.top/api/";
 let url = "https://api.a8dog.top"; // 域名
// if (
//   process.env.NODE_ENV === "development" ||
//   process.env.NODE_ENV === "testing"
// ) {
//let baseUrl = 'https://api.a8dog.top/api/' // 开发 "http://localhost:8081/api/"
//let url = 'https://manage.dpeplus.com' // 开发
//   // baseUrl = 'http://192.168.1.8:8888/' // 开发
// } else if (process.env.NODE_ENV === "preview") {
//   baseUrl = "https://api.dpepie.com/api/"; // 预发布
//   url = "https://manage.dpeplus.com";
// } else if (process.env.NODE_ENV === "production") {
//   baseUrl = "https://api.dpepie.com/api/"; // 线上环境
//   url = "https://manage.dpeplus.com";
// }
// const config = {
//   imageUrl: "https://resource.upinstar.com",
//   imageAfterUrl: "?imageslim",
//   baseUrl,
//   url
// };

// export {
//   config
// };
// 如果要跨域修改这里和 config/index的proxyTable 修改成实际要请求的地址
// let baseUrl = '/api/'

const config = {
  imageUrl: 'https://resource.upinstar.com',
  imageAfterUrl: '?imageslim',
  baseUrl,
  url
}

export {
  config
}
