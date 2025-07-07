<template>
  <div>
    <v-row class="mb-4">
      <v-col cols="12" md="6">
        <h1 class="text-h4">Projects</h1>
      </v-col>
      <v-col cols="12" md="6" class="text-right">
        <v-btn
          color="primary"
          prepend-icon="mdi-plus"
          @click="showCreateDialog = true"
        >
          New Project
        </v-btn>
      </v-col>
    </v-row>

    <v-row>
      <v-col
        v-for="project in projectStore.projects"
        :key="project.id"
        cols="12"
        md="6"
        lg="4"
      >
        <v-card class="h-100">
          <v-card-title class="d-flex align-center">
            <v-icon left>mdi-folder</v-icon>
            {{ project.name }}
            <v-spacer></v-spacer>
            <v-menu>
              <template v-slot:activator="{ props }">
                <v-btn
                  icon="mdi-dots-vertical"
                  variant="text"
                  size="small"
                  v-bind="props"
                ></v-btn>
              </template>
              <v-list>
                <v-list-item @click="editProject(project)">
                  <template v-slot:prepend>
                    <v-icon>mdi-pencil</v-icon>
                  </template>
                  <v-list-item-title>Edit</v-list-item-title>
                </v-list-item>
                <v-list-item @click="deleteProject(project)" v-if="!project.isDefault">
                  <template v-slot:prepend>
                    <v-icon color="error">mdi-delete</v-icon>
                  </template>
                  <v-list-item-title>Delete</v-list-item-title>
                </v-list-item>
              </v-list>
            </v-menu>
          </v-card-title>
          
          <v-card-subtitle v-if="project.isDefault">
            <v-chip color="primary" size="small">Default</v-chip>
          </v-card-subtitle>
          
          <v-card-text>
            <div class="text-body-2 text-medium-emphasis">
              Created: {{ formatDate(project.createdAt) }}
            </div>
            <div class="text-body-2 text-medium-emphasis mt-1">
              Tasks: {{ project.taskCount || 0 }}
            </div>
          </v-card-text>
          
          <v-card-actions>
            <v-btn
              color="primary"
              variant="text"
              :to="{ name: 'project-tasks', params: { id: project.id } }"
            >
              View Tasks
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-col>

      <v-col v-if="!projectStore.projects.length" cols="12">
        <v-alert type="info" variant="tonal">
          No projects yet. Create your first project to get started!
        </v-alert>
      </v-col>
    </v-row>

    <!-- Create Project Dialog -->
    <v-dialog v-model="showCreateDialog" max-width="500px">
      <v-card>
        <v-card-title>Create New Project</v-card-title>
        <v-card-text>
          <v-form ref="createForm" v-model="createValid">
            <v-text-field
              v-model="newProject.name"
              label="Project Name"
              :rules="nameRules"
              required
            ></v-text-field>
          </v-form>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn @click="showCreateDialog = false">Cancel</v-btn>
          <v-btn
            color="primary"
            :disabled="!createValid"
            :loading="creating"
            @click="createProject"
          >
            Create
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <!-- Edit Project Dialog -->
    <v-dialog v-model="showEditDialog" max-width="500px">
      <v-card>
        <v-card-title>Edit Project</v-card-title>
        <v-card-text>
          <v-form ref="editForm" v-model="editValid">
            <v-text-field
              v-model="editingProject.name"
              label="Project Name"
              :rules="nameRules"
              required
            ></v-text-field>
          </v-form>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn @click="showEditDialog = false">Cancel</v-btn>
          <v-btn
            color="primary"
            :disabled="!editValid"
            :loading="editing"
            @click="updateProject"
          >
            Update
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>

<script>
import { ref, onMounted } from 'vue'
import { useProjectStore } from '@/stores/project'
import { useAppStore } from '@/stores/app'

export default {
  name: 'ProjectsView',
  setup() {
    const projectStore = useProjectStore()
    const appStore = useAppStore()

    const showCreateDialog = ref(false)
    const showEditDialog = ref(false)
    const createValid = ref(false)
    const editValid = ref(false)
    const creating = ref(false)
    const editing = ref(false)

    const newProject = ref({
      name: ''
    })

    const editingProject = ref({
      id: null,
      name: ''
    })

    const nameRules = [
      v => !!v || 'Project name is required',
      v => (v && v.length >= 2) || 'Project name must be at least 2 characters'
    ]

    const formatDate = (dateString) => {
      return new Date(dateString).toLocaleDateString()
    }

    const createProject = async () => {
      creating.value = true
      try {
        const result = await projectStore.createProject(newProject.value)
        if (!result.id) {
          appStore.showSnackbar('Project created successfully!', 'success')
          showCreateDialog.value = false
          newProject.value.name = ''
        } else {
          appStore.showSnackbar(result.error || 'Failed to create project', 'error')
        }
      } finally {
        creating.value = false
      }
    }

    const editProject = (project) => {
      editingProject.value = { ...project }
      showEditDialog.value = true
    }

    const updateProject = async () => {
      editing.value = true
      try {
        const result = await projectStore.updateProject(
          editingProject.value.id,
          { name: editingProject.value.name }
        )
        if (result.success) {
          appStore.showSnackbar('Project updated successfully!', 'success')
          showEditDialog.value = false
        } else {
          appStore.showSnackbar(result.error || 'Failed to update project', 'error')
        }
      } finally {
        editing.value = false
      }
    }

    const deleteProject = async (project) => {
      if (confirm(`Are you sure you want to delete "${project.name}"?`)) {
        const result = await projectStore.deleteProject(project.id)
        if (result.success) {
          appStore.showSnackbar('Project deleted successfully!', 'success')
        } else {
          appStore.showSnackbar(result.error || 'Failed to delete project', 'error')
        }
      }
    }

    onMounted(() => {
      projectStore.fetchProjects()
    })

    return {
      projectStore,
      showCreateDialog,
      showEditDialog,
      createValid,
      editValid,
      creating,
      editing,
      newProject,
      editingProject,
      nameRules,
      formatDate,
      createProject,
      editProject,
      updateProject,
      deleteProject
    }
  }
}
</script>
