using Microsoft.AspNetCore.Mvc;
using Sistema_inventario_mvc.DTOs;
using Sistema_inventario_mvc.Models;
using Sistema_inventario_mvc.Services.Interfaces;

namespace Sistema_inventario_mvc.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // Sin [Authorize] – abierto para pruebas
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/users
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            var dtos = users.Select(MapToResponseDto);
            return Ok(dtos);
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var user = _userService.GetById(id);
                if (user == null)
                    return NotFound(new { message = $"Usuario con ID {id} no encontrado." });
                return Ok(MapToResponseDto(user));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/users
        [HttpPost]
        public IActionResult Create([FromBody] CreateUserRequestDto dto)
        {
            if (!Enum.TryParse<Role>(dto.Role, true, out var role))
                return BadRequest(new { message = "Rol inválido. Use 'Administrador' o 'Empleado'." });

            try
            {
                var user = new User(
                    username: dto.Username,
                    passwordHash: dto.Password,   // el servicio lo hasheará
                    role: role
                );
                var created = _userService.Create(user);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, MapToResponseDto(created));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // PUT: api/users/{id}
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UpdateUserRequestDto dto)
        {
            if (!Enum.TryParse<Role>(dto.Role, true, out var role))
                return BadRequest(new { message = "Rol inválido. Use 'Administrador' o 'Empleado'." });

            try
            {
                var updatedUser = new User(
                    username: dto.Username,
                    passwordHash: dto.Password,   // si viene vacío, el servicio no lo cambiará
                    role: role
                );
                var result = _userService.Update(id, updatedUser);
                return Ok(MapToResponseDto(result));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _userService.Delete(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        private UserResponseDto MapToResponseDto(User user)
        {
            return new UserResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role.ToString()
            };
        }
    }
}