using FinancialTracker.Domain.Entities;

namespace FinancialTracker.Domain.Interfaces
{
    public interface IReportService
    {
        Report GenerateMonthlyReport(int userId, DateTime month);
        Report GenerateCustomReport(int userId, DateTime start, DateTime end);
        void SaveLastReportToFile(string path = "reports");
        //string GetFormattedReport();
    }
}
