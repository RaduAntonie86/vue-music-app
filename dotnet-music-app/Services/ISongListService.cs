public interface ISongListService
{
    Task<bool> CreateSongList(SongList songList);
    Task<Album> GetAlbum(int id);
    Task<Playlist> GetPlaylist(int id);
    Task<SongList> UpdateSongList(SongList songList);
    Task<bool> DeleteAlbum(int key);
    Task<bool> DeletePlaylist(int key);
}