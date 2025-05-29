import axios from 'axios'
import { SongList } from './SongList'

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL
export class Album extends SongList {
  releaseDate: string

  constructor(id: number, name: string, imagePath: string, songIds: number[], releaseDate: string) {
    super(id, name, imagePath, songIds)
    this.releaseDate = releaseDate
  }

  static async fetchById(albumId: number): Promise<Album> {
    const response = await axios.get<Album>(`${API_BASE_URL}/Album/${albumId}`)
    return response.data
  }
  static async fetchAll(): Promise<Album[]> {
    const response = await axios.get<Album[]>(`${API_BASE_URL}/Album`)
    return response.data
  }
  static async fetchAlbumsForSongs(songIds: number[]): Promise<Record<number, Album> | null> {
    const promises = songIds.map((id) =>
      axios.get<Album>(`${API_BASE_URL}/Album/song_id/${id}`)
    )
    const responses = await Promise.all(promises)
    const albumsMap: Record<number, Album> = {}

    responses.forEach((res, idx) => {
      albumsMap[songIds[idx]] = res.data
    })

    return albumsMap
  }
  static async fetchAlbumForSong(songId: number): Promise<Album> {
    const response = await axios.get<Album>(`${API_BASE_URL}/Album/song_id/${songId}`)
    return response.data
  }
  static async fetchAlbumsByName(name: string): Promise<Album[]> {
    const response = await axios.get<Album[]>(`${API_BASE_URL}/Album/name/${name}`)
    return response.data
  }
}
