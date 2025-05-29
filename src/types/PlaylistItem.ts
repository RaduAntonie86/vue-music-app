import type { Album } from "./Album"
import type { Song } from "./Song"
import type { User } from "./User"

export type PlaylistItem = {
  song: Song
  artistGroup: User[]
  album: Album
}