<template>
  <router-view />
</template>

<script>
import { onMounted } from 'vue'
import { useUserStore } from './stores/user'

export default {
  name: 'App',
  setup() {
    const userStore = useUserStore()

    onMounted(() => {
      // Initialize authentication state on app start
      if (userStore.token && !userStore.user) {
        userStore.getCurrentUser()
      }
    })

    return {}
  }
}
</script>
