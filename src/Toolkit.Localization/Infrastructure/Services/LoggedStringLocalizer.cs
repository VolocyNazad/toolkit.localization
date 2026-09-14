using System.Globalization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Toolkit.Localizations.Infrastructure.Services;

/// <inheritdoc />
internal sealed class LoggedStringLocalizer<TResourceSource>(
    IStringLocalizerFactory factory,
    ILogger<LoggedStringLocalizer<TResourceSource>> logger) : StringLocalizer<TResourceSource>(factory)
{
    private readonly ILogger<LoggedStringLocalizer<TResourceSource>> _logger = logger;

    /// <inheritdoc />
    public override LocalizedString this[string name]
    {
        get
        {
            var result = base[name];
            Log(result);
            return result;
        }
    }

    /// <inheritdoc />
    public override LocalizedString this[string name, params object[] arguments]
    {
        get
        {
            var result = base[name, arguments];
            Log(result);
            return result;
        }
    }

    private void Log(LocalizedString localizedString)
    {
        if (!localizedString.ResourceNotFound)
        {
            _logger.LogTrace(
                """
                The localizer managed to get the value from the key '{localization.name}' 
                in the resource '{localization.location}' for the culture '{localization.culture}'.
                """, localizedString.Name, localizedString.SearchedLocation, CultureInfo.CurrentUICulture);
        }
        else
        {
            _logger.LogWarning(
                """
                Localizer failed to retrieve value for key '{localization.name}' 
                in resource '{localization.location}' for culture '{localization.culture}'.
                """, localizedString.Name, localizedString.SearchedLocation, CultureInfo.CurrentUICulture);
        }
    }
}
