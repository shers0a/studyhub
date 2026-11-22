using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proiect_dotnet.Data;
using proiect_dotnet.Models;

namespace proiect_dotnet.Controllers
{
    // Every action in this controller is available ONLY to users in the Admin role.
    // TODO (Pas 10): Ce se intampla daca un utilizator obisnuit deschide /Admin? Unde altundeva se poate pune acest atribut?
    [Authorize(Roles = Roles.Admin)]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // GET: /Admin  (dashboard)
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Admin/Users
        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users.OrderBy(u => u.CreatedAt).ToListAsync();
            var rows = new List<AdminUserViewModel>();

            foreach (var user in users)
            {
                rows.Add(new AdminUserViewModel
                {
                    Id = user.Id,
                    DisplayName = string.IsNullOrWhiteSpace(user.DisplayName) ? "(no name)" : user.DisplayName,
                    Email = user.Email ?? string.Empty,
                    CreatedAt = user.CreatedAt,
                    IsAdmin = await _userManager.IsInRoleAsync(user, Roles.Admin)
                });
            }
            return View(rows);
        }

        // POST: /Admin/ToggleAdmin/5  - promotes a User to Admin or demotes an Admin to User
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleAdmin(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            // TODO (Pas 10): De ce nu lasam adminul sa-si schimbe singur rolul?
            // An admin cannot remove their own admin role (avoids locking everyone out).
            if (user.Id == _userManager.GetUserId(User))
            {
                TempData["Error"] = "You cannot change your own role.";
                return RedirectToAction(nameof(Users));
            }

            if (await _userManager.IsInRoleAsync(user, Roles.Admin))
            {
                await _userManager.RemoveFromRoleAsync(user, Roles.Admin);
                await _userManager.AddToRoleAsync(user, Roles.User);
                TempData["Success"] = $"{user.Email} is now a regular user.";
            }
            else
            {
                await _userManager.RemoveFromRoleAsync(user, Roles.User);
                await _userManager.AddToRoleAsync(user, Roles.Admin);
                TempData["Success"] = $"{user.Email} is now an administrator.";
            }
            return RedirectToAction(nameof(Users));
        }

        // POST: /Admin/DeleteUser/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            if (user.Id == _userManager.GetUserId(User))
            {
                TempData["Error"] = "You cannot delete your own account.";
                return RedirectToAction(nameof(Users));
            }

            try
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                    TempData["Success"] = $"User {user.Email} was deleted.";
                else
                    TempData["Error"] = string.Join(" ", result.Errors.Select(e => e.Description));
            }
            // TODO (Pas 10): Cand apare aceasta exceptie? (gandeste-te la relatiile Restrict din task-ul Follow/Reaction)
            catch (DbUpdateException)
            {
                // Later, the user may still own posts/groups (Restrict relationships).
                TempData["Error"] = "This user still has related data (posts, comments or groups) and cannot be deleted yet.";
            }
            return RedirectToAction(nameof(Users));
        }
    }
}
