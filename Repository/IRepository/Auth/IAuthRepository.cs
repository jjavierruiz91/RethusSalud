using rethus_backend.Models;
using rethus_backend.Models.Dto.Auth;

namespace rethus_backend.Repository.IRepository.Auth
{
  public interface IAuthRepository : IRepository<User>
  {
    Task<AuthResponseDto> Authenticate(AuthRequestDto model);
    AuthResponseDto RefreshToken(string token);
    bool RevokeToken(string token, string ipAddress);
    IEnumerable<User> GetAll();
    User GetById(int id);
    string generateJwtToken(User _user);
  }
}