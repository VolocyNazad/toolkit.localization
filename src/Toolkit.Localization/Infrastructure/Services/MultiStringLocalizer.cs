using Microsoft.Extensions.Localization;

namespace Toolkit.Localizations.Infrastructure.Services;

/// <summary>
/// An <see cref="IStringLocalizer"/> that supports delegating to multiple <see cref="IStringLocalizer{T}"/> instances.
/// </summary>
internal sealed class MultiStringLocalizer(List<IStringLocalizer> localizers) : IStringLocalizer
{
    private readonly List<IStringLocalizer> _localizers = localizers;

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        var result = new Dictionary<string, LocalizedString>();
        foreach (var localizer in _localizers)
        {
            foreach (var entry in localizer.GetAllStrings(includeParentCultures))
            {
                if (!result.ContainsKey(entry.Name))
                {
                    result.Add(entry.Name, entry);
                }
            }
        }

        return result.Values;
    }

    public LocalizedString this[string name]
    {
        get
        {
            var result = new LocalizedString(name, name, resourceNotFound: true);
            foreach (var localizer in _localizers)
            {
                result = localizer[name];
                if (!result.ResourceNotFound)
                {
                    return result;
                }
            }
            return result;
        }
    }
    public LocalizedString this[string name, params object[] arguments]
    {
        get
        {
            var result = new LocalizedString(name, name, resourceNotFound: true);
            foreach (var localizer in _localizers)
            {
                result = localizer[name, arguments];
                if (!result.ResourceNotFound)
                {
                    return result;
                }
            }
            return result;
        }
    }

    public override string ToString() 
        => _localizers?
            .Select(i => i.ToString())
            .Aggregate((result, item) => result + Environment.NewLine + item) ?? string.Empty;
}
