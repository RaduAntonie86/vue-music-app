import { defineStore } from 'pinia'

export const useAuthStore = defineStore('auth', {
  state: () => ({
    token: localStorage.getItem('authToken') || sessionStorage.getItem('authToken') || null,
    userId: sessionStorage.getItem('userId')
      ? parseInt(sessionStorage.getItem('userId')!)
      : localStorage.getItem('userId')
        ? parseInt(localStorage.getItem('userId')!)
        : null
  }),
  getters: {
    isLoggedIn: (state) => !!state.token
  },
  actions: {
    setToken(token: string, rememberMeValue: boolean) {
      this.clearToken()
      if (rememberMeValue) {
        localStorage.setItem('authToken', token)
      } else {
        sessionStorage.setItem('authToken', token)
      }
      this.token = token
      localStorage.setItem('authToken', token) // This line always runs, overwriting sessionStorage case
    },
    clearToken() {
      this.token = null
      localStorage.removeItem('authToken')
    },
    setUserId(id: number, rememberMeValue: boolean) {
      this.userId = id
      this.clearUserId()
      const storage = rememberMeValue ? localStorage : sessionStorage
      storage.setItem('userId', id.toString())
    },
    clearUserId() {
      this.userId = null
      localStorage.removeItem('userId')
      sessionStorage.removeItem('userId')
    },
    clearAll() {
      this.clearToken()
      this.clearUserId()
    }
  }
})
