
namespace FinancialTracker.Interfaces
{
    public interface IAuthService
    {
        public bool Register(string password);
        public bool Login(string password);
        public void ResetPassword(string newPassword);
        public void ActivateAccount();
        public void DeactivateAccount();
    }
}
