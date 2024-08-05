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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Models.Data;
using Models.ViewModels.MasterData;
using Models.ViewModels.Custom_action_result;
using Models.ActionResults;
using my_books.Data.ViewModels;
using Models.ViewModels;
using Azure.Core;
using Models.Migrations;
using Models.Data.RecipeData;

namespace BakeryApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public IUserRepository _iUserRepository;
        private IConfiguration _config;
        public IRepositoryBase<AddressVM> _iAddressRepository;
        public IRepositoryBase<MasterDataVM> _masterDataRepository;
        public UserController(IUserRepository _userRepository, IRepositoryBase<AddressVM> addressRepository,
            IConfiguration config, IRepositoryBase<MasterDataVM> masterDataRepository)
        {
            _iUserRepository = _userRepository;
            _config = config;
            _iAddressRepository = addressRepository;
            _masterDataRepository = masterDataRepository;

        }

        [HttpPost("register")]
        public CustomActionResult<AddResultVM> Register([FromBody] AdduserRequest userRequest)
        {

            try
            {
                MasterDataVM _masterData = _masterDataRepository.GetById(userRequest.Address.Country);
                if (_masterData != null)
                {
                    var address = new AddressVM
                    {
                        FullAddress = userRequest.Address.Street1 + " " + userRequest.Address.Street2 + " " + userRequest.Address.PostalCode + " " +
                    userRequest.Address.City + " " + _masterData.MasterDataName,
                        AddedDate = userRequest.AddedDate,
                        City = userRequest.Address.City,
                        Country = userRequest.Address.Country,
                        PostalCode = userRequest.Address.PostalCode,
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
                    var result = new AddResultVM
                    {
                        Id = userId
                    };
                    var responseObj = new CustomActionResultVM<AddResultVM>
                    {
                        Data = result
                    };
                    return new CustomActionResult<AddResultVM>(responseObj);
                }
                else
                {
                    // Handle the case where the user is not found
                    return new CustomActionResult<AddResultVM>(new CustomActionResultVM<AddResultVM>
                    {
                        Exception = new Exception("User can't add!")
                    });
                }

            }
            catch (Exception ex)
            {
                return new CustomActionResult<AddResultVM>(new CustomActionResultVM<AddResultVM>
                {
                    Exception = ex
                });
            }


        }

        [HttpPost("login")]
        public CustomActionResult<LoginResult> Login(LoginRequest request)
        {
            try
            {
                UserDetailVM user = _iUserRepository.Login(request);

                if (user != null)
                {
                    var role = user.Role == (int)RoleEnum.Admin ? "Admin" : "User";
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
                    var result = new LoginResult
                    {
                        Token = tokenString
                    };
                    var responseObj = new CustomActionResultVM<LoginResult>
                    {
                        Data = result
                    };
                    return new CustomActionResult<LoginResult>(responseObj);
                }
                else
                {
                    return new CustomActionResult<LoginResult>(new CustomActionResultVM<LoginResult>
                    {
                        Exception = new Exception($"User with email {request.Email} not found.")
                    });
                }
            }
            catch (Exception ex)
            {
                return new CustomActionResult<LoginResult>(new CustomActionResultVM<LoginResult>
                {
                    Exception = ex
                });
            }
        }



     
        [HttpPost("listAdvance")]
        public CustomActionResult<ResultView<PaginatedUsers>> GetAllUsers([FromBody] UserAdvanceListFilter userAdvanceListFilter)
        {
            try
            {
                PaginatedUsers _users = _iUserRepository.GetAll(userAdvanceListFilter);


                var result = new ResultView<PaginatedUsers>
                {
                    Item = _users

                };

                var responseObj = new CustomActionResultVM<ResultView<PaginatedUsers>>
                {
                    Data = result

                };
                return new CustomActionResult<ResultView<PaginatedUsers>>(responseObj);
            }
            catch (Exception ex)
            {
                var responseObj = new CustomActionResultVM<ResultView<PaginatedUsers>>
                {
                    Exception = ex
                };

                return new CustomActionResult<ResultView<PaginatedUsers>>(responseObj);
            }

        }
        [HttpGet("getUserById/{userId}")]
        public CustomActionResult<ResultView<UserDetailVM>> GetUserById(int userId)
        {
            try
            {
                UserDetailVM user = _iUserRepository.GetById(userId);
                if (user != null)
                {
                    var result = new ResultView<UserDetailVM>
                    {
                        Item = user

                    };

                    var responseObj = new CustomActionResultVM<ResultView<UserDetailVM>>
                    {
                        Data = result
                     
                    };
                    return new CustomActionResult<ResultView<UserDetailVM>>(responseObj);
                }
                else
                {
                    var result = new ResultView<UserDetailVM>
                    {
                        Exception = new Exception($"User with Id {userId} not found")
                    };

                    var responseObj = new CustomActionResultVM<ResultView<UserDetailVM>>
                    {
                        Exception = result.Exception
                    };

                    return new CustomActionResult<ResultView<UserDetailVM>>(responseObj);
                }
            }
            catch (Exception ex)
            {
                var responseObj = new CustomActionResultVM<ResultView<UserDetailVM>>
                {
                    Exception = ex
                };

                return new CustomActionResult<ResultView<UserDetailVM>>(responseObj);
            }
        }


        [HttpPut("updateUser/{userId}")]
        public CustomActionResult<AddResultVM> UpdateRecipe(int userId, [FromBody] UpdateUser updateUser)
        {

            try
            {

                UserDetailVM userVM = _iUserRepository.GetById(userId);
                if (userVM != null) {
                    MasterDataVM _masterData = _masterDataRepository.GetById(updateUser.Address.Country);
                    int addressId;
                    AddressVM cuurentAddress = _iAddressRepository.GetById(updateUser.AddressId);
                    if (cuurentAddress != null && _masterData != null)
                    {
                        AddressVM address = new AddressVM
                        {
                            FullAddress = updateUser.Address.Street1 + " " + updateUser.Address.Street2 + " " + updateUser.Address.PostalCode + " " +
                        updateUser.Address.City + " " + _masterData.MasterDataName,
                            Street1 = updateUser.Address.Street1,
                            Street2 = updateUser.Address.Street2,
                            City = updateUser.Address.City,
                            Country = updateUser.Address.Country,
                            PostalCode = updateUser.Address.PostalCode
                        };
                        addressId = _iAddressRepository.UpdateById(updateUser.AddressId, address);
                    }
                    else
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
                        Gender = updateUser.Gender,
                        Email = updateUser.Email,
                        ImageUrl = updateUser.ImageUrl,
                        AddressId = addressId
                    };
                    int updatedUserId = _iUserRepository.UpdateById(userId, user);
                    var result = new AddResultVM
                    {
                        Id = updatedUserId
                    };
                    var responseObj = new CustomActionResultVM<AddResultVM>
                    {
                        Data = result
                    };
                    return new CustomActionResult<AddResultVM>(responseObj);
                }
                else
                {
                    return new CustomActionResult<AddResultVM>(new CustomActionResultVM<AddResultVM>
                    {
                        Exception = new Exception($"User with Id {userId} not found.")
                    });
                }

            }
            catch (Exception ex)
            {
                return new CustomActionResult<AddResultVM>(new CustomActionResultVM<AddResultVM>
                {
                    Exception = ex
                });
            }

        }

    }
}
