using System;
using System.Collections.Generic;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging.Abstractions;
using Toolkit.Localizations.Infrastructure.Services;
using Xunit;

namespace Toolkit.Localization.Tests;

public sealed class LoggedStringLocalizerTests
{
    [Fact]
    public void Formatted_indexer_preserves_key_and_passes_arguments_to_inner_localizer()
    {
        var innerLocalizer = new RecordingLocalizer();
        var factory = new StubFactory(innerLocalizer);
        var localizer = new LoggedStringLocalizer<Resource>(
            factory,
            NullLogger<LoggedStringLocalizer<Resource>>.Instance);

        LocalizedString result = localizer["Hello {0}", "Ivan"];

        Assert.Equal("Hello {0}", innerLocalizer.RequestedName);
        Assert.Equal(new object[] { "Ivan" }, innerLocalizer.RequestedArguments);
        Assert.Equal("Привет, Ivan", result.Value);
    }

    private sealed class Resource
    {
    }
    private sealed class StubFactory(IStringLocalizer localizer) : IStringLocalizerFactory
    {
        public IStringLocalizer Create(Type resourceSource) => localizer;
        public IStringLocalizer Create(string baseName, string location) => localizer;
    }

    private sealed class RecordingLocalizer : IStringLocalizer
    {
        public string? RequestedName { get; private set; }

        public object[]? RequestedArguments { get; private set; }

        public LocalizedString this[string name] => new(name, name, resourceNotFound: true);

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                RequestedName = name;
                RequestedArguments = arguments;
                return new LocalizedString(name, $"Привет, {arguments[0]}");
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures) => [];
    }
}
