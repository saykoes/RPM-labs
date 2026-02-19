namespace SOLID_Fundamentals
{
    // Change: Created higher abstraction
    public interface IMessageService
    {
        void Send(Customer target, string message);
    }

    public class EmailService : IMessageService
    {
        public void Send(Customer target, string message) =>
            Console.WriteLine($"Email to {target.Email}: {message}");
    }

    public class SmsService : IMessageService
    {
        public void Send(Customer target, string message) =>
            Console.WriteLine($"SMS to {target.Phone}: {message}");
    }

    // Change: Dependecy inverted 
    public class OrderService
    {
        // Change: making messaging service list to be more open to changes
        private readonly List<IMessageService> _messageServices;

        public OrderService(List<IMessageService> messageServices)
        {
            _messageServices = messageServices;
        }

        public void PlaceOrder(Order order)
        {
            foreach (var service in _messageServices)
                service.Send(order.Customer, "Your order has been placed");
        }
    }

    // Change: Dependecy inverted
    public class NotificationService
    {
        private readonly IMessageService _emailService;

        public NotificationService(IMessageService emailService)
        {
            _emailService = emailService;
        }

        public void SendPromotion(Customer target, string promotion)
        {
            _emailService.Send(target, promotion);
        }
    }
}