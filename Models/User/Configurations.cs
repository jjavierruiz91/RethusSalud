using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rethus_backend.Models
{
    public class Configurations
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ConfigurationsId { get; set; }

        public required ConfigurationsState State { get; set; }

        public required ConfigurationStep Step { get; set; }
        public required ConfigurationTypeProcedure TypeProcedure { get; set; }
        public required bool TermCondition { get; set; }
        public string? UserFormId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }

    public enum ConfigurationsState
    {
        initial,
        inprogress,
        completed,
        rejected
    }

    public enum ConfigurationStep
    {
        select_procedure,
        acept_terms_conditions,
        load_user_form,
        load_user_files,
        success,
        error
    }

    public enum ConfigurationTypeProcedure
    {
        SSO,
        RETHUS,
        DEFAULT
    }
}
