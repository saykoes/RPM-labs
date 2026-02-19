using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLID_Fundamentals
{
    // Change: string --> interface
    public interface IPaymentMethod
    {
        void GetInfo();
    }

    public class CreditCard : IPaymentMethod { public void GetInfo() => Console.WriteLine("Payment Method: Credit Card"); }
    public class PayPal : IPaymentMethod { public void GetInfo() => Console.WriteLine("Payment Method: PayPal"); }
    public class Crypto : IPaymentMethod { public void GetInfo() => Console.WriteLine("Payment Method: Crypto"); }
    public class Cash : IPaymentMethod { public void GetInfo() => Console.WriteLine("Payment Method: Cash"); }
}
