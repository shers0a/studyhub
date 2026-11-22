using System.ComponentModel.DataAnnotations;

namespace proiect_dotnet.Models
{
    // Data shown and edited on the profile edit form.
    public class ProfileViewModel
    {
        [Required(ErrorMessage = "Display name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Display name must be between 2 and 50 characters.")]
        [Display(Name = "Display name")]
        public string DisplayName { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Bio cannot exceed 500 characters.")]
        [Display(Name = "Bio")]
        public string? Bio { get; set; }

        [StringLength(100, ErrorMessage = "Study program cannot exceed 100 characters.")]
        [Display(Name = "Faculty / Study program")]
        public string? StudyProgram { get; set; }

        // The newly uploaded picture (optional).
        [Display(Name = "Profile picture")]
        public IFormFile? AvatarFile { get; set; }

        // The picture currently saved, only used to display it in the form.
        public string? CurrentAvatarUrl { get; set; }
    }
}
