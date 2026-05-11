<script setup lang="ts">
import { Album } from '@/types/Album'
import { Song } from '@/types/Song'
import { User } from '@/types/User'
import { onMounted, ref } from 'vue'
import { PerfectScrollbar } from 'vue3-perfect-scrollbar'
import { ListeningHistory } from '@/types/ListeningHistory'
import { useRoute } from 'vue-router'
import { Genre } from '@/types/Genre'

const genresMap = ref<Record<number, string>>({})
const route = useRoute()
const userId = ref(Number(route.params.id))
const songs = ref<Song[]>([])
const songArtists = ref<Record<number, User[]>>({})
const albums = ref<Record<number, Album>>({})
const listeningHistory = ref<ListeningHistory[]>([])

const fetchListeningHistory = async () => {
  try {
    listeningHistory.value = await ListeningHistory.fetchByUserId(userId.value)
    const songIds = listeningHistory.value.map((entry) => entry.songId)

    songs.value = (await Song.fetchByIds(songIds)) ?? []

    for (const song of songs.value) {
      await fetchSongArtists(song.id)
      await fetchSongAlbum(song.id)
    }
  } catch (error) {
    console.error('Error fetching listening history', error)
  }
}

const fetchSongArtists = async (songId: number) => {
  try {
    const users = await User.fetchSongArtists(songId)
    songArtists.value[songId] = users
  } catch (error) {
    console.error(`Error fetching artists for song ${songId}:`, error)
  }
}

const fetchSongAlbum = async (songId: number) => {
  try {
    const album = await Album.fetchAlbumForSong(songId)
    albums.value[songId] = album

    if (album.genreIds?.length > 0) {
      await fetchGenres(album.genreIds)
    }
  } catch (error) {
    console.error(`Error fetching album for song ${songId}:`, error)
  }
}

const formatLength = (seconds: number): string => {
  const mins = Math.floor(seconds / 60)
  const secs = Math.floor(seconds % 60)
  return `${mins}:${secs.toString().padStart(2, '0')}`
}

const getHistoryForSong = (songId: number): ListeningHistory | undefined => {
  return listeningHistory.value.find((entry) => entry.songId === songId)
}

const fetchGenres = async (genreIds: number[]) => {
  const uniqueIds = [...new Set(genreIds)]
  try {
    const genres = await Genre.fetchByIds(uniqueIds)
    for (const genre of genres) {
      genresMap.value[genre.id] = genre.name
    }
  } catch (error) {
    console.error('Error fetching genres:', error)
  }
}

onMounted(() => {
  fetchListeningHistory()
})
</script>

<template>
  <div class="bg-[#362323] rounded-md">
    <div>
      <div
        class="flex align-middle place-items-center p-4 text-6xl font-semibold font-arial mb-3 text-white"
      >
        Listening History
      </div>
      <div class="grid grid-cols-12 p-2 mx-2 font-arial text-[#753E3E] font-bold">
        <div class="col-span-2">Title</div>
        <div class="col-span-2">Album</div>
        <div class="col-span-2">Length</div>
        <div class="col-span-2">Date</div>
        <div class="col-span-2">Listening Time</div>
        <div class="col-span-2">Genres</div>
      </div>
      <div class="mx-2">
        <custom-divider :width="'100%'" :height="9" />
      </div>
    </div>
    <PerfectScrollbar class="min-h-[30vh] max-h-[60vh] overflow-y-auto">
      <div
        v-for="(song, index) in songs"
        :key="song.id"
        class="w-full text-left focus:outline-none"
        :class="'text-white'"
      >
        <div class="grid grid-cols-12 p-2 mx-2 font-arial font-bold">
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
          <div class="col-span-2 flex place-items-center font-normal">
            {{ new Date(getHistoryForSong(song.id)?.listeningDate ?? '').toLocaleDateString() }}
          </div>
          <div class="col-span-2 flex place-items-center font-normal">
            {{ Math.floor(getHistoryForSong(song.id)?.listeningTime ?? 0) }} seconds
          </div>
          <div
            class="col-span-2 text-xs flex items-center font-arial font-normal text-[#9b7c7c] italic"
          >
            <span v-if="albums[song.id]?.genreIds?.length">
              <span v-for="(genreId, idx) in albums[song.id].genreIds" :key="genreId">
                {{ genresMap[genreId] ?? 'Unknown Genre' }}
                <span v-if="idx < albums[song.id].genreIds.length - 1">, </span>
              </span>
            </span>
          </div>
        </div>
      </div>
    </PerfectScrollbar>
  </div>
</template>
