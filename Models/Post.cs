using System.ComponentModel.DataAnnotations;

namespace proiect_dotnet.Models
{
    public class Post
    {
        public int Id {get; set;}

        [Required, StringLength(200)]
        public string Title {get; set;} = string.Empty;

        [Required, StringLength(100)]
        public string Subject {get; set;} = string.Empty;

        public string? TextContent {get; set;}
        public string? ImagePath {get; set;} //path to the image uploaded
        public string? VideoPath {get; set;} //path to video uploaded

        [Url]
        public string? VideoUrl {get; set;} // link to the video

        public DateTime CreatedAt {get; set;} = DateTime.UtcNow;

        public string UserId {get; set;} = string.Empty;
        public ApplicationUser? User {get; set;}

        public ICollection<Comment> Comments {get; set;} = new List<Comment>();
    }
}
