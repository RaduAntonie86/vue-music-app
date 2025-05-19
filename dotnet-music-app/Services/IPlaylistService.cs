public interface IPlaylistService
{
    Task<bool> CreatePlaylist(Playlist playlist);
    Task<PlaylistDto> GetPlaylist(int id);
    Task<List<PlaylistDto>> GetPlaylistList();
    Task<PlaylistDto> UpdatePlaylist(Playlist playlist);
    Task<bool> DeletePlaylist(int key);
}