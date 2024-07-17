using System.Text.Json;
using Microsoft.EntityFrameworkCore;
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

        public Task<List<DepartmentResponseDto>> GetDepartments(int countryId)
        {
            var response = new ApiResponse();

            var department = _context.Departments
                .Where(c => c.CountryId == countryId)
                .Select(
                    v => new DepartmentResponseDto { DepartmentId = v.DepartmentId, Name = v.Name }
                )
                .ToListAsync();

            return department;
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

            _context.Departments.AddRange(Department);
            _context.SaveChanges();
        }
    }
}
