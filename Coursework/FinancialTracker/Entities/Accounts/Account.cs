
namespace FinancialTracker.Entities.Accounts
{
    public abstract class Account
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public DateTime CreatedAt { get; set; }
       public virtual ICollection<Transaction> Transactions { get; set; }

        protected Account() { } // Пустой конструктор для EF Core

        protected Account(string name)
        {
            Name = name;
        }
        
        public virtual void ApplyTransaction(Transaction transaction)
        {
            Balance += transaction.Amount;
            Transactions.Add(transaction);
        }
    }
}
