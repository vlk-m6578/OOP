
namespace FinancialTracker.Entities
{
    public class Category
    {
        public int Id { get; }
        public string Name { get; }
        public decimal MonthlyBudgetLimit { get; private set; }
        public bool IsSystemCategory { get; }
        public Category(int id, string name, bool isSystem = false)
        {
            Id = id;
            Name = name;
            IsSystemCategory = isSystem;
        }
        public void SetBudgetLimit(decimal limit) => MonthlyBudgetLimit = limit;
    }
}
