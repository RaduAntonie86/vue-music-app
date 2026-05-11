import axios from 'axios'

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL

export class ListeningHistory {
  songId: number
  userId: number
  listeningTime: number
  listeningDate: Date

  constructor(songId: number, userId: number, listeningTime: number, listeningDate: Date) {
    this.songId = songId
    this.userId = userId
    this.listeningTime = listeningTime
    this.listeningDate = listeningDate
  }
  static async fetchByUserId(userId: number): Promise<ListeningHistory[]> {
    const response = await axios.get<ListeningHistory[]>(`${API_BASE_URL}/ListeningHistory/user/${userId}`)
    return response.data
  }
}
