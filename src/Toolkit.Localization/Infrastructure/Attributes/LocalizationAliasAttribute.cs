using Microsoft.Extensions.Localization;

namespace Toolkit.Localizations.Infrastructure.Attributes;

/// <summary>
/// Used to specify an alias for the <see cref="IStringLocalizer{T}"/> localizer.
/// The search is performed across the entire generic type hierarchy.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class LocalizationAliasAttribute(string alias) : Attribute
{
    public string Alias { get; set; } = alias;
}
