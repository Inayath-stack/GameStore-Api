using GameStore.Api.Models;

namespace GameStore.Api.Repositories;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string Email);
    Task AddAsync(User user);

     
}