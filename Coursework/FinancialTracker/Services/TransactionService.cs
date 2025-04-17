using FinancialTracker.Interfaces;
using FinancialTracker.Entities;

namespace FinancialTracker.Services
{
    public class TransactionService : ITransactionService
    {
        private static int _transactionId = 0;
        private static readonly List<Transaction> _transactions = new List<Transaction>();
        public Transaction AddTransaction(decimal amount, int categoryId, int accountId, int userId, string description, TransactionType type)
        {
            var transaction = new Transaction(
                ++_transactionId,
                type == TransactionType.Income ? amount : -amount,
                categoryId,
                accountId,
                userId,
                description,
                type);

            _transactions.Add(transaction);
            return transaction;
        }
        public void UpdateTransaction(int transactionId, decimal newAmount, int newCategoryId, string newDescription, int editorId)
        {
            var transaction = _transactions.FirstOrDefault(t => t.Id == transactionId);
            if (transaction == null) return;

            var oldAmount = transaction.Amount;
            var oldCategoryId = transaction.CategoryId;
            var oldDescription = transaction.Description;

            transaction.Update(
                transaction.Type == TransactionType.Income ? newAmount : -newAmount,
                newCategoryId,
                newDescription,
                editorId
                );

            //if (transaction.Type != type)
            //{
            //    transaction.Amount = type == TransactionType.Income ? newAmount : -newAmount;
            //}
        }
        public void DeleteTransaction(int transactionId)
        {
            var transaction = _transactions.FirstOrDefault(t => t.Id == transactionId);
            if(transaction != null)
            {
                transaction.IsDeleted = true;
                _transactions.Remove(transaction);
            }
        }
        public List<Transaction> GetTransactionsByAccount(int accountId)
        {
            return _transactions
                .Where(t => t.AccountId == accountId)
                .ToList();
        }
        public bool ValidateBalance(int accountId, decimal amount)
        {
            return false;

        }
        public List<Transaction> GetTransactionsByPeriod(DateTime startDate, DateTime endDate)
        {
            return _transactions
                .Where(t => t.Date >= startDate && t.Date <= endDate)
                .ToList();
        }
        public List<Transaction> GetTransactionsByUser(int userId)
        {
            return _transactions
                .Where(t => t.CreatedByUserId == userId)
                .ToList();
        }
        public List<Transaction> GetTransactionsByCategory(int categoryId)
        {
            return _transactions
                .Where(t => t.CategoryId == categoryId)
                .ToList();
        }

        public List<Transaction> GetTransactionsByAmountRange(decimal min, decimal max)
        {
            return _transactions
                .Where(t => Math.Abs(t.Amount) >= min && Math.Abs(t.Amount) <= max)
                .ToList();
        }

        public List<Transaction> GetTransactionsByType(TransactionType type)
        {
            return _transactions
                .Where(t => t.Type == type)
                .ToList();
        }
    }
}
