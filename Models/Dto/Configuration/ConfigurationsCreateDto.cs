namespace rethus_backend.Models.Dto.Configuration
{
  public class ConfigurationCreateDto
  {
    public string? email { get; set; }
  }

  public class UpdateStepConfigurationDto
  {
    public string step { get; set; }
  }

  public class UpdateTypeProcessConfigurationDto
  {
    public string type { get; set; }
  }

  public class UpdateTermConditionsConfigurationDto
  {
    public bool termCondition { get; set; }
  }
}