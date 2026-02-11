using Domain.Enums;

namespace Application
{
    public class LoginService
    {
        private const string AdminLogin = "admin";
        private const string AdminPassword = "1234";

        public bool TryLogin(string login, string password, UserContext context)
        {
            if (login == AdminLogin && password == AdminPassword)
            {
                context.SetRole(UserRole.Admin);
                return true;
            }

            return false;
        }

        public void Logout(UserContext context)
        {
            context.SetRole(UserRole.User);
        }
    }
}