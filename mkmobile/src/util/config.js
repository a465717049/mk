
let baseUrl = "http://api.fsswgw.cn/api/";
let url = "https://api.snptw.cn/api"; // 域名

if (
  process.env.NODE_ENV === "development" ||
  process.env.NODE_ENV === "testing"
) {
  baseUrl = "http://localhost:8081/api/"; // 开发
  url = "https://manage.dpeplus.com"; // 开发
  // baseUrl = 'http://192.168.1.8:8888/' // 开发
} else if (process.env.NODE_ENV === "preview") {
  baseUrl = "https://api.snptw.cn/api/"; // 预发布
  url = "https://manage.dpeplus.com";
} else if (process.env.NODE_ENV === "production") {
  baseUrl = "https://api.snptw.cn/api/"; // 线上环境
  url = "https://manage.dpeplus.com";
} 
const config = {
  imageUrl: "https://resource.upinstar.com",
  imageAfterUrl: "?imageslim",
  baseUrl,
  url
};

export {
  config
};