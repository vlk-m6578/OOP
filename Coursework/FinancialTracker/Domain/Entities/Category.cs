using System.ComponentModel.DataAnnotations;

namespace FinancialTracker.Domain.Entities
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
        private Category() { }

        public Category(string name, bool isSystem = false)
        {
            Name = name;
            IsSystemCategory = isSystem;
            MonthlyBudgetLimit = 0;
        }
        public Category(string name) : this(name, false) { }
        public void SetBudgetLimit(decimal limit) => MonthlyBudgetLimit = limit;
    }
}
