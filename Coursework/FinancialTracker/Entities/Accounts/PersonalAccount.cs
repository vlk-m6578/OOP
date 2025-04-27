
namespace FinancialTracker.Entities.Accounts
{
    public class PersonalAccount : Account
    {
        public int UserId { get; set; }

        private PersonalAccount() { }
        public PersonalAccount(int id, string name, int userId) : base(id, name)
        {
            UserId = userId;
        }

        public PersonalAccount(string name, int userId) : base(name)
        {
            UserId = userId;
        }
    }
}
