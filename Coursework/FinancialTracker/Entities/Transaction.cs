
using FinancialTracker.Entities.Accounts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        public Account Account { get; set; }
        // Конструктор для EF Core
        public Transaction() { }

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
        public void Update(decimal newAmount, int newCategoryId, string newDescription, int editorId)
        {
            EditHistory.Add(new TransactionEditHistory(
                transactionId: this.Id,      // Добавляем ID текущей транзакции
                editorId: editorId,          // ID редактора
                oldAmount: this.Amount,
                newAmount: newAmount,
                oldCategory: this.CategoryId,
                newCategory: newCategoryId,
                oldDesc: this.Description,
                newDesc: newDescription      // Добавляем недостающий параметр
        ));

            Amount = newAmount;
            CategoryId = newCategoryId;
            Description = newDescription;
        }
    }
}
