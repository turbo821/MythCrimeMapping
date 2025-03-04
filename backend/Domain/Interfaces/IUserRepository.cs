using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmail(string email);
        Task AddUser(User user);
        Task<User?> GetUserById(Guid id);
        Task UpdateUser(User user);
        Task<bool> DeleteUser(Guid id);
    }
}
