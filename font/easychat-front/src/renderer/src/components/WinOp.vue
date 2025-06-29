<template>
  <div class="win-op no-drag">
    <div
      v-if="showSetTop"
      :class="['iconfont icon-top', isTop ? 'win-top' : '']"
      :title="isTop ? '取消置顶' : '置顶'"
      @click="top"
    ></div>
    <div v-if="showMin" class="iconfont icon-min" title="最小化" @click="minimize"></div>
    <div
      v-if="showMax"
      :class="['iconfont', isMax ? 'icon-maximize' : 'icon-max']"
      :title="isMax ? '向下还原' : '最大化'"
      @click="maximize"
    ></div>
    <div v-if="showClose" class="iconfont icon-close" title="关闭" @click="close"></div>
  </div>
</template>

<script setup>
import { onMounted, ref } from 'vue'

const props = defineProps({
  showSetTop: {
    type: Boolean,
    default: true
  },
  showMin: {
    type: Boolean,
    default: true
  },
  showMax: {
    type: Boolean,
    default: true
  },
  showClose: {
    type: Boolean,
    default: true
  },
  //关闭类型 0-关闭 1-隐藏
  closeType: {
    type: Number,
    default: 1
  }
})

const emit = defineEmits(['closeCallback'])

const isTop = ref(false)
const isMax = ref(false)
onMounted(() => {
  // 初始化状态
  isTop.value = false
  isMax.value = false
})

const winOp = (action, data) => {
  console.log(window.ipcRenderer, '----------------')
  window.ipcRenderer.send('winTitleOp', { action, data })
}

const close = () => {
  winOp('close', { closeType: props.closeType })
  emit('closeCallback')
}
const minimize = () => {
  winOp('minimize')
}
const maximize = () => {
  isMax.value = !isMax.value
  winOp(isMax.value ? 'maximize' : 'unmaximize')
}
const top = () => {
  isTop.value = !isTop.value
  winOp('top', { top: isTop.value })
}
</script>

<style lang="scss" scoped>
.win-op {
  top: 0px;
  right: 0px;
  position: absolute;
  z-index: 1;
  overflow: hidden;
  border-radius: 0px 3px 0px 0px;
  .iconfont {
    float: left;
    font-size: 12px;
    color: #101010;
    text-align: center;
    display: flex;
    justify-content: center;
    cursor: pointer;
    height: 25px;
    align-items: center;
    padding: 0px 10px;
    &:hover {
      background-color: #ddd;
    }
  }
  .icon-close {
    &:hover {
      background-color: #fb7373;
      color: #fff;
    }
  }
  .win-op {
    background-color: #ddd;
    color: #07c160;
  }
}
</style>
