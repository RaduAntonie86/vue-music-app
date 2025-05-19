public interface ISongListService
{
    Task<bool> CreateSongList(SongList songList);
    Task<Album> GetAlbum(int id);
    Task<List<Album>> GetAlbumList();
    Task<List<Album>> GetAlbumsFromPlaylist(int playlist_id);
    Task<List<Album>> GetAlbumListByName(string name);
    Task<Playlist> GetPlaylist(int id);
    Task<List<Playlist>> GetPlaylistList();
    Task<SongList> UpdateSongList(SongList songList);
    Task<bool> DeleteAlbum(int key);
    Task<bool> DeletePlaylist(int key);
}