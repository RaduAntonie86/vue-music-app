<script setup lang="ts">
import router from '@/router'
import { useAuthStore } from '@/stores/authStore'
import { Album } from '@/types/Album'
import { Playlist } from '@/types/Playlist'
import { Song } from '@/types/Song'
import { User } from '@/types/User'
import { onMounted, ref } from 'vue'
import { PerfectScrollbar } from 'vue3-perfect-scrollbar'

const existingPlaylistIds = ref<number[]>([])
const playlist_name = ref('')
const playlist_description = ref('')
const auth = useAuthStore()
const songsForPlaylist = ref<Song[]>([])
const songs = ref<Song[]>([])
const songArtists = ref<Record<number, User[]>>({})
const albums = ref<Record<number, Album>>({})

const fetchPlaylists = async () => {
  try {
    const playlists = await Playlist.fetchAll()
    if (!playlists) {
      existingPlaylistIds.value = []
      return
    }
    existingPlaylistIds.value = playlists
      .map((p) => p.id)
      .filter((id): id is number => id !== undefined)
  } catch (error) {
    console.error('Error fetching playlists:', error)
  }
}

const addToPlaylist = (song: Song) => {
  const index = songsForPlaylist.value.findIndex((s) => s.id === song.id)
  if (index === -1) {
    songsForPlaylist.value.push(song)
  } else {
    songsForPlaylist.value.splice(index, 1)
  }
}

const isSelected = (songId: number): boolean => {
  return songsForPlaylist.value.some((s) => s.id === songId)
}

const fetchSongs = async () => {
  try {
    songs.value = await Song.fetchAll()

    for (const song of songs.value) {
      await fetchSongArtists(song.id)
      await fetchSongAlbum(song.id)
    }
  } catch (error) {
    console.error('Error fetching songs', error)
  }
}

const fetchSongArtists = async (songId: number) => {
  try {
    const users = await User.fetchSongArtists(songId)
    songArtists.value[songId] = users
  } catch (error) {
    console.error(`Error fetching artists for album ${songId}:`, error)
  }
}

const fetchSongAlbum = async (songId: number) => {
  try {
    albums.value[songId] = await Album.fetchAlbumForSong(songId)
  } catch (error) {
    console.error(`Error fetching album for song ${songId}:`, error)
  }
}

const formatLength = (seconds: number): string => {
  const mins = Math.floor(seconds / 60)
  const secs = seconds % 60
  return `${mins}:${secs.toString().padStart(2, '0')}`
}

const createPlaylist = async () => {
  if (auth.userId == null) {
    console.log('Could not find user Id.')
    alert('You are not logged in.')
    router.push('/')
    return
  }

  if (songsForPlaylist.value.length === 0) {
    alert('Please select any songs to be added to the playlist.')
    return
  }

  try {
    const payload = {
      name: playlist_name.value,
      description: playlist_description.value,
      imagePath: '',
      userIds: [auth.userId],
      songIds: songsForPlaylist.value.map((song) => song.id)
    } as Playlist
    Playlist.createPlaylist(payload)
    console.log('Playlist created successfully')
    router.push('/')
  } catch (error) {
    console.error('Error creating playlist:', error)
  }
}

onMounted(() => {
  fetchSongs()
  fetchPlaylists()
})
</script>

<template>
  <div class="bg-[#362323] rounded-md">
    <div>
      <div
        class="flex align-middle place-items-center p-4 text-6xl font-semibold font-arial mb-3 text-white"
      >
        Create Playlist
      </div>
      <input
        v-model="playlist_name"
        class="form-control me-2 input-rounded text-[#efd0d0] bg-custom max-w-[30vh] ml-8 mb-5"
        type="text"
        placeholder="Playlist Name"
      />
      <input
        v-model="playlist_description"
        class="form-control me-2 input-rounded text-[#efd0d0] bg-custom max-w-[30vh] ml-8 mb-4"
        type="text"
        placeholder="Playlist Description"
      />
      <button
        @click="createPlaylist"
        class="font-arial text-xl font-semibold bg-white rounded-xl ml-8 px-4 py-2 hover:text-[#888888] text-[#882323] mb-4"
      >
        Create
      </button>
      <div class="grid grid-cols-6 p-2 mx-2 font-arial text-[#753E3E] font-bold">
        <div class="col-span-2">Title</div>
        <div class="col-span-2">Album</div>
        <div class="col-span-2">Length</div>
      </div>
      <div class="mx-2">
        <mat-divider></mat-divider>
      </div>
    </div>
    <PerfectScrollbar class="min-h-[30vh] max-h-[60vh] overflow-y-auto">
      <button
        v-for="(song, index) in songs"
        :key="song.id"
        @click="addToPlaylist(song)"
        class="w-full text-left cursor-pointer focus:outline-none"
        :class="{ 'text-green-400': isSelected(song.id), 'text-white': !isSelected(song.id) }"
      >
        <div class="grid grid-cols-6 p-2 mx-2 font-arial font-bold">
          <div class="col-span-2">
            <div class="flex align-middle place-items-center mb-2 mt-2">
              <img
                class="rounded-lg mr-2.5"
                :src="
                  albums[song.id]?.imagePath && albums[song.id].imagePath.trim() !== ''
                    ? albums[song.id].imagePath
                    : 'images/albums/album.jpeg'
                "
                width="50"
                height="50"
              />
              <div>
                <div class="text-lg font-arial">{{ song.name }}</div>
                <div class="text-xs font-arial font-normal">
                  <div v-if="songArtists[songs[index]?.id]">
                    <span v-for="(artist, idx) in songArtists[songs[index].id]" :key="artist.id">
                      {{ artist.displayName
                      }}<span v-if="idx < songArtists[songs[index].id].length - 1">, </span>
                    </span>
                  </div>
                  <div v-else>Unknown Artist</div>
                </div>
              </div>
            </div>
          </div>
          <div class="col-span-2 flex place-items-center font-normal">
            {{ albums[song.id]?.name || 'Unknown Album' }}
          </div>
          <div class="col-span-2 flex place-items-center font-normal">
            {{ formatLength(song.length) }}
          </div>
        </div>
      </button>
    </PerfectScrollbar>
  </div>
</template>
