using FinancialTracker.Interfaces;
using FinancialTracker.Entities;
using Microsoft.EntityFrameworkCore;
using FinancialTracker.Data;

namespace FinancialTracker.Utilities
{
    public class ReportGenerator : IReportService
    {
        private readonly ITransactionService _transactionService;
        private readonly ICategoryService _categoryService;
        private readonly AppDbContext _context;
        public ReportGenerator(AppDbContext context, ITransactionService transactionService, ICategoryService categoryService)
        {
            _context = context;
            _transactionService = transactionService;
            _categoryService = categoryService;
        }
        public Report GenerateFinancialReport(DateTime startDate, DateTime endDate)
        {
            var report = new Report(startDate, endDate);
            var transactions = _transactionService.GetTransactionsByPeriod(startDate, endDate);

            foreach (var transaction in transactions)
            {
                var category = _context.Categories.FirstOrDefault(c => c.Id == transaction.CategoryId);
                report.AddCategoryData(
                    transaction.CategoryId,
                    transaction.Type == TransactionType.Income ? transaction.Amount : 0,
                    transaction.Type == TransactionType.Expense ? Math.Abs(transaction.Amount) : 0
                );
            }
            return report;
        }

        private string GetCategoryName(int categoryId)
        {
            return _context.Categories
                .FirstOrDefault(c => c.Id == categoryId)?
                .Name ?? "Unknown";
        }
    }
}
