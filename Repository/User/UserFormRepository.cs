using rethus_backend.Data;
using rethus_backend.Models;
using rethus_backend.Models.Dto.UserForm;
using rethus_backend.Repository.IRepository;

namespace rethus_backend.Repository
{
  public class UserFormRepository : Repository<UserForm>, IUserFormRepository
  {
    private readonly ApplicationDbContext _context;

    public UserFormRepository(ApplicationDbContext db) : base(db)
    {
      _context = db;
    }

    public IEnumerable<UserForm> GetAll()
    {
      throw new NotImplementedException();
    }

    public UserForm GetById(string id)
    {
      throw new NotImplementedException();
    }

    public bool IsExistUser(string userFormId)
    {
      throw new NotImplementedException();
    }

    public bool IsUniqueUser(string userId)
    {
      UserForm user = _context.UserForm.FirstOrDefault(x => x.PersonalEmail == userId);
      if (user == null) return false;
      return true;
    }

    public Task<ApiResponse> post(UserFormCreateDto createRequestDto)
    {
      throw new NotImplementedException();
    }
  }
}