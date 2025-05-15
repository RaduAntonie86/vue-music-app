public class User
{
    public string? Id { get; set; }
    public required string DisplayName { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public string? ImagePath { get; set; }
    public string? Description { get; set; }
    public List<int>? ListeningHistory { get; set; }
    public User() { }
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