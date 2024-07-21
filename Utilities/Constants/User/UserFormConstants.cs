using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
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

        public static UserFormStatus? GetStatus(string step)
        {
            switch (step)
            {
                case "approved":
                    return UserFormStatus.approved;

                case "pending":
                    return UserFormStatus.pending;

                case "reject":
                    return UserFormStatus.reject;
                default:
                    return null;
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
