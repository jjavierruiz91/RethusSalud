using rethus_backend.Models;

namespace rethus_backend.Utilities.Constants.User.UserConfiguration
{
    public class UserConfiguration
    {
        public static ConfigurationStep GetNextStepOnboarding(ConfigurationStep step)
        {
            switch (step)
            {
                case ConfigurationStep.acept_terms_conditions:
                    return ConfigurationStep.select_procedure;

                case ConfigurationStep.select_procedure:
                    return ConfigurationStep.load_user_form;

                case ConfigurationStep.load_user_form:
                    return ConfigurationStep.load_user_files;

                case ConfigurationStep.load_user_files:
                    return ConfigurationStep.success;
                default:
                    return ConfigurationStep.error;
            }
        }

        public static ConfigurationTypeProcedure GetConfigurationType(string type)
        {
            switch (type.ToUpper())
            {
                case "SSO":
                    return ConfigurationTypeProcedure.SSO;
                case "RETHUS":
                    return ConfigurationTypeProcedure.RETHUS;
                default:
                    return ConfigurationTypeProcedure.DEFAULT;
            }
        }
    }
}
