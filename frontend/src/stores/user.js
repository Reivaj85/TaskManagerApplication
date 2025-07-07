import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import authService from '@/services/authService'

export const useUserStore = defineStore('user', () => {
  const user = ref(null)
  const token = ref(localStorage.getItem('token') || null)

  const isAuthenticated = computed(() => !!token.value)

  const login = async (credentials) => {
    try {
      const response = await authService.login(credentials)
      if (response.isSuccess) {
        console.debug('Login response:', response)
        user.value = response.value.user
        token.value = response.value.token
        localStorage.setItem('token', response.value.token)
        return { success: true }
      } else {
        return { success: false, error: response.errorMessage }
      }
    } catch (error) {
      return { success: false, error: error.message }
    }
  }

  const register = async (userData) => {
    try {
      const response = await authService.register(userData)
      if (response.isSuccess) {
        user.value = response.user
        token.value = response.token
        localStorage.setItem('token', response.token)
        return { success: true }
      } else {
        return { success: false, error: response.errorMessage }
      }
    } catch (error) {
      return { success: false, error: error.message }
    }
  }

  const logout = () => {
    user.value = null
    token.value = null
    localStorage.removeItem('token')
  }

  const getCurrentUser = async () => {
    if (!token.value) return
    
    try {
      const response = await authService.getCurrentUser()
      if (response.isSuccess) {
        user.value = response.user
      } else {
        logout()
      }
    } catch (error) {
      logout()
    }
  }

  return {
    user,
    token,
    isAuthenticated,
    login,
    register,
    logout,
    getCurrentUser
  }
})
