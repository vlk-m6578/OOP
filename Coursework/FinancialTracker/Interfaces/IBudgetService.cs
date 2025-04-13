using FinancialTracker.Entities;

namespace FinancialTracker.Interfaces
{
    public interface IBudgetService
    {
        void SetBudgetLimit(int categoryId, decimal limit);
        decimal CalculateMonthlySpending(int categoryId);
        List<Budget> GetActiveAlerts();
    }
}
