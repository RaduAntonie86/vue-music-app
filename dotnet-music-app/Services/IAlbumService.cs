public interface IAlbumService
{
    Task<bool> CreateAlbum(Album album);
    Task<Album> GetAlbum(int id);
    Task<List<Album>> GetAlbumList();
    Task<List<Album>> GetAlbumsFromPlaylist(int playlist_id);
    Task<List<Album>> GetAlbumListByName(string name);
    Task<SongList> UpdateAlbum(Album album);
    Task<bool> DeleteAlbum(int key);
}