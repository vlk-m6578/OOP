using FinancialTracker.Domain.Entities.Accounts;

namespace FinancialTracker.Domain.Interfaces
{
    public interface IAuthService
    {
        public bool Register(string password);
        public bool Login(string password);
        public void ResetPassword(string newPassword);
        public void ActivateAccount();
        public void DeactivateAccount();
        void SetUsername(string newUsername);
        void SetEmail(string newEmail);
        bool IsCreatorOf(SharedAccount account);
    }
}
