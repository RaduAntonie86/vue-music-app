public interface ISongService
{
    Task<bool> CreateSong(Song song);
    Task<List<Song>> GetSongList();
    Task<Song> GetSong(int id);
    Task<Song> UpdateSong(Song song);
    Task<bool> DeleteSong(int key);
}