namespace rethus_backend.Models.Dto.Configuration
{
    public class ConsecutiveAutomaticGenerate
    {
        public bool ActiveConsecutive { get; set; }
        public string ConsecutiveStart { get; set; }
        public string ConsecutiveEnd { get; set; }
        public string ConsecutiveDate { get; set; }
    }

    public class ConfigDto
    {
        public ConfigKeys Key { get; set; } // Clave basada en el enum
        public string Value { get; set; } // Valor como string (se convertirá según el tipo en el controlador)
    }

    public enum ConfigKeys
    {
        ActiveConsecutive, // Para el booleano
        ConsecutiveStart, // Para el string
        ConsecutiveEnd, // Para el string
        ConsecutiveDate, // para el DateTime,
        ConsevutiveCurrent // string consecutivo actual
    }
}
