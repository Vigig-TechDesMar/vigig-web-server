using Vigig.DAL.Interfaces;
using Vigig.Domain.Entities;

namespace Vigig.DAL.Implementations;

public class ComplaintTypeRepository : GenericRepository<ComplaintType>,IComplaintTypeRepository
{
    public ComplaintTypeRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }
}