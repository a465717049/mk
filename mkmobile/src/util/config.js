   let baseUrl = 'https://api.a8dog.top/api/'
   let url = 'https://api.a8dog.top' // 域名
  // let baseUrl = 'http://localhost:8083/api/'
  // let url = 'http://localhost:8083' // 域名



  // 如果要跨域修改这里和 config/index的proxyTable 修改成实际要请求的地址

  const config = {
    imageUrl: 'https://api.a8dog.top',
    shopimgUrl: 'https://api.a8dog.top/shopimg/',
    imageAfterUrl: '?imageslim',
    baseUrl,
    url
  }

  export {
    config
  }
