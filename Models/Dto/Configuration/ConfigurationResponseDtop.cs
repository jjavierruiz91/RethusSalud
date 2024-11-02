namespace rethus_backend.Models.Dto.Configuration
{
    public class ConfigurationResponseDto
    {
        public string ConfigurationsId { get; set; }
        public string state { get; set; }
        public string step { get; set; }
        public string typeProcedure { get; set; }
        public bool termCondition { get; set; }
        public string userId { get; set; }
        public string? userFormId { get; set; }

        public ConfigurationResponseDto(Configurations _configuration)
        {
            this.ConfigurationsId = _configuration.ConfigurationsId;
            this.state = _configuration.State.ToString();
            this.step = _configuration.Step.ToString();
            this.typeProcedure = _configuration.TypeProcedure.ToString();
            this.termCondition = _configuration.TermCondition;
            this.userId = _configuration.UserId;
            this.userFormId = _configuration.UserFormId;
        }
    }
}
