
using Microsoft.AspNetCore.Mvc;
using rethus_backend.Models;
using rethus_backend.Repository.IRepository;
using AutoMapper;

namespace rethus_backend.Controllers;

[ApiController]
public class ApiBaseController : ControllerBase
{
  [Inject] protected readonly IMapper _mapper = null!;
  [Inject] protected readonly IUnitOfWork _unitOfWork = null!;
  protected readonly ApiResponse _response;

  public ApiBaseController(IServiceProvider provider)
  {
    provider.Inject(this);
    _response = new ApiResponse();
  }
}