using Microsoft.Extensions.Localization;

namespace Toolkit.Localizations.Infrastructure.Attributes;

/// <summary>
/// Used to instruct the <see cref="IStringLocalizer{T}"/> localizer to also search for a value in the localization of its generic type arguments.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class UseGenericStringLocalizationsAttribute : Attribute
{
    /// <summary>
    /// Gets or sets the number of additional nesting levels of generic type arguments that are searched.
    /// Zero searches the immediate type arguments only.
    /// </summary>
    public int Deep { get; set; } = 0;

    /// <summary>
    /// Initializes the attribute with the number of additional nesting levels to search.
    /// </summary>
    /// <param name="deep">The number of additional nesting levels; zero searches the immediate type arguments only.</param>
    public UseGenericStringLocalizationsAttribute(int deep)
    {
        Deep = deep;
    }

    /// <summary>
    /// Initializes the attribute with the default depth, which searches the immediate type arguments only.
    /// </summary>
    public UseGenericStringLocalizationsAttribute()
    {

    }
}
