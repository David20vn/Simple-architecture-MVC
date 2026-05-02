namespace Sistema_inventario_mvc.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string PasswordHash { get; set; }

        public Role Role { get; set; }

        public User(){}
        public User(string username, string passwordHash, Role role)
        {
            Username = username;
            PasswordHash = passwordHash;
            Role = role;
        }
    }
}