using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp
{
    internal class GeoFactoryProvider
    {
        private static readonly List<IGeoFactory> _instances;
        static GeoFactoryProvider()
        {
            _instances = typeof(IGeoFactory).Assembly.GetTypes()                                        // Get all types
                .Where(t => typeof(IGeoFactory).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract) // That implement IGeoFactory, are classes, and not abstractions
                .Select(t => (IGeoFactory)Activator.CreateInstance(t)!)                                 // We can create instances of ALL factories since they are stateless they won't be using a lot of memory 
                .ToList();
        }
        public static IEnumerable<IGeoFactory> GetFactories() => _instances;

    }
}
