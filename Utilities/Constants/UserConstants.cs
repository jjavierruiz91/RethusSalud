
namespace rethus_backend.Utilities.Constants.UserConstants
{

  public enum EnumUserType
  {
    user,
    admin,
    superAdmin,
    functionary,

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
          return "etapa_1";

        case "etapa_1":
          return "etapa_2";

        case "etapa_2":
          return "etapa_3";

        case "etapa_3":
          return "etapa_4";

        case "etapa_4":
          return "success";

        case "success":
          return "success";
        default:
          return "Paso desconocido";
      }
    }
  }



}