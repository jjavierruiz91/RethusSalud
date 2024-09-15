using System.ComponentModel.DataAnnotations.Schema;
using rethus_backend.Utilities.Constants.User.UserFormConstants;
using rethus_backend.Utilities.Constants.UserConstants;

namespace rethus_backend.Models
{
    public class UserForm
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string UserFormId { get; set; }

        public required TypeIdentification PersonalTypeIdentification { get; set; }
        public required TypeGender PersonalGender { get; set; }
        public required string PersonalIdentification { get; set; }
        public required string PersonalFirstName { get; set; }
        public required string PersonalLastName { get; set; }
        public required int PersonalCountryBirthId { get; set; }
        public Country CountryOfBirth { get; set; }
        public required int PersonalDepartmentBirthId { get; set; }
        public Department DepartmentBirth { get; set; }
        public required int PersonalMunicipalityBirthId { get; set; }
        public City MunicipalityBirth { get; set; }
        public required int PersonalPlaceResidenceId { get; set; }
        public Country PlaceResidence { get; set; }
        public required int PersonalDepartmentResidenceId { get; set; }
        public Department DepartmentResidence { get; set; }
        public required int PersonalMunicipalityResidenceId { get; set; }
        public City MunicipalityResidence { get; set; }
        public required string PersonalAddress { get; set; }
        public required DateTime DateBirth { get; set; }
        public string? PersonalTelephone { get; set; }
        public string? PersonalPhone { get; set; }
        public string? PersonalEmail { get; set; }
        public required TypeEthnicGroup PersonalEthnicGroup { get; set; }
        public required string AcademicsOriginTitle { get; set; }
        public required TypeInstitution AcademicsTypeInstitution { get; set; }
        public required string AcademicsProgramType { get; set; }
        public required int AcademicsCountryInstitution { get; set; }
        public Country CountryInstitution { get; set; }

        public int AcademicsDepartmentInstitutionId { get; set; }
        public Department DepartmentInstitution { get; set; }
        public int AcademicsMunicipalityInstitutionId { get; set; }
        public City MunicipalityInstitution { get; set; }
        public required string AcademicsNameInstitution { get; set; }
        public required string AcademicsProgramName { get; set; }
        public required DateTime AcademicsGradeDate { get; set; }
        public required ConfigurationTypeProcedure TypeProcedure { get; set; }
        public string? AcademicsNumberConvalidation { get; set; }
        public DateTime? AcademicsDateConvalidation { get; set; }
        public string? AcademicsEquivalentTitle { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public ICollection<UserFormFiles>? UserFormFiles { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public UserFormStatus Status { get; set; }

        public ReviewStepForm StepForm { get; set; }

        public string? Consecutive { get; set; }
    }
}
