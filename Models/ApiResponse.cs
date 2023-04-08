using System.Net;

namespace rethus_backend.Models
{
  public class ApiResponse
  {
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
    public bool IsSuccess { get; set; } = true;
    public List<string> Messages { get; set; } = new List<string>();
    public object Result { get; set; } = "";

    public ApiResponse(ApiResponse response)
    {
      this.StatusCode = response.StatusCode;
      this.IsSuccess = response.IsSuccess;
      this.Messages = response.Messages;
      this.Result = response.Result;
    }

    public ApiResponse()
    {
    }
  }
}