using Microsoft.AspNetCore.Mvc;

using Sistema_inventario_mvc.DTOs;
using Sistema_inventario_mvc.Repositories.Interfaces;    // Para IUserRepository
using Sistema_inventario_mvc.Services.Interfaces;    // Para IJwtService
using Sistema_inventario_mvc.Helpers;         // Para PasswordHelper

namespace Sistema_inventario_mvc.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        public AuthController(IUserRepository userRepository, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // 1. Buscar usuario por nombre de usuario
            var user = _userRepository.GetByUsername(request.Username);
            if (user == null){
                return Unauthorized(new { message = "Usuario o contraseña inválidos" });
            }
                

            // 2. Verificar contraseña
            if (!PasswordHelper.VerifyPassword(request.Password, user.PasswordHash))
                return Unauthorized(new { message = "Usuario o contraseña inválidos" });

            // 3. Generar JWT
            var token = _jwtService.GenerateToken(user);

            // 4. Devolver el token
            return Ok(new { token });
        }
    }
}