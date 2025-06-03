public interface IPlaylistService
{
    Task<bool> CreatePlaylist(Playlist playlist);
    Task<PlaylistDto> GetPlaylist(int id);
    Task<List<PlaylistDto>> GetPlaylistList();
    Task<PlaylistDto> UpdatePlaylist(Playlist playlist);
    Task<bool> AddSongToPlaylist(int playlistId, int songId);
    Task<bool> DeletePlaylist(int key);
    Task<List<PlaylistDto>> GetPlaylistsByUserId(int userId);
    Task<List<PlaylistDto>> GetPlaylistsByName(string name);
    Task<bool> RemoveSongFromPlaylist(int playlistId, int songId);
}