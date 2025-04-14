
namespace FinancialTracker.Entities
{
    public class AccountHistoryEntry
    {
        public DateTime Timestamp { get; }
        public int UserId { get; }
        public string Action {  get; }
        public string Details { get; }
        public AccountHistoryEntry(int userId, string action, string details)
        {
            Timestamp = DateTime.Now;
            UserId = userId;
            Action = action;
            Details = details;
        }
    }
}
