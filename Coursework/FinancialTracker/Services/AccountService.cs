using FinancialTracker.Data;
using FinancialTracker.Entities;
using FinancialTracker.Entities.Accounts;
using Microsoft.EntityFrameworkCore;

namespace FinancialTracker.Services
{
    public class AccountService
    {

        private readonly AppDbContext _context;

        public AccountService(AppDbContext context)
        {
            _context = context;
        }
        public PersonalAccount CreatePersonalAccount(string name, int userId)
        {
            var account = new PersonalAccount(name, userId);
            _context.Accounts.Add(account);
            _context.SaveChanges();

            return account;
        }

        public List<PersonalAccount> GetPersonalAccounts(int userId)
        {
            return _context.Accounts
                .OfType<PersonalAccount>()
                .Where(a => a.UserId == userId)
                .ToList();
        }
        public bool UpdatePersonalAccountName(int accountId, string newName, int userId)
        {
            var account = _context.Accounts
                .OfType<PersonalAccount>()
                .FirstOrDefault(a => a.Id == accountId && a.UserId == userId);

            if (account == null) return false;

            account.Name = newName;
            _context.SaveChanges(); // Важно сохранить изменения

            return true;
        }
        public bool DeletePersonalAccount(int accountId, int userId)
        {
            var account = _context.Accounts
                .OfType<PersonalAccount>()
                .FirstOrDefault(a => a.Id == accountId && a.UserId == userId);

            if (account == null || account.Balance != 0) return false;

            _context.Accounts.Remove(account);
            _context.SaveChanges();
            return true;
        }
        public SharedAccount CreateSharedAccount(string name, int creatorId)
        {
            var account = new SharedAccount(name, creatorId);
            _context.Accounts.Add(account);
            _context.SaveChanges();
            return account;
        }
        public List<Transaction> GetTransactionsByUser(int userId)
        {
            return _context.Transactions
                .Include(t => t.Category)
                .Include(t => t.Account) // Добавляем загрузку счета
                .Where(t => t.CreatedByUserId == userId && !t.IsDeleted)
                .ToList();
        }

        public List<SharedAccount> GetSharedAccountsForUser(int userId)
        {
            return _context.Accounts
                .OfType<SharedAccount>()
                .AsEnumerable() // Фильтрация в памяти
                .Where(a => a.MemberUserIdsList.Contains(userId))
                .ToList();
        }
        public Account GetAccountById(int accountId)
        {
            return _context.Accounts.FirstOrDefault(a => a.Id == accountId);
        }
        public void UpdateAccountBalance(int accountId, decimal amount)
        {
            var account = _context.Accounts.Find(accountId);
            account.Balance += amount;
            _context.SaveChanges();
        }
        public bool UserHasAccounts(int userId)
        {
            var personal = GetPersonalAccounts(userId).Count;
            var shared = GetSharedAccountsForUser(userId).Count;
            return personal + shared > 0;
        }
    }
}
