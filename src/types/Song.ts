import axios from 'axios'

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL
export class Song {
  id: number
  name: string
  length: number
  listens: number

  constructor(id: number, name: string, length: number, listens: number) {
    this.id = id
    this.name = name
    this.length = length
    this.listens = listens
  }

  static async fetchAll(): Promise<Song[]> {
    const response = await axios.get<Song[]>(`${API_BASE_URL}/Song`)
    return response.data.map((data) => new Song(data.id!, data.name, data.length, data.listens))
  }
  static async fetchByIds(ids: number[]): Promise<Song[] | null> {
    try {
      const promises = ids.map((id) => axios.get<Song>(`${API_BASE_URL}/Song/${id}`))
      const responses = await Promise.all(promises)
      const songs = responses.map(
        (res) => new Song(res.data.id!, res.data.name, res.data.length, res.data.listens)
      )
      return songs
    } catch (error) {
      console.error('Error fetching songs:', error)
      return null
    }
  }
  static async fetchById(id: number): Promise<Song> {
    const response = await axios.get<Song>(`${API_BASE_URL}/Song/${id}`)
    return response.data
  }
}
