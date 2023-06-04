

namespace rethus_backend.Models.Dto.Auth
{
  public class AuthResponseDto
  {
    public string email { get; set; }
    public string name { get; set; }
    public string token { get; set; }
    public string Roles { get; set; }
    public string UserId { get; set; }

    public string step { get; set; }
    public string state { get; set; }

    public bool termCondition { get; set; }
    public string type_procedure { get; set; }

    public string configurationId { get; set; }

    public AuthResponseDto(Models.User user, string jwtToken, Models.Configurations user_config)
    {
      this.email = user.email;
      this.name = user.name;
      this.Roles = user.Roles;
      this.token = jwtToken;
      this.UserId = user.UserId;
      this.step = user_config.step;
      this.state = user_config.state;
      this.configurationId = user_config.ConfigurationsId;
      this.termCondition = user_config.termCondition == true ? true : false;
      this.type_procedure = user_config.type_procedure;
    }

  }
}