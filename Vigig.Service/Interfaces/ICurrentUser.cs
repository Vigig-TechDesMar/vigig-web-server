using Vigig.Service.Models.Common;

namespace Vigig.Service.Interfaces;

public interface ICurrentUser
{
    AuthModel User { get; set; }
}