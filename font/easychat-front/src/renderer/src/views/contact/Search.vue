<template>
  <ContentPanel>
    <div class="search-form">
      <el-input
        v-model="contactId"
        clearable
        placeholder="请输入用户ID"
        size="large"
        @keydown.enter="search"
      >
      </el-input>
      <div class="search-btn iconfont icon-search" @click="search"></div>
    </div>
    <div v-if="searchResult && Object.keys(searchResult).length > 0" class="search-result-panel">
      <div class="search-result">
        <span class="contact-type">{{ contactTypeName }}</span>
        <UserBaseInfo
          :user-info="searchResult"
          :show-area="searchResult.contanctType == 0"
        ></UserBaseInfo>
      </div>
      <div v-if="searchResult.contactId != userStore.getUserInfo().userId" class="op-btn">
        <el-button
          v-if="
            searchResult.status == null ||
            searchResult.status == 0 ||
            searchResult.status == 2 ||
            searchResult.status == 3 ||
            searchResult.status == 4
          "
          type="primary"
          @click="applyContact"
        >
          {{ searchResult.contanctType == 0 ? '添加联系人' : '申请加入群组' }}
        </el-button>
        <el-button v-if="searchResult.status == 1" type="primary" @click="sendMessage"
          >发消息</el-button
        >
        <span v-if="searchResult.status == 5 || searchResult.status == 6">对方拉黑了你</span>
      </div>
    </div>
    <div v-else class="no-data">没有搜到任何结果</div>
  </ContentPanel>
</template>

<script setup>
import { ref, getCurrentInstance, computed } from 'vue'
const { proxy } = getCurrentInstance()
import { useUserInfoStore } from '@/store/UserInfoStore.js'

const userStore = useUserInfoStore()
const contactTypeName = computed(() => {
  if (userStore.getUserInfo().userId == searchResult.value.contactId) {
    return '自己'
  }
  if (searchResult.value.contanctType === 0) {
    return '用户'
  } else {
    return '群组'
  }
})

const contactId = ref('')
const searchResult = ref({})
const search = async () => {
  if (!contactId.value) {
    proxy.Message.warning('请输入用户ID')
    return
  }

  let result = await proxy.request({
    url: proxy.Api.userContactSearch,
    dataType: 'json',
    params: {
      contactId: contactId.value,
      contactType: 0
    }
  })

  if (!result) return

  searchResult.value = result.result

  console.log('搜索结果', searchResult.value)
}

//添加联系人人或群组
const applyContact = () => {}
</script>

<style lang="scss" scoped>
.search-form {
  padding-top: 50px;
  display: flex;
  align-items: center;
  :deep(.el-input__wrapper) {
    border-radius: 4px 0px 0px 4px;
    border-right: none;
  }
  .search-btn {
    background-color: #07c160;
    color: #fff;
    line-height: 40px;
    width: 80px;
    text-align: center;
    border-radius: 0px 5px 5px 0px;
    cursor: pointer;
    &:hover {
      background-color: #0dd36c;
    }
  }
}
.no-data {
  padding: 30px 0px;
}
.search-result-panel {
  .search-result {
    padding: 30px 20px 20px 20px;
    background-color: #fff;
    border-radius: 5px;
    margin-top: 10px;
    position: relative;
    .contact-type {
      position: absolute;
      top: 0px;
      left: 0px;
      background: #2cb6fe;
      padding: 2px 5px;
      color: #fff;
      border-radius: 5px 0px 0px 0px;
      font-size: 12px;
    }
  }
  .op-btn {
    border-radius: 5px;
    margin-top: 10px;
    padding: 10px;
    background: #fff;
    text-align: center;
  }
}
</style>
