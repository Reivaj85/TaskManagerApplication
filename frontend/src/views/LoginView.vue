<template>
  <div class="min-h-screen bg-gradient-to-br from-blue-50 to-indigo-100 flex items-center justify-center p-4">
    <!-- Login Card -->
    <div class="w-full max-w-md">
      <!-- Header -->
      <div class="text-center mb-8">
        <div class="mx-auto w-12 h-12 bg-blue-600 rounded-xl flex items-center justify-center mb-4">
          <v-icon size="24" color="white">mdi-check-circle-outline</v-icon>
        </div>
        <h1 class="text-2xl font-bold text-gray-900 mb-2">Welcome back</h1>
        <p class="text-gray-600">Sign in to your account</p>
      </div>

      <!-- Login Form -->
      <div class="bg-white rounded-2xl shadow-xl p-6 sm:p-8">
        <form @submit.prevent="handleSubmit" class="space-y-6">
          <!-- Username Field -->
          <div class="space-y-1">
            <label class="block text-sm font-medium text-gray-700">Username</label>
            <div class="relative">
              <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                <v-icon size="20" color="gray">mdi-account-outline</v-icon>
              </div>
              <input
                v-model="form.username"
                type="text"
                required
                class="block w-full pl-10 pr-3 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 transition-colors"
                :class="{ 'border-red-500 focus:ring-red-500 focus:border-red-500': errors.username }"
                placeholder="Enter your username"
                @input="clearError('username')"
              />
            </div>
            <p v-if="errors.username" class="text-sm text-red-600">{{ errors.username }}</p>
          </div>

          <!-- Password Field -->
          <div class="space-y-1">
            <label class="block text-sm font-medium text-gray-700">Password</label>
            <div class="relative">
              <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                <v-icon size="20" color="gray">mdi-lock-outline</v-icon>
              </div>
              <input
                v-model="form.password"
                :type="showPassword ? 'text' : 'password'"
                required
                class="block w-full pl-10 pr-12 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 transition-colors"
                :class="{ 'border-red-500 focus:ring-red-500 focus:border-red-500': errors.password }"
                placeholder="Enter your password"
                @input="clearError('password')"
              />
              <button
                type="button"
                @click="togglePasswordVisibility"
                class="absolute inset-y-0 right-0 pr-3 flex items-center"
              >
                <v-icon size="20" color="gray">
                  {{ showPassword ? 'mdi-eye-off' : 'mdi-eye' }}
                </v-icon>
              </button>
            </div>
            <p v-if="errors.password" class="text-sm text-red-600">{{ errors.password }}</p>
          </div>

          <!-- Remember Me & Forgot Password -->
          <div class="flex items-center justify-between">
            <label class="flex items-center">
              <input
                v-model="form.rememberMe"
                type="checkbox"
                class="h-4 w-4 text-blue-600 focus:ring-blue-500 border-gray-300 rounded"
              />
              <span class="ml-2 text-sm text-gray-700">Remember me</span>
            </label>
            <button
              type="button"
              class="text-sm text-blue-600 hover:text-blue-800 font-medium"
            >
              Forgot password?
            </button>
          </div>

          <!-- Submit Button -->
          <button
            type="submit"
            :disabled="loading || !isFormValid"
            class="w-full flex justify-center items-center py-3 px-4 border border-transparent rounded-lg shadow-sm text-sm font-medium text-white bg-blue-600 hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 disabled:opacity-50 disabled:cursor-not-allowed transition-colors"
          >
            <v-progress-circular
              v-if="loading"
              indeterminate
              size="20"
              width="2"
              color="white"
              class="mr-2"
            />
            <v-icon v-else size="20" class="mr-2">mdi-login</v-icon>
            {{ loading ? 'Signing in...' : 'Sign in' }}
          </button>
        </form>

        <!-- Divider -->
        <div class="mt-6">
          <div class="relative">
            <div class="absolute inset-0 flex items-center">
              <div class="w-full border-t border-gray-300"></div>
            </div>
            <div class="relative flex justify-center text-sm">
              <span class="px-2 bg-white text-gray-500">Or continue with</span>
            </div>
          </div>
        </div>

        <!-- Social Login -->
        <div class="mt-6 grid grid-cols-2 gap-3">
          <button
            type="button"
            disabled
            class="w-full inline-flex justify-center py-2 px-4 border border-gray-300 rounded-lg shadow-sm bg-white text-sm font-medium text-gray-500 hover:bg-gray-50 transition-colors disabled:opacity-50"
          >
            <v-icon size="20" class="mr-2">mdi-google</v-icon>
            Google
          </button>
          <button
            type="button"
            disabled
            class="w-full inline-flex justify-center py-2 px-4 border border-gray-300 rounded-lg shadow-sm bg-white text-sm font-medium text-gray-500 hover:bg-gray-50 transition-colors disabled:opacity-50"
          >
            <v-icon size="20" class="mr-2">mdi-github</v-icon>
            GitHub
          </button>
        </div>

        <!-- Register Link -->
        <div class="mt-6 text-center">
          <span class="text-sm text-gray-600">Don't have an account? </span>
          <router-link
            :to="{ name: 'register' }"
            class="text-sm font-medium text-blue-600 hover:text-blue-800 transition-colors"
          >
            Create account
          </router-link>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { ref, reactive, computed } from 'vue'
import { useRouter } from 'vue-router'
import { useUserStore } from '@/stores/user'
import { useAppStore } from '@/stores/app'

// Validation Service (Single Responsibility Principle)
class ValidationService {
  static validateUsername(username) {
    if (!username) return 'Username is required'
    if (username.length < 3) return 'Username must be at least 3 characters'
    if (!/^[a-zA-Z0-9_]+$/.test(username)) return 'Username can only contain letters, numbers, and underscores'
    return null
  }

  static validatePassword(password) {
    if (!password) return 'Password is required'
    if (password.length < 6) return 'Password must be at least 6 characters'
    return null
  }
}

// Form State Manager (Single Responsibility Principle)
class FormStateManager {
  constructor() {
    this.form = reactive({
      username: '',
      password: '',
      rememberMe: false
    })
    
    this.errors = reactive({
      username: '',
      password: ''
    })
  }

  clearError(field) {
    this.errors[field] = ''
  }

  clearAllErrors() {
    this.errors.username = ''
    this.errors.password = ''
  }

  validate() {
    this.clearAllErrors()
    
    const usernameError = ValidationService.validateUsername(this.form.username)
    const passwordError = ValidationService.validatePassword(this.form.password)
    
    if (usernameError) this.errors.username = usernameError
    if (passwordError) this.errors.password = passwordError
    
    return !usernameError && !passwordError
  }

  isValid() {
    return this.form.username.trim() && 
           this.form.password.trim() && 
           !this.errors.username && 
           !this.errors.password
  }

  reset() {
    this.form.username = ''
    this.form.password = ''
    this.form.rememberMe = false
    this.clearAllErrors()
  }
}

// Authentication Service (Open/Closed Principle - can be extended without modification)
class AuthenticationService {
  constructor(userStore, appStore, router) {
    this.userStore = userStore
    this.appStore = appStore
    this.router = router
  }

  async login(credentials) {
    try {
      const result = await this.userStore.login(credentials)
      
      if (result.success) {
        this.appStore.showSnackbar('Welcome back! Login successful.', 'success')
        // Add slight delay for better UX
        setTimeout(() => {
          this.router.push({ name: 'dashboard' })
        }, 500)
        return { success: true }
      } else {
        this.appStore.showSnackbar(result.error || 'Invalid credentials. Please try again.', 'error')
        return { success: false, error: result.error }
      }
    } catch (error) {
      console.error('Login error:', error)
      this.appStore.showSnackbar('Unable to connect. Please check your connection and try again.', 'error')
      return { success: false, error: 'Connection failed' }
    }
  }
}

export default {
  name: 'LoginView',
  setup() {
    // Dependencies (Dependency Inversion Principle)
    const router = useRouter()
    const userStore = useUserStore()
    const appStore = useAppStore()
    
    // Services
    const formManager = new FormStateManager()
    const authService = new AuthenticationService(userStore, appStore, router)
    
    // Reactive state
    const loading = ref(false)
    const showPassword = ref(false)
    
    // Computed properties (Interface Segregation Principle)
    const isFormValid = computed(() => formManager.isValid())
    
    // Methods (Single Responsibility Principle)
    const togglePasswordVisibility = () => {
      showPassword.value = !showPassword.value
    }
    
    const clearError = (field) => {
      formManager.clearError(field)
    }
    
    const handleSubmit = async () => {
      if (!formManager.validate()) return
      
      loading.value = true
      
      try {
        const result = await authService.login({
          username: formManager.form.username,
          password: formManager.form.password,
          rememberMe: formManager.form.rememberMe
        })
        
        if (!result.success && result.error) {
          // Set specific field errors if available
          if (result.error.includes('username')) {
            formManager.errors.username = 'Username not found'
          } else if (result.error.includes('password')) {
            formManager.errors.password = 'Incorrect password'
          }
        }
      } finally {
        loading.value = false
      }
    }
    
    // Public interface (Interface Segregation Principle)
    return {
      form: formManager.form,
      errors: formManager.errors,
      loading,
      showPassword,
      isFormValid,
      togglePasswordVisibility,
      clearError,
      handleSubmit
    }
  }
}
</script>

<style scoped>
/* Custom focus styles for better accessibility */
input:focus {
  outline: none;
}

/* Smooth transitions for interactive elements */
button, input, a {
  transition: all 0.2s ease-in-out;
}

/* Loading state for form submission */
button:disabled {
  cursor: not-allowed;
}

/* Ensure proper spacing for mobile devices */
@media (max-width: 640px) {
  .container {
    padding: 1rem;
  }
}
</style>
