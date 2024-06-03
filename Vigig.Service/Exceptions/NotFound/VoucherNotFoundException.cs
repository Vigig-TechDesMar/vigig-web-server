using Vigig.Domain.Entities;

namespace Vigig.Service.Exceptions.NotFound;

public class VoucherNotFoundException : EntityNotFoundException<Voucher>
{
    public VoucherNotFoundException(object id) : base(id)
    {
    }
}