using FinancialTracker.Interfaces;
using FinancialTracker.Entities;

namespace FinancialTracker.Utilities
{
    public class ReportGenerator : IReportService
    {
        private readonly ITransactionService _transactionService;
        private readonly ICategoryService _categoryService;
        public ReportGenerator(ITransactionService transactionService, ICategoryService categoryService)
        {
            _transactionService = transactionService;
            _categoryService = categoryService;
        }
        public Report GenerateFinancialReport(DateTime startDate, DateTime endDate)
        {
            var report = new Report(startDate, endDate);
            var transactions = _transactionService.GetTransactionsByPeriod(startDate, endDate);

            foreach(var transaction in transactions )
            {
                var category = _categoryService.GetCategory(transaction.CategoryId);
                var amount = transaction.Type == TransactionType.Income ? transaction.Amount : -transaction.Amount;
                report.AddCategoryData(
                        transaction.CategoryId,
                        transaction.Type == TransactionType.Income ? transaction.Amount : 0,
                        transaction.Type == TransactionType.Expense ? Math.Abs(transaction.Amount) : 0
                        );
            }
            return report;
        }
    }
}
