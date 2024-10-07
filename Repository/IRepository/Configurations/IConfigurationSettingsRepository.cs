namespace rethus_backend.Repository.IRepository
{
    public interface IConfigurationSettingRepository : IRepository<ConfigurationSetting>
    {
        string GetSettingStringValue(string key);
        bool GetSettingBoolValue(string key);
        void SaveSettingStringValue(string key, string value);
        void SaveSettingBoolValue(string key, bool value);
    }
}
