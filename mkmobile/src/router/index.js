import Vue from 'vue'
import Router from 'vue-router'
import {
  accessToken
} from 'util/const'
import {
  storage
} from 'util/storage'

// meta: { requireLogin: true }
Vue.use(Router)

const router = new Router({
  routes: [{
    path: '/',
    name: '/Login',
    component: resolve => require(['@/pages/Login/Login'], resolve)
  },
  {
    path: '/',
    redirect: '/Home',
    component: resolve => require(['@/pages/Index'], resolve),

    // 需要tabbar的放这里
    children: [{
      name: 'Home',
      path: '/home',
      component: resolve => require(['@/pages/home/Home'], resolve)
    },
    {
      name: 'shopDetail',
      path: '/shopDetail',
      component: resolve => require(['@/pages/shop/shopDetail'], resolve)
    },
    {
      name: 'shop',
      path: '/shop',
      component: resolve => require(['@/pages/shop/shop'], resolve)
    },
    {
      name: 'shopCar',
      path: '/shopCar',
      component: resolve => require(['@/pages/shop/shopCar'], resolve)
    },
    {
      name: 'shopList',
      path: '/shopList',
      component: resolve => require(['@/pages/shop/shopList'], resolve)
    },
    {
      name: 'orderDetail',
      path: '/orderDetail',
      component: resolve => require(['@/pages/shop/orderDetail'], resolve)
    },
    {
      name: 'setting',
      path: '/setting',
      component: resolve => require(['@/pages/setUp/set'], resolve)
    },
    {
      name: 'SetAnswer',
      path: '/SetAnswer',
      component: resolve => require(['@/pages/setUp/SetAnswer'], resolve)
    },
    {
      name: 'friendsList',
      path: '/friendsList',
      component: resolve => require(['@/pages/setUp/friendsList'], resolve)
    },

    {
      name: 'friends',
      path: '/friends',
      component: resolve => require(['@/pages/setUp/friends'], resolve)
    },
    {
      name: 'FAQ',
      path: '/faq',
      component: resolve => require(['@/pages/faq/FAQ'], resolve)
    },
    {
      name: 'Charge',
      path: '/charge',
      component: resolve => require(['@/pages/charge/Charge'], resolve)
    },
    {
      name: 'ChargeAll',
      path: '/chargeAll',
      component: resolve => require(['@/pages/charge/ChargeAll'], resolve)
    },
    {
      name: 'Password',
      path: '/password',
      component: resolve => require(['@/pages/Password/password'], resolve)
    },
    {
      name: 'SellEp',
      path: '/sellEp',
      component: resolve => require(['@/pages/sell/SellEp'], resolve)
    },

    {
      name: 'TransEP',
      path: '/transEp',
      component: resolve => require(['@/pages/trans/TransEp'], resolve)
    },
    {
      name: 'TransRP',
      path: '/transRp',
      component: resolve => require(['@/pages/trans/TransRp'], resolve)
    },
    {
      name: 'TransWithMe',
      path: '/transWithMe',
      component: resolve => require(['@/pages/trans/TransWithMe'], resolve)
    },
    {
      name: 'Withdrawal',
      path: '/withdrawal',
      component: resolve => require(['@/pages/trans/Withdrawal'], resolve)
    },
    {
      name: 'News',
      path: '/news',
      component: resolve => require(['@/pages/news/index'], resolve)
    },
    {
      name: 'FX',
      path: '/FX',
      component: resolve => require(['@/pages/news/FX'], resolve)
    },
    {
      name: 'NewsDetail',
      path: '/newsDetail',
      component: resolve => require(['@/pages/news/NewsDetail'], resolve)
    },
    {
      name: 'Analyse',
      path: '/analyse',
      component: resolve => require(['@/pages/analyse/Analyse'], resolve)
    },
    {
      name: 'AuthenticatorOne',
      path: '/authenticatorOne',
      component: resolve =>
        require(['@/pages/authenticator/AuthenticatorOne'], resolve)
    },
    {
      name: 'AuthenticatorTwo',
      path: '/authenticatorTwo',
      component: resolve =>
        require(['@/pages/authenticator/AuthenticatorTwo'], resolve)
    },
    {
      name: 'AuthenticatorThree',
      path: '/authenticatorThree',
      component: resolve =>
        require(['@/pages/authenticator/AuthenticatorThree'], resolve)
    },
    {
      name: 'AuthenticatorLast',
      path: '/authenticatorLast',
      component: resolve =>
        require(['@/pages/authenticator/AuthenticatorLast'], resolve)
    },
    {
      name: 'SetUp',
      path: '/setUp',
      component: resolve => require(['@/pages/SetUp/setUp'], resolve)
    },
    {
      name: 'Wallet',
      path: '/wallet',
      component: resolve => require(['@/pages/wallet/Wallet'], resolve)
    },
    {
      name: 'Bank',
      path: '/bank',
      component: resolve => require(['@/pages/bank/Bank'], resolve)
    },
    {
      name: 'Search',
      path: '/search',
      component: resolve => require(['@/pages/search/Search'], resolve)
    },
    {
      name: 'lanSet',
      path: '/lanSet',
      component: resolve => require(['@/pages/SetUp/lanSet'], resolve)
    },
    {
      name: 'etInfomation',
      path: '/etInfomation',
      component: resolve => require(['@/pages/trade/etInfomation'], resolve)
    },
    {
      name: 'epTrade',
      path: '/epTrade',
      component: resolve => require(['@/pages/trade/epTrade'], resolve)
    },
    {
      name: 'receiving',
      path: '/receiving',
      component: resolve => require(['@/pages/trade/receiving'], resolve)
    },
    {
      name: 'paying',
      path: '/paying',
      component: resolve => require(['@/pages/trade/paying'], resolve)
    },
    {
      name: 'epList',
      path: '/epList',
      component: resolve => require(['@/pages/trade/list'], resolve)
    },
    {
      name: 'exChangeData',
      path: '/exChangeData',
      component: resolve => require(['@/pages/exChangeData/exChangeData'], resolve)
    },
    {
      name: 'stockList',
      path: '/stocklist',
      component: resolve => require(['@/pages/trade/stocklist'], resolve)
    },

    {
      name: 'Config',
      path: '/config',
      component: resolve => require(['@/pages/config/Config'], resolve)
    },
    {
      name: 'JoinUs',
      path: '/joinUs',
      component: resolve => require(['@/pages/join/JoinUs'], resolve)
    },
    {
      name: 'VipUpgrade',
      path: '/vipUpgrade',
      component: resolve => require(['@/pages/join/VipUpgrade'], resolve)
    },
    {
      name: 'JoinUs',
      path: '/joinUs',
      component: resolve => require(['@/pages/join/JoinUs'], resolve)
    },
    {
      name: 'Additional',
      path: '/additional',
      component: resolve =>
        require(['@/pages/additional/Additional'], resolve)
    },
    {
      name: 'CheckData',
      path: '/checkData',
      component: resolve =>
        require(['@/pages/checkData/CheckData'], resolve)
    },
    {
      name: 'Feedback',
      path: '/feedback',
      component: resolve => require(['@/pages/feedback/Feedback'], resolve)
    },
    {
      name: 'Queue',
      path: '/queue',
      component: resolve => require(['@/pages/queue/queue'], resolve)
    },
    {
      name: 'setRelation',
      path: '/setRelation',
      component: resolve => require(['@/pages/setUp/setRelation'], resolve)
    },

    {
      name: 'task',
      path: '/task',
      component: resolve => require(['@/pages/task/Task'], resolve)
    },
    {
      name: 'ActivityList',
      path: '/activityList',
      component: resolve =>
        require(['@/pages/activity/ActivityList'], resolve)
    },
    {
      name: 'Activity',
      path: '/activity',
      component: resolve => require(['@/pages/activity/Activity'], resolve)
    },
    {
      name: 'ActivityDetails',
      path: '/activityDetails',
      component: resolve =>
        require(['@/pages/activity/ActivityDetails'], resolve)
    },
    {
      name: 'SecretQuestion',
      path: '/secretQuestion',
      component: resolve =>
        require(['@/pages/secretQuestion/SecretQuestion'], resolve)
    },
    {
      name: 'Details',
      path: '/details',
      component: resolve => require(['@/pages/details/Details'], resolve)
    },
    {
      name: 'OpenShop',
      path: '/openShop',
      component: resolve => require(['@/pages/openShop/OpenShop'], resolve)
    }

      // ActivityList.vue
    ]
  },
    // 不需要的放外边

  {
    path: '/forgot',
    name: 'Forgot',
    component: resolve => require(['@/pages/forgot/ForgotPassword'], resolve)
  },
  {
    path: '/verification',
    name: 'Verification',
    component: resolve => require(['@/pages/forgot/Verification'], resolve)
  },
  {
    path: '/helloWorld',
    name: 'HelloWorld',
    component: resolve => require(['@/pages/HelloWorld'], resolve)
  },
  {
    path: '*',
    redirect: '/'
  }
  ]
})

router.beforeEach((to, from, next) => {
  if (to.matched.some(res => res.meta.requireLogin)) {
    // 判断是否需要登录权限
    if (!storage.getLocalStorage(accessToken)) {
      next({
        path: '/Login'
      })
    } else {
      next()
    }
  } else {
    next()
  }
})

export default router
