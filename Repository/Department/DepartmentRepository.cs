using System.Text.Json;
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

        public void LoadDeparmentJsonToBd()
        {
            var jsonFilePath = "./resources/loadFiles/department.json";
            using var fileStream = new FileStream(jsonFilePath, FileMode.Open);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
                WriteIndented = false
            };

            var Department = JsonSerializer.Deserialize<List<Department>>(fileStream, options);

            // Agregar los países al DbSet
            _context.Departments.AddRange(Department);
            _context.SaveChanges();
        }
    }
}
