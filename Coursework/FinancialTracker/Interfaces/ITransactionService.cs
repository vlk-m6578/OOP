using FinancialTracker.Entities;

namespace FinancialTracker.Interfaces
{
    public interface ITransactionService
    {
        Transaction AddTransaction(decimal amount, int categoryId, int accountId, int userId, string description, TransactionType type);
        void UpdateTransaction(int transactionId, decimal newAmount, int newCategoryId, string newDscription, int editorId);
        void DeleteTransaction(int transactionId, int userId);
        List<Transaction> GetTransactionsByAccount(int accountId);
        bool ValidateBalance(int accountId, decimal amount);
        List<Transaction> GetTransactionsByPeriod(DateTime startDate, DateTime endDate);
        List<Transaction> GetTransactionsByUser(int userId);
        List<Transaction> GetTransactionsByCategory(int categoryId);
        List<Transaction> GetTransactionsByAmountRange(decimal min, decimal max);
        List<Transaction> GetTransactionsByType(TransactionType type);
    }
}
