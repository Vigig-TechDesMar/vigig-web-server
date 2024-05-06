namespace Vigig.Common.Constants.Messages;

public static class ExceptionMessage
{
    public const string MissingConnectionString = "Can not find specified connection string.";

    public const string MissingJwtSettingsConfiguration = "Can not specified the jwt settings.";
    public static class AuthenticationMessage
    { 
        public static string UserAlreadyExistMessage(string userEmail) =>  $"User {userEmail} is already exist.";
    }
    

}