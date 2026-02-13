using FurnitureWarehouse.Domain.Enums;

namespace Application
{
    public class LoginService
    {
        private const string AdminLogin = "admin";
        private const string AdminPassword = "1234";

        private int _failedAttempts = 0;
        private DateTime? _lockUntil = null;

        public bool TryLogin(string login, string password, UserContext context, out string message)
        {
            message = string.Empty;

            if (_lockUntil.HasValue && DateTime.Now < _lockUntil.Value)
            {
                var remaining = (_lockUntil.Value - DateTime.Now).Minutes;
                message = $"Too many failed attempts. Try again in {remaining} minute(s).";
                return false;
            }

            if (login == AdminLogin && password == AdminPassword)
            {
                context.SetRole(UserRole.Admin);
                _failedAttempts = 0;
                _lockUntil = null;
                message = "Login successful. Admin mode enabled.";
                return true;
            }

            _failedAttempts++;

            if (_failedAttempts >= 3)
            {
                _lockUntil = DateTime.Now.AddMinutes(5);
                _failedAttempts = 0;
                message = "Too many failed attempts. Admin login locked for 5 minutes.";
                return false;
            }

            message = $"Invalid credentials. Attempts left: {3 - _failedAttempts}";
            return false;
        }

        public void Logout(UserContext context)
        {
            context.SetRole(UserRole.User);
        }
    }
}