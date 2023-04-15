using AutoMapper;
using rethus_backend.Models;
using rethus_backend.Models.Dto.User;
using rethus_backend.Models.Dto.UserForm;
using rethus_backend.Models.Dto.Auth;
using rethus_backend.Models.Dto.UserFormFiles;

namespace rethus_backend
{
  public class AutoMapperConfig : Profile
  {
    /// <summary>
    /// AutoMapper設定檔
    /// </summary>
    public AutoMapperConfig()
    {
      // Sample: CreateMap<InputClass, OutputClass>();

      CreateMap<User, CreateRequestDto>();
      CreateMap<User, UserRequestDto>().ReverseMap();
      CreateMap<User, UserRequestDto>().ReverseMap();
      CreateMap<User, AuthRequestDto>().ReverseMap();
      CreateMap<User, AuthResponseDto>().ReverseMap();

      CreateMap<UserForm, UserFormCreateDto>();

      CreateMap<UserFormFiles, UserFormCreateDto>().ReverseMap();
    }
  }
}