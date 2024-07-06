using rethus_backend.Models;

namespace rethus_backend.Utilities.Constants.UserConstants
{
    public enum UserRoles
    {
        User,
        Admin,
        SuperAdmin,
        FuncionarioEtapa1,
        FuncionarioEtapa2,
        FuncionarioEtapa3,
        Inventory
    }

    public enum EnumMaximumAmountFiles
    {
        DF = 0, //Default
        SSO = 5, //tramite sso
        RETHUS = 4 // tremite rethus
    }

    public enum UserStatus
    {
        active = 1,
        inactive = 0
    }

    // DTO para los parámetros de consulta de usuario
    public class UserQueryParametersDto
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        // Otros campos de filtro que puedas necesitar
    }

    // DTO para los datos de usuario que se devolverán en la paginación
    public class UserDto
    {
        public string UserId { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public UserStatus? Status { get; set; }
        public string Roles { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class UserConstants
    {
        // public static EnumStepConfiguration GetNextStep(int currentStep)
        // {
        //   int nextValue = (int)currentStep + 1;
        //   EnumStepConfiguration[] enumValues = (EnumStepConfiguration[])Enum.GetValues(typeof(EnumStepConfiguration));

        //   if (nextValue <= (int)EnumStepConfiguration.success)
        //     return enumValues[nextValue];

        //   return EnumStepConfiguration.success;
        // }
    }
}
