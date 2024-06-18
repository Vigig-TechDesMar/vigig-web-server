namespace Vigig.Common.Settings;

public class GoogleSetting
{
    public string ClientId { get; init; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string RedirectUri { get; set; } = string.Empty;
}