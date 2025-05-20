import { defineStore } from 'pinia'

export const useAuthStore = defineStore('auth', {
  state: () => ({
    token: localStorage.getItem('authToken') || sessionStorage.getItem('authToken') || null
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
    }
  }
})
