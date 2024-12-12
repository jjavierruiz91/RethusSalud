using rethus_backend.Models;
using rethus_backend.Utilities.Constants.User.UserFormConstants;

public class UserFormQueryParametersDto
{
    public string PersonalIdentification { get; set; }
    public string TypeProcedure { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class UserFormResponseDto
{
    public string UserFormId { get; set; }
    public string PersonalIdentification { get; set; }
    public string PersonalFirstName { get; set; }
    public string TypeProcedure { get; set; }
    public ReviewStepForm StepForm { get; set; }
    public string Status { get; set; }
    public string Consecutive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdateAt { get; set; }
}
