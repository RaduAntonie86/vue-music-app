public interface IPlaylistService
{
    Task<bool> CreatePlaylist(Playlist playlist);
    Task<Playlist> GetPlaylist(int id);
    Task<List<Playlist>> GetPlaylistList();
    Task<SongList> UpdatePlaylist(Playlist playlist);
    Task<bool> DeletePlaylist(int key);
}