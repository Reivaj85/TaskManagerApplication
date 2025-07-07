import { defineStore } from 'pinia'
import { ref } from 'vue'

export const useAppStore = defineStore('app', () => {
  const loading = ref(false)
  const snackbar = ref({
    show: false,
    message: '',
    color: 'success',
    timeout: 3000
  })

  const setLoading = (value) => {
    loading.value = value
  }

  const showSnackbar = (message, color = 'success', timeout = 3000) => {
    snackbar.value = {
      show: true,
      message,
      color,
      timeout
    }
  }

  const hideSnackbar = () => {
    snackbar.value.show = false
  }

  return {
    loading,
    snackbar,
    setLoading,
    showSnackbar,
    hideSnackbar
  }
})
