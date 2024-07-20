using rethus_backend.Models;

namespace rethus_backend.Utilities.Constants.User.UserFormConstants
{
    public enum UserFormStatus
    {
        pending,
        reject,
        approved
    }

    public enum ReviewStepForm
    {
        officer1,
        officer2,
        officer3,
        success,
        error
    }

    public class UserFormConstants
    {
        public static ReviewStepForm GetNextRebiewStepForm(ReviewStepForm step)
        {
            switch (step)
            {
                case ReviewStepForm.officer1:
                    return ReviewStepForm.officer2;

                case ReviewStepForm.officer2:
                    return ReviewStepForm.officer3;

                case ReviewStepForm.officer3:
                    return ReviewStepForm.success;
                default:
                    return ReviewStepForm.error;
            }
        }
    }

    public enum TypeUploadFile
    {
        sso_cedula,
        sso_diploma_grado,
        sso_acta_grado,
        sso_nombramiento_cargo,
        sso_certificado_prestacion,
        rethus_cedula_ampliada,
        rethus_diploma_grado,
        rethus_acta_grado,
        rethus_tarjeta_profesional,
    }
}
