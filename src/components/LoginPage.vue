<script setup lang="ts">
import { ref } from 'vue'

// Reactive variables
const username = ref('')
const password = ref('')

// Submit handler
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
    console.log('Login success:', data)
  } catch (error) {
    console.error('', error)
  }
}

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
    <input type="checkbox" id="checkbox" />
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
</template>
