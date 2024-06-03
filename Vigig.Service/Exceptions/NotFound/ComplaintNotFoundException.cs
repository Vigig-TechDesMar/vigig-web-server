using Vigig.Domain.Entities;

namespace Vigig.Service.Exceptions.NotFound;

public class ComplaintNotFoundException : EntityNotFoundException<Complaint>
{
    public ComplaintNotFoundException(object id) : base(id)
    {
    }
}