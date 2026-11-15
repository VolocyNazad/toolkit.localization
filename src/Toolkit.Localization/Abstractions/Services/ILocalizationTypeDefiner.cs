namespace Toolkit.Localizations.Abstractions.Services;

/// <summary>
/// Selects the types whose resources a string localizer searches when it localizes a given type.
/// </summary>
public interface ILocalizationTypeDefiner
{
    /// <summary>
    /// Returns the types to search for resources when <paramref name="type"/> is localized.
    /// </summary>
    /// <param name="type">The type passed to the localizer factory.</param>
    /// <returns>The types whose resources are searched, in search order.</returns>
    IEnumerable<Type> Define(Type type);
}
