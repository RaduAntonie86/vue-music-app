public interface ISongService
{
    Task<bool> CreateSong(Song song);
    Task<List<SongDto>> GetSongList();
    Task<SongDto> GetSong(int id);
    Task<List<SongDto>> GetSongsFromPlaylist(int playlist_id);
    Task<List<SongDto>> GetSongsFromAlbum(int album_id);
    Task<SongDto> UpdateSong(Song song);
    Task<bool> DeleteSong(int key);
}