

namespace rethus_backend.Models.Dto.Auth
{
  public class AuthResponseDto
  {
    public string email { get; set; }
    public string name { get; set; }
    public string token { get; set; }
    public string Roles { get; set; }
    public string UserId { get; set; }

    public AuthResponseDto(Models.User user, string jwtToken)
    {
      this.email = user.email;
      this.name = user.name;
      this.Roles = user.Roles;
      this.token = jwtToken;
      this.UserId = user.UserId;
    }

  }
}