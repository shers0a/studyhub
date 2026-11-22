namespace proiect_dotnet.Models
{
    // One row in the admin users table.
    public class AdminUserViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsAdmin { get; set; }
    }
}
