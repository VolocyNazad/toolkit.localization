using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Reflection;
using Toolkit.Localizations.Infrastructure.Attributes;

namespace Toolkit.Localizations.Infrastructure.Services;

/// <summary>
/// A localization factory that, unlike <see cref="ResourceManagerStringLocalizerFactory"/>, is able to search for localization resources across generic classes.
/// </summary>
/// <remarks>
/// The base out-of-the-box implementation of <see cref="IStringLocalizerFactory"/>, <see cref="GenericResourceManagerStringLocalizerFactory"/>,
/// has a significant drawback: it cannot find a resource when <see cref="IStringLocalizer{T}"/>'s generic type argument is itself a generic type.
/// This implementation addresses that drawback.
/// </remarks>
internal sealed class GenericResourceManagerStringLocalizerFactory(
    IOptions<LocalizationOptions> localizationOptions,
    ILoggerFactory loggerFactory) : IStringLocalizerFactory
{
    private readonly ResourceManagerStringLocalizerFactory _factory = new(localizationOptions, loggerFactory);
    private readonly ILogger<GenericResourceManagerStringLocalizerFactory> _logger = loggerFactory.CreateLogger<GenericResourceManagerStringLocalizerFactory>();

    public IStringLocalizer Create(Type resourceSource)
    {
        _logger.LogDebug("Creating a localizer for a type '{localization.type}'.", resourceSource);

        TypeInfo typeInfo = resourceSource.GetTypeInfo();
        string assemblyName = typeInfo.Assembly.GetName().Name;

        var attribute = resourceSource.GetCustomAttribute<LocalizationAliasAttribute>();
        IStringLocalizer localizer;
        if (attribute != null) {
            var typeName = attribute.Alias;
            _logger.LogTrace("An alias '{localization.alias}' was found for the type '{localization.type}'.", attribute.Alias, resourceSource);
            string baseName = $"{resourceSource.Namespace}.{typeName}"[assemblyName.Length..].Trim('.');
            localizer = _factory.Create(baseName, assemblyName);
        }
        else
        {
            if (typeInfo.IsGenericType) // Prior to version 6, the base library did not work with generic types
            {
                var typeName = resourceSource.Name;
                typeName = typeName[..resourceSource.Name.IndexOf('`')];
                string baseName = $"{resourceSource.Namespace}.{typeName}"[assemblyName.Length..].Trim('.');
                localizer = _factory.Create(baseName, assemblyName);
            }
            else
            {
                localizer = _factory.Create(resourceSource);
            }
        }

        _logger.LogTrace("localizer for type '{localization.type}' created.", resourceSource);

        return localizer;
    }

    public IStringLocalizer Create(string baseName, string location)
    {
        _logger.LogTrace("Creating a localizer for baseName '{localization.baseName}' and '{localization.location}'.", baseName, location);

        var localizer = _factory.Create(baseName, location);

        _logger.LogTrace("localizer for baseName '{localization.baseName}' and '{localization.location}' created.", baseName, location);

        return localizer;
    }
}
