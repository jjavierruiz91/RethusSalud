using Microsoft.Extensions.Caching.Memory;
using rethus_backend.Data;
using rethus_backend.Repository.IRepository;

namespace rethus_backend.Repository
{
    public class ConfigurationSettingRepository
        : Repository<ConfigurationSetting>,
            IConfigurationSettingRepository
    {
        private readonly ApplicationDbContext _context;

        private readonly MemoryCacheRepository<ConfigurationSetting> _memoryCache;

        public ConfigurationSettingRepository(ApplicationDbContext db, IMemoryCache memoryCache)
            : base(db)
        {
            _context = db;
            _memoryCache = new MemoryCacheRepository<ConfigurationSetting>(memoryCache);
        }

        public string GetSettingStringValue(string key)
        {
            var cacheKey = $"CONFIGURATION_SETTING_{key}";
            ConfigurationSetting setting;

            // Intentar obtener el valor desde la caché
            setting = _memoryCache.GetFromCache(cacheKey);

            if (setting == null)
            {
                // Si no está en caché, consulta la base de datos
                setting = _context.ConfigurationSetting.FirstOrDefault(c => c.SettingKey == key);

                // Verificar si el setting obtenido de la base de datos es nulo
                if (setting != null)
                {
                    // Establecer en caché por un tiempo específico si se encontró el setting
                    var cacheDuration = TimeSpan.FromDays(7);
                    _memoryCache.SetToCache(cacheKey, setting, cacheDuration);
                }
                else
                {
                    // Manejo en caso de que no se encuentre el setting en la base de datos
                    return null; // O lanza una excepción si es necesario
                }
            }

            // Asegurarse de que el setting no sea nulo antes de retornar su valor
            return setting?.SettingValue;
        }

        public bool GetSettingBoolValue(string key)
        {
            var cacheKey = $"CONFIGURATION_SETTING_{key}";
            ConfigurationSetting setting;

            // Obtener el valor de la caché si existe
            setting = _memoryCache.GetFromCache(cacheKey);

            if (setting == null)
            {
                // Si no está en caché, consulta la base de datos
                setting = _context.ConfigurationSetting.FirstOrDefault(c => c.SettingKey == key);

                // Si se encuentra en la base de datos, almacenarlo en caché
                if (setting != null)
                {
                    setting.SettingValue = "false";
                    var cacheDuration = TimeSpan.FromDays(7);
                    _memoryCache.SetToCache(cacheKey, setting, cacheDuration);
                    return false;
                }
            }

            // Intentar convertir el valor string a bool
            if (setting != null && bool.TryParse(setting.SettingValue, out bool boolValue))
            {
                return boolValue;
            }

            // Devolver false en caso de que el valor sea nulo o no se pueda convertir a bool
            return false;
        }

        public void SaveSettingStringValue(string key, string value)
        {
            var setting = _context.ConfigurationSetting.FirstOrDefault(c => c.SettingKey == key);

            if (setting != null)
            {
                // Si existe, actualizar el valor
                setting.SettingValue = value;
                _context.ConfigurationSetting.Update(setting);
            }
            else
            {
                // Si no existe, crear una nueva configuración
                setting = new ConfigurationSetting { SettingKey = key, SettingValue = value };
                _context.ConfigurationSetting.Add(setting);
            }

            // Guardar los cambios en la base de datos
            try
            {
                _context.SaveChanges();
                var cacheKey = $"CONFIGURATION_SETTING_{key}";
                _memoryCache.RemoveFromCache(cacheKey);
                var cacheDuration = TimeSpan.FromDays(7);
                _memoryCache.SetToCache(cacheKey, setting, cacheDuration);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public void SaveSettingBoolValue(string key, bool value)
        {
            string stringValue = value ? "true" : "false";

            var existingSetting = _context.ConfigurationSetting.FirstOrDefault(
                c => c.SettingKey == key
            );

            if (existingSetting != null)
            {
                existingSetting.SettingValue = stringValue;
                _context.ConfigurationSetting.Update(existingSetting);
            }
            else
            {
                existingSetting = new ConfigurationSetting
                {
                    SettingKey = key,
                    SettingValue = stringValue
                };

                _context.ConfigurationSetting.Add(existingSetting);
            }

            try
            {
                _context.SaveChanges();
                var cacheKey = $"CONFIGURATION_SETTING_{key}";
                var cacheDuration = TimeSpan.FromDays(7);
                _memoryCache.SetToCache(cacheKey, existingSetting, cacheDuration);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public bool? ValidateActiveConsecutive()
        {
            bool? isActive = this.GetSettingBoolValue("ActiveConsecutive");

            if (isActive == null)
            {
                return null;
            }

            return isActive;
        }

        public int validateConsevutiveCurrent()
        {
            string ConsevutiveCurrent = this.GetSettingStringValue("ConsevutiveCurrent");

            int consecutiveNumber;
            bool isConvert = int.TryParse(ConsevutiveCurrent, out consecutiveNumber);

            if (!isConvert)
            {
                throw new Exception("No se logro convertir el numero de string a int");
            }

            return consecutiveNumber;
        }

        public string GetConsecutiveDate()
        {
            string ConsecutiveDate = this.GetSettingStringValue("ConsecutiveDate");

            if (ConsecutiveDate == null)
            {
                throw new Exception("El valor de 'ConsecutiveDate' es nulo.");
            }

            return ConsecutiveDate;
        }

        public string GetConsevutiveCurrent()
        {
            bool? isActiveGenerate = this.ValidateActiveConsecutive();

            if (isActiveGenerate == null)
            {
                throw new Exception("La generacion de automatica de cosecutivo no est activa");
            }

            int ConsecutiveCurrent = this.validateConsevutiveCurrent();
            ConsecutiveCurrent++;
            return ConsecutiveCurrent.ToString();
        }
    }
}
