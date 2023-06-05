using AutoStoreLib.Enums;
using AutoStoreLib.Models;
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
    }
}
