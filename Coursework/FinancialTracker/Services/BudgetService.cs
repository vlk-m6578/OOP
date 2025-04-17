using FinancialTracker.Interfaces;
using FinancialTracker.Entities;

namespace FinancialTracker.Services
{
    public class BudgetService : IBudgetService
    {
        private static readonly List<Budget> _budgets = new List<Budget>();
        private readonly ITransactionService _transactionService;
        public BudgetService(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }
        public void SetBudgetLimit(int categoryId, decimal limit)
        {
            var existing = _budgets.FirstOrDefault(b =>
            b.CategoryId == categoryId &&
            b.Month.Month == DateTime.Now.Month);

            if (existing != null)
            {
                existing.UpdateLimit(limit);
            }
            else
            {
                _budgets.Add(new Budget(categoryId, limit));
            }
        }
        public List<Budget> GetCurrentBudgets()
        {
            var currentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            return _budgets
                .Where(b => b.Month == currentMonth)
                .ToList();
        }

        public void UpdateBudgetSpending()
        {
            var currentMonthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var transactions = _transactionService.GetTransactionsByPeriod(
                currentMonthStart,
                currentMonthStart.AddMonths(1).AddDays(-1));

            foreach (var budget in _budgets)
            {
                var categorySpending = transactions
                    .Where(t => t.CategoryId == budget.CategoryId && t.Type == TransactionType.Expense)
                    .Sum(t => t.Amount);

                budget.UpdateSpending(Math.Abs(categorySpending));
            }
        }
    }
}
