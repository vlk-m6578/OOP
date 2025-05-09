namespace FinancialTracker.Domain.Entities.Accounts
{
    public class PersonalAccount : Account
    {
        public int UserId { get; set; }

        private PersonalAccount() { }

        public PersonalAccount(string name, int userId) : base(name)
        {
            UserId = userId;
        }
    }
}
