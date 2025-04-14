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
    }
}
