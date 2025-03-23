using System;
using System.Collections.Generic;

namespace FinancialTracker.Services
{
    public class PasswordRecoveryService
    {
        private static readonly Dictionary<string, (string Code, DateTime Time)> _recoveryCodes = new();
        private const int CodeLength = 0;
        private const int CodeTimeMinutes = 5;

        public string GenerateRecoveryPassword(string email)
        {
            var random = new Random();
            var code = random.Next(100000, 999999).ToString();
            _recoveryCodes[email] = (code, DateTime.Now.AddMinutes(CodeTimeMinutes));

            return code;
        }
        public bool ValidateCode(string email, string code)
        {
            if(!_recoveryCodes.TryGetValue(email, out var codeAndTime)) return false;
            if(DateTime.Now > codeAndTime.Time) return false;
            return codeAndTime.Code == code;
        }
    }
}
