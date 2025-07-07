<template>
  <div class="min-h-screen bg-gradient-to-br from-green-50 to-emerald-100 flex items-center justify-center p-4">
    <!-- Register Card -->
    <div class="w-full max-w-md">
      <!-- Header -->
      <div class="text-center mb-8">
        <div class="mx-auto w-12 h-12 bg-green-600 rounded-xl flex items-center justify-center mb-4">
          <v-icon size="24" color="white">mdi-account-plus-outline</v-icon>
        </div>
        <h1 class="text-2xl font-bold text-gray-900 mb-2">Create account</h1>
        <p class="text-gray-600">Join thousands of productive users</p>
      </div>

      <!-- Register Form -->
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
                class="block w-full pl-10 pr-3 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-green-500 transition-colors"
                :class="{ 'border-red-500 focus:ring-red-500 focus:border-red-500': errors.username }"
                placeholder="Enter your username"
                @input="clearError('username')"
              />
            </div>
            <p v-if="errors.username" class="text-sm text-red-600">{{ errors.username }}</p>
          </div>

          <!-- Email Field -->
          <div class="space-y-1">
            <label class="block text-sm font-medium text-gray-700">Email</label>
            <div class="relative">
              <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                <v-icon size="20" color="gray">mdi-email-outline</v-icon>
              </div>
              <input
                v-model="form.email"
                type="email"
                required
                class="block w-full pl-10 pr-3 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-green-500 transition-colors"
                :class="{ 'border-red-500 focus:ring-red-500 focus:border-red-500': errors.email }"
                placeholder="Enter your email"
                @input="clearError('email')"
              />
            </div>
            <p v-if="errors.email" class="text-sm text-red-600">{{ errors.email }}</p>
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
                class="block w-full pl-10 pr-12 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-green-500 transition-colors"
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

          <!-- Confirm Password Field -->
          <div class="space-y-1">
            <label class="block text-sm font-medium text-gray-700">Confirm Password</label>
            <div class="relative">
              <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                <v-icon size="20" color="gray">mdi-lock-check-outline</v-icon>
              </div>
              <input
                v-model="form.confirmPassword"
                :type="showConfirmPassword ? 'text' : 'password'"
                required
                class="block w-full pl-10 pr-12 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-green-500 transition-colors"
                :class="{ 'border-red-500 focus:ring-red-500 focus:border-red-500': errors.confirmPassword }"
                placeholder="Confirm your password"
                @input="clearError('confirmPassword')"
              />
              <button
                type="button"
                @click="toggleConfirmPasswordVisibility"
                class="absolute inset-y-0 right-0 pr-3 flex items-center"
              >
                <v-icon size="20" color="gray">
                  {{ showConfirmPassword ? 'mdi-eye-off' : 'mdi-eye' }}
                </v-icon>
              </button>
            </div>
            <p v-if="errors.confirmPassword" class="text-sm text-red-600">{{ errors.confirmPassword }}</p>
          </div>

          <!-- Terms & Conditions -->
          <div class="flex items-start">
            <input
              v-model="form.acceptTerms"
              type="checkbox"
              required
              class="h-4 w-4 text-green-600 focus:ring-green-500 border-gray-300 rounded mt-1"
            />
            <div class="ml-3">
              <label class="text-sm text-gray-700">
                I agree to the 
                <button type="button" class="text-green-600 hover:text-green-800 font-medium">
                  Terms and Conditions
                </button>
                and 
                <button type="button" class="text-green-600 hover:text-green-800 font-medium">
                  Privacy Policy
                </button>
              </label>
            </div>
          </div>

          <!-- Submit Button -->
          <button
            type="submit"
            :disabled="loading || !isFormValid"
            class="w-full flex justify-center items-center py-3 px-4 border border-transparent rounded-lg shadow-sm text-sm font-medium text-white bg-green-600 hover:bg-green-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-green-500 disabled:opacity-50 disabled:cursor-not-allowed transition-colors"
          >
            <v-progress-circular
              v-if="loading"
              indeterminate
              size="20"
              width="2"
              color="white"
              class="mr-2"
            />
            <v-icon v-else size="20" class="mr-2">mdi-account-plus</v-icon>
            {{ loading ? 'Creating account...' : 'Create account' }}
          </button>
        </form>

        <!-- Login Link -->
        <div class="mt-6 text-center">
          <span class="text-sm text-gray-600">Already have an account? </span>
          <router-link
            :to="{ name: 'login' }"
            class="text-sm font-medium text-green-600 hover:text-green-800 transition-colors"
          >
            Sign in
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

// Validation Service for Registration (Single Responsibility Principle)
class RegisterValidationService {
  static validateUsername(username) {
    if (!username) return 'Username is required'
    if (username.length < 3) return 'Username must be at least 3 characters'
    if (!/^[a-zA-Z0-9_]+$/.test(username)) return 'Username can only contain letters, numbers, and underscores'
    return null
  }

  static validateEmail(email) {
    if (!email) return 'Email is required'
    if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) return 'Please enter a valid email address'
    return null
  }

  static validatePassword(password) {
    if (!password) return 'Password is required'
    if (password.length < 8) return 'Password must be at least 8 characters'
    if (!/(?=.*[a-z])(?=.*[A-Z])(?=.*\d)/.test(password)) return 'Password must contain uppercase, lowercase, and number'
    return null
  }

  static validateConfirmPassword(password, confirmPassword) {
    if (!confirmPassword) return 'Please confirm your password'
    if (password !== confirmPassword) return 'Passwords do not match'
    return null
  }
}

// Registration Form State Manager
class RegisterFormStateManager {
  constructor() {
    this.form = reactive({
      username: '',
      email: '',
      password: '',
      confirmPassword: '',
      acceptTerms: false
    })
    
    this.errors = reactive({
      username: '',
      email: '',
      password: '',
      confirmPassword: ''
    })
  }

  clearError(field) {
    this.errors[field] = ''
  }

  clearAllErrors() {
    Object.keys(this.errors).forEach(key => {
      this.errors[key] = ''
    })
  }

  validate() {
    this.clearAllErrors()
    
    const usernameError = RegisterValidationService.validateUsername(this.form.username)
    const emailError = RegisterValidationService.validateEmail(this.form.email)
    const passwordError = RegisterValidationService.validatePassword(this.form.password)
    const confirmPasswordError = RegisterValidationService.validateConfirmPassword(this.form.password, this.form.confirmPassword)
    
    if (usernameError) this.errors.username = usernameError
    if (emailError) this.errors.email = emailError
    if (passwordError) this.errors.password = passwordError
    if (confirmPasswordError) this.errors.confirmPassword = confirmPasswordError
    
    return !usernameError && !emailError && !passwordError && !confirmPasswordError && this.form.acceptTerms
  }

  isValid() {
    return this.form.username.trim() && 
           this.form.email.trim() && 
           this.form.password.trim() && 
           this.form.confirmPassword.trim() &&
           this.form.acceptTerms &&
           !Object.values(this.errors).some(error => error)
  }
}

// Registration Service
class RegistrationService {
  constructor(userStore, appStore, router) {
    this.userStore = userStore
    this.appStore = appStore
    this.router = router
  }

  async register(userData) {
    try {
      const result = await this.userStore.register(userData)
      
      if (result.success) {
        this.appStore.showSnackbar('Account created successfully! Please sign in.', 'success')
        setTimeout(() => {
          this.router.push({ name: 'login' })
        }, 1000)
        return { success: true }
      } else {
        this.appStore.showSnackbar(result.error || 'Failed to create account. Please try again.', 'error')
        return { success: false, error: result.error }
      }
    } catch (error) {
      console.error('Registration error:', error)
      this.appStore.showSnackbar('Unable to connect. Please check your connection and try again.', 'error')
      return { success: false, error: 'Connection failed' }
    }
  }
}

export default {
  name: 'RegisterView',
  setup() {
    // Dependencies
    const router = useRouter()
    const userStore = useUserStore()
    const appStore = useAppStore()
    
    // Services
    const formManager = new RegisterFormStateManager()
    const registrationService = new RegistrationService(userStore, appStore, router)
    
    // Reactive state
    const loading = ref(false)
    const showPassword = ref(false)
    const showConfirmPassword = ref(false)
    
    // Computed properties
    const isFormValid = computed(() => formManager.isValid())
    
    // Methods
    const togglePasswordVisibility = () => {
      showPassword.value = !showPassword.value
    }
    
    const toggleConfirmPasswordVisibility = () => {
      showConfirmPassword.value = !showConfirmPassword.value
    }
    
    const clearError = (field) => {
      formManager.clearError(field)
    }
    
    const handleSubmit = async () => {
      if (!formManager.validate()) return
      
      loading.value = true
      
      try {
        const result = await registrationService.register({
          username: formManager.form.username,
          email: formManager.form.email,
          password: formManager.form.password
        })
        
        if (!result.success && result.error) {
          // Set specific field errors if available
          if (result.error.includes('username')) {
            formManager.errors.username = 'Username already exists'
          } else if (result.error.includes('email')) {
            formManager.errors.email = 'Email already registered'
          }
        }
      } finally {
        loading.value = false
      }
    }
    
    return {
      form: formManager.form,
      errors: formManager.errors,
      loading,
      showPassword,
      showConfirmPassword,
      isFormValid,
      togglePasswordVisibility,
      toggleConfirmPasswordVisibility,
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
