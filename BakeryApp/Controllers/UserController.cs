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
using Models.ViewModels.Address;
using System.Data;
using Models.Filters;
using Models.Requests.Update_Requests;

namespace BakeryApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public IUserRepository _iUserRepository;
        private IConfiguration _config;
        public IRepositoryBase<AddressVM> _iAddressRepository;
        public UserController(IUserRepository _userRepository, IRepositoryBase<AddressVM> addressRepository,
            IConfiguration config)
        {
            _iUserRepository = _userRepository;
            _config = config;
            _iAddressRepository = addressRepository;

        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] AdduserRequest userRequest)
        {

            try
            {
                var address = new AddressVM
                {
                    FullAddress = userRequest.Address.Street1 + " " + userRequest.Address.Street2 + " " + userRequest.Address.PostalCode + " " + 
                    userRequest.Address.City + " " + userRequest.Address.Country,
                    AddedDate = userRequest.AddedDate,
                    City = userRequest.Address.City,
                    Country = userRequest.Address.Country,
                    PostalCode= userRequest.Address.PostalCode,
                    Street1 = userRequest.Address.Street1,
                    Street2 = userRequest.Address?.Street2,
                };
                int addressId = _iAddressRepository.Add(address);
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
                    AddressId = addressId
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
                    var role = "";
                    if (user.Role == 0)
                    {
                        role = "Admin";
                    }
                    else
                    {
                        role = "User";
                    }
                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(ClaimTypes.Role, role)
                    };

                    var token = new JwtSecurityToken(
                        issuer: _config["Jwt:Issuer"],
                        audience: _config["Jwt:Issuer"],
                        claims: claims,
                        expires: DateTime.Now.AddDays(1),
                        signingCredentials: credentials
                    );

                    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                    LoginResult result = new LoginResult();
                    result.Token =  tokenString;

                    return Ok(result);
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
        [HttpPost("listAdvance")]
        public IActionResult GetAllUsers([FromBody] UserAdvanceListFilter userAdvanceListFilter)
        {
            try
            {
                var _users = _iUserRepository.GetAll(userAdvanceListFilter);
                return Created(nameof(GetAllUsers), _users);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error adding Food item: {ex.Message}");
            }

        }

        [HttpGet("getUserById/{userId}")]
        public IActionResult GetUserById(int userId)
        {
            try
            {
                UserDetailVM user = _iUserRepository.GetById(userId);
                if (user != null)
                {
                    return Created(nameof(GetUserById), user);
                }
                else
                {
                    return NotFound($"User with ID {userId} not found.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving Raw material: {ex.Message}");
            }
        }

        [HttpPut("updateUser/{userId}")]
        public IActionResult UpdateRecipe(int userId, [FromBody] UpdateUser updateUser)
        {

            try
            {
                int addressId;
                AddressVM cuurentAddress = _iAddressRepository.GetById(updateUser.AddressId);
                if (cuurentAddress != null)
                {
                    AddressVM address = new AddressVM
                    {
                        FullAddress = updateUser.Address.Street1 + " " + updateUser.Address.Street2 + " " + updateUser.Address.PostalCode + " " +
                    updateUser.Address.City + " " + updateUser.Address.Country,
                        Street1 = updateUser.Address.Street1,
                        Street2 = updateUser.Address.Street2,
                        City = updateUser.Address.City,
                        Country = updateUser.Address.Country,
                        PostalCode = updateUser.Address.PostalCode
                    };
                     addressId = _iAddressRepository.UpdateById(updateUser.AddressId, address);
                } else
                {
                    var address = new AddressVM
                    {
                        FullAddress = updateUser.Address.Street1 + " " + updateUser.Address.Street2 + " " + updateUser.Address.PostalCode + " " +
                    updateUser.Address.City + " " + updateUser.Address.Country,
                        Street1 = updateUser.Address.Street1,
                        Street2 = updateUser.Address.Street2,
                        City = updateUser.Address.City,
                        Country = updateUser.Address.Country,
                        PostalCode = updateUser.Address.PostalCode
                    };
                    addressId = _iAddressRepository.Add(address);
                }
                

                UserDetailVM user = new UserDetailVM
                {
                    FirstName = updateUser.FirstName,
                    LastName = updateUser.LastName,
                    PhoneNumber = updateUser.PhoneNumber,
                    Role = updateUser.Role,
                    Gender =  updateUser.Gender,
                    Email =  updateUser.Email,
                    ImageUrl = updateUser.ImageUrl,
                    AddressId = addressId
                };
                int updatedRecipeId = _iUserRepository.UpdateById(userId, user);
                if (updatedRecipeId != -1)
                {
                    // Return a successful response
                    return Created(nameof(UpdateRecipe), updatedRecipeId);
                }
                else
                {
                    // Handle the case where the recipe is not found
                    return NotFound($"User with ID {userId} not found.");
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Error updating User: {ex.Message}");
            }

        }

    }
}
