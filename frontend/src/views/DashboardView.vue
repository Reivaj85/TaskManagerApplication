<template>
  <div class="min-h-screen bg-gradient-to-br from-blue-50 to-indigo-100 p-4 md:p-6">
    <!-- Header -->
    <div class="mb-8 text-center">
      <h1 class="text-3xl md:text-4xl font-bold text-gray-800 mb-2">
        Dashboard
      </h1>
      <p class="text-gray-600">Welcome back! Here's your overview</p>
    </div>

    <!-- Stats Grid -->
    <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4 md:gap-6 mb-8">
      <!-- Projects Card -->
      <div class="bg-white rounded-xl shadow-md p-6 border border-gray-100 hover:shadow-lg transition-shadow">
        <div class="flex items-center justify-between mb-4">
          <div class="flex items-center">
            <div class="bg-blue-100 p-3 rounded-lg">
              <v-icon size="24" color="blue">mdi-folder-multiple</v-icon>
            </div>
            <div class="ml-4">
              <p class="text-2xl font-bold text-gray-800">{{ dashboardStats.totalProjects }}</p>
              <p class="text-sm text-gray-600">Projects</p>
            </div>
          </div>
        </div>
        <router-link
          :to="{ name: 'projects' }"
          class="text-blue-600 hover:text-blue-800 text-sm font-medium flex items-center"
        >
          View All
          <v-icon size="16" class="ml-1">mdi-arrow-right</v-icon>
        </router-link>
      </div>

      <!-- Total Tasks Card -->
      <div class="bg-white rounded-xl shadow-md p-6 border border-gray-100 hover:shadow-lg transition-shadow">
        <div class="flex items-center justify-between mb-4">
          <div class="flex items-center">
            <div class="bg-green-100 p-3 rounded-lg">
              <v-icon size="24" color="green">mdi-format-list-checks</v-icon>
            </div>
            <div class="ml-4">
              <p class="text-2xl font-bold text-gray-800">{{ dashboardStats.totalTasks }}</p>
              <p class="text-sm text-gray-600">Total Tasks</p>
            </div>
          </div>
        </div>
        <router-link
          :to="{ name: 'tasks' }"
          class="text-green-600 hover:text-green-800 text-sm font-medium flex items-center"
        >
          View All
          <v-icon size="16" class="ml-1">mdi-arrow-right</v-icon>
        </router-link>
      </div>

      <!-- Completed Tasks Card -->
      <div class="bg-white rounded-xl shadow-md p-6 border border-gray-100 hover:shadow-lg transition-shadow">
        <div class="flex items-center justify-between mb-4">
          <div class="flex items-center">
            <div class="bg-emerald-100 p-3 rounded-lg">
              <v-icon size="24" color="success">mdi-check-circle</v-icon>
            </div>
            <div class="ml-4">
              <p class="text-2xl font-bold text-gray-800">{{ dashboardStats.completedTasks }}</p>
              <p class="text-sm text-gray-600">Completed</p>
            </div>
          </div>
        </div>
        <div class="flex items-center">
          <div class="flex-1 bg-gray-200 rounded-full h-2 mr-2">
            <div 
              class="bg-emerald-500 h-2 rounded-full transition-all duration-300"
              :style="{ width: dashboardStats.completionRate + '%' }"
            ></div>
          </div>
          <span class="text-sm text-gray-600">{{ dashboardStats.completionRate }}%</span>
        </div>
      </div>

      <!-- Pending Tasks Card -->
      <div class="bg-white rounded-xl shadow-md p-6 border border-gray-100 hover:shadow-lg transition-shadow">
        <div class="flex items-center justify-between mb-4">
          <div class="flex items-center">
            <div class="bg-orange-100 p-3 rounded-lg">
              <v-icon size="24" color="warning">mdi-clock-outline</v-icon>
            </div>
            <div class="ml-4">
              <p class="text-2xl font-bold text-gray-800">{{ dashboardStats.pendingTasks }}</p>
              <p class="text-sm text-gray-600">Pending</p>
            </div>
          </div>
        </div>
        <span class="inline-block bg-orange-100 text-orange-800 text-xs px-2 py-1 rounded-full">
          {{ dashboardStats.urgentTasks }} urgent
        </span>
      </div>
    </div>

    <!-- Content Grid -->
    <div class="grid grid-cols-1 lg:grid-cols-2 gap-6 mb-8">
      <!-- Recent Projects -->
      <div class="bg-white rounded-xl shadow-md border border-gray-100">
        <div class="p-6 border-b border-gray-100">
          <div class="flex items-center justify-between">
            <h3 class="text-lg font-semibold text-gray-800 flex items-center">
              <v-icon size="20" color="blue" class="mr-2">mdi-folder</v-icon>
              Recent Projects
            </h3>
            <router-link
              :to="{ name: 'projects' }"
              class="text-blue-600 hover:text-blue-800 text-sm font-medium"
            >
              View all
            </router-link>
          </div>
        </div>
        <div class="p-6">
          <div v-if="recentProjects.length" class="space-y-3">
            <div
              v-for="project in recentProjects"
              :key="project.id"
              @click="navigateToProject(project.id)"
              class="flex items-center justify-between p-3 bg-gray-50 rounded-lg hover:bg-gray-100 cursor-pointer transition-colors"
            >
              <div class="flex items-center">
                <div class="bg-blue-100 p-2 rounded-lg mr-3">
                  <v-icon size="16" color="blue">mdi-folder</v-icon>
                </div>
                <div>
                  <p class="font-medium text-gray-800 text-sm">{{ project.name }}</p>
                  <p class="text-xs text-gray-500">{{ formatDate(project.createdAt) }}</p>
                </div>
              </div>
              <span class="bg-blue-100 text-blue-800 text-xs px-2 py-1 rounded-full">
                {{ getProjectTaskCount(project.id) }} tasks
              </span>
            </div>
          </div>
          <div v-else class="text-center py-8">
            <v-icon size="48" color="gray" class="mb-4">mdi-folder-plus</v-icon>
            <p class="text-gray-600 mb-2">No projects yet</p>
            <p class="text-sm text-gray-500 mb-4">Create your first project to get started</p>
            <router-link
              :to="{ name: 'projects' }"
              class="inline-flex items-center bg-blue-600 text-white px-4 py-2 rounded-lg hover:bg-blue-700 transition-colors"
            >
              <v-icon size="16" class="mr-2">mdi-plus</v-icon>
              Create Project
            </router-link>
          </div>
        </div>
      </div>

      <!-- Recent Tasks -->
      <div class="bg-white rounded-xl shadow-md border border-gray-100">
        <div class="p-6 border-b border-gray-100">
          <div class="flex items-center justify-between">
            <h3 class="text-lg font-semibold text-gray-800 flex items-center">
              <v-icon size="20" color="green" class="mr-2">mdi-format-list-checks</v-icon>
              Recent Tasks
            </h3>
            <router-link
              :to="{ name: 'tasks' }"
              class="text-green-600 hover:text-green-800 text-sm font-medium"
            >
              View all
            </router-link>
          </div>
        </div>
        <div class="p-6">
          <div v-if="recentTasks.length" class="space-y-3">
            <div
              v-for="task in recentTasks"
              :key="task.id"
              class="flex items-center p-3 bg-gray-50 rounded-lg hover:bg-gray-100 transition-colors"
              :class="{ 'opacity-60': task.isCompleted }"
            >
              <v-icon
                size="20"
                :color="task.isCompleted ? 'success' : 'gray'"
                class="mr-3 flex-shrink-0"
              >
                {{ task.isCompleted ? 'mdi-check-circle' : 'mdi-circle-outline' }}
              </v-icon>
              <div class="flex-1 min-w-0">
                <p 
                  class="font-medium text-sm"
                  :class="{
                    'text-gray-800': !task.isCompleted,
                    'text-gray-500 line-through': task.isCompleted
                  }"
                >
                  {{ task.title }}
                </p>
                <p class="text-xs text-gray-500 truncate">
                  {{ task.description || 'No description' }}
                </p>
              </div>
              <span 
                class="text-xs px-2 py-1 rounded-full flex-shrink-0 ml-2"
                :class="getPriorityClasses(task.priority)"
              >
                {{ task.priority || 'Normal' }}
              </span>
            </div>
          </div>
          <div v-else class="text-center py-8">
            <v-icon size="48" color="gray" class="mb-4">mdi-playlist-plus</v-icon>
            <p class="text-gray-600 mb-2">No tasks yet</p>
            <p class="text-sm text-gray-500 mb-4">Create your first task to get organized</p>
            <router-link
              :to="{ name: 'tasks' }"
              class="inline-flex items-center bg-green-600 text-white px-4 py-2 rounded-lg hover:bg-green-700 transition-colors"
            >
              <v-icon size="16" class="mr-2">mdi-plus</v-icon>
              Create Task
            </router-link>
          </div>
        </div>
      </div>
    </div>

    <!-- Quick Actions -->
    <div class="bg-white rounded-xl shadow-md border border-gray-100">
      <div class="p-6 border-b border-gray-100">
        <h3 class="text-lg font-semibold text-gray-800 flex items-center">
          <v-icon size="20" color="purple" class="mr-2">mdi-lightning-bolt</v-icon>
          Quick Actions
        </h3>
      </div>
      <div class="p-6">
        <div class="grid grid-cols-2 md:grid-cols-4 gap-4">
          <router-link
            :to="{ name: 'projects' }"
            class="flex flex-col items-center p-4 bg-blue-50 rounded-lg hover:bg-blue-100 transition-colors group"
          >
            <v-icon size="24" color="blue" class="mb-2 group-hover:scale-110 transition-transform">
              mdi-folder-plus
            </v-icon>
            <span class="text-sm font-medium text-blue-800">New Project</span>
          </router-link>
          <router-link
            :to="{ name: 'tasks' }"
            class="flex flex-col items-center p-4 bg-green-50 rounded-lg hover:bg-green-100 transition-colors group"
          >
            <v-icon size="24" color="green" class="mb-2 group-hover:scale-110 transition-transform">
              mdi-plus
            </v-icon>
            <span class="text-sm font-medium text-green-800">New Task</span>
          </router-link>
          <button
            @click="refreshData"
            class="flex flex-col items-center p-4 bg-purple-50 rounded-lg hover:bg-purple-100 transition-colors group"
          >
            <v-icon size="24" color="purple" class="mb-2 group-hover:scale-110 transition-transform">
              mdi-refresh
            </v-icon>
            <span class="text-sm font-medium text-purple-800">Refresh</span>
          </button>
          <button
            @click="markAllTasksRead"
            class="flex flex-col items-center p-4 bg-emerald-50 rounded-lg hover:bg-emerald-100 transition-colors group"
          >
            <v-icon size="24" color="success" class="mb-2 group-hover:scale-110 transition-transform">
              mdi-check-all
            </v-icon>
            <span class="text-sm font-medium text-emerald-800">Mark Read</span>
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { computed, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useProjectStore } from '@/stores/project'
import { useTaskStore } from '@/stores/task'
import { useAppStore } from '@/stores/app'

// SOLID Principle: Single Responsibility - Statistics calculation
class DashboardStatistics {
  constructor(taskStore, projectStore) {
    this.taskStore = taskStore
    this.projectStore = projectStore
  }

  get totalProjects() {
    return this.projectStore.projects.length
  }

  get totalTasks() {
    return this.taskStore.tasks.length
  }

  get completedTasks() {
    return this.taskStore.tasks.filter(task => task.isCompleted).length
  }

  get pendingTasks() {
    return this.taskStore.tasks.filter(task => !task.isCompleted).length
  }

  get urgentTasks() {
    return this.taskStore.tasks.filter(task => !task.isCompleted && task.priority === 'high').length
  }

  get completionRate() {
    if (this.totalTasks === 0) return 0
    return Math.round((this.completedTasks / this.totalTasks) * 100)
  }
}

// SOLID Principle: Single Responsibility - Date formatting utility
class DateFormatter {
  static format(dateString) {
    const date = new Date(dateString)
    const now = new Date()
    const diffTime = Math.abs(now - date)
    const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24))
    
    if (diffDays === 1) return 'Yesterday'
    if (diffDays < 7) return `${diffDays} days ago`
    return date.toLocaleDateString()
  }
}

// SOLID Principle: Single Responsibility - UI utility functions
class UIHelpers {
  static getPriorityClasses(priority) {
    const priorityMap = {
      'high': 'bg-red-100 text-red-800',
      'medium': 'bg-yellow-100 text-yellow-800', 
      'low': 'bg-green-100 text-green-800',
      'normal': 'bg-gray-100 text-gray-800'
    }
    return priorityMap[priority?.toLowerCase()] || priorityMap.normal
  }
}

// SOLID Principle: Single Responsibility - Data operations
class DataManager {
  constructor(projectStore, taskStore, appStore) {
    this.projectStore = projectStore
    this.taskStore = taskStore
    this.appStore = appStore
  }

  async refreshData() {
    try {
      await Promise.all([
        this.projectStore.fetchProjects(),
        this.taskStore.fetchAllTasks()
      ])
      this.appStore.showSnackbar('Data refreshed successfully', 'success')
    } catch (error) {
      this.appStore.showSnackbar('Failed to refresh data', 'error')
    }
  }

  markAllTasksRead() {
    this.appStore.showSnackbar('All tasks marked as read', 'success')
  }

  getProjectTaskCount(projectId) {
    return this.taskStore.tasks.filter(task => task.projectId === projectId).length
  }

  getRecentProjects(limit = 5) {
    return this.projectStore.projects
      .slice()
      .sort((a, b) => new Date(b.createdAt) - new Date(a.createdAt))
      .slice(0, limit)
  }

  getRecentTasks(limit = 6) {
    return this.taskStore.tasks
      .slice()
      .sort((a, b) => new Date(b.createdAt) - new Date(a.createdAt))
      .slice(0, limit)
  }
}

export default {
  name: 'DashboardView',
  setup() {
    const router = useRouter()
    const projectStore = useProjectStore()
    const taskStore = useTaskStore()
    const appStore = useAppStore()

    // SOLID Principle: Dependency Injection
    const statistics = new DashboardStatistics(taskStore, projectStore)
    const dataManager = new DataManager(projectStore, taskStore, appStore)

    // Computed properties using SOLID principles
    const dashboardStats = computed(() => ({
      totalProjects: statistics.totalProjects,
      totalTasks: statistics.totalTasks,
      completedTasks: statistics.completedTasks,
      pendingTasks: statistics.pendingTasks,
      urgentTasks: statistics.urgentTasks,
      completionRate: statistics.completionRate
    }))

    const recentProjects = computed(() => dataManager.getRecentProjects())
    const recentTasks = computed(() => dataManager.getRecentTasks())

    // Methods using SOLID principles
    const formatDate = DateFormatter.format
    const getPriorityClasses = UIHelpers.getPriorityClasses
    const getProjectTaskCount = (projectId) => dataManager.getProjectTaskCount(projectId)
    const refreshData = () => dataManager.refreshData()
    const markAllTasksRead = () => dataManager.markAllTasksRead()

    const navigateToProject = (projectId) => {
      router.push({ name: 'project-tasks', params: { id: projectId } })
    }

    // Initialize data on component mount
    onMounted(() => {
      refreshData()
    })

    return {
      dashboardStats,
      recentProjects,
      recentTasks,
      formatDate,
      getPriorityClasses,
      getProjectTaskCount,
      refreshData,
      markAllTasksRead,
      navigateToProject
    }
  }
}
</script>

<style scoped>
/* Minimal custom styles - Tailwind handles most styling */

/* Smooth transitions for interactive elements */
.transition-colors {
  transition-property: color, background-color, border-color;
  transition-timing-function: cubic-bezier(0.4, 0, 0.2, 1);
  transition-duration: 200ms;
}

.transition-shadow {
  transition-property: box-shadow;
  transition-timing-function: cubic-bezier(0.4, 0, 0.2, 1);
  transition-duration: 200ms;
}

.transition-transform {
  transition-property: transform;
  transition-timing-function: cubic-bezier(0.4, 0, 0.2, 1);
  transition-duration: 200ms;
}

.transition-all {
  transition-property: all;
  transition-timing-function: cubic-bezier(0.4, 0, 0.2, 1);
  transition-duration: 300ms;
}

/* Accessibility improvements */
@media (prefers-reduced-motion: reduce) {
  * {
    animation-duration: 0.01ms !important;
    animation-iteration-count: 1 !important;
    transition-duration: 0.01ms !important;
  }
}

/* Focus styles for keyboard navigation */
.focus-visible:focus-visible {
  outline: 2px solid #3b82f6;
  outline-offset: 2px;
}

/* Custom scrollbar for better UX */
::-webkit-scrollbar {
  width: 6px;
}

::-webkit-scrollbar-track {
  background: #f1f5f9;
}

::-webkit-scrollbar-thumb {
  background: #cbd5e1;
  border-radius: 3px;
}

::-webkit-scrollbar-thumb:hover {
  background: #94a3b8;
}
</style>
