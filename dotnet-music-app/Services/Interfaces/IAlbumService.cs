public interface IAlbumService
{
    Task<bool> CreateAlbum(Album album);
    Task<AlbumDto> GetAlbum(int id);
    Task<List<AlbumDto>> GetAlbumList();
    Task<List<AlbumDto>> GetAlbumsFromPlaylist(int playlist_id);
    Task<AlbumDto> GetAlbumFromSong(int song_id);
    Task<List<AlbumDto>> GetAlbumListByName(string name);
    Task<AlbumDto> UpdateAlbum(Album album);
    Task<bool> DeleteAlbum(int key);
}