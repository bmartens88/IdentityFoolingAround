namespace IdentityFoolingAround.Settings;

public sealed class GoogleSettings
{
    public static string SectionName = "Google";

    public string ClientId { get; set; } = string.Empty;

    public string ClientSecret { get; set; } = string.Empty;
}