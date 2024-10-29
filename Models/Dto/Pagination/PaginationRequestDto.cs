using rethus_backend.Utilities.Constants.User.CommentsConstants;
using rethus_backend.Utilities.Constants.User.UserFormConstants;

namespace rethus_backend.Models.Dto.Pagination
{

     public enum GenericStatus
    {
        pending,
        approved,
        rejected
    }
    public class FilterQueryParametersDto
    {
        public string? UserFormId { get; set; }
        public string? Status { get; set; }
        public string? PersonalIdentification { get; set; }
        public ConfigurationTypeProcedure? TypeProcedure { get; set; }
        public string? startDate { get; set; }
        public string? endDate { get; set; }
        public ReviewStepForm? Step { get; set; }
        public string? Type { get; set; }
    }
}
