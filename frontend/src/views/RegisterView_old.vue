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
            @submit.prevent="register"
          >
            <div class="input-group">
              <v-text-field
                v-model="userData.username"
                :rules="usernameRules"
                label="Username"
                name="username"
                prepend-inner-icon="mdi-account-outline"
                variant="outlined"
                color="primary"
                class="modern-input"
                :error-messages="errors.username"
                @focus="clearError('username')"
                required
              ></v-text-field>
            </div>

            <div class="input-group">
              <v-text-field
                v-model="userData.password"
                :rules="passwordRules"
                label="Password"
                name="password"
                prepend-inner-icon="mdi-lock-outline"
                :append-inner-icon="showPassword ? 'mdi-eye-off' : 'mdi-eye'"
                :type="showPassword ? 'text' : 'password'"
                variant="outlined"
                color="primary"
                class="modern-input"
                :error-messages="errors.password"
                @focus="clearError('password')"
                @click:append-inner="showPassword = !showPassword"
                required
              ></v-text-field>
            </div>

            <div class="input-group">
              <v-text-field
                v-model="confirmPassword"
                :rules="confirmPasswordRules"
                label="Confirm Password"
                name="confirmPassword"
                prepend-inner-icon="mdi-lock-check-outline"
                :append-inner-icon="showConfirmPassword ? 'mdi-eye-off' : 'mdi-eye'"
                :type="showConfirmPassword ? 'text' : 'password'"
                variant="outlined"
                color="primary"
                class="modern-input"
                :error-messages="errors.confirmPassword"
                @focus="clearError('confirmPassword')"
                @click:append-inner="showConfirmPassword = !showConfirmPassword"
                required
              ></v-text-field>
            </div>

            <div class="form-options">
              <v-checkbox
                v-model="agreeToTerms"
                color="primary"
                density="compact"
                hide-details
              >
                <template v-slot:label>
                  <span class="terms-text">
                    I agree to the 
                    <v-btn variant="text" size="small" color="primary" class="terms-link">
                      Terms of Service
                    </v-btn>
                    and
                    <v-btn variant="text" size="small" color="primary" class="terms-link">
                      Privacy Policy
                    </v-btn>
                  </span>
                </template>
              </v-checkbox>
            </div>

            <v-btn
              :disabled="!valid || !agreeToTerms || loading"
              :loading="loading"
              color="primary"
              size="large"
              type="submit"
              class="register-btn"
              block
              elevation="0"
            >
              <v-icon start>mdi-account-plus</v-icon>
              Create Account
            </v-btn>
          </v-form>

          <div class="divider">
            <span class="divider-text">or sign up with</span>
          </div>

          <div class="social-login">
            <v-btn
              variant="outlined"
              color="grey-darken-1"
              class="social-btn"
              disabled
            >
              <v-icon start>mdi-google</v-icon>
              Google
            </v-btn>
            <v-btn
              variant="outlined"
              color="grey-darken-1"
              class="social-btn"
              disabled
            >
              <v-icon start>mdi-github</v-icon>
              GitHub
            </v-btn>
          </div>

          <div class="form-footer">
            <span class="footer-text">Already have an account?</span>
            <v-btn
              variant="text"
              color="primary"
              :to="{ name: 'login' }"
              class="login-link"
            >
              Sign in
            </v-btn>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { ref, reactive } from 'vue'
import { useRouter } from 'vue-router'
import { useUserStore } from '@/stores/user'
import { useAppStore } from '@/stores/app'

export default {
  name: 'RegisterView',
  setup() {
    const router = useRouter()
    const userStore = useUserStore()
    const appStore = useAppStore()

    const valid = ref(false)
    const loading = ref(false)
    const showPassword = ref(false)
    const showConfirmPassword = ref(false)
    const agreeToTerms = ref(false)
    
    const userData = ref({
      username: '',
      password: ''
    })
    const confirmPassword = ref('')

    const errors = reactive({
      username: '',
      password: '',
      confirmPassword: ''
    })

    const usernameRules = [
      v => !!v || 'Username is required',
      v => (v && v.length >= 3) || 'Username must be at least 3 characters',
      v => (v && v.length <= 20) || 'Username must be less than 20 characters',
      v => /^[a-zA-Z0-9_]+$/.test(v) || 'Username can only contain letters, numbers, and underscores'
    ]

    const passwordRules = [
      v => !!v || 'Password is required',
      v => (v && v.length >= 6) || 'Password must be at least 6 characters',
      v => (v && v.length <= 50) || 'Password must be less than 50 characters'
    ]

    const confirmPasswordRules = [
      v => !!v || 'Password confirmation is required',
      v => v === userData.value.password || 'Passwords do not match'
    ]

    const clearError = (field) => {
      errors[field] = ''
    }

    const register = async () => {
      if (!valid.value || !agreeToTerms.value) return

      loading.value = true
      clearError('username')
      clearError('password')
      clearError('confirmPassword')

      try {
        const result = await userStore.register(userData.value)
        if (result.success) {
          appStore.showSnackbar('Account created successfully! Welcome aboard!', 'success')
          // Add slight delay for better UX
          setTimeout(() => {
            router.push({ name: 'dashboard' })
          }, 500)
        } else {
          appStore.showSnackbar(result.error || 'Registration failed. Please try again.', 'error')
          
          // Set specific field errors if available
          if (result.error?.includes('username')) {
            errors.username = 'Username already taken'
          }
        }
      } catch (error) {
        console.error('Registration error:', error)
        appStore.showSnackbar('Unable to create account. Please check your connection and try again.', 'error')
      } finally {
        loading.value = false
      }
    }

    return {
      valid,
      loading,
      showPassword,
      showConfirmPassword,
      agreeToTerms,
      userData,
      confirmPassword,
      errors,
      usernameRules,
      passwordRules,
      confirmPasswordRules,
      clearError,
      register
    }
  }
}
</script>

<style scoped>
/* Use the same styles as LoginView but with register-specific class names */
.register-container {
  --primary-color: #1976d2;
  --primary-light: #42a5f5;
  --primary-dark: #1565c0;
  --secondary-color: #f5f5f5;
  --accent-color: #ff4081;
  --success-color: #4caf50;
  --warning-color: #ff9800;
  --error-color: #f44336;
  --text-primary: #212121;
  --text-secondary: #757575;
  --background-primary: #ffffff;
  --background-secondary: #fafafa;
  --surface: #ffffff;
  --border-color: #e0e0e0;
  --shadow-light: 0 2px 4px rgba(0,0,0,0.05);
  --shadow-medium: 0 4px 12px rgba(0,0,0,0.1);
  --shadow-heavy: 0 8px 24px rgba(0,0,0,0.15);
  --border-radius: 12px;
  --border-radius-small: 8px;
  --border-radius-large: 16px;
  --transition-fast: 0.2s ease;
  --transition-medium: 0.3s ease;
  --transition-slow: 0.5s ease;
  --font-family: 'Roboto', -apple-system, BlinkMacSystemFont, 'Segoe UI', sans-serif;
}

.register-container {
  min-height: 100vh;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  position: relative;
  overflow: hidden;
  font-family: var(--font-family);
}

.bg-decoration {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  overflow: hidden;
  z-index: 0;
}

.bg-circle {
  position: absolute;
  border-radius: 50%;
  background: rgba(255, 255, 255, 0.1);
  backdrop-filter: blur(10px);
  animation: float 6s ease-in-out infinite;
}

.bg-circle-1 {
  width: 300px;
  height: 300px;
  top: -150px;
  right: -150px;
  animation-delay: 0s;
}

.bg-circle-2 {
  width: 200px;
  height: 200px;
  bottom: -100px;
  left: -100px;
  animation-delay: 2s;
}

.bg-circle-3 {
  width: 150px;
  height: 150px;
  top: 50%;
  right: 10%;
  animation-delay: 4s;
}

@keyframes float {
  0%, 100% { transform: translateY(0px) rotate(0deg); }
  50% { transform: translateY(-20px) rotate(180deg); }
}

.register-grid {
  position: relative;
  z-index: 1;
  min-height: 100vh;
  display: grid;
  grid-template-columns: 1fr;
  max-width: 1400px;
  margin: 0 auto;
}

.brand-section {
  display: none;
  background: rgba(255, 255, 255, 0.1);
  backdrop-filter: blur(20px);
  border-right: 1px solid rgba(255, 255, 255, 0.2);
  position: relative;
  overflow: hidden;
}

.brand-content {
  padding: 3rem;
  height: 100%;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  text-align: center;
  color: white;
}

.brand-logo {
  margin-bottom: 2rem;
  animation: pulse 2s ease-in-out infinite;
}

@keyframes pulse {
  0%, 100% { transform: scale(1); }
  50% { transform: scale(1.05); }
}

.brand-title {
  font-size: 3rem;
  font-weight: 700;
  margin-bottom: 1rem;
  background: linear-gradient(135deg, #ffffff, #e3f2fd);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
  text-shadow: 0 4px 8px rgba(0,0,0,0.1);
}

.brand-subtitle {
  font-size: 1.25rem;
  margin-bottom: 3rem;
  opacity: 0.9;
  font-weight: 300;
}

.feature-list {
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
}

.feature-item {
  display: flex;
  align-items: center;
  gap: 1rem;
  font-size: 1.1rem;
  opacity: 0.9;
  transition: var(--transition-medium);
}

.feature-item:hover {
  opacity: 1;
  transform: translateX(5px);
}

.form-section {
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 2rem;
  background: rgba(255, 255, 255, 0.05);
  backdrop-filter: blur(10px);
}

.form-container {
  width: 100%;
  max-width: 420px;
  background: var(--background-primary);
  border-radius: var(--border-radius-large);
  box-shadow: var(--shadow-heavy);
  padding: 2.5rem;
  border: 1px solid rgba(255, 255, 255, 0.2);
  animation: slideUp 0.6s ease-out;
}

@keyframes slideUp {
  from {
    opacity: 0;
    transform: translateY(30px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.form-header {
  text-align: center;
  margin-bottom: 2.5rem;
}

.form-title {
  font-size: 2rem;
  font-weight: 600;
  color: var(--text-primary);
  margin-bottom: 0.5rem;
}

.form-subtitle {
  color: var(--text-secondary);
  font-size: 1rem;
  font-weight: 400;
}

.register-form {
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
}

.input-group {
  position: relative;
}

.modern-input :deep(.v-field) {
  border-radius: var(--border-radius) !important;
  box-shadow: var(--shadow-light);
  transition: var(--transition-medium);
}

.modern-input :deep(.v-field:hover) {
  box-shadow: var(--shadow-medium);
}

.modern-input :deep(.v-field--focused) {
  box-shadow: 0 0 0 2px rgba(25, 118, 210, 0.2);
}

.form-options {
  margin: 0.5rem 0;
}

.terms-text {
  font-size: 0.875rem;
  color: var(--text-secondary);
}

.terms-link {
  padding: 0 !important;
  min-width: auto !important;
  height: auto !important;
  text-decoration: underline;
  font-size: 0.875rem;
}

.register-btn {
  margin-top: 1rem;
  height: 48px;
  border-radius: var(--border-radius) !important;
  font-weight: 600;
  font-size: 1rem;
  text-transform: none;
  letter-spacing: 0.5px;
  box-shadow: var(--shadow-medium);
  transition: var(--transition-medium);
}

.register-btn:hover {
  box-shadow: var(--shadow-heavy);
  transform: translateY(-2px);
}

.divider {
  position: relative;
  text-align: center;
  margin: 2rem 0;
}

.divider::before {
  content: '';
  position: absolute;
  top: 50%;
  left: 0;
  right: 0;
  height: 1px;
  background: var(--border-color);
}

.divider-text {
  background: var(--background-primary);
  padding: 0 1rem;
  color: var(--text-secondary);
  font-size: 0.875rem;
  position: relative;
  z-index: 1;
}

.social-login {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 1rem;
  margin-bottom: 2rem;
}

.social-btn {
  border-radius: var(--border-radius) !important;
  height: 44px;
  text-transform: none;
  font-weight: 500;
  transition: var(--transition-medium);
}

.social-btn:hover {
  transform: translateY(-1px);
  box-shadow: var(--shadow-medium);
}

.form-footer {
  text-align: center;
  padding-top: 1rem;
  border-top: 1px solid var(--border-color);
}

.footer-text {
  color: var(--text-secondary);
  font-size: 0.95rem;
  margin-right: 0.5rem;
}

.login-link {
  font-weight: 600;
  text-transform: none;
}

/* ===== RESPONSIVE BREAKPOINTS ===== */
@media (min-width: 576px) {
  .form-container {
    padding: 3rem;
  }
}

@media (min-width: 768px) {
  .register-grid {
    grid-template-columns: 1fr 1fr;
  }
  
  .brand-section {
    display: flex;
  }
  
  .form-section {
    background: var(--background-secondary);
    backdrop-filter: none;
  }
}

@media (min-width: 992px) {
  .register-grid {
    grid-template-columns: 3fr 2fr;
  }
  
  .brand-content {
    padding: 4rem;
  }
  
  .form-container {
    max-width: 480px;
    padding: 3.5rem;
  }
}

@media (min-width: 1200px) {
  .brand-title {
    font-size: 4rem;
  }
  
  .form-container {
    max-width: 520px;
    padding: 4rem;
  }
}

.modern-input :deep(.v-field--error) {
  animation: shake 0.5s ease-in-out;
}

@keyframes shake {
  0%, 100% { transform: translateX(0); }
  25% { transform: translateX(-5px); }
  75% { transform: translateX(5px); }
}
</style>
