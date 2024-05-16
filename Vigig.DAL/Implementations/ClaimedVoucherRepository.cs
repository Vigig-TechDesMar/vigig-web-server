using Vigig.DAL.Interfaces;
using Vigig.Domain.Entities;

namespace Vigig.DAL.Implementations;

public class ClaimedVoucherRepository : GenericRepository<ClaimedVoucher>, IClaimedVoucherRepository
{
    public ClaimedVoucherRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }
}