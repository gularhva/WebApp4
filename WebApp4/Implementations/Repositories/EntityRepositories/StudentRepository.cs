using WebApp4.Abstractions.IRepositories.IEntityRepositories;
using WebApp4.Contexts;
using WebApp4.Entities;

namespace WebApp4.Implementations.Repositories.EntityRepositories;

public class StudentRepository : GenericRepository<Student>, IStudentRepository
{
    public StudentRepository(AppDbContext context) : base(context)
    {
    }
}
