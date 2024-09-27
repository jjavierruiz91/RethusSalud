using System.ComponentModel.DataAnnotations;
using rethus_backend.Utilities.Constants.UserConstants;

namespace rethus_backend.Models.Dto.UserForm
{
    public class UserFormUpdateDto
    {
        [EnumDataType(
            typeof(TypeIdentification),
            ErrorMessage = "Invalid typeIdentification Value"
        )]
        public TypeIdentification PersonalTypeIdentification { get; set; }

        [EnumDataType(typeof(TypeGender), ErrorMessage = "Invalid Gender Value")]
        public TypeGender? PersonalGender { get; set; }
        public string? PersonalIdentification { get; set; }
        public string? PersonalFirstName { get; set; }
        public string? PersonalLastName { get; set; }
        public int? PersonalCountryBirth { get; set; }
        public int? PersonalDepartmentBirth { get; set; }
        public int? PersonalMunicipalityBirth { get; set; }
        public DateTime? DateBirth { get; set; }
        public int? PersonalPlaceResidence { get; set; }
        public int? PersonalDepartmentResidence { get; set; }
        public int? PersonalMunicipalityResidence { get; set; }
        public string? PersonalAddress { get; set; }

        public string? PersonalTelephone { get; set; }

        public string? PersonalPhone { get; set; }
        public string? PersonalEmail { get; set; }

        [EnumDataType(typeof(TypeEthnicGroup), ErrorMessage = "Invalid PersonalEthnicGroup Value")]
        public TypeEthnicGroup? PersonalEthnicGroup { get; set; }
        public string? AcademicsOriginTitle { get; set; }

        [EnumDataType(typeof(TypeInstitution), ErrorMessage = "Invalid PersonalEthnicGroup Value")]
        public TypeInstitution? AcademicsTypeInstitution { get; set; }
        public string? AcademicsProgramType { get; set; }
        public int? AcademicsCountryInstitution { get; set; }
        public int? AcademicsDepartmentInstitution { get; set; }
        public int? AcademicsMunicipalityInstitution { get; set; }
        public string? AcademicsNameInstitution { get; set; }
        public string? AcademicsProgramName { get; set; }
        public DateTime? AcademicsGradeDate { get; set; }
        public string? AcademicsEquivalentTitle { get; set; }
        public string? AcademicsNumberConvalidation { get; set; }
        public DateTime? AcademicsDateConvalidation { get; set; }
    }
}
