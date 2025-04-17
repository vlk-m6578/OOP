using FinancialTracker.Entities;

namespace FinancialTracker.Interfaces
{
    public interface IBudgetService
    {
        void SetBudgetLimit(int categoryId, decimal limit);
        List<Budget> GetCurrentBudgets();
        void UpdateBudgetSpending();
    }
}
