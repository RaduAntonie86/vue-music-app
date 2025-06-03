import axios from 'axios'
import { SongList } from './SongList'

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL
export class Playlist extends SongList {
  description: string
  userIds: number[]

  constructor(
    id: number,
    name: string,
    imagePath: string,
    songIds: number[],
    description: string,
    userIds: number[]
  ) {
    super(id, name, imagePath, songIds)
    this.description = description
    this.userIds = userIds
  }

  static async fetchAll(): Promise<Playlist[]> {
    const response = await axios.get<Playlist[]>(`${API_BASE_URL}/Playlist`)
    return response.data
  }

  static async fetchById(playlistId: number): Promise<Playlist> {
    const response = await axios.get<Playlist>(`${API_BASE_URL}/Playlist/${playlistId}`)
    return response.data
  }

  static async addSong(playlistId: number, selectedSongId: number) {
    await axios.post(`${API_BASE_URL}/Playlist/${playlistId}/addSong/${selectedSongId}`)
  }

  static async fetchFromUser(userId: number): Promise<Playlist[]> {
    const response = await axios.get<Playlist[]>(`${API_BASE_URL}/Playlist/user/${userId}`)
    return response.data
  }

  static async deletePlaylist(playlistId: number): Promise<void> {
    await axios.delete(`${API_BASE_URL}/Playlist/${playlistId}`)
  }

  static async removeSong(playlistId: number, songId: number): Promise<void> {
    await axios.delete(`${API_BASE_URL}/Playlist/${playlistId}/song/${songId}`)
  }

  static async createPlaylist(playlist: Playlist): Promise<void> {
    await axios.post(`${API_BASE_URL}/Playlist/add`, playlist)
  }
  static async fetchPlaylistsByName(name: string): Promise<Playlist[]> {
    const response = await axios.get<Playlist[]>(`${API_BASE_URL}/Playlist/name/${name}`)
    return response.data
  }
}
