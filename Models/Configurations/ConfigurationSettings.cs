using System.ComponentModel.DataAnnotations.Schema;

public class ConfigurationSetting
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string ConfigurationSettingId { get; set; }
    public required string SettingKey { get; set; }
    public required string SettingValue { get; set; }
}
