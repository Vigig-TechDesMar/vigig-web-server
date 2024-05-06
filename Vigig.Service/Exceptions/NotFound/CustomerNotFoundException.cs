using Vigig.Domain.Models;

namespace Vigig.Service.Exceptions.NotFound;

public class CustomerNotFoundException : EntityNotFoundException<Customer>
{
    public CustomerNotFoundException(string id) : base(id)
    {
    }
}