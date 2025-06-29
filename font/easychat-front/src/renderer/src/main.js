import { createApp } from 'vue'

import ElementPlus from 'element-plus'
import router from './router/index'
import App from './App.vue'
import Utils from './utils/Utils'
import Verify from './utils/Verify'
import request from './utils/Request'
import Api from './utils/Api'
import Message from './utils/Message'
import { createPinia } from 'pinia'

import Layout from './components/Layout.vue'
import WinOp from './components/WinOp.vue'
import ContentPanel from './components/ContentPanel.vue'
import ShowLocalImage from './components/ShowLocalImage.vue'
import UserBaseInfo from './components/UserBaseInfo.vue'

import 'element-plus/dist/index.css'
import '@/assets/cust-elementplus.scss'
import '@/assets/icon/iconfont.css'
import '@/assets/base.scss'

const app = createApp(App)
app.use(ElementPlus)

app.use(router)

const pinia = createPinia()
app.use(pinia)

app.component('Layout', Layout)
app.component('WinOp', WinOp)
app.component('ContentPanel', ContentPanel)
app.component('ShowLocalImage', ShowLocalImage)
app.component('UserBaseInfo', UserBaseInfo)

app.config.globalProperties.Utils = Utils
app.config.globalProperties.Verify = Verify
app.config.globalProperties.request = request
app.config.globalProperties.Api = Api
app.config.globalProperties.Message = Message

app.mount('#app')
