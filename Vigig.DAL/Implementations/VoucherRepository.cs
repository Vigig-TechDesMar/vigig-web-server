using Vigig.DAL.Interfaces;
using Vigig.Domain.Entities;

namespace Vigig.DAL.Implementations;

public class VoucherRepository : GenericRepository<Voucher>
{
    public VoucherRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }
}