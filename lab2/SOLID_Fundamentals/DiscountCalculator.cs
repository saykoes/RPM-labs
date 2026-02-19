namespace SOLID_Fundamentals
{
    // Change if-else ---> interface and calculating discount separately 
    public interface IDiscountStrategy
    {
        decimal Calculate(decimal amount);
    }

    public class RegularDiscount : IDiscountStrategy { public decimal Calculate(decimal amount) => amount * 0.05m; }
    public class PremiumDiscount : IDiscountStrategy { public decimal Calculate(decimal amount) => amount * 0.10m; }
    public class VipDiscount : IDiscountStrategy { public decimal Calculate(decimal amount) => amount * 0.15m; }
    public class StudentDiscount : IDiscountStrategy { public decimal Calculate(decimal amount) => amount * 0.08m; }
}
