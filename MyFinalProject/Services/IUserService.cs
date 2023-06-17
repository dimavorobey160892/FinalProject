using AutoStoreLib.Models;

namespace MyFinalProject.Services
{
    public interface IUserService
    {
        bool isAdmin { get; }
        int UserId { get; }
        Task Login(User user);
    }
}
