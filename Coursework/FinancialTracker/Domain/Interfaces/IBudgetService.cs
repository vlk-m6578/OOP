using FinancialTracker.Domain.Entities;

namespace FinancialTracker.Domain.Interfaces
{
    public interface IBudgetService
    {
        void SetBudgetLimit(int userId, int categoryId, decimal limit);
        List<Budget> GetCurrentBudgets(int userId);
        void UpdateSpending(int userId, int categoryId, decimal amount);
        List<Budget> GetExpiredBudgets();
    }
}
