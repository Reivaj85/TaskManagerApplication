import apiClient from './apiClient'

const projectService = {
  async getProjects() {
    try {
      const response = await apiClient.get('/projects')
      return response.data
    } catch (error) {
      throw new Error(error.response?.data?.message || 'Failed to fetch projects')
    }
  },

  async getProject(id) {
    try {
      const response = await apiClient.get(`/projects/${id}`)
      return response.data
    } catch (error) {
      throw new Error(error.response?.data?.message || 'Failed to fetch project')
    }
  },

  async createProject(projectData) {
    try {
      const response = await apiClient.post('/projects', projectData)
      return response.data
    } catch (error) {
      throw new Error(error.response?.data?.message || 'Failed to create project')
    }
  },

  async updateProject(id, projectData) {
    try {
      const response = await apiClient.put(`/projects/${id}`, projectData)
      return response.data
    } catch (error) {
      throw new Error(error.response?.data?.message || 'Failed to update project')
    }
  },

  async deleteProject(id) {
    try {
      const response = await apiClient.delete(`/projects/${id}`)
      return response.data
    } catch (error) {
      throw new Error(error.response?.data?.message || 'Failed to delete project')
    }
  }
}

export default projectService
