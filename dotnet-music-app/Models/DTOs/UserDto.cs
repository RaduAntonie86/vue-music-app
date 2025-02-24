public class UserDto{
    public string Id { get; set; }
    public string DisplayName { get; set; }
    public string Username { get; set; }
    public string ImagePath { get; set; }
    public string Description { get; set; }
    public List<int> ListeningHistory { get; set; }
    protected UserDto(string id, string displayName, string username, string imagePath = "", string description = "")
    {
        Id = id;
        DisplayName = displayName;
        Username = username;
        ImagePath = imagePath;
        Description = description;
        ListeningHistory = new List<int>();
    }    
    public static UserDto CopyUserToDto(User user)
    {
        return new UserDto(
            user.Id,
            user.DisplayName,
            user.Username,
            user.ImagePath,
            user.Description
        );
    }
}