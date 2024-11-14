using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using rethus_backend.Models;

namespace rethus_backend.Utilities.Constants.User.UserFormConstants
{
    public enum UserFormStatus
    {
        pending, // Formulario pendiente de revisión inicial
        reject, // Formulario rechazado por el usuario 1
        approved, // Formulario aprobado
        needsReview // Usuario 2 ha realizado cambios y el formulario requiere revisión nuevamente
    }

    public enum ReviewStepForm
    {
        FuncionarioEtapa1,
        FuncionarioEtapa2,
        FuncionarioEtapa3,
        success,
        error
    }

    public class UserFormConstants
    {
        public static ReviewStepForm GetNextRebiewStepForm(ReviewStepForm step)
        {
            switch (step)
            {
                case ReviewStepForm.FuncionarioEtapa1:
                    return ReviewStepForm.FuncionarioEtapa2;

                case ReviewStepForm.FuncionarioEtapa2:
                    return ReviewStepForm.FuncionarioEtapa3;

                case ReviewStepForm.FuncionarioEtapa3:
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
