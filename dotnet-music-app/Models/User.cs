public class User
{
    public required string Id { get; set; }
    public required string DisplayName { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string ImagePath { get; set; }
    public required string Description { get; set; }
    public required List<int> ListeningHistory { get; set; }
    public User(string id, string displayName, string username, string password, string imagePath = "", string description = "")
    {
        Id = id;
        DisplayName = displayName;
        Username = username;
        Password = password;
        ImagePath = imagePath;
        Description = description;
        ListeningHistory = new List<int>();
    }
}