using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace rethus_backend.Models
{
  public class UserForm
  {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserFormId { get; set; }

    public string? PersonalType_identification { get; set; }
    public string? PersonalGender { get; set; }
    public int? PersonalIdentification { get; set; }
    public string? PersonalFirstName { get; set; }
    public string? PersonalLastName { get; set; }

    public string PersonalFullName
    {
      get
      {
        return PersonalFirstName + " " + PersonalLastName;
      }
    }
    public string? PersonalCountryBirth { get; set; }
    public string? PersonalDepartmentBirth { get; set; }
    public string? PersonalMunicipalityBirth { get; set; }
    public DateTime DateBirth { get; set; }
    public string? PersonalPlaceResidence { get; set; }
    public string? PersonalDepartmentResidence { get; set; }
    public string? PersonalMunicipalityResidence { get; set; }
    public string? PersonalAddress { get; set; }
    public int? PersonalTelephone { get; set; }
    public int? PersonalPhone { get; set; }
    public string? PersonalEmail { get; set; }
    public string? PersonalEthnicGroup { get; set; }
    public string? AcademicsOriginTitle { get; set; }
    public string? AcademicsTypeInstitution { get; set; }
    public string? AcademicsProgramType { get; set; }
    public string? AcademicsDepartmentInstitution { get; set; }
    public string? AcademicsMunicipalityInstitution { get; set; }
    public string? AcademicsNameInstitution { get; set; }
    public string? AcademicsProgramName { get; set; }
    public string? AcademicsDateInstitution { get; set; }
    public string? AcademicsGradeDate { get; set; }
    public string? AcademicsNumberConvalidation { get; set; }
    public string? AcademicsDateConvalidation { get; set; }
    public string? AcademicsTitle { get; set; }
    public string? AcademicsNumberAdministrative { get; set; }
    public string? AcademicsDateAdministrative { get; set; }

    public string? UserId { get; set; }

    public User? User { get; set; }

    public ICollection<UserFormFiles>? UserFormFiles { get; set; }
  }
}