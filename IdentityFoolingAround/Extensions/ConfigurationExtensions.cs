namespace IdentityFoolingAround.Extensions;

public static class ConfigurationExtensions
{
    public static TSettings GetSettings<TSettings>(this IConfiguration configuration, string sectionName)
        where TSettings : new()
    {
        TSettings settings = new();
        configuration.GetSection(sectionName).Bind(settings);
        return settings;
    }
}