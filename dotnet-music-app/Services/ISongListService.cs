public interface ISongListService
{
    Task<bool> CreateSongList(SongList songList);
    Task<SongList> GetSongList(int id);
    Task<SongList> UpdateSongList(SongList songList);
    Task<bool> DeleteSongList(int key);
}