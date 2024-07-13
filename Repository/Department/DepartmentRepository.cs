using rethus_backend.Data;
using rethus_backend.Models;
using rethus_backend.Repository.IRepository;

namespace rethus_backend.Repository
{
    public class DepartmentRepository : Repository<Department>, IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;

        public DepartmentRepository(ApplicationDbContext db)
            : base(db)
        {
            _context = db;
        }
    }
}
