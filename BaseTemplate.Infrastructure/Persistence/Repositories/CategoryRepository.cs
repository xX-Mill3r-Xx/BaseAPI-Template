using BaseTemplate.Domain.Entities;
using BaseTemplate.Domain.Repositories;

namespace BaseTemplate.Infrastructure.Persistence.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context) { }
    }
}
