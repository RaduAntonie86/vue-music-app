export class SongList {
  id?: number
  name: string
  imagePath: string
  songIds: number[]

  constructor(id: number, name: string, imagePath: string, songIds: number[]) {
    this.id = id
    this.name = name
    this.imagePath = imagePath
    this.songIds = songIds
  }
}
