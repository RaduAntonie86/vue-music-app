<script setup lang="ts">
import router from '@/router'
import { ref } from 'vue'
import { useAuthStore } from '@/stores/authStore'

const authStore = useAuthStore()

const rememberMe = ref(false)
const username = ref('')
const password = ref('')

const handleLogin = async () => {
  if (!username.value || !password.value) {
    alert('Please fill in both username and password.')
    return
  }

  try {
    const response = await fetch('http://localhost:5091/Auth/login', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({
        username: username.value,
        password: password.value
      })
    })

    if (!response.ok) throw new Error('Login failed')

    const data = await response.json()
    authStore.setToken(data.token, rememberMe.value)
    authStore.setUserId(data.id, rememberMe.value)

    console.log('Log in success:', data)
    router.push('/')
  } catch (error) {
    console.error('Login error:', error)
  }
}

// Passwords are still stored in plain text for now, remains unused
async function toSHA256(message: string): Promise<string> {
  const encoder = new TextEncoder()
  const data = encoder.encode(message)
  const hashBuffer = await crypto.subtle.digest('SHA-256', data)
  const hashArray = Array.from(new Uint8Array(hashBuffer))
  const hashHex = hashArray.map((b) => b.toString(16).padStart(2, '0')).join('')
  return hashHex
}
</script>

<template>
  <div class="flex justify-center">
    <p class="text-white font-arial mt-5 text-2xl">Username:</p>
  </div>
  <div class="flex justify-center">
    <input
      v-model="username"
      class="form-control me-2 input-rounded text-[#efd0d0] bg-custom max-w-[30vh]"
      type="text"
      placeholder="Username"
    />
  </div>

  <div class="flex justify-center mt-5">
    <p class="text-white font-arial text-2xl">Password:</p>
  </div>
  <div class="flex justify-center">
    <input
      v-model="password"
      class="form-control me-2 input-rounded text-[#efd0d0] bg-custom max-w-[30vh]"
      type="password"
      placeholder="Password"
      minlength="8"
    />
  </div>

  <div class="flex justify-center mt-5">
    <input type="checkbox" v-model="rememberMe" />
    <div class="text-white font-arial ml-1">Remember me</div>
  </div>

  <div class="flex justify-center mt-5">
    <button
      @click="handleLogin"
      class="text-white font-arial text-2xl font-semibold bg-[#362323] rounded-xl px-4 py-2"
    >
      Login
    </button>
  </div>

  <div class="flex justify-center mt-5">
    <RouterLink
      to="/signup"
      class="text-white font-arial text-xl font-semibold bg-[#362323] rounded-xl px-2 py-1 no-underline"
    >
      Sign up instead
    </RouterLink>
  </div>
</template>
