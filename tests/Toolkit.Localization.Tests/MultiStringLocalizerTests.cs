using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Localization;
using Toolkit.Localizations.Infrastructure.Services;
using Xunit;

namespace Toolkit.Localization.Tests;

public sealed class MultiStringLocalizerTests
{
    [Fact]
    public void Indexer_uses_first_localizer_that_contains_the_resource()
    {
        var first = new StubLocalizer(new LocalizedString("Shared", "from class"));
        var second = new StubLocalizer(new LocalizedString("Shared", "from interface"));
        var localizer = new MultiStringLocalizer([first, second]);

        LocalizedString result = localizer["Shared"];

        Assert.Equal("from class", result.Value);
        Assert.Equal(0, second.LookupCount);
    }

    [Fact]
    public void Indexer_falls_back_when_an_earlier_localizer_misses()
    {
        var first = new StubLocalizer();
        var second = new StubLocalizer(new LocalizedString("InterfaceOnly", "fallback"));
        var localizer = new MultiStringLocalizer([first, second]);

        LocalizedString result = localizer["InterfaceOnly"];

        Assert.Equal("fallback", result.Value);
        Assert.False(result.ResourceNotFound);
    }

    [Fact]
    public void Formatted_indexer_passes_arguments_to_fallback_localizer()
    {
        var localizer = new MultiStringLocalizer([
            new StubLocalizer(),
            new StubLocalizer(new LocalizedString("Greeting", "Hello, {0}"))
        ]);

        LocalizedString result = localizer["Greeting", "Ivan"];

        Assert.Equal("Hello, Ivan", result.Value);
    }

    [Fact]
    public void GetAllStrings_keeps_the_first_value_for_duplicate_keys()
    {
        var localizer = new MultiStringLocalizer([
            new StubLocalizer(new LocalizedString("Shared", "first")),
            new StubLocalizer(
                new LocalizedString("Shared", "second"),
                new LocalizedString("Unique", "value"))
        ]);

        LocalizedString[] result = localizer.GetAllStrings(false).ToArray();

        Assert.Equal(2, result.Length);
        Assert.Equal("first", result.Single(value => value.Name == "Shared").Value);
        Assert.Equal("value", result.Single(value => value.Name == "Unique").Value);
    }

    [Fact]
    public void Empty_localizer_reports_missing_resources()
    {
        var localizer = new MultiStringLocalizer([]);

        LocalizedString plain = localizer["Missing"];
        LocalizedString formatted = localizer["Missing {0}", "value"];

        Assert.True(plain.ResourceNotFound);
        Assert.Equal("Missing", plain.Value);
        Assert.True(formatted.ResourceNotFound);
        Assert.Equal("Missing {0}", formatted.Value);
    }

    private sealed class StubLocalizer(params LocalizedString[] values) : IStringLocalizer
    {
        private readonly Dictionary<string, LocalizedString> _values = values.ToDictionary(value => value.Name);
        public int LookupCount { get; private set; }

        public LocalizedString this[string name]
        {
            get
            {
                LookupCount++;
                return _values.TryGetValue(name, out LocalizedString? value)
                    ? value
                    : new LocalizedString(name, name, resourceNotFound: true);
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                LocalizedString value = this[name];
                return value.ResourceNotFound
                    ? value
                    : new LocalizedString(name, string.Format(value.Value, arguments));
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures) => _values.Values;
    }
}
