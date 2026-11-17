using Microsoft.Extensions.Localization;

namespace Toolkit.Localizations.Infrastructure.Attributes;

/// <summary>
/// Used to specify an additional type for the <see cref="IStringLocalizer{T}"/> localizer to search for a resource in.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class UsePseudoInheritorStringLocalizationAttribute(Type inheritFrom) : Attribute
{
    /// <summary>
    /// Gets the additional type whose resources the localizer searches.
    /// </summary>
    public Type InheritFrom { get; } = inheritFrom ?? throw new ArgumentNullException(nameof(inheritFrom));

    /// <summary>
    /// Gets or sets the search order of the additional type; a lower value is searched first.
    /// </summary>
    public int Priority { get; set; }
}
