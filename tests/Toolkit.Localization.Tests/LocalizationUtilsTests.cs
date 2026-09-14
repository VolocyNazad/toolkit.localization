using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using Toolkit.Localizations.Infrastructure.Utils;
using Xunit;

namespace Toolkit.Localization.Tests;

public sealed class LocalizationUtilsTests
{
    [Fact]
    public void Check_returns_true_when_every_resource_is_localized()
    {
        var localizer = new StubLocalizer(new LocalizedString("Greeting", "Hello"));

        Assert.True(LocalizationUtils.Check(localizer));
    }

    [Fact]
    public void Check_returns_false_when_resource_value_matches_its_name()
    {
        var localizer = new StubLocalizer(new LocalizedString("Greeting", "Greeting"));

        Assert.False(LocalizationUtils.Check(localizer));
    }

    [Fact]
    public void ThrowCheck_throws_when_resource_is_not_localized()
    {
        var localizer = new StubLocalizer(new LocalizedString("Greeting", "Greeting"));

        Assert.Throws<Exception>(() => LocalizationUtils.ThrowCheck(localizer));
    }

    private sealed class StubLocalizer(params LocalizedString[] strings) : IStringLocalizer
    {
        public LocalizedString this[string name] => strings.Single(value => value.Name == name);
        public LocalizedString this[string name, params object[] arguments] => this[name];

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures) => strings;
    }
}
