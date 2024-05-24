using Microsoft.AspNetCore.Mvc;
using Models.ViewModels.RawMaterial;
using Models.ViewModels.Recipe;
using Repositories.RecipeRepository;
using Repositories;
using Repositories.UserRepository;
using Models.Requests;
using Models.ViewModels.User;
using Models.Data.User;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;

namespace BakeryApp.Controllers
{
    public class UserController : ControllerBase
    {
        public IUserRepository _iUserRepository;
        private IConfiguration _config;
        public UserController(IUserRepository _userRepository,
            IConfiguration config)
        {
            _iUserRepository = _userRepository;
            _config = config;

        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] AdduserRequest userRequest)
        {

            try
            {


                // add new recipe
                var user = new UserVM
                {
                    FirstName = userRequest.FirstName,
                    LastName = userRequest.LastName,
                    Email = userRequest.Email,
                    Password = userRequest.Password,
                    AddedDate = userRequest.AddedDate,
                    PhoneNumber = userRequest.PhoneNumber,
                    Role = userRequest.Role,
                    ImageUrl = userRequest.ImageUrl,
                    Address =  userRequest.Address

                };
                int userId = _iUserRepository.Register(user);
                return Created(nameof(Register), userId);

            }
            catch (Exception ex)
            {
                return BadRequest($"Error adding user: {ex.Message}");
            }


        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            try
            {
                // Call the repository to get the recipe by ID
                UserDetailVM user = _iUserRepository.Login(request);

                if (user != null)
                {
                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(ClaimTypes.Role, user.Role)
                    };

                    var token = new JwtSecurityToken(
                        issuer: _config["Jwt:Issuer"],
                        audience: _config["Jwt:Issuer"],
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(120),
                        signingCredentials: credentials
                    );

                    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                    return Ok(tokenString);
                }
                {
                    // Handle the case where the recipe is not found
                    return NotFound($"Recipe with ID {request.Email} not found.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving user: {ex.Message}");
            }
        }


    }
}
