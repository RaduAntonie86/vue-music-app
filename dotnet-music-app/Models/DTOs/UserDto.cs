public class UserDto{
    public required int Id { get; set; }
    public required string DisplayName { get; set; }
    public required string Username { get; set; }
    public string ImagePath { get; set; }
    public string Description { get; set; }
    public List<int> ListeningHistory { get; set; }
    public UserDto() { }
    protected UserDto(int id, string displayName, string username, string imagePath = "", string description = "")
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
        return new UserDto
        {
            Id = user.Id,
            DisplayName = user.DisplayName,
            Username = user.Username,
            ImagePath = user.ImagePath,
            Description = user.Description
        };
    }
}