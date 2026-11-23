using System.ComponentModel.DataAnnotations;

namespace proiect_dotnet.Models
{
    public class StudyGroup
    {
        public int Id{get; set;}

        [Required, StringLength(100)] //if anyone will be reading this code i swear java annotations look so much more aesthetic
        public string Name {get; set;} = string.Empty;

        [Required, StringLength(1000)]
        public string Description {get; set;} = string.Empty;

        [Required, StringLength(100)]
        public string Subject {get; set;} = string.Empty;

        public DateTime CreatedAt {get; set;} = DateTime.UtcNow;

        public string ModeratorId {get; set;} = string.Empty;
        public ApplicationUser? Moderator {get; set;}

        public ICollection<GroupMembership> Memberships {get; set;} = new List<GroupMembership>();
        public ICollection<GroupMessage> Messages {get; set; } = new List<GroupMessage>();

    }
}