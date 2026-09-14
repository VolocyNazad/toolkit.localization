using System;
using System.Linq;
using Toolkit.Localizations.Infrastructure.Attributes;
using Toolkit.Localizations.Infrastructure.Services;
using Xunit;

namespace Toolkit.Localization.Tests;

public sealed class InterfaceLocalizationTypeDefinerTests
{
    [Fact]
    public void Define_returns_implemented_interfaces_for_decorated_class()
    {
        var definer = new InterfaceLocalizationTypeDefiner();

        Type[] result = definer.Define(typeof(DecoratedResource)).ToArray();

        Assert.Equal(2, result.Length);
        Assert.Contains(typeof(IFirstResource), result);
        Assert.Contains(typeof(ISecondResource), result);
    }

    [Fact]
    public void Define_returns_no_interfaces_for_undecorated_class()
    {
        var definer = new InterfaceLocalizationTypeDefiner();

        Type[] result = definer.Define(typeof(UndecoratedResource)).ToArray();

        Assert.Empty(result);
    }

    private interface IFirstResource
    {
    }

    private interface ISecondResource
    {
    }

    [UseInterfacesStringLocalizations]
    private sealed class DecoratedResource : IFirstResource, ISecondResource
    {
    }

    private sealed class UndecoratedResource : IFirstResource
    {
    }
}
