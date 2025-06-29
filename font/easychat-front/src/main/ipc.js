import { ipcMain } from 'electron'
import store from './store'

const onLoginOrRegister = (callback) => {
  ipcMain.on('loginOrRegister', (e, isLogin) => {
    callback(isLogin)
  })
}

const onLoginSuccess = (callback) => {
  ipcMain.on('openChat', (e, config) => {
    store.initUserId(config.userId)
    store.setUserData('token', config.token)
    //todo 增加用户配置
    callback(config)
    //todo 初始化ws链接
  })
}

const OnWinTitleOp = (callback) => {
  ipcMain.on('winTitleOp', (e, data) => {
    callback(e, data)
  })
}

export { onLoginOrRegister, onLoginSuccess, OnWinTitleOp }
