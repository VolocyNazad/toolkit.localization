using Microsoft.Extensions.Localization;

namespace Toolkit.Localizations.Infrastructure.Attributes;

/// <summary>
/// Used to instruct the <see cref="IStringLocalizer{T}"/> localizer to also search for a value in the localization of its generic type arguments.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class UseGenericStringLocalizationsAttribute : Attribute
{
    public int Deep { get; set; } = 0;

    public UseGenericStringLocalizationsAttribute(int deep)
    {
        Deep = deep;
    }

    public UseGenericStringLocalizationsAttribute()
    {

    }
}
