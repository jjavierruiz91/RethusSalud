using rethus_backend.Utilities.Constants.User.UserFormConstants;

namespace rethus_backend.Models.Dto.Pagination
{
    public class FilterQueryParametersDto
    {
        public string? UserFormId { get; set; }
        public UserFormStatus? Status { get; set; }
        public string? PersonalIdentification { get; set; }
        public ConfigurationTypeProcedure? TypeProcedure { get; set; }
        public string? startDate { get; set; }
        public string? endDate { get; set; }
    }
}
