using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Sistema_inventario_mvc.Models;
using Sistema_inventario_mvc.Services.Interfaces;

namespace Sistema_inventario_mvc.Services.Implementations
{
    public class JwtService : IJwtService
    {
        private readonly string _key;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _expireMinutes;

        public JwtService(IConfiguration configuration)
        {
            _key = configuration["Jwt:Key"];
            _issuer = configuration["Jwt:Issuer"];
            _audience = configuration["Jwt:Audience"];
            _expireMinutes = int.Parse(configuration["Jwt:ExpireMinutes"]);
        }

        public string GenerateToken(User user)
        {
            // 1. Crear claims con id, nombre y rol
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),          // ← nombre de usuario
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            // 2. Configurar la clave secreta simétrica
            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_key));

            // 3. Credenciales de firma
            var credentials = new SigningCredentials(
                securityKey, SecurityAlgorithms.HmacSha256);

            // 4. Crear el token
            var tokenDescriptor = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_expireMinutes),
                signingCredentials: credentials
            );

            // 5. Serializar a string y devolver
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}