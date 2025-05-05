
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialTracker.Entities
{
    public class Budget
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [Required]
        public int UserId { get; set; }
        public decimal CurrentSpending { get; set; }

        [Required]
        public DateTime Month { get; set; }

        [Required]
        [Range(0, 1000000)]
        public decimal Limit { get; set; }

        // Конструктор для EF Core
        private Budget() { }

        public Budget(int userId, int categoryId, decimal limit, DateTime month)
        {
            UserId = userId;
            CategoryId = categoryId;
            Limit = limit;
            Month = month;
            CurrentSpending = 0;
        }

        public void UpdateLimit(decimal newLimit) => Limit = newLimit;
        public void UpdateSpending(decimal amount) => CurrentSpending += amount;
        public bool IsLimitReached() => CurrentSpending >= Limit;
        public bool IsWarningThresholdReached() => CurrentSpending >= Limit * 0.8m;
    }
}
