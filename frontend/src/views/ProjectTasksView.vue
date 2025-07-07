<template>
  <div>
    <v-row class="mb-4">
      <v-col cols="12" md="6">
        <h1 class="text-h4">{{ project?.name || 'Project Tasks' }}</h1>
        <v-breadcrumbs :items="breadcrumbs"></v-breadcrumbs>
      </v-col>
      <v-col cols="12" md="6" class="text-right">
        <v-btn
          color="primary"
          prepend-icon="mdi-plus"
          @click="showCreateDialog = true"
        >
          New Task
        </v-btn>
      </v-col>
    </v-row>

    <v-row>
      <v-col cols="12">
        <v-card>
          <v-card-text>
            <v-data-table
              :headers="headers"
              :items="taskStore.tasks"
              :loading="taskStore.loading"
              item-value="id"
            >
              <template v-slot:item.isCompleted="{ item }">
                <v-chip
                  :color="item.isCompleted ? 'success' : 'warning'"
                  variant="tonal"
                  size="small"
                >
                  {{ item.isCompleted ? 'Completed' : 'Pending' }}
                </v-chip>
              </template>

              <template v-slot:item.createdAt="{ item }">
                {{ formatDate(item.createdAt) }}
              </template>

              <template v-slot:item.actions="{ item }">
                <v-btn
                  icon="mdi-pencil"
                  variant="text"
                  size="small"
                  @click="editTask(item)"
                ></v-btn>
                <v-btn
                  v-if="!item.isCompleted"
                  icon="mdi-check"
                  variant="text"
                  size="small"
                  color="success"
                  @click="completeTask(item)"
                ></v-btn>
                <v-btn
                  icon="mdi-delete"
                  variant="text"
                  size="small"
                  color="error"
                  @click="deleteTask(item)"
                ></v-btn>
              </template>
            </v-data-table>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>

    <!-- Create Task Dialog -->
    <v-dialog v-model="showCreateDialog" max-width="600px">
      <v-card>
        <v-card-title>Create New Task</v-card-title>
        <v-card-text>
          <v-form ref="createForm" v-model="createValid">
            <v-text-field
              v-model="newTask.title"
              label="Task Title"
              :rules="titleRules"
              required
            ></v-text-field>
            <v-textarea
              v-model="newTask.description"
              label="Description"
              rows="3"
            ></v-textarea>
          </v-form>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn @click="showCreateDialog = false">Cancel</v-btn>
          <v-btn
            color="primary"
            :disabled="!createValid"
            :loading="creating"
            @click="createTask"
          >
            Create
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <!-- Edit Task Dialog -->
    <v-dialog v-model="showEditDialog" max-width="600px">
      <v-card>
        <v-card-title>Edit Task</v-card-title>
        <v-card-text>
          <v-form ref="editForm" v-model="editValid">
            <v-text-field
              v-model="editingTask.title"
              label="Task Title"
              :rules="titleRules"
              required
            ></v-text-field>
            <v-textarea
              v-model="editingTask.description"
              label="Description"
              rows="3"
            ></v-textarea>
          </v-form>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn @click="showEditDialog = false">Cancel</v-btn>
          <v-btn
            color="primary"
            :disabled="!editValid"
            :loading="editing"
            @click="updateTask"
          >
            Update
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>

<script>
import { ref, computed, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { useTaskStore } from '@/stores/task'
import { useProjectStore } from '@/stores/project'
import { useAppStore } from '@/stores/app'

export default {
  name: 'ProjectTasksView',
  setup() {
    const route = useRoute()
    const taskStore = useTaskStore()
    const projectStore = useProjectStore()
    const appStore = useAppStore()

    const projectId = route.params.id
    const showCreateDialog = ref(false)
    const showEditDialog = ref(false)
    const createValid = ref(false)
    const editValid = ref(false)
    const creating = ref(false)
    const editing = ref(false)

    const project = computed(() => 
      projectStore.projects.find(p => p.id === projectId)
    )

    const breadcrumbs = computed(() => [
      { title: 'Projects', to: '/projects' },
      { title: project.value?.name || 'Project', disabled: true }
    ])

    const headers = [
      { title: 'Title', key: 'title' },
      { title: 'Description', key: 'description' },
      { title: 'Status', key: 'isCompleted' },
      { title: 'Created', key: 'createdAt' },
      { title: 'Actions', key: 'actions', sortable: false }
    ]

    const newTask = ref({
      title: '',
      description: ''
    })

    const editingTask = ref({
      id: null,
      title: '',
      description: ''
    })

    const titleRules = [
      v => !!v || 'Task title is required',
      v => (v && v.length >= 2) || 'Task title must be at least 2 characters'
    ]

    const formatDate = (dateString) => {
      return new Date(dateString).toLocaleDateString()
    }

    const createTask = async () => {
      creating.value = true
      try {
        const taskData = {
          ...newTask.value,
          projectId: projectId
        }
        const result = await taskStore.createTask(taskData)
        if (result.success) {
          appStore.showSnackbar('Task created successfully!', 'success')
          showCreateDialog.value = false
          newTask.value = { title: '', description: '' }
        } else {
          appStore.showSnackbar(result.error || 'Failed to create task', 'error')
        }
      } finally {
        creating.value = false
      }
    }

    const editTask = (task) => {
      editingTask.value = { ...task }
      showEditDialog.value = true
    }

    const updateTask = async () => {
      editing.value = true
      try {
        const result = await taskStore.updateTask(editingTask.value.id, {
          title: editingTask.value.title,
          description: editingTask.value.description
        })
        if (result.success) {
          appStore.showSnackbar('Task updated successfully!', 'success')
          showEditDialog.value = false
        } else {
          appStore.showSnackbar(result.error || 'Failed to update task', 'error')
        }
      } finally {
        editing.value = false
      }
    }

    const completeTask = async (task) => {
      const result = await taskStore.completeTask(task.id)
      if (result.success) {
        appStore.showSnackbar('Task completed!', 'success')
      } else {
        appStore.showSnackbar(result.error || 'Failed to complete task', 'error')
      }
    }

    const deleteTask = async (task) => {
      if (confirm(`Are you sure you want to delete "${task.title}"?`)) {
        const result = await taskStore.deleteTask(task.id)
        if (result.success) {
          appStore.showSnackbar('Task deleted successfully!', 'success')
        } else {
          appStore.showSnackbar(result.error || 'Failed to delete task', 'error')
        }
      }
    }

    onMounted(() => {
      taskStore.fetchTasks(projectId)
      if (!project.value) {
        projectStore.fetchProjects()
      }
    })

    return {
      taskStore,
      project,
      breadcrumbs,
      showCreateDialog,
      showEditDialog,
      createValid,
      editValid,
      creating,
      editing,
      headers,
      newTask,
      editingTask,
      titleRules,
      formatDate,
      createTask,
      editTask,
      updateTask,
      completeTask,
      deleteTask
    }
  }
}
</script>
