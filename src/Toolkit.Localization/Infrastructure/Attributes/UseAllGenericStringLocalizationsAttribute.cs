using Microsoft.Extensions.Localization;

namespace Toolkit.Localizations.Infrastructure.Attributes;

/// <summary>
/// Used to instruct the <see cref="IStringLocalizer{T}"/> localizer to also search for a value in the localization of its generic type arguments.
/// The search is performed across the entire generic type hierarchy.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class UseAllGenericStringLocalizationsAttribute : Attribute
{
}
