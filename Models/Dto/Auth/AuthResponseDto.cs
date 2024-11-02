namespace rethus_backend.Models.Dto.Auth
{
    public class AuthResponseDto
    {
        public string email { get; set; }
        public string name { get; set; }
        public string token { get; set; }
        public string roles { get; set; }
        public string UserId { get; set; }

        public string step { get; set; }
        public string state { get; set; }

        public bool termCondition { get; set; }
        public string typeProcedure { get; set; }

        public string configurationId { get; set; }
        public string? UserFormId { get; set; }

        public AuthResponseDto(Models.User user, string jwtToken, Models.Configurations user_config)
        {
            this.email = user.email;
            this.name = user.name;
            this.roles = user.roles;
            this.token = jwtToken;
            this.UserId = user.UserId;
            this.step = user_config.Step.ToString();
            this.state = user_config.State.ToString();
            this.configurationId = user_config.ConfigurationsId;
            this.termCondition = user_config.TermCondition == true ? true : false;
            this.typeProcedure = user_config.TypeProcedure.ToString();
            this.UserFormId = user_config.UserFormId;
        }
    }
}
