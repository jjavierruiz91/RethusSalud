using rethus_backend.Utilities.Constants.User.UserFormConstants;
using rethus_backend.Utilities.Constants.UserConstants;

public class UserFormProcessDto
{
    public TypeIdentification identificationType { get; set; }
    public string identification { get; set; }
}

public class UserReviewRolDto
{
    public ReviewStepForm userRol { get; set; }
}
