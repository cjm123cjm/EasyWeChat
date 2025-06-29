<template>
  <div class="login-panel">
    <div class="title drag">EsayChat</div>
    <div v-if="showLoading" class="loading-panel">
      <img src="../assets/img/loading.gif" />
    </div>
    <div class="login-form">
      <div class="error-msg">{{ errorMessage }}</div>
      <el-form ref="formDataRef" :model="formData" label-width="0" @submit.prevent>
        <el-form-item prop="email">
          <el-input
            v-model.trim="formData.email"
            size="large"
            placeholder="请输入邮箱"
            clearable
            max-length="30"
            @focus="cleanVerify"
          >
            <template #prefix>
              <span class="iconfont icon-email"></span>
            </template>
          </el-input>
        </el-form-item>

        <el-form-item v-if="!isLogin" prop="nickName">
          <el-input
            v-model.trim="formData.nickName"
            size="large"
            placeholder="请输入昵称"
            clearable
            max-length="15"
            @focus="cleanVerify"
          >
            <template #prefix>
              <span class="iconfont icon-user-nick"></span>
            </template>
          </el-input>
        </el-form-item>

        <el-form-item prop="password">
          <el-input
            v-model.trim="formData.password"
            size="large"
            placeholder="请输入密码"
            clearable
            show-password
            @focus="cleanVerify"
          >
            <template #prefix>
              <span class="iconfont icon-password"></span>
            </template>
          </el-input>
        </el-form-item>

        <el-form-item v-if="!isLogin" prop="rePassword">
          <el-input
            v-model.trim="formData.rePassword"
            size="large"
            placeholder="请输入再次密码"
            clearable
            show-password
            @focus="cleanVerify"
          >
            <template #prefix>
              <span class="iconfont icon-password"></span>
            </template>
          </el-input>
        </el-form-item>

        <el-form-item prop="checkCode">
          <div class="check-code-panel">
            <el-input
              v-model.trim="formData.checkCode"
              size="large"
              placeholder="请输入验证码"
              clearable
              @focus="cleanVerify"
            >
              <template #prefix>
                <span class="iconfont icon-checkcode"></span>
              </template>
            </el-input>
            <img :src="checkCodeUrl" class="check-code" @click="changeCheckCode" />
          </div>
        </el-form-item>

        <el-form-item>
          <el-button type="primary" class="login-btn" @click="submit">{{
            isLogin ? '登录' : '注册'
          }}</el-button>
        </el-form-item>

        <div class="bottom-link">
          <span class="a-link" @click="changeOpType">{{ isLogin ? '没有账号?' : '已有账号' }}</span>
        </div>
      </el-form>
    </div>
  </div>
  <WinOp :show-set-top="false" :show-min="false" :show-max="false" :close-type="0"></WinOp>
</template>

<script setup>
import { ref, getCurrentInstance, nextTick } from 'vue'
import { useUserInfoStore } from '@/store/UserInfoStore.js'
import { useRouter } from 'vue-router'
const instance = getCurrentInstance()
const userStore = useUserInfoStore()
const router = useRouter()

const formData = ref({})
const formDataRef = ref()

const errorMessage = ref(null)
const cleanVerify = () => {
  errorMessage.value = null
}
const checkValue = (type, value, msg) => {
  if (instance.proxy.Utils.isEmpty(value)) {
    errorMessage.value = msg
    return false
  }
  if (type && !instance.proxy.Verify[type](value)) {
    errorMessage.value = msg
    return false
  }
  return true
}
const showLoading = ref(false)
const submit = async () => {
  cleanVerify()
  if (!checkValue('checkEmail', formData.value.email, '请输入正确的邮箱')) return
  if (!isLogin.value && !checkValue('', formData.value.nickName, '请输入昵称')) return
  if (!checkValue('checkPassword', formData.value.password, '密码只能是数字、字母、特殊字符8-18位'))
    return
  if (!isLogin.value && formData.value.password != formData.value.rePassword) {
    errorMessage.value = '两次输入的密码不一致'
    return
  }
  if (!checkValue('', formData.value.checkCode, '请输入验证码')) return

  if (isLogin.value) {
    showLoading.value = true
  }
  //发送请求
  const result = await instance.proxy.request({
    url: isLogin.value ? instance.proxy.Api.login : instance.proxy.Api.register,
    showError: false,
    showLoading: isLogin.value ? false : true,
    dataType: 'json',
    params: {
      email: formData.value.email,
      password: formData.value.password,
      verifyCode: formData.value.checkCode,
      codeKey: localStorage.getItem('checkCodeKey'),
      nickName: formData.value.nickName
    },
    errorCallback: (response) => {
      showLoading.value = false
      changeCheckCode()
      errorMessage.value = response.info
    }
  })

  if (!result) return

  if (isLogin.value) {
    showLoading.value = false
    userStore.setUserInfo(result.result)
    localStorage.setItem('token', result.token)

    router.push('/main')

    const screenWidth = window.screen.width
    const screenHeight = window.screen.height
    window.ipcRenderer.send('openChat', {
      email: formData.value.email,
      token: result.token,
      userId: result.result.userId,
      nickName: result.result.nickName,
      isAdmin: result.result.isAdmin,
      screenWidth: screenWidth,
      screenHeight: screenHeight
    })
  } else {
    instance.proxy.Message.success('注册成功')
    changeOpType()
    changeCheckCode()
  }
}

const isLogin = ref(true)
const changeOpType = () => {
  window.ipcRenderer.send('loginOrRegister', !isLogin.value)
  isLogin.value = !isLogin.value
  nextTick(() => {
    formDataRef.value.resetFields()
    formData.value = {}
    cleanVerify()
  })
}

//获取验证码
const checkCodeUrl = ref(null)
const changeCheckCode = async () => {
  const result = await instance.proxy.request({
    url: instance.proxy.Api.checkCode
  })
  if (!result) return

  checkCodeUrl.value = `data:image/png;base64,${result.result.image}`
  localStorage.setItem('checkCodeKey', result.result.codeKey)
}
changeCheckCode()
</script>

<style lang="scss" scoped>
.email-select {
  width: 250px;
}
.loading-panel {
  height: calc(100vh - 32px);
  display: flex;
  justify-content: center;
  align-items: center;
  overflow: hidden;
  img {
    width: 300px;
  }
}
.login-panel {
  background-color: #fff;
  border-radius: 3px;
  border: 1px solid #ddd;
  .title {
    height: 30px;
    padding: 5px 0px 0px 10px;
  }

  .login-form {
    padding: 0px 15px 29px 15px;
    :deep(.el-input__wrapper) {
      box-shadow: none;
      border-radius: none;
    }
    .el-form-item {
      border-bottom: 1px solid #ddd;
    }

    .email-panel {
      align-items: center;
      width: 100%;
      display: flex;
      .input {
        flex: 1;
      }
      .icon-down {
        margin-left: 3px;
        width: 16px;
        cursor: pointer;
        border: none;
      }
    }

    .error-msg {
      line-height: 30px;
      height: 30px;
      color: #fb7373;
    }
    .check-code-panel {
      display: flex;
      .check-code {
        cursor: pointer;
        width: 120px;
        margin-left: 5px;
      }
    }

    .login-btn {
      margin-top: 20px;
      width: 100%;
      background: #07c160;
      height: 36px;
      font-size: 16px;
    }
    .bottom-link {
      text-align: right;
    }
  }
}
</style>
