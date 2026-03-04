using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GofPBS
{
    public sealed class PrototypeRegistry
    {
        private static readonly Lazy<PrototypeRegistry> _instance =
            new Lazy<PrototypeRegistry>(() => new PrototypeRegistry());

        private readonly Dictionary<string, Computer> _dict;

        private PrototypeRegistry()
        {
            _dict = new Dictionary<string, Computer>
        {
            {"office", new OfficeComputerFactory().CreateComputer() },
            {"pro", new ProComputerFactory().CreateComputer() },
            {"mac", new MacMiniComputerFactory().CreateComputer() }
        };
        }

        public static PrototypeRegistry Instance => _instance.Value;

        public Computer GetPrototype(string key)
        {
            if (_dict.TryGetValue(key, out var prototype))
            {
                return prototype.DeepCopy();
            }
            return new Computer();
        }
    }
}
