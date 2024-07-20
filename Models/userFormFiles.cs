using System.ComponentModel.DataAnnotations.Schema;
using rethus_backend.Utilities.Constants.User.UserFormConstants;

namespace rethus_backend.Models
{
    public class UserFormFiles
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? UserFormFilesId { get; set; }
        public long Size { get; set; }
        public string Filename { get; set; }
        public string Type { get; set; }
        public TypeUploadFile TypeUploadFile { get; set; }
        public string Url { get; set; }
        public string UserFormId { get; set; }
        public UserForm UserForm { get; set; }
    }
}
