using FinancialTracker.Data;

namespace FinancialTracker.Domain.Services
{
    public class PasswordRecoveryService
    {

        private const int CodeLength = 0;
        private const int CodeTimeMinutes = 5;

        private readonly AppDbContext _context;
        public PasswordRecoveryService(AppDbContext context)
        {
            _context = context;
        }

        public bool ValidateEmail(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public string GenerateRecoveryPassword(string email)
        {
            var code = new Random().Next(100000, 999999).ToString();
            _context.RecoveryCodes.Add(new RecoveryCode
            {
                Email = email,
                Code = code,
                ExpiresAt = DateTime.Now.AddMinutes(15)
            });
            _context.SaveChanges();
            return code;
        }
        public bool ValidateCode(string email, string code)
        {
            return _context.RecoveryCodes.Any(rc =>
            rc.Email == email &&
            rc.Code == code &&
            rc.ExpiresAt > DateTime.Now
            );
        }
    }

    public class RecoveryCode
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Code { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
