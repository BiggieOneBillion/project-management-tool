using Project.CORE.Entities;

namespace Project.CORE.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
}
