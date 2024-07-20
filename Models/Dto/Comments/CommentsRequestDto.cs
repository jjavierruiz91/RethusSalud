using rethus_backend.Utilities.Constants.User.UserFormConstants;

namespace rethus_backend.Models.Dto.Comments
{
    public class CommentsCreateDto
    {
        public required string Description { get; set; }
        public required string UserFuncionarioId { get; set; }
        public required string UserFormId { get; set; }
    }

    public class CommentsQueryParametersDto
    {
        public string UserFormId { get; set; }
        public string Status { get; set; }
    }

    public class CommonQueryParametersDto
    {
        public string? UserFormId { get; set; }
        public UserFormStatus? Status { get; set; }
        public string? PersonalIdentification { get; set; }
        public ConfigurationTypeProcedure? TypeProcedure { get; set; }
        public DateTime? CreatedAt { get; set; }

        public string LogicalOperator { get; set; } = "AND";

        public ReviewStepForm? StepForm { get; set; }

        // Nueva propiedad para especificar el operador de comparación para cada propiedad
        public Dictionary<string, string> ComparisonOperators { get; set; } =
            new Dictionary<string, string>();
    }

    public class CommentsResponseDto
    {
        public string CommentId { get; set; }
        public string Description { get; set; }
        public string UserFormId { get; set; }
        public string status { get; set; }
        public string type { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
