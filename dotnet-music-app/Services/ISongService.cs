public interface ISongService
{
    Task<bool> CreateSong(Song song);
    Task<List<Song>> GetSongList();
    Task<Song> GetSong(int id);
    Task<List<Song>> GetSongsFromPlaylist(int playlist_id);
    Task<List<Song>> GetSongsFromAlbum(int album_id);
    Task<Song> UpdateSong(Song song);
    Task<bool> DeleteSong(int key);
}