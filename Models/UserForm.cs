using System.ComponentModel.DataAnnotations.Schema;
namespace rethus_backend.Models
{
  public class UserForm
  {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserFormId { get; set; }

    public string PersonalType_identification { get; set; }
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
    public DateTime? DateBirth { get; set; }
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
    public DateTime? AcademicsDateInstitution { get; set; }
    public DateTime AcademicsGradeDate { get; set; }
    public string? AcademicsNumberConvalidation { get; set; }
    public DateTime? AcademicsDateConvalidation { get; set; }
    public string? AcademicsTitle { get; set; }
    public string? AcademicsNumberAdministrative { get; set; }
    public DateTime? AcademicsDateAdministrative { get; set; }

    public string? UserId { get; set; }

    public User? User { get; set; }

    public ICollection<UserFormFiles>? UserFormFiles { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    //   public UserForm(string personalType_identification, string personalGender, int personalIdentification, string personalFirstName, string personalLastName, string personalCountryBirth, string personalDepartmentBirth, string personalMunicipalityBirth, DateTime dateBirth, string personalPlaceResidence, string personalDepartmentResidence, string personalMunicipalityResidence, string personalAddress, int personalTelephone, int personalPhone, string personalEmail, string personalEthnicGroup, string academicsOriginTitle, string academicsTypeInstitution, string academicsProgramType, string academicsDepartmentInstitution, string academicsMunicipalityInstitution, string academicsNameInstitution, string academicsProgramName, string academicsDateInstitution, string academicsGradeDate, string academicsNumberConvalidation, DateTime academicsDateConvalidation, string academicsTitle, string academicsNumberAdministrative, DateTime academicsDateAdministrative)
    //   {
    //     PersonalType_identification = personalType_identification;
    //     PersonalGender = personalGender;
    //     PersonalIdentification = personalIdentification;
    //     PersonalFirstName = personalFirstName;
    //     PersonalLastName = personalLastName;
    //     PersonalCountryBirth = personalCountryBirth;
    //     PersonalDepartmentBirth = personalDepartmentBirth;
    //     PersonalMunicipalityBirth = personalMunicipalityBirth;
    //     DateBirth = dateBirth;
    //     PersonalPlaceResidence = personalPlaceResidence;
    //     PersonalDepartmentResidence = personalDepartmentResidence;
    //     PersonalMunicipalityResidence = personalMunicipalityResidence;
    //     PersonalAddress = personalAddress;
    //     PersonalTelephone = personalTelephone;
    //     PersonalPhone = personalPhone;
    //     PersonalEmail = personalEmail;
    //     PersonalEthnicGroup = personalEthnicGroup;
    //     AcademicsOriginTitle = academicsOriginTitle;
    //     AcademicsTypeInstitution = academicsTypeInstitution;
    //     AcademicsProgramType = academicsProgramType;
    //     AcademicsDepartmentInstitution = academicsDepartmentInstitution;
    //     AcademicsMunicipalityInstitution = academicsMunicipalityInstitution;
    //     AcademicsNameInstitution = academicsNameInstitution;
    //     AcademicsProgramName = academicsProgramName;
    //     AcademicsDateInstitution = academicsDateInstitution;
    //     AcademicsGradeDate = academicsGradeDate;
    //     AcademicsNumberConvalidation = academicsNumberConvalidation;
    //     AcademicsDateConvalidation = academicsDateConvalidation;
    //     AcademicsTitle = academicsTitle;
    //     AcademicsNumberAdministrative = academicsNumberAdministrative;
    //     AcademicsDateAdministrative = academicsDateAdministrative;
    //   }
  }
}