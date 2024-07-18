using System.ComponentModel.DataAnnotations;
using rethus_backend.Utilities.Constants.User.UserFormConstants;
using rethus_backend.Utilities.Constants.UserConstants;

namespace rethus_backend.Models.Dto.UserForm
{
    public class UserFormCreateDto
    {
        [Required(ErrorMessage = "UseId is required")]
        public string? userId { get; set; }

        [Required(ErrorMessage = "PersonalTypeIdentification is required")]
        [EnumDataType(typeof(TypeInstitution), ErrorMessage = "Invalid typeIdentification Value")]
        public TypeIdentification PersonalTypeIdentification { get; set; }

        [Required(ErrorMessage = "PersonalGender is required")]
        [EnumDataType(typeof(TypeGender), ErrorMessage = "Invalid Gender Value")]
        public TypeGender PersonalGender { get; set; }

        [Required(ErrorMessage = "PersonalIdentification is required")]
        public string PersonalIdentification { get; set; }

        [Required(ErrorMessage = "PersonalFirstName is required")]
        public string PersonalFirstName { get; set; }

        [Required(ErrorMessage = "PersonalLastName is required")]
        public string PersonalLastName { get; set; }

        [Required(ErrorMessage = "PersonalCountryBirth is required")]
        public string PersonalCountryBirth { get; set; }

        [Required(ErrorMessage = "PersonalDepartmentBirth is required")]
        public string PersonalDepartmentBirth { get; set; }

        [Required(ErrorMessage = "PersonalMunicipalityBirth is required")]
        public string PersonalMunicipalityBirth { get; set; }

        [Required(ErrorMessage = "DateBirth is required")]
        public DateTime DateBirth { get; set; }

        [Required(ErrorMessage = "PersonalPlaceResidence is required")]
        public string PersonalPlaceResidence { get; set; }

        [Required(ErrorMessage = "PersonalDepartmentResidence is required")]
        public string PersonalDepartmentResidence { get; set; }

        [Required(ErrorMessage = "PersonalMunicipalityResidence is required")]
        public string PersonalMunicipalityResidence { get; set; }

        [Required(ErrorMessage = "PersonalAddress is required")]
        public string PersonalAddress { get; set; }

        [Required(ErrorMessage = "PersonalTelephone is required")]
        public int PersonalTelephone { get; set; }

        [Required(ErrorMessage = "PersonalPhone is required")]
        public int PersonalPhone { get; set; }

        [Required(ErrorMessage = "PersonalEmail is required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "PersonalEmail is not valid.")]
        public string PersonalEmail { get; set; }

        [Required(ErrorMessage = "PersonalEthnicGroup is required")]
        [EnumDataType(typeof(TypeEthnicGroup), ErrorMessage = "Invalid PersonalEthnicGroup Value")]
        public TypeEthnicGroup PersonalEthnicGroup { get; set; }

        [Required(ErrorMessage = "AcademicsOriginTitle is required")]
        public string AcademicsOriginTitle { get; set; }

        [Required(ErrorMessage = "AcademicsTypeInstitution is required")]
        [EnumDataType(typeof(TypeInstitution), ErrorMessage = "Invalid PersonalEthnicGroup Value")]
        public TypeInstitution AcademicsTypeInstitution { get; set; }

        [Required(ErrorMessage = "AcademicsProgramType is required")]
        public string AcademicsProgramType { get; set; }

        [Required(ErrorMessage = "AcademicsCountryInstitution is required")]
        public string AcademicsCountryInstitution { get; set; }

        [Required(ErrorMessage = "AcademicsDepartmentInstitution is required")]
        public string AcademicsDepartmentInstitution { get; set; }

        [Required(ErrorMessage = "AcademicsMunicipalityInstitution is required")]
        public string AcademicsMunicipalityInstitution { get; set; }

        [Required(ErrorMessage = "AcademicsNameInstitution is required")]
        public string AcademicsNameInstitution { get; set; }

        [Required(ErrorMessage = "AcademicsProgramName is required")]
        public string AcademicsProgramName { get; set; }

        [Required(ErrorMessage = "TypeProcedure is required")]
        public string? TypeProcedure { get; set; }

        [Required(ErrorMessage = "TypeProcedure is required")]
        public DateTime AcademicsGradeDate { get; set; }

        public string? AcademicsEquivalentTitle { get; set; }

        public string? AcademicsNumberConvalidation { get; set; }

        public DateTime? AcademicsDateConvalidation { get; set; }
    }

    // public class ResponseUserFormPaginate
    // {
    //   public int pages { get; set; }
    //   public IQueryable<UserForm> records { get; set; }
    //   public int total_records { get; set; }
    //   public int current_page { get; set; }
    // }


    public class PaginationResult<T>
    {
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public int CurrentPage { get; set; }
        public List<T> Records { get; set; }
    }

    public class DetailsProcessUserDto
    {
        public TypeIdentification PersonalTypeIdentification { get; set; }

        public string PersonalIdentification { get; set; }

        public string? PersonalFirstName { get; set; }
        public string? PersonalLastName { get; set; }

        public string Status { get; set; }
    }

    public class DetailsProccessPersonalDto
    {
        public string PersonalTypeIdentification { get; set; }
        public string? PersonalGender { get; set; }
        public string? PersonalIdentification { get; set; }
        public string? PersonalFirstName { get; set; }
        public string? PersonalLastName { get; set; }
        public string? PersonalCountryBirth { get; set; }
        public string? PersonalDepartmentBirth { get; set; }
        public string? PersonalMunicipalityBirth { get; set; }
        public DateTime? DateBirth { get; set; }
        public string? PersonalPlaceResidence { get; set; }
        public string? PersonalDepartmentResidence { get; set; }
        public string? PersonalMunicipalityResidence { get; set; }
        public string? PersonalAddress { get; set; }
        public int? PersonalTelephone { get; set; }
        public int? PersonalPhone { get; set; }
        public string? PersonalEmail { get; set; }
        public string? PersonalEthnicGroup { get; set; }
    }

    public class DetailsProccessAcademicDto
    {
        public string AcademicsOriginTitle { get; set; }
        public string AcademicsTypeInstitution { get; set; }
        public string AcademicsProgramType { get; set; }
        public string AcademicsDepartmentInstitution { get; set; }
        public string AcademicsMunicipalityInstitution { get; set; }
        public string AcademicsNameInstitution { get; set; }
        public string AcademicsProgramName { get; set; }
        public DateTime AcademicsDateInstitution { get; set; }
        public DateTime? AcademicsGradeDate { get; set; }
        public string? AcademicsNumberConvalidation { get; set; }
        public DateTime? AcademicsDateConvalidation { get; set; }
        public string AcademicsEquivalentTitle { get; set; }
        public string AcademicsNumberAdministrative { get; set; }
        public DateTime AcademicsDateAdministrative { get; set; }
    }

    public class DetailsProccessRegisterFile { }

    public class SelectInformationFileIventory
    {
        public UserFormStatus Status { get; set; }
        public ConfigurationTypeProcedure TypeProcedure { get; set; }
        public string StepForm { get; set; }
        public string PersonalIdentification { get; set; }
    }

    // public class ResponseUserFormPaginate
    // {
    //   public int pages { get; set; }
    //   public IQueryable<UserForm> records { get; set; }
    //   public int total_records { get; set; }
    //   public int current_page { get; set; }
    // }
}
