using System;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Toolkit.Localizations.Abstractions.Services;
using Toolkit.Localizations.Infrastructure.Attributes;
using Toolkit.Localizations.Infrastructure.Services;
using Xunit;

namespace Toolkit.Localization.Tests;

public sealed class LocalizerFactoryTests
{
    [Fact]
    public void Inherit_factory_reuses_cached_localizer_for_a_type()
    {
        var factory = CreateInheritFactory([]);

        IStringLocalizer first = factory.Create(typeof(PlainResource));
        IStringLocalizer second = factory.Create(typeof(PlainResource));

        Assert.Same(first, second);
    }

    [Fact]
    public void Inherit_factory_builds_multi_localizer_when_a_definer_adds_a_type()
    {
        var factory = CreateInheritFactory([new FixedDefiner(typeof(FallbackResource))]);

        IStringLocalizer result = factory.Create(typeof(PlainResource));

        Assert.IsType<MultiStringLocalizer>(result);
    }

    [Fact]
    public void Inherit_factory_ignores_cycles_in_dependent_types()
    {
        var factory = CreateInheritFactory([new FixedDefiner(typeof(PlainResource))]);

        IStringLocalizer result = factory.Create(typeof(PlainResource));

        Assert.IsNotType<MultiStringLocalizer>(result);
    }

    [Fact]
    public void Inherit_factory_rejects_null_resource_type()
    {
        var factory = CreateInheritFactory([]);

        Assert.Throws<ArgumentNullException>(() => factory.Create((Type)null!));
    }

    [Fact]
    public void Generic_factory_creates_localizers_for_plain_generic_and_aliased_types()
    {
        var factory = new GenericResourceManagerStringLocalizerFactory(
            Options.Create(new LocalizationOptions()),
            NullLoggerFactory.Instance);

        IStringLocalizer plain = factory.Create(typeof(PlainResource));
        IStringLocalizer generic = factory.Create(typeof(GenericResource<PlainResource>));
        IStringLocalizer aliased = factory.Create(typeof(AliasedResource));
        IStringLocalizer named = factory.Create("Missing", typeof(PlainResource).Assembly.GetName().Name!);

        Assert.True(plain["Missing"].ResourceNotFound);
        Assert.True(generic["Missing"].ResourceNotFound);
        Assert.True(aliased["Missing"].ResourceNotFound);
        Assert.True(named["Missing"].ResourceNotFound);
    }

    private static InheritStringLocalizerFactory CreateInheritFactory(
        ILocalizationTypeDefiner[] definers)
    {
        return new InheritStringLocalizerFactory(
            definers,
            Options.Create(new LocalizationOptions()),
            NullLoggerFactory.Instance);
    }

    private sealed class FixedDefiner(params Type[] types) : ILocalizationTypeDefiner
    {
        public IEnumerable<Type> Define(Type type) => types;
    }

    private sealed class PlainResource
    {
    }

    private sealed class FallbackResource
    {
    }

    private sealed class GenericResource<T>
    {
    }

    [LocalizationAlias("Alias")]
    private sealed class AliasedResource
    {
    }
}
