using AutoStoreLib;
using AutoStoreLib.Enums;
using AutoStoreLib.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace MyFinalProject.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Context _context;
        public bool isAdmin { get; private set; }
        public int UserId { get; private set; }
        public UserService(IHttpContextAccessor httpContextAccessor, Context context)
        { 
            _httpContextAccessor = httpContextAccessor;
            isAdmin = _httpContextAccessor.HttpContext.User.IsInRole(RolesEnum.Admin.ToString());
            _context = context;
            var email = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            if (email != null)
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == email);
                if (user != null)
                {
                    UserId = user.Id;
                }
            }
        }

        public async Task Login(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, ((RolesEnum)user.RoleId).ToString())
            };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
        }
    }
}
