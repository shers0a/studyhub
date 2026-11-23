using System.ComponentModel.DataAnnotations;

namespace proiect_dotnet.Models
{
    public class GroupMessage
    {
        public int Id {get; set;}

        [Required, StringLength(2000)]
        public string Content {get; set;} = string.Empty;

        public DateTime CreatedAt {get; set;} = DateTime.UtcNow;

        public int GroupId {get; set;}
        public StudyGroup? Group {get; set;}
        
        public string UserId {get; set;} = string.Empty;
        public ApplicationUser? User {get; set;}
    }
}