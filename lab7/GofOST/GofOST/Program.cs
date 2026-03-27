using GofOST.Model;

internal class Program
{
    private static void Main(string[] args)
    {
        EventMonitor myMonitor = new EventMonitor();
        myMonitor.OnMetricExceeded += e => Console.WriteLine($"{e.Data.MetricName} is too high!");
        myMonitor.OnMetricExceeded += e => { if (e.Data.MetricName == "Temperature") Console.WriteLine("Throttling CPU down"); };
        myMonitor.CheckMetric();
    }
}