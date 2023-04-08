namespace rethus_backend.Models.Dto.User
{
  public class CreateRequestDto
  {
    // public string? Identification { get; set; }
    public string? email { get; set; }
    public string? name { get; set; }
    public string? password { get; set; }
  }
}