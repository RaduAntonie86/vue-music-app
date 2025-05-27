import axios from 'axios'

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL
export class User {
  id: number
  displayName: string
  username: string
  description: string
  imagePath: string

  constructor(
    id: number,
    displayName: string,
    username: string,
    description: string,
    imagePath: string
  ) {
    this.id = id
    this.displayName = displayName
    this.username = username
    this.description = description
    this.imagePath = imagePath
  }

  static async fetchSongArtists(songId: number): Promise<User[]> {
    const response = await axios.get<User[]>(`${API_BASE_URL}/User/song_id/${songId}`)
    return response.data
  }

  static async fetchUsersByIds(ids: number[]): Promise<User[] | null> {
    try {
      const promises = ids.map((id) => axios.get<User>(`${API_BASE_URL}/User/${id}`))
      const responses = await Promise.all(promises)
      const users = responses.map(
        (res) =>
          new User(
            res.data.id,
            res.data.displayName,
            res.data.username,
            res.data.description,
            res.data.imagePath
          )
      )
      return users
    } catch (error) {
      console.error('Error fetching users:', error)
      return null
    }
  }
  static async fetchAlbumArtists(albumId: number): Promise<User[]> {
    const response = await axios.get<User[]>(`${API_BASE_URL}/User/album_id/${albumId}`)
    return response.data
  }
}
