import { defineStore } from 'pinia'
import { ref } from 'vue'
import taskService from '@/services/taskService'

export const useTaskStore = defineStore('task', () => {
  const tasks = ref([])
  const loading = ref(false)

  const fetchTasks = async (projectId) => {
    loading.value = true
    try {
      const response = await taskService.getTasks(projectId)
      tasks.value = !response ? response.data.value : []
    } catch (error) {
      console.error('Error fetching tasks:', error)
    } finally {
      loading.value = false
    }
  }

  const fetchAllTasks = async () => {
    loading.value = true
    try {
      const response = await taskService.getAllTasks()
      tasks.value = !response ? response.data.value : []
    } catch (error) {
      console.error('Error fetching all tasks:', error)
    } finally {
      loading.value = false
    }
  }

  const createTask = async (taskData) => {
    try {
      const response = await taskService.createTask(taskData)
      tasks.value.push(response.data.value)
      return { success: true, data: response.data.value }
    } catch (error) {
      return { success: false, error: error.message }
    }
  }

  const updateTask = async (id, taskData) => {
    try {
      const response = await taskService.updateTask(id, taskData)
      const index = tasks.value.findIndex(t => t.id === id)
      if (index !== -1) {
        tasks.value[index] = response.data.value
      }
      return { success: true, data: response.data.value }
    } catch (error) {
      return { success: false, error: error.message }
    }
  }

  const completeTask = async (id) => {
    try {
      await taskService.completeTask(id)
      const task = tasks.value.find(t => t.id === id)
      if (task) {
        task.isCompleted = true
      }
      return { success: true }
    } catch (error) {
      return { success: false, error: error.message }
    }
  }

  const deleteTask = async (id) => {
    try {
      await taskService.deleteTask(id)
      tasks.value = tasks.value.filter(t => t.id !== id)
      return { success: true }
    } catch (error) {
      return { success: false, error: error.message }
    }
  }

  return {
    tasks,
    loading,
    fetchTasks,
    fetchAllTasks,
    createTask,
    updateTask,
    completeTask,
    deleteTask
  }
})
