using FinancialTracker.Interfaces;
using FinancialTracker.Entities;
using FinancialTracker.Data;
using Microsoft.EntityFrameworkCore;

namespace FinancialTracker.Services
{
    public class BudgetService : IBudgetService
    {
        //private readonly ITransactionService _transactionService;

        private readonly AppDbContext _context;
        public BudgetService(AppDbContext context)
        {
            _context = context;
        }
        public void SetBudgetLimit(int userId, int categoryId, decimal limit)
        {
            var currentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var budget = _context.Budgets
                .FirstOrDefault(b => b.UserId == userId &&
                                    b.CategoryId == categoryId &&
                                    b.Month == currentMonth);

            if (budget != null)
            {
                budget.Limit = limit;
            }
            else
            {
                _context.Budgets.Add(new Budget(userId, categoryId, limit, currentMonth));
            }
            _context.SaveChanges();
        }
        public List<Budget> GetCurrentBudgets(int userId)
        {
            var currentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            return _context.Budgets
                .Where(b => b.UserId == userId && b.Month == currentMonth)
                .Include(b => b.Category)
                .ToList();
        }


        public void UpdateSpending(int userId, int categoryId, decimal amount)
        {
            var currentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var budget = _context.Budgets
                .FirstOrDefault(b => b.UserId == userId &&
                                   b.CategoryId == categoryId &&
                                   b.Month == currentMonth);

            if (budget != null)
            {
                budget.UpdateSpending(amount);
                CheckAndNotify(budget);
                _context.SaveChanges();
            }
        }
        private void CheckAndNotify(Budget budget)
        {
            if (budget.IsLimitReached())
            {
                CreateNotification(budget.UserId,
                    $"Лимит категории '{GetCategoryName(budget.CategoryId)}' достигнут! ({budget.CurrentSpending}/{budget.Limit})");
            }
            else if (budget.IsWarningThresholdReached())
            {
                CreateNotification(budget.UserId,
                    $"Лимит категории '{GetCategoryName(budget.CategoryId)}' достиг 80%! ({budget.CurrentSpending}/{budget.Limit})");
            }
        }
        private string GetCategoryName(int categoryId)
        {
            return _context.Categories
                .FirstOrDefault(c => c.Id == categoryId)?
                .Name ?? "Unknown Category";
        }

        private void CreateNotification(int userId, string message)
        {
            _context.Notifications.Add(new Notification
            {
                UserId = userId,
                Message = message,
                CreatedAt = DateTime.Now,
                IsRead = false
            });
        }

        public List<Budget> GetExpiredBudgets()
        {
            return _context.Budgets
                .Where(b => b.Month < DateTime.Now.AddMonths(-1))
                .ToList();
        }
    }
}
