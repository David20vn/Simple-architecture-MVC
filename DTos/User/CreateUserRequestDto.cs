namespace Sistema_inventario_mvc.DTOs
{
    public class CreateUserRequestDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}