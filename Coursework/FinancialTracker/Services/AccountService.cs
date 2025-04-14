using FinancialTracker.Entities.Accounts;

namespace FinancialTracker.Services
{
    public class AccountService
    {
        private static int _accountId = 0;
        private static readonly List<Account> _accounts = new List<Account>();
        public PersonalAccount CreatePersonalAccount(string name, int userId)
        {
            var account = new PersonalAccount(
                id: ++_accountId,
                name: name,
                userId: userId
                );
            _accounts.Add(account);
            return account;
        }
        public List<PersonalAccount> GetPersonalAccount(int userId)
        {
            return _accounts.OfType<PersonalAccount>()
                .Where(a => a.UserId == userId)
                .ToList();
        }
        public bool UpdatePersonalAccountName(int accountId, string newName, int userId)
        {
            var account = _accounts.OfType<PersonalAccount>()
                .FirstOrDefault(a => a.Id == accountId && a.UserId == userId);

            if (account == null) return false;

            account.Name = newName;
            return true;
        }
        public bool DeletePersonalAccount(int accountId, int userId)
        {
            var account = _accounts.OfType<PersonalAccount>()
                .FirstOrDefault(a => a.Id == accountId && a.UserId == userId);

            if (account == null || account.Balance != 0) return false;

            _accounts.Remove(account);
            return true;
        }
        public SharedAccount CreateSharedAccount(string name, int creatorId)
        {
            var account = new SharedAccount(
                id: ++_accountId,
                name: name,
                creatorId: creatorId
                );
            _accounts.Add(account);
            return account;
        }
        public List<SharedAccount> GetSharedAccountsForUser(int userId)
        {
            return _accounts.OfType<SharedAccount>()
                .Where(a => a.CreatorUserId == userId && a.MemberUserIds.Contains(userId))
                .ToList();
        }
        public Account GetAccountById(int accountId)
        {
            return _accounts.FirstOrDefault(a => a.Id == accountId);
        }
    }
}
