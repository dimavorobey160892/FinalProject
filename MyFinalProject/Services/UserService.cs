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
        public bool isAdmin { get; private set; }
        public UserService(IHttpContextAccessor httpContextAccessor)
        { 
            _httpContextAccessor = httpContextAccessor;
            isAdmin = _httpContextAccessor.HttpContext.User.IsInRole(RolesEnum.Admin.ToString());
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
