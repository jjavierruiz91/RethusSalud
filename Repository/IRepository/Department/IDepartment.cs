using rethus_backend.Models;

namespace rethus_backend.Repository.IRepository
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        void LoadDeparmentJsonToBd();
    }
}
