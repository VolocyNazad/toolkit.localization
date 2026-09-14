using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Toolkit.Localizations.Abstractions.Services;
using Toolkit.Localizations.Infrastructure.Attributes;
using Toolkit.Localizations.Infrastructure.Extensions;
using Toolkit.Localizations.Infrastructure.Services;
using Xunit;

namespace Toolkit.Localization.Tests;

public sealed class ConfigurationAndAttributeTests
{
    [Fact]
    public void AddInheritLocalization_registers_factory_localizer_and_all_definers()
    {
        var services = new ServiceCollection();

        IServiceCollection result = services.AddInheritLocalization();

        Assert.Same(services, result);
        Assert.Contains(services, item => item.ServiceType == typeof(IStringLocalizerFactory));
        Assert.Contains(services, item => item.ServiceType == typeof(IStringLocalizer<>));
        Assert.Equal(5, services.Count(item => item.ServiceType == typeof(ILocalizationTypeDefiner)));
    }

    [Fact]
    public void Default_options_configuration_uses_localizations_path()
    {
        var configuration = new DefaultLocalizationOptionsResourcePathConfigureOptions("Localizations");
        var options = new LocalizationOptions();

        configuration.Configure(options);

        Assert.Equal("Localizations", options.ResourcesPath);
    }

    [Fact]
    public void AddInheritLocalization_rejects_null_collection()
    {
        IServiceCollection services = null!;

        Assert.Throws<ArgumentNullException>(() => services.AddInheritLocalization());
    }

    [Fact]
    public void Attributes_preserve_their_configuration()
    {
        var alias = new LocalizationAliasAttribute("Alias");
        var generic = new UseGenericStringLocalizationsAttribute(2);
        var pseudo = new UsePseudoInheritorStringLocalizationAttribute(typeof(Resource)) { Priority = 7 };

        Assert.Equal("Alias", alias.Alias);
        Assert.Equal(2, generic.Deep);
        Assert.Equal(typeof(Resource), pseudo.InheritFrom);
        Assert.Equal(7, pseudo.Priority);
    }

    [Fact]
    public void Pseudo_attribute_rejects_null_type()
    {
        Assert.Throws<ArgumentNullException>(() => new UsePseudoInheritorStringLocalizationAttribute(null!));
    }

    private sealed class Resource
    {
    }
}
