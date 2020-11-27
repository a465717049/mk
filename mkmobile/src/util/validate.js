export const isPhone = (value) => /^1\d{10}$/gi.test(value)

export const regTel = (value) => /^0((\d{2}-\d{8})|(\d{3}-\d{7,8}))/.test(value) // 座机带区号

export const isEmail = (value) => /^[a-z0-9]+([._\\-]*[a-z0-9])*@([a-z0-9]+[-a-z0-9]*[a-z0-9]+.){1,63}[a-z0-9]+$/.test(value)

export const isidCode = (value) => /^[0-9a-zA-Z]{15,18}$/.test(value)

export const isAccountNumber = (value) => /^([1-9]{1})(\d{15}|\d{18})$/.test(value)

export const isPhoneCode = (value) => /^\d{6}$/gi.test(value)// 验证码

export const isAlias = (value) => !/[@#$%^&*]+/gi.test(value)// 用户昵称

export const rightPassword = (value) => /[0-9A-Za-z]{6,12}/gi.test(value)// 用户昵称
