using System.ComponentModel.DataAnnotations.Schema;
using rethus_backend.Utilities.Constants.User.UserFormConstants;
using rethus_backend.Utilities.Constants.UserConstants;

namespace rethus_backend.Models
{
    public class UserForm
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string UserFormId { get; set; }

        public required string PersonalTypeIdentification { get; set; }
        public required string PersonalGender { get; set; }
        public required string PersonalIdentification { get; set; }
        public required string PersonalFirstName { get; set; }
        public required string PersonalLastName { get; set; }
        public required string PersonalCountryBirth { get; set; }
        public required string PersonalDepartmentBirth { get; set; }
        public required string PersonalMunicipalityBirth { get; set; }
        public required DateTime DateBirth { get; set; }
        public required string PersonalPlaceResidence { get; set; }
        public required string PersonalDepartmentResidence { get; set; }
        public required string PersonalMunicipalityResidence { get; set; }
        public required string PersonalAddress { get; set; }
        public required int PersonalTelephone { get; set; }
        public required int PersonalPhone { get; set; }
        public required string PersonalEmail { get; set; }
        public required string PersonalEthnicGroup { get; set; }
        public required string AcademicsOriginTitle { get; set; }
        public required string AcademicsTypeInstitution { get; set; }
        public required string AcademicsProgramType { get; set; }
        public required string AcademicsDepartmentInstitution { get; set; }
        public required string AcademicsMunicipalityInstitution { get; set; }
        public required string AcademicsNameInstitution { get; set; }
        public required string AcademicsProgramName { get; set; }
        public required DateTime AcademicsDateInstitution { get; set; }
        public required DateTime AcademicsGradeDate { get; set; }
        public required string AcademicsNumberConvalidation { get; set; }
        public required DateTime AcademicsDateConvalidation { get; set; }
        public required string AcademicsTitle { get; set; }
        public required string AcademicsNumberAdministrative { get; set; }
        public required DateTime AcademicsDateAdministrative { get; set; }

        public required string TypeProcedure { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public ICollection<UserFormFiles>? UserFormFiles { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public UserFormStatus Status { get; set; }

        public ReviewStepForm StepForm { get; set; }

        public string? Consecutive { get; set; }
    }

    public enum ReviewStepForm
    {
        officer1,
        officer2,
        officer3,
        success,
        error
    }
}
