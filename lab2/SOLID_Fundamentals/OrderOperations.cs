namespace SOLID_Fundamentals
{
    // Base interface
    public interface IOrderService
    {
        void CreateOrder(Order order);
        void UpdateOrder(Order order);
        void DeleteOrder(int orderId);
    }

    // Processing interface
    public interface IOrderProcessor
    {
        void ProcessPayment(Order order);
        void ShipOrder(Order order);
    }

    // Notification interface
    public interface IDocumentationService
    {
        void GenerateInvoice(Order order);
        void SendNotification(Order order);
    }

    // Report interface
    public interface IReportingService
    {
        void GenerateReport(DateTime from, DateTime to);
        void ExportToExcel(string filePath);
    }

    // Admin interface
    public interface IDatabaseAdmin
    {
        void BackupDatabase();
        void RestoreDatabase();
    }

    public class OrderManager : IOrderService, IOrderProcessor, IDocumentationService, IReportingService
    {
        public void CreateOrder(Order order) { Console.WriteLine("Order created"); }
        public void UpdateOrder(Order order) { Console.WriteLine("Order updated"); }
        public void DeleteOrder(int orderId) { Console.WriteLine("Order deleted"); }
        public void ProcessPayment(Order order) { Console.WriteLine("Payment processed"); }
        public void ShipOrder(Order order) { Console.WriteLine("Order shipped"); }
        public void GenerateInvoice(Order order) { Console.WriteLine("Invoice generated"); }
        public void SendNotification(Order order) { Console.WriteLine("Notification sent"); }
        public void GenerateReport(DateTime from, DateTime to) { Console.WriteLine("Report generated"); }
        public void ExportToExcel(string filePath) { Console.WriteLine("Exported to Excel"); }
    }

    public class CustomerPortal : IOrderService
    {
        public void CreateOrder(Order order) => Console.WriteLine("Order created by customer");
        public void UpdateOrder(Order order) => Console.WriteLine("Order updated by customer");
        public void DeleteOrder(int orderId) => Console.WriteLine("Order deleted by customer");
    }

    // Change: Moved db operations to Admin class (to reduce responsibility of OrderManager)
    public class DbAdministrator : IDatabaseAdmin
    {
        public void BackupDatabase() => Console.WriteLine("Database backed up");
        public void RestoreDatabase() => Console.WriteLine("Database restored");
    }
}
