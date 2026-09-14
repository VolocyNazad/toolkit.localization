using System;
using System.Linq;
using Toolkit.Localizations.Infrastructure.Attributes;
using Toolkit.Localizations.Infrastructure.Services;
using Xunit;

namespace Toolkit.Localization.Tests;

public sealed class LocalizationTypeDefinerTests
{
    [Fact]
    public void Inherited_definer_returns_direct_base_type_only_when_enabled()
    {
        var definer = new InheritedLocalizationTypeDefiner();

        Assert.Equal(new[] { typeof(BaseResource) }, definer.Define(typeof(InheritedResource)));
        Assert.Empty(definer.Define(typeof(PlainResource)));
    }

    [Fact]
    public void Pseudo_definer_orders_types_by_priority()
    {
        var definer = new PseudoLocalizationTypeDefiner();

        Type[] result = definer.Define(typeof(PseudoResource)).ToArray();

        Assert.Equal(new[] { typeof(FirstResource), typeof(SecondResource) }, result);
    }

    [Fact]
    public void Generic_definer_honors_requested_depth()
    {
        var definer = new GenericLocalizationTypeDefiner();

        Type[] result = definer.Define(typeof(DepthResource<Container<LeafResource>>)).ToArray();

        Assert.Equal(new[] { typeof(Container<LeafResource>), typeof(LeafResource) }, result);
    }

    [Fact]
    public void Generic_definer_uses_immediate_arguments_by_default()
    {
        var definer = new GenericLocalizationTypeDefiner();

        Type[] result = definer.Define(typeof(DefaultDepthResource<Container<LeafResource>>)).ToArray();

        Assert.Equal(new[] { typeof(Container<LeafResource>) }, result);
    }

    [Fact]
    public void All_generic_definer_walks_the_complete_generic_tree()
    {
        var definer = new AllGenericLocalizationTypeDefiner();

        Type[] result = definer.Define(typeof(AllDepthResource<Container<LeafResource>>)).ToArray();

        Assert.Equal(new[] { typeof(Container<LeafResource>), typeof(LeafResource) }, result);
    }

    [Fact]
    public void Definers_return_empty_results_without_their_attributes()
    {
        Assert.Empty(new PseudoLocalizationTypeDefiner().Define(typeof(PlainResource)));
        Assert.Empty(new GenericLocalizationTypeDefiner().Define(typeof(Container<LeafResource>)));
        Assert.Empty(new AllGenericLocalizationTypeDefiner().Define(typeof(Container<LeafResource>)));
    }

    private class BaseResource
    {
    }
    private sealed class PlainResource : BaseResource
    {
    }
    private sealed class FirstResource
    {
    }
    private sealed class SecondResource
    {
    }
    private sealed class LeafResource
    {
    }
    private sealed class Container<T>
    {
    }
    [UseInheritorStringLocalization]
    private sealed class InheritedResource : BaseResource
    {
    }
    [UsePseudoInheritorStringLocalization(typeof(SecondResource), Priority = 20)]
    [UsePseudoInheritorStringLocalization(typeof(FirstResource), Priority = 10)]
    private sealed class PseudoResource
    {
    }
    [UseGenericStringLocalizations(1)]
    private sealed class DepthResource<T>
    {
    }
    [UseGenericStringLocalizations]
    private sealed class DefaultDepthResource<T>
    {
    }
    [UseAllGenericStringLocalizations]
    private sealed class AllDepthResource<T>
    {
    }
}
