using Models.Data.RawMaterialData;
using Models.Data.RecipeData;
using Models.Data;
using Models.Filters;
using Models.Helpers;


using Models.Data;
using Models.ViewModels.FoodType;
using Models.ViewModels.Recipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Data.RecipeData;
using Models.Data.RawMaterialData;
using Models.ViewModels.RawMaterial;
using Models.Filters;
using Models.Helpers;
using Microsoft.EntityFrameworkCore;
using Models.Requests;
using Models.Data.User;
namespace Repositories.UserRepository

{
    using BCrypt.Net;
    using Models.ViewModels.User;

    public interface IUserRepository
    {
        int Register(UserVM model);
        UserDetailVM Login(LoginRequest loginRequest);
    }
    public class UserRepository: IUserRepository
    {
        private AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public int Register(UserVM user)
        {
            string newUserCode = Guid.NewGuid().ToString();
            string passwordHash = BCrypt.HashPassword(user.Password);

            Users _user = new Users()
            {
                UserCode = newUserCode,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role,
                HashPassword = passwordHash,
                AddedDate = user.AddedDate,
                ImageUrl = user.ImageUrl

            };

            _context.Users.Add(_user);
            object value = _context.SaveChanges();
        
            int userId = _user.Id;
            return userId;
        }
        public UserDetailVM Login(LoginRequest request)
        {
            // get account from database
            Users user = _context.Users.SingleOrDefault(x => x.Email == request.Email);

            // check account found and verify password
            if (user != null && BCrypt.Verify(request.Password, user.HashPassword))
            {
                var userDetail = new UserDetailVM
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Address = user.Address,
                    AddedDate = user.AddedDate,
                    PhoneNumber = user.PhoneNumber,
                    ImageUrl =  user.ImageUrl,
                    Role = user.Role
                };

                return userDetail;
            } else
            {
                return null;
            }
          
        }
    }
}
