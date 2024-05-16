using Vigig.Common.Attribute;
using Vigig.Common.Interfaces;

namespace Vigig.DAL.Interfaces;
[ServiceRegister]
public interface IUnitOfWork
{
    void Commit();
    Task CommitAsync();
    void Rollback();
    Task RollbackAsync();
}