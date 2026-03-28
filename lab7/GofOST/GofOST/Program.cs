using GofOST.Model;

internal class Program
{
    private static void Main(string[] args)
    {
        EventMonitor myMonitor = new EventMonitor();
        myMonitor.OnMetricExceeded += e => Console.WriteLine($"{e.Data.MetricName} is too high!");
        myMonitor.OnMetricExceeded += e => { if (e.Data.MetricName == "Temperature") Console.WriteLine("Throttling CPU down"); };
        myMonitor.CheckMetric();

        JsonFormatStrategy jsonStrategy = new JsonFormatStrategy();
        TextFormatStrategy textStrategy = new TextFormatStrategy();

        ConsoleHandler consoleHandler = new ConsoleHandler(jsonStrategy);
        FileHandler fileHandler = new FileHandler(textStrategy);

        consoleHandler.ProcessEvent(new MetricEventArgs("myFavouriteConsoleLog", new MetricData("temp", 250, 100, DateTime.Now)));
        fileHandler.ProcessEvent(new MetricEventArgs("myBelovedFileLog", new MetricData("network", 1000, 100, DateTime.Now)));
    }
}