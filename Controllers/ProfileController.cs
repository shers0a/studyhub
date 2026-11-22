using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using proiect_dotnet.Models;

namespace proiect_dotnet.Controllers
{
    // Only logged-in users can see and edit their profile.
    // TODO (Pas 9): Ce se intampla daca un utilizator nelogat deschide /Profile?
    [Authorize]
    public class ProfileController : Controller
    {
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
        private const long MaxAvatarSize = 2 * 1024 * 1024; // 2 MB

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _environment;

        public ProfileController(UserManager<ApplicationUser> userManager, IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _environment = environment;
        }

        // GET: /Profile
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();
            return View(user);
        }

        // GET: /Profile/Edit
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var model = new ProfileViewModel
            {
                DisplayName = user.DisplayName,
                Bio = user.Bio,
                StudyProgram = user.StudyProgram,
                CurrentAvatarUrl = user.AvatarUrl
            };
            return View(model);
        }

        // POST: /Profile/Edit
        [HttpPost]
        // TODO (Pas 9): Ce atac previne acest atribut (CSRF) si de ce se pune pe actiunile POST?
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProfileViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            // TODO (Pas 9): De ce validam poza (extensie, dimensiune) si pe server, nu doar in browser?
            // Server-side validation of the uploaded picture.
            if (model.AvatarFile != null)
            {
                var extension = Path.GetExtension(model.AvatarFile.FileName).ToLowerInvariant();
                if (!AllowedExtensions.Contains(extension))
                    ModelState.AddModelError(nameof(model.AvatarFile), "Only .jpg, .jpeg, .png and .webp images are allowed.");
                else if (model.AvatarFile.Length > MaxAvatarSize)
                    ModelState.AddModelError(nameof(model.AvatarFile), "The image must be smaller than 2 MB.");
            }

            if (!ModelState.IsValid)
            {
                model.CurrentAvatarUrl = user.AvatarUrl;
                return View(model);
            }

            if (model.AvatarFile != null)
            {
                var extension = Path.GetExtension(model.AvatarFile.FileName).ToLowerInvariant();
                // TODO (Pas 9): De ce numele fisierului este un Guid, iar in baza salvam doar calea?
                var fileName = $"{Guid.NewGuid()}{extension}"; // unique name, avoids overwriting
                var folder = Path.Combine(_environment.WebRootPath, "uploads", "avatars");
                Directory.CreateDirectory(folder);

                using (var stream = new FileStream(Path.Combine(folder, fileName), FileMode.Create))
                {
                    await model.AvatarFile.CopyToAsync(stream);
                }
                user.AvatarUrl = $"/uploads/avatars/{fileName}";
            }

            user.DisplayName = model.DisplayName;
            user.Bio = model.Bio;
            user.StudyProgram = model.StudyProgram;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                model.CurrentAvatarUrl = user.AvatarUrl;
                return View(model);
            }

            TempData["Success"] = "Profile updated successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
