namespace Vigig.Common.Constants.Messages;

public static class ExceptionMessage
{
    public const string MissingConnectionString = "Can not find specified connection string.";

    public const string MissingJwtSettingsConfiguration = "Can not find specified the jwt settings.";

    public const string MissingCorsSettingsConfiguration = "Can not find specified cors configuration.";

    public const string MissingCustomSwaggerConfiguration = "Can not specified swagger configuration.";
    
    
    public static class AuthenticationMessage
    { 
        // public static string UserAlreadyExistMessage(string userEmail) =>  $"User {userEmail} is already exist.";
        public static string UserAlreadyExistMessage(string userEmail) =>  $"Email {userEmail} đã tồn tại.";
        // public static string InvalidTokenMessage => "The token is invalid.";
        public static string InvalidTokenMessage => "Token không hợp lệ.";
    }

    public static class BuildingMessage
    {
        public static string BuildingAlreadyExistMessage(string buildingName) => $"Building {buildingName} is already exist.";
    }


}