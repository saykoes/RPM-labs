using System;
using System.Collections.Generic;
using System.Text;

namespace SOLID_Fundamentals
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    // Change: defined interfaces outside of class
    public interface IPaymentService { void Process(PaymentMethod method, decimal amount); }
    public interface INotificationService { void Notify(string destination, string message); }
    public interface IInventoryService { void Update(List<string> items); }
    public interface ILogger { void Log(string message); }

    // Change: defined Order Repository (to reduce responcibility)
    public interface IOrderRepository
    {
        void Add(Order order);
        Order? GetById(int id);
        List<Order> GetAll();
    }

    internal class OrderRepository : IOrderRepository
    {
        private List<Order> orders = new List<Order>();

        public void Add(Order order)
        {
            orders.Add(order);
            Console.WriteLine($"Order {order.Id} added");
        }

        public Order? GetById(int id) => orders.FirstOrDefault(o => o.Id == id); 

        public List<Order> GetAll() => orders;
    }

    public class OrderProcessor
    {
        private readonly IOrderRepository _repository;
        private readonly IPaymentService _paymentService;
        private readonly INotificationService _notificationService;
        private readonly IInventoryService _inventoryService;
        private readonly ILogger _logger;

        // Constructor Injection
        public OrderProcessor(
            IOrderRepository repository,
            IPaymentService paymentService,
            INotificationService notificationService,
            IInventoryService inventoryService,
            ILogger logger)
        {
            _repository = repository;
            _paymentService = paymentService;
            _notificationService = notificationService;
            _inventoryService = inventoryService;
            _logger = logger;
        }

        public void ProcessOrder(int orderId)
        {
            var order = _repository.GetById(orderId);
            if (order == null) return;

            if (order.TotalAmount <= 0)
                throw new InvalidOperationException("Invalid order amount");

            _paymentService.Process(order.PaymentMethod, order.TotalAmount);
            _inventoryService.Update(order.Items);
            _notificationService.Notify(order.Customer.Email, $"Order {orderId} processed");
            _logger.Log($"Order {orderId} processed at {DateTime.Now}");
        }
    }

    // Change: Separated Reporting into another class
    public class OrderReportGenerator
    {
        public void GenerateMonthlyReport(List<Order> orders)
        {
            decimal totalRevenue = orders.Sum(o => o.TotalAmount);
            Console.WriteLine($"Report: Revenue {totalRevenue:C}");
        }
    }
}
