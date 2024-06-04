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
        DF = 0,
        TCSSO = 5,
        RGNTHST = 4
    }

    public enum EnumProcedure
    {
        TCSSO = 1,
        RGNTHST = 2
    }

    public enum EnumStepConfiguration
    {
        select_procedure = 1,
        acept_terms_conditions = 2,
        load_user_form = 3,
        load_user_files = 4,
        etapa_1 = 5,
        etapa_2 = 6,
        etapa_3 = 7,
        etapa_4 = 8,
        success = 9,
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

        public static string GetNextStep(string step)
        {
            switch (step)
            {
                case "select_procedure":
                    return "acept_terms_conditions";

                case "acept_terms_conditions":
                    return "load_user_form";

                case "load_user_form":
                    return "load_user_files";

                case "load_user_files":
                    return "step1";

                case "step1":
                    return "step2";

                case "step2":
                    return "step3";

                case "step3":
                    return "inventory";

                case "inventory":
                    return "success";
                default:
                    return "step1";
            }
        }
    }
}
