using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProductInventorySystem.Models;
using ProductInventorySystem.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Text;


namespace ProductInventorySystem.Controllers
{
    // Controller for managing Login operations 

    [Route("api/[controller]")]
    [ApiController]
    public class LoginsController : ControllerBase
    {
        // Get Configuration from Appsetting - SecretKey
        private IConfiguration _configuration;

        //Login Repository
        private readonly ILoginRepository _loginRepository;

        // Dependency Injection
        public LoginsController(IConfiguration configuration, ILoginRepository loginRepository)
        {
            _configuration = configuration;
            _loginRepository = loginRepository;
        }



        // Valid Credentials -- UserName and Password
        [HttpGet("{username}/{password}")] // Query String
        public async Task<IActionResult> Login(string username, string password)

        {
            // variable for tracking unauthorised
            IActionResult response = Unauthorized();
            User user = null;

            // 1- Authenticate the user by passing username and pssword
            user = await _loginRepository.ValidateUser(username, password);

            // 2-Generate JWT Token
            if (user != null)
            {
                // Custom Method for generate Token
                var tokenString = GenerateJWTToken(user);

                response = Ok(new
                {
                    username = user.UserName,
                    password = user.Password,
                    token = tokenString

                });
            }
            return response;
        }


        // GenerateJWTToken - Custom for set time
        private string GenerateJWTToken(User user)
        {
            // 1- Secret Security key
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            // 2- algorithm
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

            // 3- JWT 
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"], null, expires: DateTime.Now.AddMinutes(20),
                signingCredentials: credentials);

            // 4- Writing Token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
