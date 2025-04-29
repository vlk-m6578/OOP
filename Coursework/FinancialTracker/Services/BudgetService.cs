using FinancialTracker.Interfaces;
using FinancialTracker.Entities;
using FinancialTracker.Data;

namespace FinancialTracker.Services
{
    public class BudgetService : IBudgetService
    {
        private readonly ITransactionService _transactionService;

        private readonly AppDbContext _context;
        public BudgetService(AppDbContext context, ITransactionService transactionService)
        {
            _context = context;
            _transactionService = transactionService;
        }
        public void SetBudgetLimit(int categoryId, decimal limit)
        {
            var currentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var budget = _context.Budgets
                .FirstOrDefault(b => b.CategoryId == categoryId && b.Month == currentMonth);

            if (budget != null)
            {
                budget.UpdateLimit(limit);
            }
            else
            {
                _context.Budgets.Add(new Budget(categoryId, limit, currentMonth));
            }
            _context.SaveChanges();
        }
        public List<Budget> GetCurrentBudgets()
        {
            var currentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            return _context.Budgets
                .Where(b => b.Month == currentMonth)
                .ToList();
        }

        public void UpdateBudgetSpending()
        {
            var currentMonthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var transactions = _transactionService.GetTransactionsByPeriod(
                currentMonthStart,
                currentMonthStart.AddMonths(1).AddDays(-1));

            foreach (var budget in _context.Budgets)
            {
                var categorySpending = transactions
                    .Where(t => t.CategoryId == budget.CategoryId && t.Type == TransactionType.Expense)
                    .Sum(t => t.Amount);

                budget.UpdateSpending(Math.Abs(categorySpending));
            }
        }

        public List<Budget> GetExpiredBudgets()
        {
            return _context.Budgets
                .Where(b => b.Month < DateTime.Now.AddMonths(-1))
                .ToList();
        }
    }
}
