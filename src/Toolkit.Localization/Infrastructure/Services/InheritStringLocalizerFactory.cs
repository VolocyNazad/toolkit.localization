using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using Toolkit.Localizations.Abstractions.Services;

namespace Toolkit.Localizations.Infrastructure.Services;

/// <summary>
/// A localization factory that, unlike <see cref="ResourceManagerStringLocalizerFactory"/>, is able to search for localization
/// resources not only by the <see cref="{T}"/> type of <see cref="IStringLocalizer{T}"/>, but also by: <br /> <br />
///     - searching the interfaces the class uses, <br />
///     - searching the class's parent classes, <br />
///     - searching the class's generic type arguments, <br />
///     - and even using types entirely unrelated to the class <br />
/// </summary>
/// <remarks>
/// Supports multi-level checking of classes for the presence of attributes.
/// Takes advantage of <see cref="GenericResourceManagerStringLocalizerFactory"/>.
/// </remarks>
internal sealed class InheritStringLocalizerFactory(
    IEnumerable<ILocalizationTypeDefiner> localizationTypeDefiners,
    IOptions<LocalizationOptions> localizationOptions,
    ILoggerFactory loggerFactory) : IStringLocalizerFactory
{
    private readonly ConcurrentDictionary<Type, IStringLocalizer> _cache = new();
    private readonly GenericResourceManagerStringLocalizerFactory _factory = new(localizationOptions, loggerFactory);
    private readonly IEnumerable<ILocalizationTypeDefiner> _localizationTypeDefiners = localizationTypeDefiners;

    public IStringLocalizer Create(string baseName, string location) => _factory.Create(baseName, location);
    public IStringLocalizer Create(Type resourceSource)
    {
        if (resourceSource == null)
            throw new ArgumentNullException(nameof(resourceSource));

        return CreateStringLocalizer(resourceSource);
    }
    private IStringLocalizer CreateStringLocalizer(Type type) => _cache.GetOrAdd(type, CreateStringLocalizerDirect);
    private IStringLocalizer CreateStringLocalizerDirect(Type type)
    {
        var localizers = new List<IStringLocalizer>();
        var visitedTypes = new HashSet<Type>();

        AddLocalizers(type, visitedTypes, localizers);

        return localizers.Count > 1
            ? new MultiStringLocalizer(localizers)
            : localizers[0];
    }

    private void AddLocalizers(
        Type type,
        HashSet<Type> visitedTypes,
        List<IStringLocalizer> localizers)
    {
        if (!visitedTypes.Add(type)) return;

        localizers.Add(_factory.Create(type));
        foreach (ILocalizationTypeDefiner definer in _localizationTypeDefiners)
        {
            foreach (Type dependentType in definer.Define(type))
            {
                AddLocalizers(dependentType, visitedTypes, localizers);
            }
        }
    }
}
