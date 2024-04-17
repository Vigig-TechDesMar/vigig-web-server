using Vigig.Common.Interfaces;

namespace Vigig.DAL.Interfaces;

public interface IUnitOfWork: IAutoRegisterable
{
    void Commit();
    Task CommitAsync();
    void Rollback();
    Task RollbackAsync();
}