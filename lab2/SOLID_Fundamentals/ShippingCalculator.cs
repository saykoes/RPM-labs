using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLID_Fundamentals
{
    // Moved Shipping from DiscountCalculator

    // Change: (decimal, string) -> (decimal, Destination)
    public interface IShippingStrategy
    {
        decimal Calculate(decimal weight, Destination dest);
    }

    public class StandardShipping : IShippingStrategy { public decimal Calculate(decimal weight, Destination dest) => 5.00m + (weight * 0.5m); }
    public class ExpressShipping : IShippingStrategy { public decimal Calculate(decimal weight, Destination dest) => 15.00m + (weight * 1.0m); }
    public class OvernightShipping : IShippingStrategy { public decimal Calculate(decimal weight, Destination dest) => 25.00m + (weight * 2.0m); }
    public class InternationalShipping : IShippingStrategy { public decimal Calculate(decimal weight, Destination dest) => (dest is null) ? 50.00m : dest.shippingFee; }

    // Change: defining destination classes isntead of if-else statement
    public abstract class Destination
    {
        public abstract decimal shippingFee { get; }
    }
    public class DestDefault : Destination { public override decimal shippingFee => 50.00m; }
    public class DestUSA : Destination { public override decimal shippingFee => 30.00m; }
    public class DestEU : Destination { public override decimal shippingFee => 35.00m; }
    public class DestAsia : Destination { public override decimal shippingFee => 40.00m; }

}
