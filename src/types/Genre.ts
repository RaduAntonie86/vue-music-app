import axios from 'axios'

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL

export class Genre {
  id: number
  name: string
  constructor(id: number, name: string) {
    this.id = id
    this.name = name
  }
  static async fetchById(genreId: number): Promise<Genre> {
    const response = await axios.get<Genre>(`${API_BASE_URL}/Genre/${genreId}`)
    return response.data
  }
  static async fetchByIds(ids: number[]): Promise<Genre[]> {
    const response = await axios.post<Genre[]>(`${API_BASE_URL}/Genre/byIds`, ids)
    return response.data
  }
  static async fetchAll(): Promise<Genre[]> {
    const response = await axios.get<Genre[]>(`${API_BASE_URL}/Genre`)
    return response.data
  }
}
