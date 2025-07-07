<template>
  <v-app>
    <v-main class="bg-grey-lighten-4">
      <v-container fluid class="fill-height">
        <v-row justify="center" align="center" class="fill-height">
          <v-col cols="12" sm="8" md="6" lg="4" xl="3">
            <router-view />
          </v-col>
        </v-row>
      </v-container>
    </v-main>

    <!-- Global Loading -->
    <v-overlay v-model="loading" class="align-center justify-center">
      <v-progress-circular
        color="primary"
        indeterminate
        size="64"
      ></v-progress-circular>
    </v-overlay>

    <!-- Global Snackbar -->
    <v-snackbar
      v-model="snackbar.show"
      :color="snackbar.color"
      :timeout="snackbar.timeout"
    >
      {{ snackbar.message }}
      <template v-slot:actions>
        <v-btn
          color="white"
          variant="text"
          @click="snackbar.show = false"
        >
          Close
        </v-btn>
      </template>
    </v-snackbar>
  </v-app>
</template>

<script>
import { computed } from 'vue'
import { useAppStore } from '@/stores/app'

export default {
  name: 'AuthLayout',
  setup() {
    const appStore = useAppStore()

    const loading = computed(() => appStore.loading)
    const snackbar = computed(() => appStore.snackbar)

    return {
      loading,
      snackbar
    }
  }
}
</script>
