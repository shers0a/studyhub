using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace proiect_dotnet.Models
{
    // The application user: extends IdentityUser (Id, UserName, Email, PasswordHash, etc.)
    // and adds the StudyHub profile fields.
    // TODO (Pas 2): De ce mostenim IdentityUser? Ce campuri primim gratis (Id, Email, PasswordHash)?
    public class ApplicationUser : IdentityUser
    {
        // TODO (Pas 2): [Required] si [StringLength] valideaza formularul si seteaza coloana din baza. Ce diferenta este intre string si string? (camp optional)?
        [Required(ErrorMessage = "Display name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Display name must be between 2 and 50 characters.")]
        [Display(Name = "Display name")]
        public string DisplayName { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Bio cannot exceed 500 characters.")]
        [Display(Name = "Bio")]
        public string? Bio { get; set; }

        [StringLength(200)]
        [Display(Name = "Profile picture (URL)")]
        public string? AvatarUrl { get; set; }

        [StringLength(100)]
        [Display(Name = "Faculty / Study program")]
        public string? StudyProgram { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
