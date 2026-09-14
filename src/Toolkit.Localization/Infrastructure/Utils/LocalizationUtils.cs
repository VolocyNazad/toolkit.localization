using Microsoft.Extensions.Localization;

namespace Toolkit.Localizations.Infrastructure.Utils;

public static class LocalizationUtils
{
    /// <summary> Checks that the localization resource is properly connected</summary>
    /// <param name="localizer"> The localization being checked </param>
    /// <returns> Returns true if the localization is connected, otherwise false</returns>
    public static bool Check(IStringLocalizer localizer)
    {
        IEnumerable<LocalizedString> localizedStrings = localizer.GetAllStrings();
        foreach (LocalizedString localizedString in localizedStrings)
        {
            if (localizedString.Name == localizedString.Value) return false;
        }

        return true;
    }

    /// <summary> Checks that the localization resource is properly connected</summary>
    /// <param name="localizer"> The localization being checked </param>
    /// <returns> Returns nothing if the localization is connected, otherwise throws an exception </returns>
    public static void ThrowCheck(IStringLocalizer localizer)
    {
        IEnumerable<LocalizedString> localizedStrings = localizer.GetAllStrings();
        foreach (LocalizedString localizedString in localizedStrings)
        {
            if (localizedString.Name == localizedString.Value) 
                throw new Exception("Not all localization data was connected");
        }
    }
}
