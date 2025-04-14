
namespace FinancialTracker.Entities.Accounts
{
    public abstract class Account
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public DateTime createdAt { get; set; }
        public List<Transaction> Transactions { get; } = new List<Transaction>();
        protected Account(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public virtual void ApplyTransaction(Transaction transaction)
        {
            Balance += transaction.Amount;
            Transactions.Add(transaction);
        }
    }
}
