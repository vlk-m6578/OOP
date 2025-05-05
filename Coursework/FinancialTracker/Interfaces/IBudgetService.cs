using FinancialTracker.Entities;

namespace FinancialTracker.Interfaces
{
    public interface IBudgetService
    {
        void SetBudgetLimit(int userId, int categoryId, decimal limit);
        List<Budget> GetCurrentBudgets(int userId);
        void UpdateSpending(int userId, int categoryId, decimal amount);
    }
}
