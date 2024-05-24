using Microsoft.AspNetCore.Mvc;
using Models.ViewModels.RawMaterial;
using Models.ViewModels.Recipe;
using Repositories.RecipeRepository;
using Repositories;
using Repositories.UserRepository;
using Models.Requests;
using Models.ViewModels.User;
using Models.Data.User;

namespace BakeryApp.Controllers
{
    public class UserController : ControllerBase
    {
        public IUserRepository _iUserRepository;
        public UserController(IUserRepository _userRepository)
        {
            _iUserRepository = _userRepository;
            
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

        [HttpGet("login")]
        public IActionResult Login(LoginRequest request)
        {
            try
            {
                // Call the repository to get the recipe by ID
                UserDetailVM user = _iUserRepository.Login(request);

                if (user != null)
                {
                    return Created(nameof(Login), user);
                }
                else
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
