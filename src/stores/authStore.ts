import { defineStore } from 'pinia'

export const useAuthStore = defineStore('auth', {
  state: () => ({
    token: localStorage.getItem('authToken') || sessionStorage.getItem('authToken') || null,
    userId: localStorage.getItem('userId') || sessionStorage.getItem('userId')
    ? parseInt(localStorage.getItem('userId') || sessionStorage.getItem('userId')!)
    : null,
  }),
  getters: {
    isLoggedIn: (state) => !!state.token
  },
  actions: {
    setToken(token: string, rememberMeValue: boolean) {
      if (rememberMeValue) {
        localStorage.setItem('authToken', token)
      } else {
        sessionStorage.setItem('authToken', token)
      }
      this.token = token
      localStorage.setItem('authToken', token)
    },
    clearToken() {
      this.token = null
      localStorage.removeItem('authToken')
    },
    setUserId(id: number, rememberMeValue: boolean) {
      this.userId = id
      const storage = rememberMeValue ? localStorage : sessionStorage
      storage.setItem('userId', id.toString())
    }
  }
})
