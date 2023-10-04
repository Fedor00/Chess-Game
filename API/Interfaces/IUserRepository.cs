using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByEmailAsync(string email);
        Task<IEnumerable<User>> GetUsersAsync();
        void AddUserAsync(User user);
        void UpdateUser(User user);
        void DeleteUser(User user);
        Task<bool> SaveChangesAsync();

    }
}