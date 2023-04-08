using rethus_backend.Models;
using rethus_backend.Models.Dto.User;

namespace rethus_backend.Repository.IRepository
{
  public interface IUserRepository : IRepository<User>
  {
    bool IsUniqueUser(string email);
    IEnumerable<User> GetAll();
    User GetById(string id);
    Task<User> Register(CreateRequestDto createRequestDto);
  }
}