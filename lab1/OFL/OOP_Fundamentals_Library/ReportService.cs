using System.Xml.Linq;

namespace OOP_Fundamentals_Library
{
    public class ReportService
    {
        public void GenerateReport(IReportable rep)
        {
            Console.WriteLine(rep.GenerateReport());
        }
    }
}
