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

    public enum TypeIdentification
    {
        CC, // Cédula de ciudadanía
        CE, // Cédula de extranjería
        TP, // "Permiso por proteccion personal"
        TI, // Tarjeta de identidad
    }

    public enum TypeGender
    {
        M,
        F,
    }

    public enum TypeEthnicGroup
    {
        indigenous,
        palenquero,
        rom,
        afro,
        razal,
        noneAbove,
    }

    public enum TypeInstitution
    {
        educacionSuperior,
        educacionTdh,
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
        public string identification { get; set; }
    }

    public class UserConstants
    {
        public static UserStatus getFormatStringToUserStatus(string status)
        {
            switch (status)
            {
                case "active":
                    return UserStatus.active;

                case "inactive":
                    return UserStatus.inactive;

                default:
                    return UserStatus.inactive;
            }
        }

        public static string EthnicGroupToSpanish(TypeEthnicGroup? ethnicGroup)
        {
            string translation;

            switch (ethnicGroup)
            {
                case TypeEthnicGroup.indigenous:
                    translation = "Indígena";
                    break;
                case TypeEthnicGroup.palenquero:
                    translation = "Palenquero";
                    break;
                case TypeEthnicGroup.rom:
                    translation = "Rom";
                    break;
                case TypeEthnicGroup.afro:
                    translation = "Afrodescendiente";
                    break;
                case TypeEthnicGroup.razal:
                    translation = "Razal";
                    break;
                case TypeEthnicGroup.noneAbove:
                    translation = "Ninguno de los anteriores";
                    break;
                default:
                    translation = "Ninguno de los anteriores";
                    break;
            }

            return translation;
        }

        public static string GetIdentificationTypeInSpanish(TypeIdentification tipo)
        {
            switch (tipo)
            {
                case TypeIdentification.CC:
                    return "Cédula de ciudadanía";
                case TypeIdentification.TI:
                    return "Tarjeta de identidad";
                case TypeIdentification.CE:
                    return "Cédula de extranjería";
                case TypeIdentification.TP:
                    return "Permiso por proteccion temporal";
                default:
                    return "Tipo de identificación desconocido";
            }
        }
    }
}
