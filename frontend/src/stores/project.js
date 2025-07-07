import { defineStore } from 'pinia'
import { ref } from 'vue'
import projectService from '@/services/projectService'

export const useProjectStore = defineStore('project', () => {
  const projects = ref([])
  const currentProject = ref(null)
  const loading = ref(false)

  const fetchProjects = async () => {
    loading.value = true
    try {
      const response = await projectService.getProjects()
      if(response !== null && response !== undefined) {
        console.log('Projects fetched:', response.data.value)
            projects.value = response.data.value
      }
      else {
        projects.value = []
      }
    } catch (error) {
      console.error('Error fetching projects:', error)
    } finally {
      loading.value = false
    }
  }

  const createProject = async (projectData) => {
    try {
      const response = await projectService.createProject(projectData)
      projects.value.push(response.data.value)
      return { success: true, data: response.data }
    } catch (error) {
      return { success: false, error: error.message }
    }
  }

  const updateProject = async (id, projectData) => {
    try {
      const response = await projectService.updateProject(id, projectData)
      const index = projects.value.findIndex(p => p.id === id)
      if (index !== -1) {
        projects.value[index] = response.data.value
      }
      return { success: true, data: response.data.value }
    } catch (error) {
      return { success: false, error: error.message }
    }
  }

  const deleteProject = async (id) => {
    try {
      await projectService.deleteProject(id)
      projects.value = projects.value.filter(p => p.id !== id)
      return { success: true }
    } catch (error) {
      return { success: false, error: error.message }
    }
  }

  const setCurrentProject = (project) => {
    currentProject.value = project
  }

  return {
    projects,
    currentProject,
    loading,
    fetchProjects,
    createProject,
    updateProject,
    deleteProject,
    setCurrentProject
  }
})
