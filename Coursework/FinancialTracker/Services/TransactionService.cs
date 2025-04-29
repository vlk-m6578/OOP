using FinancialTracker.Interfaces;
using FinancialTracker.Entities;
using Microsoft.EntityFrameworkCore;
using FinancialTracker.Data;

namespace FinancialTracker.Services
{
    public class TransactionService : ITransactionService
    {
        //private static int _transactionId = 0;
        public virtual ICollection<Transaction> Transactions { get; set; }
        private readonly AppDbContext _context;

        public TransactionService(AppDbContext context)
        {
            _context = context;
        }
        public Transaction AddTransaction(decimal amount, int categoryId, int accountId, int userId, string description, TransactionType type)
        {
            var transaction = new Transaction(
                amount: type == TransactionType.Income ? amount : -amount,
                categoryId: categoryId,
                accountId: accountId,
                userId: userId,
                description: description,
                type: type
            );

            _context.Transactions.Add(transaction);
            _context.SaveChanges();
            return transaction;
        }
        public bool ValidateBalance(int accountId, decimal amount)
        {
            var account = _context.Accounts.Find(accountId);
            return account?.Balance + amount >= 0;
        }
        public void UpdateTransaction(int transactionId, decimal newAmount, int newCategoryId, string newDescription, int editorId)
        {
            var transaction = _context.Transactions
                .Include(t => t.EditHistory)
                .FirstOrDefault(t => t.Id == transactionId);

            if (transaction == null) return;

            transaction.Update(newAmount, newCategoryId, newDescription, editorId);
            _context.SaveChanges();
        }
        public void DeleteTransaction(int transactionId)
        {
            var transaction = _context.Transactions.FirstOrDefault(t => t.Id == transactionId);
            if(transaction != null)
            {
                transaction.IsDeleted = true;
                _context.Transactions.Remove(transaction);
            }
        }
        public List<Transaction> GetTransactionsByAccount(int accountId)
        {
            return _context.Transactions
                .Where(t => t.AccountId == accountId)
                .ToList();
        }
        
        public List<Transaction> GetTransactionsByPeriod(DateTime startDate, DateTime endDate)
        {
            return _context.Transactions
                .Where(t => t.Date >= startDate && t.Date <= endDate)
                .ToList();
        }
        public List<Transaction> GetTransactionsByUser(int userId)
        {
            return _context.Transactions
                .Where(t => t.CreatedByUserId == userId)
                .ToList();
        }
        public List<Transaction> GetTransactionsByCategory(int categoryId)
        {
            return _context.Transactions
                .Where(t => t.CategoryId == categoryId)
                .ToList();
        }

        public List<Transaction> GetTransactionsByAmountRange(decimal min, decimal max)
        {
            return _context.Transactions
                .Where(t => Math.Abs(t.Amount) >= min && Math.Abs(t.Amount) <= max)
                .ToList();
        }

        public List<Transaction> GetTransactionsByType(TransactionType type)
        {
            return _context.Transactions
                .Where(t => t.Type == type)
                .ToList();
        }

        public void ViewEditHistory(int transactionId)
        {
            var history = _context.TransactionEditHistories
                .Where(h => h.Id == transactionId)
                .ToList();
            // Отображение истории
        }
    }
}
