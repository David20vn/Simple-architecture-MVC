namespace Sistema_inventario_mvc.DTOs
{
    public class UpdateUserRequestDto
    {
        public string Username { get; set; }
        public string Password { get; set; }  // opcional, solo si se desea cambiar
        public string Role { get; set; }
    }
}