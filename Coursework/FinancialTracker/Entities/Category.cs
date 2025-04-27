
using System.ComponentModel.DataAnnotations;

namespace FinancialTracker.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public decimal MonthlyBudgetLimit { get; set; }

        [Required]
        public bool IsSystemCategory { get; set; }

        // Конструктор для EF Core
        private Category() { }

        public Category(string name, bool isSystem = false)
        {
            Name = name;
            IsSystemCategory = isSystem;
            MonthlyBudgetLimit = 0;
        }
        public Category(int id, string name, bool isSystem = false)
        {
            Id = id;
            Name = name;
            IsSystemCategory = isSystem;
        }
        public void SetBudgetLimit(decimal limit) => MonthlyBudgetLimit = limit;
    }
}
