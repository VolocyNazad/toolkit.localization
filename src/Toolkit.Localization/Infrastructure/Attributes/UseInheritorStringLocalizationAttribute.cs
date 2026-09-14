using Microsoft.Extensions.Localization;

namespace Toolkit.Localizations.Infrastructure.Attributes;

/// <summary>
/// Used to instruct the <see cref="IStringLocalizer{T}"/> localizer to also search for a value in the localization of its inheritors.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class UseInheritorStringLocalizationAttribute : Attribute
{
}
