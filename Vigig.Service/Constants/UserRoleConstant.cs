namespace Vigig.Service.Constants;

public static class UserRoleConstant
{
    public const string Admin = nameof(Admin);
    public const string Provider = nameof(Provider);
    public const string Client = nameof(Client);
    public const string Staff = nameof(Staff);
    public const string InternalUser = Admin + "," + Staff;
}