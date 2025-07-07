<template>
  <v-app class="app-layout">
    <!-- Mobile Navigation Drawer -->
    <v-navigation-drawer
      v-model="drawer"
      temporary
      :width="280"
      class="mobile-drawer"
      :class="{ 'mobile-only': $vuetify.display.mdAndUp }"
    >
      <div class="drawer-header">
        <div class="user-info">
          <v-avatar size="48" class="user-avatar">
            <v-img
              :src="userStore.user?.avatar || `https://ui-avatars.com/api/?name=${userStore.user?.username || 'User'}&background=1976d2&color=fff`"
              :alt="userStore.user?.username || 'User'"
            />
          </v-avatar>
          <div class="user-details">
            <div class="user-name">{{ userStore.user?.username || 'Guest' }}</div>
            <div class="user-role">{{ userStore.user?.role || 'Member' }}</div>
          </div>
        </div>
        <v-btn
          icon="mdi-close"
          variant="text"
          size="small"
          @click="drawer = false"
          class="close-btn"
        />
      </div>

      <v-divider />

      <nav class="drawer-nav">
        <v-list nav class="nav-list">
          <v-list-item
            v-for="item in navigationItems"
            :key="item.value"
            :to="item.to"
            :value="item.value"
            :prepend-icon="item.icon"
            :title="item.title"
            class="nav-item"
            :class="{ active: $route.name === item.value }"
          />
        </v-list>
      </nav>

      <template #append>
        <div class="drawer-footer">
          <v-btn
            block
            color="error"
            variant="outlined"
            @click="logout"
            prepend-icon="mdi-logout"
            class="logout-btn"
          >
            Logout
          </v-btn>
        </div>
      </template>
    </v-navigation-drawer>

    <!-- Desktop Sidebar -->
    <aside 
      v-if="$vuetify.display.mdAndUp"
      class="desktop-sidebar"
      :class="{ collapsed: sidebarCollapsed }"
    >
      <div class="sidebar-header">
        <div class="brand">
          <v-icon size="32" color="primary" class="brand-icon">mdi-check-circle-outline</v-icon>
          <transition name="fade">
            <span v-if="!sidebarCollapsed" class="brand-text">TaskManager</span>
          </transition>
        </div>
        <v-btn
          icon
          variant="text"
          size="small"
          @click="sidebarCollapsed = !sidebarCollapsed"
          class="collapse-btn"
        >
          <v-icon>{{ sidebarCollapsed ? 'mdi-chevron-right' : 'mdi-chevron-left' }}</v-icon>
        </v-btn>
      </div>

      <nav class="sidebar-nav">
        <v-list nav class="nav-list">
          <v-list-item
            v-for="item in navigationItems"
            :key="item.value"
            :to="item.to"
            :value="item.value"
            :prepend-icon="item.icon"
            :title="sidebarCollapsed ? '' : item.title"
            class="nav-item"
            :class="{ active: $route.name === item.value }"
          >
            <v-tooltip
              v-if="sidebarCollapsed"
              activator="parent"
              location="end"
              :text="item.title"
            />
          </v-list-item>
        </v-list>
      </nav>

      <div class="sidebar-footer">
        <div class="user-section" :class="{ collapsed: sidebarCollapsed }">
          <v-avatar size="36" class="user-avatar">
            <v-img
              :src="userStore.user?.avatar || `https://ui-avatars.com/api/?name=${userStore.user?.username || 'User'}&background=1976d2&color=fff`"
              :alt="userStore.user?.username || 'User'"
            />
          </v-avatar>
          <transition name="fade">
            <div v-if="!sidebarCollapsed" class="user-info">
              <div class="user-name">{{ userStore.user?.username || 'Guest' }}</div>
              <v-btn
                icon="mdi-logout"
                variant="text"
                size="small"
                @click="logout"
                class="logout-icon"
              />
            </div>
          </transition>
        </div>
      </div>
    </aside>

    <!-- Main App Bar (Mobile) -->
    <v-app-bar
      v-if="$vuetify.display.smAndDown"
      elevation="0"
      class="mobile-app-bar"
    >
      <v-app-bar-nav-icon @click="drawer = !drawer" />
      <v-toolbar-title class="app-title">
        <v-icon size="24" color="primary" start>mdi-check-circle-outline</v-icon>
        TaskManager
      </v-toolbar-title>
      <v-spacer />
      <v-btn icon="mdi-bell-outline" variant="text" class="notification-btn" />
    </v-app-bar>

    <!-- Main Content Area -->
    <v-main class="main-content" :class="{ 'with-sidebar': $vuetify.display.mdAndUp }">
      <div class="content-wrapper">
        <router-view />
      </div>
    </v-main>

    <!-- Global Loading Overlay -->
    <v-overlay v-model="loading" class="loading-overlay">
      <div class="loading-content">
        <v-progress-circular
          color="primary"
          indeterminate
          size="64"
          width="4"
        />
        <div class="loading-text">Loading...</div>
      </div>
    </v-overlay>

    <!-- Global Snackbar -->
    <v-snackbar
      v-model="snackbar.show"
      :color="snackbar.color"
      :timeout="snackbar.timeout"
      location="bottom right"
      class="modern-snackbar"
    >
      <div class="snackbar-content">
        <v-icon size="20" start>
          {{ getSnackbarIcon(snackbar.color) }}
        </v-icon>
        {{ snackbar.message }}
      </div>
      <template #actions>
        <v-btn
          icon="mdi-close"
          variant="text"
          size="small"
          @click="snackbar.show = false"
        />
      </template>
    </v-snackbar>
  </v-app>
</template>

<script>
import { ref, computed, onMounted } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { useUserStore } from '@/stores/user'
import { useAppStore } from '@/stores/app'

export default {
  name: 'AppLayout',
  setup() {
    const drawer = ref(false)
    const sidebarCollapsed = ref(false)
    const router = useRouter()
    const route = useRoute()
    const userStore = useUserStore()
    const appStore = useAppStore()

    const loading = computed(() => appStore.loading)
    const snackbar = computed(() => appStore.snackbar)

    const navigationItems = [
      {
        title: 'Dashboard',
        icon: 'mdi-view-dashboard',
        value: 'dashboard',
        to: { name: 'dashboard' }
      },
      {
        title: 'Projects',
        icon: 'mdi-folder-multiple',
        value: 'projects',
        to: { name: 'projects' }
      },
      {
        title: 'Tasks',
        icon: 'mdi-format-list-checks',
        value: 'tasks',
        to: { name: 'tasks' }
      }
    ]

    const logout = async () => {
      try {
        await userStore.logout()
        router.push({ name: 'login' })
      } catch (error) {
        appStore.showSnackbar('Failed to logout', 'error')
      }
    }

    const getSnackbarIcon = (color) => {
      switch (color) {
        case 'success': return 'mdi-check-circle'
        case 'error': return 'mdi-alert-circle'
        case 'warning': return 'mdi-alert'
        case 'info': return 'mdi-information'
        default: return 'mdi-information'
      }
    }

    onMounted(() => {
      // Check if user is already logged in
      if (userStore.token && !userStore.user) {
        userStore.getCurrentUser()
      }
    })

    return {
      drawer,
      sidebarCollapsed,
      userStore,
      loading,
      snackbar,
      navigationItems,
      logout,
      getSnackbarIcon
    }
  }
}
</script>

<style scoped>
:root {
  --primary-color: #1976d2;
  --secondary-color: #424242;
  --success-color: #4caf50;
  --error-color: #f44336;
  --warning-color: #ff9800;
  --info-color: #2196f3;
  --surface-color: #ffffff;
  --background-color: #f5f5f5;
  --text-primary: #212121;
  --text-secondary: #757575;
  --border-color: #e0e0e0;
  --sidebar-width: 280px;
  --sidebar-width-collapsed: 72px;
  --shadow-light: 0 2px 8px rgba(0, 0, 0, 0.1);
  --shadow-medium: 0 4px 16px rgba(0, 0, 0, 0.12);
  --border-radius: 8px;
  --transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
}

.app-layout {
  display: flex;
  min-height: 100vh;
  background: var(--background-color);
}

/* Mobile Drawer */
.mobile-drawer {
  background: var(--surface-color) !important;
  border-right: 1px solid var(--border-color) !important;
}

.mobile-drawer.mobile-only {
  display: none;
}

.drawer-header {
  padding: 1.5rem;
  display: flex;
  align-items: center;
  justify-content: space-between;
  background: linear-gradient(135deg, #f8f9fa 0%, #e9ecef 100%);
}

.user-info {
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.user-avatar {
  box-shadow: var(--shadow-light);
  border: 2px solid var(--primary-color);
}

.user-details {
  display: flex;
  flex-direction: column;
}

.user-name {
  font-weight: 600;
  color: var(--text-primary);
  font-size: 0.875rem;
}

.user-role {
  font-size: 0.75rem;
  color: var(--text-secondary);
  text-transform: uppercase;
  letter-spacing: 0.5px;
}

.close-btn {
  color: var(--text-secondary) !important;
}

/* Desktop Sidebar */
.desktop-sidebar {
  position: fixed;
  left: 0;
  top: 0;
  height: 100vh;
  width: var(--sidebar-width);
  background: var(--surface-color);
  border-right: 1px solid var(--border-color);
  display: flex;
  flex-direction: column;
  transition: var(--transition);
  z-index: 1000;
  box-shadow: var(--shadow-medium);
}

.desktop-sidebar.collapsed {
  width: var(--sidebar-width-collapsed);
}

.sidebar-header {
  padding: 1.5rem;
  display: flex;
  align-items: center;
  justify-content: space-between;
  border-bottom: 1px solid var(--border-color);
  background: linear-gradient(135deg, #f8f9fa 0%, #e9ecef 100%);
  min-height: 80px;
}

.brand {
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.brand-icon {
  flex-shrink: 0;
}

.brand-text {
  font-size: 1.25rem;
  font-weight: 700;
  color: var(--text-primary);
  white-space: nowrap;
}

.collapse-btn {
  color: var(--text-secondary) !important;
  flex-shrink: 0;
}

/* Navigation */
.drawer-nav,
.sidebar-nav {
  flex: 1;
  padding: 1rem 0;
  overflow-y: auto;
}

.nav-list {
  padding: 0 0.75rem;
}

.nav-item {
  margin-bottom: 0.25rem;
  border-radius: var(--border-radius) !important;
  transition: var(--transition) !important;
  position: relative;
}

.nav-item:hover {
  background: rgba(var(--primary-color), 0.08) !important;
  transform: translateX(2px);
}

.nav-item.active {
  background: rgba(var(--primary-color), 0.12) !important;
  color: var(--primary-color) !important;
}

.nav-item.active::before {
  content: '';
  position: absolute;
  left: -0.75rem;
  top: 0;
  bottom: 0;
  width: 3px;
  background: var(--primary-color);
  border-radius: 0 2px 2px 0;
}

/* Sidebar Footer */
.drawer-footer,
.sidebar-footer {
  padding: 1rem;
  border-top: 1px solid var(--border-color);
  background: var(--surface-color);
}

.user-section {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  padding: 0.75rem;
  border-radius: var(--border-radius);
  background: rgba(var(--primary-color), 0.05);
  border: 1px solid rgba(var(--primary-color), 0.1);
  transition: var(--transition);
}

.user-section.collapsed {
  justify-content: center;
  padding: 0.75rem;
}

.user-section:hover {
  background: rgba(var(--primary-color), 0.08);
}

.user-section .user-info {
  flex: 1;
  display: flex;
  align-items: center;
  justify-content: space-between;
}

.logout-icon {
  color: var(--error-color) !important;
}

.logout-btn {
  border-radius: var(--border-radius) !important;
  text-transform: none !important;
  font-weight: 500 !important;
}

/* Mobile App Bar */
.mobile-app-bar {
  background: var(--surface-color) !important;
  border-bottom: 1px solid var(--border-color) !important;
  box-shadow: var(--shadow-light) !important;
}

.app-title {
  font-weight: 600 !important;
  display: flex !important;
  align-items: center !important;
  gap: 0.5rem !important;
}

.notification-btn {
  color: var(--text-secondary) !important;
}

/* Main Content */
.main-content {
  background: var(--background-color);
  min-height: 100vh;
  transition: var(--transition);
}

.main-content.with-sidebar {
  margin-left: var(--sidebar-width);
}

.main-content.with-sidebar.collapsed {
  margin-left: var(--sidebar-width-collapsed);
}

.content-wrapper {
  padding: 2rem;
  max-width: 1400px;
  margin: 0 auto;
}

/* Global Components */
.loading-overlay {
  background: rgba(255, 255, 255, 0.9) !important;
  backdrop-filter: blur(4px);
}

.loading-content {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 1rem;
}

.loading-text {
  font-size: 1rem;
  font-weight: 500;
  color: var(--text-primary);
}

.modern-snackbar {
  border-radius: var(--border-radius) !important;
  box-shadow: var(--shadow-medium) !important;
}

.snackbar-content {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  font-weight: 500;
}

/* Transitions */
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.3s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}

/* Responsive Design */
@media (max-width: 1264px) {
  .main-content.with-sidebar {
    margin-left: 0;
  }
  
  .desktop-sidebar {
    transform: translateX(-100%);
  }
  
  .desktop-sidebar.show {
    transform: translateX(0);
  }
}

@media (max-width: 960px) {
  .content-wrapper {
    padding: 1.5rem;
  }
}

@media (max-width: 600px) {
  .content-wrapper {
    padding: 1rem;
  }
  
  .drawer-header,
  .sidebar-header {
    padding: 1rem;
  }
  
  .user-info {
    gap: 0.5rem;
  }
  
  .user-name {
    font-size: 0.8rem;
  }
  
  .user-role {
    font-size: 0.7rem;
  }
}

/* Dark Mode Support */
@media (prefers-color-scheme: dark) {
  :root {
    --surface-color: #1e1e1e;
    --background-color: #121212;
    --text-primary: #ffffff;
    --text-secondary: #aaaaaa;
    --border-color: #333333;
  }
  
  .drawer-header,
  .sidebar-header {
    background: linear-gradient(135deg, #2a2a2a 0%, #333333 100%);
  }
  
  .user-section {
    background: rgba(var(--primary-color), 0.1);
    border-color: rgba(var(--primary-color), 0.2);
  }
  
  .loading-overlay {
    background: rgba(0, 0, 0, 0.8) !important;
  }
}

/* Accessibility */
@media (prefers-reduced-motion: reduce) {
  * {
    animation-duration: 0.01ms !important;
    animation-iteration-count: 1 !important;
    transition-duration: 0.01ms !important;
  }
}

/* Focus States */
.nav-item:focus-visible,
.collapse-btn:focus-visible,
.close-btn:focus-visible,
.logout-btn:focus-visible {
  outline: 2px solid var(--primary-color);
  outline-offset: 2px;
}

/* Custom Scrollbar */
.drawer-nav::-webkit-scrollbar,
.sidebar-nav::-webkit-scrollbar {
  width: 6px;
}

.drawer-nav::-webkit-scrollbar-track,
.sidebar-nav::-webkit-scrollbar-track {
  background: transparent;
}

.drawer-nav::-webkit-scrollbar-thumb,
.sidebar-nav::-webkit-scrollbar-thumb {
  background: var(--border-color);
  border-radius: 3px;
}

.drawer-nav::-webkit-scrollbar-thumb:hover,
.sidebar-nav::-webkit-scrollbar-thumb:hover {
  background: var(--text-secondary);
}
</style>
