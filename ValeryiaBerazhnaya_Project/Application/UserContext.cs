using FurnitureWarehouse.Domain.Enums;

namespace Application
{
    public class UserContext
    {
        public UserRole Role { get; private set; }

        public bool IsAdmin => Role == UserRole.Admin;

        public UserContext()
        {
            Role = UserRole.User;
        }

        public void SetRole(UserRole role)
        {
            Role = role;
        }
    }
}