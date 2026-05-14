using System.Reflection;
using Toolkit.Localizations.Abstractions.Services;
using Toolkit.Localizations.Infrastructure.Attributes;

namespace Toolkit.Localizations.Infrastructure.Services;

internal sealed class AllGenericLocalizationTypeDefiner : ILocalizationTypeDefiner
{
    public IEnumerable<Type> Define(Type type)
    {
        var genericAttributes = type.GetCustomAttributes<UseAllGenericStringLocalizationsAttribute>();
        if (genericAttributes.Any())
        {
            List<Type> result = [];

            IEnumerable<Type> types = type.GetGenericArguments();
            while (types.Any())
            {
                foreach (Type genericType in types)
                {
                    result.Add(genericType);
                }

                types = types.SelectMany(i => i.GetGenericArguments()).ToList();
            }
            return result;
        }

        return [];
    }
}
