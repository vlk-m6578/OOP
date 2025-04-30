using FinancialTracker.Interfaces;
using FinancialTracker.Entities;
using Microsoft.EntityFrameworkCore;
using FinancialTracker.Data;
using FinancialTracker.Entities.Accounts;

namespace FinancialTracker.Services
{
    public class TransactionService : ITransactionService
    {
        public virtual ICollection<Transaction> Transactions { get; set; }
        private readonly AppDbContext _context;

        public TransactionService(AppDbContext context)
        {
            _context = context;
        }
        public Transaction AddTransaction(decimal amount, int categoryId, int accountId, int userId,
                         string description, TransactionType type)
        {
            var account = _context.Accounts
                .FirstOrDefault(a => a.Id == accountId);

            if (account == null) throw new Exception("Account not found");

            // Для SharedAccount проверяем членство
            if (account is SharedAccount shared)
            {
                if (!shared.MemberUserIdsList.Contains(userId))
                    throw new Exception("You are not a member of this shared account");
            }
            else if (account is PersonalAccount personal && personal.UserId != userId)
            {
                throw new Exception("No access to this account");
            }

            // Создание транзакции
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

            // Создаем новый объект для обновления
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

                    // Обновляем баланс счета
                    var account = freshContext.Accounts.Find(existing.AccountId);
                    if (account is SharedAccount shared && !shared.MemberUserIdsList.Contains(editorId))
                        throw new Exception("No permission to edit");
                    account.Balance += (updatedTransaction.Amount - existing.Amount);

            // Добавляем запись в историю
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

            // Обновляем транзакцию
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
            using var freshContext = new AppDbContext();
            using var transaction = freshContext.Database.BeginTransaction();

            try
            {
                // Получаем транзакцию без отслеживания
                var existing = freshContext.Transactions
                    .AsNoTracking()
                    .FirstOrDefault(t => t.Id == transactionId);

                if (existing == null || existing.CreatedByUserId != userId)
                    throw new Exception("Транзакция не найдена или нет прав доступа");

                // Создаем новый объект для удаления
                var transactionToDelete = new Transaction { Id = existing.Id };
                freshContext.Transactions.Attach(transactionToDelete);
                freshContext.Transactions.Remove(transactionToDelete);

                // Корректируем баланс
                var account = freshContext.Accounts.Find(existing.AccountId);
                account.Balance -= existing.Amount;

                freshContext.SaveChanges();
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
        public List<Transaction> GetTransactionsByAccount(int accountId)
        {
            var account = _context.Accounts
                .Include(a => a.Transactions)
                .FirstOrDefault(a => a.Id == accountId);

            if (account is SharedAccount)
            {
                // Для общего счета берем все транзакции
                return _context.Transactions
                    .Include(t => t.Category)
                    .Include(t => t.Account)
                    .Where(t => t.AccountId == accountId)
                    .ToList();
            }
            else
            {
                // Для личного счета только текущего пользователя
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
            // Отображение истории
        }

    }
}
