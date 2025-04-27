
using System.ComponentModel.DataAnnotations;

namespace FinancialTracker.Entities
{
    public class Budget
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public decimal CurrentSpending { get; set; }

        [Required]
        public DateTime Month { get; set; }

        [Required]
        [Range(0, 1000000)]
        public decimal Limit { get; set; }

        // Конструктор для EF Core
        private Budget() { }

        public Budget(int categoryId, decimal limit, DateTime month)
        {
            CategoryId = categoryId;
            Limit = limit;
            Month = month;
            CurrentSpending = 0;
        }
        public Budget(int categoryId, decimal limit) 
        {
            CategoryId = categoryId;
            Limit = limit;
            Month = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        }

        public void UpdateLimit(decimal newLimit) => Limit = newLimit;
        public void UpdateSpending(decimal amount) => CurrentSpending += amount;
        public bool isLimitReached() => CurrentSpending >= Limit;
        public bool IsWarningThresholdReached() => CurrentSpending >= Limit * 0.8m;
    }
}
