using FinancialTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using FinancialTracker.Data;
using FinancialTracker.Domain.Entities;
using FinancialTracker.Domain.Entities.Accounts;
using FinancialTracker.Domain.Interfaces;

namespace FinancialTracker.Domain.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly AppDbContext _context;
        private readonly IBudgetService _budgetService;

        public TransactionService(AppDbContext context, IBudgetService budgetService)
        {
            _context = context;
            _budgetService = budgetService;
        }
        public Transaction AddTransaction(decimal amount, int categoryId, int accountId, int userId,
                         string description, TransactionType type)
        {
            var account = _context.Accounts
                .FirstOrDefault(a => a.Id == accountId);

            if (account == null) throw new Exception("Account not found");

            if (account is SharedAccount shared)
            {
                if (!shared.MemberUserIdsList.Contains(userId))
                    throw new Exception("You are not a member of this shared account");
            }
            else if (account is PersonalAccount personal && personal.UserId != userId)
            {
                throw new Exception("No access to this account");
            }

            var transaction = new Transaction(
                amount: type == TransactionType.Income ? Math.Abs(amount) : -Math.Abs(amount),
                categoryId: categoryId,
                accountId: accountId,
                userId: userId,
                description: description,
                type: type
            );

            account.Balance += transaction.Amount;
            _context.Transactions.Add(transaction);
            _context.SaveChanges();


            if (type == TransactionType.Expense)
            {
                _budgetService.UpdateSpending(
                    transaction.CreatedByUserId,
                    transaction.CategoryId,
                    Math.Abs(transaction.Amount)
                );
            }

            return transaction;
        }
        public bool ValidateBalance(int accountId, decimal amount)
        {
            var account = _context.Accounts.Find(accountId);
            return account?.Balance + amount >= 0;
        }

        public void UpdateTransaction(int transactionId, decimal newAmount, int newCategoryId, string newDescription, int editorId)
        {
            using (var freshContext = new AppDbContext())
            {
                var transaction = freshContext.Database.BeginTransaction();
                try
                {
                    var existing = freshContext.Transactions
                        .AsNoTracking()
                        .FirstOrDefault(t => t.Id == transactionId);

                    if (existing == null) throw new Exception("Transaction not found");

                    var updatedTransaction = new Transaction
                    {
                        Id = existing.Id,
                        Amount = existing.Type == TransactionType.Income ? newAmount : -newAmount,
                        CategoryId = newCategoryId,
                        Description = newDescription,
                        Date = existing.Date,
                        Type = existing.Type,
                        AccountId = existing.AccountId,
                        CreatedByUserId = existing.CreatedByUserId,
                        IsDeleted = existing.IsDeleted
                    };

                    var account = freshContext.Accounts.Find(existing.AccountId);
                    if (account is SharedAccount shared && !shared.MemberUserIdsList.Contains(editorId))
                        throw new Exception("No permission to edit");
                    account.Balance += updatedTransaction.Amount - existing.Amount;

                    freshContext.TransactionEditHistories.Add(new TransactionEditHistory
                    {
                        TransactionId = existing.Id,
                        EditedByUserId = editorId,
                        OldAmount = existing.Amount,
                        NewAmount = updatedTransaction.Amount,
                        OldCategoryId = existing.CategoryId,
                        NewCategoryId = newCategoryId,
                        OldDescription = existing.Description,
                        NewDescription = newDescription,
                        EditedAt = DateTime.Now
                    });

                    if (existing.Type == TransactionType.Expense)
                    {
                        _budgetService.UpdateSpending(
                        existing.CreatedByUserId,
                        existing.CategoryId,
                        -Math.Abs(existing.Amount)
                        );

                        _budgetService.UpdateSpending(
                        existing.CreatedByUserId,
                        newCategoryId,
                        Math.Abs(newAmount)
                        );
                    }

                    freshContext.Entry(updatedTransaction).State = EntityState.Modified;
                    freshContext.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception($"Update failed: {ex.Message}");
                }
            }
        }

        public void DeleteTransaction(int transactionId, int userId)
        {
            using var dbTransaction = _context.Database.BeginTransaction();
            try
            {
                var existing = _context.Transactions
                    .Include(t => t.Account)
                    .FirstOrDefault(t => t.Id == transactionId);

                if (existing == null || existing.IsDeleted)
                    throw new Exception("Transaction not found");

                if (existing.Account is SharedAccount shared &&
                    !shared.MemberUserIdsList.Contains(userId))
                {
                    throw new Exception("No permission to delete this transaction");
                }

                if (existing.Type == TransactionType.Income)
                {
                    existing.Account.Balance -= existing.Amount;
                }
                else
                {
                    existing.Account.Balance += Math.Abs(existing.Amount);
                }

                if (existing.Type == TransactionType.Expense)
                {
                    _budgetService.UpdateSpending(
                        existing.CreatedByUserId,
                        existing.CategoryId,
                        -Math.Abs(existing.Amount)
                    );
                }

                existing.IsDeleted = true;
                existing.Description = $"[DELETED] {existing.Description}";

                _context.SaveChanges();
                dbTransaction.Commit();
            }
            catch (Exception ex)
            {
                dbTransaction.Rollback();
                throw new Exception($"Delete failed: {ex.Message}");
            }
        }
        public List<Transaction> GetTransactionsByAccount(int accountId)
        {
            var account = _context.Accounts
                .Include(a => a.Transactions)
                .FirstOrDefault(a => a.Id == accountId);

            if (account is SharedAccount)
            {
                return _context.Transactions
                    .Include(t => t.Category)
                    .Include(t => t.Account)
                    .Where(t => t.AccountId == accountId)
                    .ToList();
            }
            else
            {
                return _context.Transactions
                    .Include(t => t.Category)
                    .Include(t => t.Account)
                    .Where(t => t.AccountId == accountId)
                    .ToList();
            }
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
                .Include(t => t.Category)
                .Where(t => t.CreatedByUserId == userId && !t.IsDeleted)
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
        }

    }
}
