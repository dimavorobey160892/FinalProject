using AutoStoreLib.Models;

namespace MyFinalProject.Services
{
    public interface IUserService
    {
        bool isAdmin { get; }

        Task Login(User user);
    }
}
