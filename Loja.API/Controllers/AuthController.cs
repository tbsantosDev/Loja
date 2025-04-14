using Loja.Application.DTOs.UserDTOs;
using Loja.Application.Interfaces;
using Loja.Domain.Models;
using Loja.Infra.Data.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Loja.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IUserInterface _userInterface;

        public AuthController(AppDbContext context, IUserInterface userInterface)
        {
            _context = context;
            _userInterface = userInterface;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            ResponseModel<LoginModel> response = new ResponseModel<LoginModel>();

            try
            {
                var user = _context.Users.SingleOrDefault(u => u.Email == model.Email);
                // Aqui, você verificaria as credenciais do usuário
                if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
                {
                    response.Message = "Usuário não encontrado ou senha incorreta!";
                    return Unauthorized(); // Retorna 401 se o usuário não for encontrado ou se a senha estiver incorreta
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtSecretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY");
                var key = Encoding.ASCII.GetBytes(jwtSecretKey);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, model.Email),
                new Claim(ClaimTypes.Role, user.UserType.ToString()),
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return Ok(new { Token = tokenString });

            }
            catch (Exception ex)
            {
                response.Message = ex.InnerException?.Message ?? ex.Message;
                response.Status = false;
                return BadRequest(response);
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<ResponseModel<UserModel>>> CreateUser(CreateUserDto createUserDto)
        {
            var users = await _userInterface.CreateUser(createUserDto);
            return Ok(users);
        }

        [HttpPatch("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string token)
        {

            var user = await _context.Users.SingleOrDefaultAsync(u => u.EmailConfirmationToken == token);
            if (user == null)
            {
                return BadRequest("Token inválido.");
            }

            user.EmailConfirmed = true;
            user.EmailConfirmationToken = null;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok("E-mail confirmado com sucesso!");
        }
    } 
}

