import axios from 'axios'
import { ElLoading } from 'element-plus'
import Api from './Api'
import Message from './Message'

const contentTypeForm = 'application/x-www-form-urlencoded;charset=UTF-8'
const contentTypeJson = 'application/json'
const responseTypeJson = 'json'
let loading = null

const instance = axios.create({
  withCredentials: true, //允许携带头部信息
  // baseURL: (import.meta.env.PROD ? Api.proDomain : '') + '/api',
  baseURL: Api.proDomain,
  timeout: 10 * 1000
})

//请求拦截器
instance.interceptors.request.use(
  (config) => {
    if (config.showLoading) {
      loading = ElLoading.service({
        lock: true,
        text: '加载中...',
        background: 'rgba(0,0,0,0.7)'
      })
    }
    return config
  },
  (error) => {
    if (error.config.showLoading && loading) {
      loading.close()
    }
    Message.error('请求发送失败')
    return Promise.reject('请求发送失败')
  }
)

//响应
instance.interceptors.response.use(
  (response) => {
    const { showLoading, errorCallback, responseType } = response.config
    if (showLoading && loading) {
      loading.close()
    }
    const responseData = response.data

    if (responseType == 'arraybuffer' || responseType == 'blob') {
      return responseData
    }

    //正常请求
    if (responseData.code == 200) return responseData
    else if (responseData.code == 901) {
      //登录超时
      setTimeout(() => {
        window.ipcRenderer.send('reLogin')
      }, 2000)

      return Promise.reject({ showError: true, msg: '登录超时' })
    } else {
      //其它错误
      if (errorCallback) errorCallback(responseData)
      return Promise.reject({ showError: true, msg: responseData.message })
    }
  },
  (error) => {
    if (error.config.showLoading && loading) loading.close()
    return Promise.reject({ showError: true, msg: '网络异常' })
  }
)

const request = (config) => {
  const {
    url,
    params,
    dataType,
    showLoading = true,
    responseType = responseTypeJson,
    showError = true
  } = config
  let contentType = contentTypeForm
  let formData = new FormData()
  for (let key in params) {
    formData.append(key, params[key] == undefined ? '' : params[key])
  }
  if (dataType != null && dataType == 'json') {
    contentType = contentTypeJson
  }
  const token = localStorage.getItem('token')
  let headers = {
    'Content-Type': contentType,
    'X-Requested-With': 'XMLHttpRequest',
    token: token
  }

  return instance
    .post(url, formData, {
      headers: headers,
      showLoading: showLoading,
      showError: showError,
      errorCallback: config.errorCallback,
      responseType: responseType
    })
    .catch((error) => {
      if (error.showError) {
        Message.error(error.msg)
      }
      return null
    })
}

export default request
