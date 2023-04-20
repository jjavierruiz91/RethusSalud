using System.ComponentModel.DataAnnotations;
namespace rethus_backend.Models.Dto.UserForm
{
  public class UserFormCreateDto
  {
    [Required(ErrorMessage = "PersonalType_identification is required")]
    public string? PersonalType_identification { get; set; }

    [Required(ErrorMessage = "PersonalGender is required")]
    public string? PersonalGender { get; set; }

    [Required(ErrorMessage = "PersonalIdentification is required")]
    public int? PersonalIdentification { get; set; }

    [Required(ErrorMessage = "PersonalFirstName is required")]
    public string? PersonalFirstName { get; set; }

    [Required(ErrorMessage = "PersonalLastName is required")]
    public string? PersonalLastName { get; set; }

    [Required(ErrorMessage = "PersonalCountryBirth is required")]
    public string? PersonalCountryBirth { get; set; }

    [Required(ErrorMessage = "PersonalDepartmentBirth is required")]
    public string? PersonalDepartmentBirth { get; set; }

    [Required(ErrorMessage = "PersonalMunicipalityBirth is required")]
    public string? PersonalMunicipalityBirth { get; set; }

    [Required(ErrorMessage = "DateBirth is required")]
    public DateTime DateBirth { get; set; }

    [Required(ErrorMessage = "PersonalPlaceResidence is required")]
    public string? PersonalPlaceResidence { get; set; }

    [Required(ErrorMessage = "PersonalDepartmentResidence is required")]
    public string? PersonalDepartmentResidence { get; set; }

    [Required(ErrorMessage = "PersonalMunicipalityResidence is required")]
    public string? PersonalMunicipalityResidence { get; set; }

    [Required(ErrorMessage = "PersonalAddress is required")]
    public string? PersonalAddress { get; set; }

    [Required(ErrorMessage = "PersonalTelephone is required")]
    public int? PersonalTelephone { get; set; }

    [Required(ErrorMessage = "PersonalPhone is required")]
    public int? PersonalPhone { get; set; }

    [Required(ErrorMessage = "PersonalEmail is required")]
    [DataType(DataType.EmailAddress, ErrorMessage = "PersonalEmail is not valid.")]
    public string? PersonalEmail { get; set; }

    [Required(ErrorMessage = "PersonalEthnicGroup is required")]
    public string? PersonalEthnicGroup { get; set; }

    [Required(ErrorMessage = "AcademicsOriginTitle is required")]
    public string? AcademicsOriginTitle { get; set; }

    [Required(ErrorMessage = "AcademicsTypeInstitution is required")]
    public string? AcademicsTypeInstitution { get; set; }

    [Required(ErrorMessage = "AcademicsProgramType is required")]
    public string? AcademicsProgramType { get; set; }

    [Required(ErrorMessage = "AcademicsDepartmentInstitution is required")]
    public string? AcademicsDepartmentInstitution { get; set; }

    [Required(ErrorMessage = "AcademicsMunicipalityInstitution is required")]
    public string? AcademicsMunicipalityInstitution { get; set; }

    [Required(ErrorMessage = "AcademicsNameInstitution is required")]
    public string? AcademicsNameInstitution { get; set; }

    [Required(ErrorMessage = "AcademicsProgramName is required")]
    public string? AcademicsProgramName { get; set; }

    [Required(ErrorMessage = "AcademicsDateInstitution is required")]
    public string? AcademicsDateInstitution { get; set; }

    [Required(ErrorMessage = "AcademicsGradeDate is required")]
    public string? AcademicsGradeDate { get; set; }

    [Required(ErrorMessage = "AcademicsNumberConvalidation is required")]
    public string? AcademicsNumberConvalidation { get; set; }

    [Required(ErrorMessage = "AcademicsDateConvalidation is required")]
    public string? AcademicsDateConvalidation { get; set; }

    [Required(ErrorMessage = "AcademicsTitle is required")]
    public string? AcademicsTitle { get; set; }

    [Required(ErrorMessage = "AcademicsNumberAdministrative is required")]
    public string? AcademicsNumberAdministrative { get; set; }

    [Required(ErrorMessage = "AcademicsDateAdministrative is required")]
    public string? AcademicsDateAdministrative { get; set; }
  }
}