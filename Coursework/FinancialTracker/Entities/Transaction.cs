
using System.ComponentModel.DataAnnotations;

namespace FinancialTracker.Entities
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int AccountId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public int CreatedByUserId { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public List<TransactionEditHistory> EditHistory { get; set; } = new();

        [Required]
        public TransactionType Type { get; set; }

        public bool IsDeleted { get; set; }

        // Конструктор для EF Core
        private Transaction() { }

        public Transaction(decimal amount, int categoryId, int accountId, int userId,
                          string description, TransactionType type)
        {
            Date = DateTime.Now;
            CategoryId = categoryId;
            AccountId = accountId;
            Amount = amount;
            CreatedByUserId = userId;
            Description = description;
            Type = type;
        }
        public Transaction(int id, decimal amount, int categoryId, int accountId, int userId, string description, TransactionType type) 
        {
            Id = id;
            Date = DateTime.Now;
            CategoryId = categoryId;
            AccountId = accountId;
            Amount = amount;
            CreatedByUserId = userId;
            Description = description;
            Type = type;
        }
        public void Update(decimal newAmount, int newCategoryId, string newDescription, int editorId)
        {
            EditHistory.Add(new TransactionEditHistory(
                editorId,
                Amount, newAmount,
                CategoryId, newCategoryId,
                Description, newDescription
                ));
            Amount = newAmount;
            CategoryId = newCategoryId;
            Description = newDescription;
        }
    }
}
