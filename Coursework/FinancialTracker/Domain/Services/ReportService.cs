using FinancialTracker.Data;
using FinancialTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinancialTracker.Domain.Services
{
    public class ReportService
    {
        private readonly AppDbContext _context;
        private Report _lastReport;

        public ReportService(AppDbContext context)
        {
            _context = context;
        }
        public Report GenerateMonthlyReport(int userId, DateTime month)
        {
            var start = new DateTime(month.Year, month.Month, 1);
            var end = start.AddMonths(1).AddDays(-1);
            return GenerateReport(userId, start, end);
        }

        public Report GenerateCustomReport(int userId, DateTime start, DateTime end)
        {
            return GenerateReport(userId, start, end);
        }
        public Report GenerateReport(int userId, DateTime start, DateTime end)
        {
            var report = new Report(start, end);

            var transactions = _context.Transactions
                .Include(t => t.Category)
                .Where(t => t.CreatedByUserId == userId
                          && !t.IsDeleted
                          && t.Date >= start
                          && t.Date <= end)
                .AsNoTracking()
                .ToList();

            foreach (var transaction in transactions)
            {
                var categoryName = transaction.Category?.Name ?? "Without a category";
                var budget = GetBudget(userId, transaction);

                report.AddCategoryData(
                    categoryName,
                    transaction.Type == TransactionType.Income ? Math.Abs(transaction.Amount) : 0,
                    transaction.Type == TransactionType.Expense ? Math.Abs(transaction.Amount) : 0,
                    budget?.Limit ?? 0,
                    transaction.Type == TransactionType.Expense ? Math.Abs(transaction.Amount) : 0
                );
            }

            _lastReport = report;
            return report;
        }

        private Budget GetBudget(int userId, Transaction transaction)
        {
            if (transaction.Category == null) return null;

            return _context.Budgets
                .FirstOrDefault(b => b.UserId == userId
                                  && b.CategoryId == transaction.Category.Id
                                  && b.Month.Month == transaction.Date.Month
                                  && b.Month.Year == transaction.Date.Year);
        }

        public void SaveLastReport(string path = "Reports")
        {
            if (_lastReport == null) return;

            Directory.CreateDirectory(path);
            var fileName = $"Отчет_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
            File.WriteAllText(Path.Combine(path, fileName), _lastReport.GetFormattedReport());
        }
        public void SaveLastReportToFile(string path = "reports")
        {
            if (_lastReport == null)
                throw new InvalidOperationException("There are no reports available to save.");

            Directory.CreateDirectory(path);
            string fileName = $"report_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
            string fullPath = Path.Combine(path, fileName);

            File.WriteAllText(fullPath, _lastReport.GetFormattedReport());
        }
    }
}