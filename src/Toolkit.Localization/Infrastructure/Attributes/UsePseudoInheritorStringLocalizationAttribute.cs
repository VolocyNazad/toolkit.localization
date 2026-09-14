using Microsoft.Extensions.Localization;

namespace Toolkit.Localizations.Infrastructure.Attributes;

/// <summary>
/// Used to specify an additional type for the <see cref="IStringLocalizer{T}"/> localizer to search for a resource in.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class UsePseudoInheritorStringLocalizationAttribute(Type inheritFrom) : Attribute
{
    public Type InheritFrom { get; } = inheritFrom ?? throw new ArgumentNullException(nameof(inheritFrom));
    public int Priority { get; set; }
}
