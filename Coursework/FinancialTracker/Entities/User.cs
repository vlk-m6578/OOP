using System.ComponentModel.DataAnnotations;
using FinancialTracker.Entities.Accounts;
using FinancialTracker.Interfaces;
using FinancialTracker.Utilities;

namespace FinancialTracker.Entities
{
    public class User : IAuthService
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Username { get; private set; }

        [Required]
        [EmailAddress]
        public string Email { get; private set; }

        [Required]
        public string PasswordHash { get; private set; }
        public bool IsActive { get; private set; }


        private User() { }
        public User(string username, string email)
        {
            Username = username;
            Email = email;
            IsActive = true;
        }
        public bool IsCreatorOf(SharedAccount account) => account.CreatorUserId == this.Id;
        public bool Register(string password)
        {
            PasswordHash = PasswordHasher.Hash(password);
            return true;
        }

        public bool Login(string password)
        {
            if (!IsActive) return false;
            return PasswordHasher.Verify(password, PasswordHash);
        }
        public void ResetPassword(string newPassword)
        {
            PasswordHash = PasswordHasher.Hash(newPassword);
        }
        public void ActivateAccount() => IsActive = true;
        public void DeactivateAccount() => IsActive = false;

        public void SetUsername(string newUsername)
        {
            if (string.IsNullOrWhiteSpace(newUsername))
                throw new ArgumentException("Username can't be empty");
            Username = newUsername;
        }

        public void SetEmail(string newEmail)
        {
            if (string.IsNullOrWhiteSpace(newEmail) || !new EmailAddressAttribute().IsValid(newEmail))
                throw new ArgumentException("Invalid email format");
            Email = newEmail;
        }
    }
}
