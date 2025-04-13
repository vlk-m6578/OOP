using FinancialTracker.Entities;

namespace FinancialTracker.Interfaces
{
    public interface IReportService
    {
        Report GenerateFinancialReport(DateTime startDate, DateTime endDate);
    }
}
