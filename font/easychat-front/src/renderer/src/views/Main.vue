<template>
  <div class="main">
    <div class="left-sider">
      <div></div>
      <div class="menu-list">
        <template v-for="item in menuList">
          <div
            v-if="item.position == 'top'"
            :key="item.path"
            :class="['tab-item iconfont', item.icon, item.path == currentMenu.path ? 'active' : '']"
            @click="changeMenu(item)"
          >
            <template v-if="item.name == 'chat'"></template>
          </div>
        </template>
      </div>
      <div class="menu-list menu-bottom">
        <template v-for="item in menuList">
          <div
            v-if="item.position == 'bottom'"
            :key="item.path"
            :class="['tab-item iconfont', item.icon, item.path == currentMenu.path ? 'active' : '']"
            @click="changeMenu(item)"
          ></div>
        </template>
      </div>
    </div>
    <div class="right-sider">
      <router-view v-slot="{ Component }">
        <keep-alive include="chat">
          <component :is="Component" ref="componentRef"></component>
        </keep-alive>
      </router-view>
    </div>
  </div>
  <WinOp></WinOp>
</template>

<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
const router = useRouter()

const menuList = ref([
  {
    name: 'chat',
    icon: 'icon-chat',
    path: '/chat',
    countKey: 'chatCount',
    position: 'top'
  },
  {
    name: 'contact',
    icon: 'icon-user',
    path: '/contact',
    countKey: 'contactApplyCount',
    position: 'top'
  },
  {
    name: 'mysetting',
    icon: 'icon-more2',
    path: '/setting',
    position: 'bottom'
  }
])
const changeMenu = (item) => {
  console.log('changeMenu', item)
  if (item.path === currentMenu.value.path) return
  currentMenu.value = item
  router.push(item.path)
}

const currentMenu = ref(menuList.value[0])
</script>

<style lang="scss" scoped>
.main {
  background: #fff;
  display: flex;
  border-radius: 0px 3px 3px 0px;
  overflow: hidden;
  .left-sider {
    width: 55px;
    background: #2e2e2e;
    text-align: center;
    display: flex;
    flex-direction: column;
    align-items: center;
    padding-top: 35px;
    border: 1px solid #2e2e2e;
    border-right: none;
    padding-bottom: 10px;
    .menu-list {
      width: 100%;
      flex: 1;
      .tab-item {
        color: #d3d3d3;
        font-size: 20px;
        height: 40px;
        display: flex;
        align-items: center;
        justify-content: center;
        margin-top: 10px;
        cursor: pointer;
      }
      .active {
        color: #07c160;
      }
    }
    .menu-bottom {
      display: flex;
      flex-direction: column;
      justify-content: flex-end;
    }
  }
  .right-sider {
    flex: 1;
    overflow: hidden;
    border: 1px solid #ddd;
    border-left: none;
  }
}

.popover-user-panel {
  padding: 10px;
  .popover-user {
    display: flex;
    border-bottom: 1px solid #ddd;
    padding-bottom: 20px;
  }
  .send-message {
    margin-top: 10px;
    text-align: center;
    padding: 20px 0px 0px 0px;
  }
}
</style>
