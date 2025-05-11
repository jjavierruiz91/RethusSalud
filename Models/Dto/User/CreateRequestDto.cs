using rethus_backend.Utilities.Constants.UserConstants;

namespace rethus_backend.Models.Dto.User
{
    public class CreateRequestDto
    {
        // public string? Identification { get; set; }
        public string email { get; set; }
        public string name { get; set; }
        public string password { get; set; }
        public string confirmPassword { get; set; }
        public string identification { get; set; }
    }

    public class CreateUserAdministrativeRequestDto
    {
        public string? email { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string password { get; set; }
        public string identification { get; set; }
    }

    public class UpdateUserAdministrativeRequestDto
    {
        public string? name { get; set; }
        public string? type { get; set; }
        public string? password { get; set; }
        public string? status { get; set; }
    }
}
