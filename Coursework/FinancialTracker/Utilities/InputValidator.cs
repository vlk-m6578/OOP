using System.Text.RegularExpressions;
using FinancialTracker.Data;
using FinancialTracker.Entities;

namespace FinancialTracker.Utilities
{
    public static class InputValidator
    {
        static AppDbContext _context = new AppDbContext();
        public static int GetIntInput(int a, int b)
        {
            int output;
            string s;
            while (true)
            {
                s = Console.ReadLine();
                if (int.TryParse(s, out output) && output >= a && output <= b)
                {
                    return output;
                }
                else
                {
                    Console.Write("Incorrect input. Try again: ");
                }
            }
        }
        public static string GetValidUsername()
        {
            string username;
            var regex = new Regex(@"^[a-zA-Z0-9_]{6,20}$");

            while (true)
            {
                Console.Write("Enter username (min 6 chars): ");
                username = Console.ReadLine()?.Trim();

                if(string.IsNullOrEmpty(username) || !regex.IsMatch(username))
                {
                    Console.WriteLine(" !!!!!> Invalid username. Must be at least 6 characters (letters, numbers, '_'). ");
                    continue;
                }

                if(_context.Users.Any(u => u.Username == username))
                {
                    Console.WriteLine(" -----> Username already exists.");
                    continue;
                }
                return username;
            }
        }
        public static string GetValidEmail()
        {
            string email;
            var regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");

            while (true)
            {
                Console.Write("Enter email: ");
                email = Console.ReadLine()?.Trim();

                if (!regex.IsMatch(email))
                    Console.WriteLine(" !!!!!> Invalid email format.");
                else if (_context.Users.Any(u => u.Email == email))
                    Console.WriteLine(" -----> Email already exists.");
                else
                    return email;
            }
        }
        public static string GetValidPassword()
        {
            string password;

            while (true)
            {
                Console.Write("Enter password: ");
                password = Console.ReadLine();

                if(string.IsNullOrEmpty(password) || password.Length < 8)
                {
                    Console.WriteLine(" !!!!!> Password must be at least 8 characters.");
                    continue;
                }
                if(!password.Any(char.IsDigit) || !password.Any(char.IsLetter))
                {
                    Console.WriteLine(" !!!!!> Password must contain letters and numbers.");
                    continue;
                }
                return password;
            }
        }
        public static decimal GetDecimalInput(string prompt, decimal min, decimal max)
        {
            while (true)
            {
                Console.Write(prompt);
                if (decimal.TryParse(Console.ReadLine(), out decimal result) && result >= min && result <= max)
                {
                    return result;
                }
                Console.WriteLine($"Invalid input. Enter value between {min} and {max}");
            }
        }

        public static DateTime GetDateInput(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (DateTime.TryParse(Console.ReadLine(), out DateTime result))
                {
                    return result;
                }
                Console.WriteLine("Invalid date format. Use yyyy-MM-dd");
            }
        }
    }
}
