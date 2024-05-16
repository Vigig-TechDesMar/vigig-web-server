using Vigig.DAL.Interfaces;
using Vigig.Domain.Entities;

namespace Vigig.DAL.Implementations;

public class ComplaintRepository : GenericRepository<Complaint>,IComplaintRepository
{
    public ComplaintRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }
}