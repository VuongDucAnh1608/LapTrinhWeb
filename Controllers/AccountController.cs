using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Website_QuanLyKhoHangThucPham.Models;
using Website_QuanLyKhoHangThucPham.Services;
using Website_QuanLyKhoHangThucPham.ViewModels;

namespace Website_QuanLyKhoHangThucPham.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser>  _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole>     _roleManager;
        private readonly IEmailService  _emailService;
        private readonly IAuditService  _auditService;

        public AccountController(UserManager<ApplicationUser> um, SignInManager<ApplicationUser> sm,
            RoleManager<IdentityRole> rm, IEmailService email, IAuditService audit)
        {
            _userManager   = um;  _signInManager = sm;
            _roleManager   = rm;  _emailService  = email; _auditService = audit;
        }

        // ── Login ──────────────────────────────────────────────────────────
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true) return RedirectToAction("Index", "Home");
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !user.IsActive)
            { ModelState.AddModelError("", "Email hoac mat khau khong chinh xac."); return View(model); }

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, true);
            if (result.Succeeded)
            {
                await _auditService.LogAsync(user.Id, "LOGIN", "User", user.Id,
                    HttpContext.Connection.RemoteIpAddress?.ToString());
                return !string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)
                    ? Redirect(returnUrl) : RedirectToAction("Index", "Home");
            }
            if (result.IsLockedOut) { ModelState.AddModelError("", "Tai khoan bi khoa tam thoi."); return View(model); }
            ModelState.AddModelError("", "Email hoac mat khau khong chinh xac.");
            return View(model);
        }

        // ── Google OAuth ───────────────────────────────────────────────────
        [HttpGet]
        public IActionResult LoginWithGoogle(string? returnUrl = null)
        {
            var redirectUrl = Url.Action("GoogleCallback", "Account", new { returnUrl });
            var props = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return Challenge(props, "Google");
        }

        [HttpGet]
        public async Task<IActionResult> GoogleCallback(string? returnUrl = null)
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null) return RedirectToAction("Login");

            var email    = info.Principal.FindFirstValue(ClaimTypes.Email)!;
            var fullName = info.Principal.FindFirstValue(ClaimTypes.Name) ?? email;

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                // Tự động tạo tài khoản với role SalesStaff
                user = new ApplicationUser
                {
                    UserName = email, Email = email, FullName = fullName,
                    EmailConfirmed = true, IsActive = true
                };
                await _userManager.CreateAsync(user);
                await _userManager.AddToRoleAsync(user, "SalesStaff");
                await _userManager.AddLoginAsync(user, info);
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            await _auditService.LogAsync(user.Id, "LOGIN_GOOGLE", "User", user.Id,
                HttpContext.Connection.RemoteIpAddress?.ToString());

            return !string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)
                ? Redirect(returnUrl) : RedirectToAction("Index", "Home");
        }

        // ── Logout ─────────────────────────────────────────────────────────
        [HttpPost, ValidateAntiForgeryToken, Authorize]
        public async Task<IActionResult> Logout()
        {
            var uid = _userManager.GetUserId(User);
            await _auditService.LogAsync(uid, "LOGOUT", "User", uid);
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        // ── Register (Admin tao user) ──────────────────────────────────────
        [HttpGet, Authorize(Policy = "AdminOnly")]
        public IActionResult Register()
        {
            ViewBag.Roles = new List<string> { "Admin", "WarehouseStaff", "SalesStaff" };
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) { ViewBag.Roles = new List<string> { "Admin","WarehouseStaff","SalesStaff" }; return View(model); }

            var user = new ApplicationUser
            {
                UserName = model.Email, Email = model.Email, FullName = model.FullName,
                PhoneNumber = model.PhoneNumber, EmailConfirmed = true, IsActive = true
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(model.Role)) await _userManager.AddToRoleAsync(user, model.Role);
                TempData["Success"] = $"Tao tai khoan thanh cong cho {model.FullName}.";
                return RedirectToAction("UserList");
            }
            foreach (var e in result.Errors) ModelState.AddModelError("", e.Description);
            ViewBag.Roles = new List<string> { "Admin","WarehouseStaff","SalesStaff" };
            return View(model);
        }

        // ── Forgot / Reset Password ────────────────────────────────────────
        [HttpGet]
        public IActionResult ForgotPassword() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && user.IsActive)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var link  = Url.Action("ResetPassword","Account",
                    new { token = Uri.EscapeDataString(token), email = user.Email }, Request.Scheme) ?? "";
                await _emailService.SendPasswordResetAsync(user.Email!, link);
            }
            return View("ForgotPasswordConfirmation");
        }

        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
            => View(new ResetPasswordViewModel { Token = Uri.UnescapeDataString(token), Email = email });

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
                if (result.Succeeded) { TempData["Success"] = "Dat lai mat khau thanh cong!"; return RedirectToAction("Login"); }
                foreach (var e in result.Errors) ModelState.AddModelError("", e.Description);
            }
            return View(model);
        }

        [Authorize] public IActionResult AccessDenied() => View();

        [HttpGet, Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UserList()
        {
            var users = _userManager.Users.Where(u => u.IsActive).ToList();
            var result = new List<(ApplicationUser, IList<string>)>();
            foreach (var u in users) result.Add((u, await _userManager.GetRolesAsync(u)));
            return View(result);
        }
    }
}
