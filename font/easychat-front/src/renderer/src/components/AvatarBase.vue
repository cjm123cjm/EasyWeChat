<template>
  <div
    class="user-avatar"
    :style="{ width: width + 'px', height: width + 'px', 'border-radius': borderRadius + 'px' }"
    @click="showDetailHandle"
  >
    <ShowLocalImage
      :width="width"
      :file-id="userId"
      part-type="avatar"
      :force-get="true"
    ></ShowLocalImage>
  </div>
</template>

<script setup>
const props = defineProps({
  userId: {
    type: Number,
    default: 0
  },
  width: {
    type: Number,
    default: 170
  },
  borderRadius: {
    type: Number,
    default: 0
  },
  showDetail: {
    type: Boolean,
    default: false
  }
})

const showDetailHandle = () => {
  if (!props.showDetail) {
    return
  }
  //todo 展示图片详情
}
</script>

<style lang="scss" scoped>
.user-avatar {
  background: #d3d3d3;
  display: flex;
  overflow: hidden;
  cursor: pointer;
  align-items: center;
  justify-content: center;
}
</style>
