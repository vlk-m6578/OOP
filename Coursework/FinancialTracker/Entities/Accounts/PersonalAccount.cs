
namespace FinancialTracker.Entities.Accounts
{
    public class PersonalAccount : Account
    {
        public int UserId { get; }
        public PersonalAccount(int id, string name, int userId) : base(id, name)
        {
            UserId = userId;
        }

        public override void ApplyTransaction(Transaction transaction)
        {

        }
    }
}
