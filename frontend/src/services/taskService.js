import apiClient from './apiClient'

const taskService = {
  async getTasks(projectId) {
    try {
      const response = await apiClient.get(`/tasks?projectId=${projectId}`)
      return response.data
    } catch (error) {
      throw new Error(error.response?.data?.message || 'Failed to fetch tasks')
    }
  },

  async getAllTasks() {
    try {
      const response = await apiClient.get('/tasks')
      return response.data
    } catch (error) {
      throw new Error(error.response?.data?.message || 'Failed to fetch tasks')
    }
  },

  async getTask(id) {
    try {
      const response = await apiClient.get(`/tasks/${id}`)
      return response.data
    } catch (error) {
      throw new Error(error.response?.data?.message || 'Failed to fetch task')
    }
  },

  async createTask(taskData) {
    try {
      const response = await apiClient.post('/tasks', taskData)
      return response.data
    } catch (error) {
      throw new Error(error.response?.data?.message || 'Failed to create task')
    }
  },

  async updateTask(id, taskData) {
    try {
      const response = await apiClient.put(`/tasks/${id}`, taskData)
      return response.data
    } catch (error) {
      throw new Error(error.response?.data?.message || 'Failed to update task')
    }
  },

  async completeTask(id) {
    try {
      const response = await apiClient.patch(`/tasks/${id}/complete`)
      return response.data
    } catch (error) {
      throw new Error(error.response?.data?.message || 'Failed to complete task')
    }
  },

  async deleteTask(id) {
    try {
      const response = await apiClient.delete(`/tasks/${id}`)
      return response.data
    } catch (error) {
      throw new Error(error.response?.data?.message || 'Failed to delete task')
    }
  }
}

export default taskService
