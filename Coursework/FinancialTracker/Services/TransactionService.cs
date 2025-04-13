using FinancialTracker.Interfaces;
using FinancialTracker.Entities;

namespace FinancialTracker.Services
{
    public class TransactionService : ITransactionService
    {
        private static int _transactionId = 0;
        public Transaction AddTransaction(decimal amount, int categoryId, int accountId, int userId, string description, TransactionType type)
        {
            var transaction = new Transaction(
                ++_transactionId,
                type == TransactionType.Income ? amount : -amount,
                categoryId,
                accountId,
                userId,
                description,
                type
                );
            return transaction;
        }
        public void UpdateTransaction(int transactionId, decimal newAmount, int newCategoryId, string newDscription, int editorId)
        {
            return;
        }
        public void DeleteTransaction(int transactionId)
        {
            return;
        }
        public List<Transaction> GetTransactionsByAccount(int accountId)
        {
            return new List<Transaction>();
        }
        public bool ValidateBalance(int accountId, decimal amount)
        {
            return true;
        }
        public List<Transaction> GetTransactionsByPeriod(DateTime startDate, DateTime endDate)
        {
            return new List<Transaction>();
        }
    }
}
