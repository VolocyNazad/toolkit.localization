using System.Reflection;
using Toolkit.Localizations.Abstractions.Services;
using Toolkit.Localizations.Infrastructure.Attributes;

namespace Toolkit.Localizations.Infrastructure.Services;

internal sealed class InterfaceLocalizationTypeDefiner : ILocalizationTypeDefiner
{
    public IEnumerable<Type> Define(Type type)
    {
        return type.IsDefined(typeof(UseInterfacesStringLocalizationsAttribute), inherit: false)
            ? type.GetInterfaces()
            : [];
    }
}
