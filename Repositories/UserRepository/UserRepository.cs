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
    using Azure.Core;
    using BCrypt.Net;
    using Models.Requests.Update_Requests;
    using Models.ViewModels.Address;
    using Models.ViewModels.User;

    public interface IUserRepository
    {
        public int Register(UserVM model);
        public UserDetailVM Login(LoginRequest loginRequest);
        public PaginatedUsers GetAll(UserAdvanceListFilter filter);

        public UserDetailVM GetById(int userId);
        public int UpdateById(int userId, UserDetailVM user);
    }
    public class UserRepository : IUserRepository
    {
        private AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public PaginatedUsers GetAll(UserAdvanceListFilter filter)
        {
            IQueryable<Users> query = _context.Users
         .Where(fi => !fi.IsDeleted);

            // Apply filtering
            if (!string.IsNullOrEmpty(filter.SearchString))
            {
                query = query.Where(fi =>
                    fi.FirstName.Contains(filter.SearchString) || fi.LastName.Contains(filter.SearchString)
                    || fi.UserCode.Contains(filter.SearchString)
                );
            }

            if (filter.Gender.HasValue)
            {
                query = query.Where(fi => fi.Gender == filter.Gender);
            }

            if (filter.Role.HasValue)
            {
                query = query.Where(fi => fi.Role == filter.Role);
            }

            if (!string.IsNullOrEmpty(filter.PhoneNumber))
            {
                query = query.Where(fi => fi.PhoneNumber == filter.PhoneNumber);
            }

            if (!string.IsNullOrEmpty(filter.Email))
            {
                query = query.Where(fi => fi.Email == filter.Email);
            }

            if (!string.IsNullOrEmpty(filter.AddedDate) && DateTime.TryParse(filter.AddedDate, out DateTime filterDate))
            {
                // Adjust date filter to consider the whole day
                DateTime nextDay = filterDate.AddDays(1);
                query = query.Where(fi => fi.AddedDate >= filterDate && fi.AddedDate < nextDay);
            }

            // Get total count before pagination
            int totalCount = query.Count();

            // Apply sorting
            query = SortHelper.ApplySorting(query.AsQueryable(), filter.SortBy, filter.IsAscending);

            // Apply pagination
            query = query.Skip((filter.Pagination.PageIndex - 1) * filter.Pagination.PageSize)
                         .Take(filter.Pagination.PageSize);

            // Project and materialize the results

            var paginatedResult = query
          .Select(fi => new AllUserVM
          {
              Id = fi.Id,
              UserCode = fi.UserCode,
              FirstName = fi.FirstName,
              AddedDate = fi.AddedDate,
              LastName = fi.LastName,
              PhoneNumber = fi.PhoneNumber,
              Role = fi.Role,
              Email = fi.Email,
              ImageUrl = fi.ImageUrl,
              Gender = fi.Gender,
              AddressId = fi.AddressId,
              ModifiedDate = fi.ModifiedDate,
              // Add raw material details using provided queries
              Addresses = _context.Address
                        .Where(address => address.Id == fi.AddressId)
                        .Select(address => address)
                        .FirstOrDefault(),
              RoleName  = _context.Role
                        .Where(role => role.Id == fi.Role)
                        .Select(role => role.RoleName)
                        .FirstOrDefault(),
              GenderName  = _context.MasterData
                        .Where(masterData => masterData.Id == fi.Gender)
                        .Select(masterData => masterData.MasterDataName)
                        .FirstOrDefault()
          })
          .ToList();



            // Create PaginatedRawMaterials object
            var result = new PaginatedUsers
            {
                Items = paginatedResult,
                TotalCount = totalCount,
                PageIndex = filter.Pagination.PageIndex,
                PageSize = filter.Pagination.PageSize
            };

            return result;
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
                AddressId = user.AddressId,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role,
                HashPassword = passwordHash,
                AddedDate = user.AddedDate,
                ImageUrl = user.ImageUrl,
                Gender = user.Gender,

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
                    AddressId = user.AddressId,
                    AddedDate = user.AddedDate,
                    PhoneNumber = user.PhoneNumber,
                    ImageUrl = user.ImageUrl,
                    Role = user.Role,
                    Gender=  user.Gender,
                    Address = _context.Address
                    .Where(address => address.Id == user.AddressId)
                    .Select(address => new AddressVM
                    {
                        Id = address.Id,
                        FullAddress = address.FullAddress,
                        Street1 = address.Street1,
                        Street2 = address.Street2,
                        City = address.City,
                        Country = address.Country,
                        PostalCode = address.PostalCode,
                        AddedDate = address.AddedDate,
                        ModifiedDate = address.ModifiedDate,
                        IsDeleted = address.IsDeleted
                    })
                    .FirstOrDefault()

                };

                return userDetail;
            } else
            {
                return null;
            }

        }

        public UserDetailVM GetById(int userId)
        {


            UserDetailVM? user = _context.Users.Where(user => user.Id == userId && !user.IsDeleted).Select(user => new UserDetailVM()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                AddressId = user.AddressId,
                AddedDate = user.AddedDate,
                PhoneNumber = user.PhoneNumber,
                ImageUrl = user.ImageUrl,
                Role = user.Role,
                Gender = user.Gender,
                Address = _context.Address
                 .Where(address => address.Id == user.AddressId)
                 .Select(address => new AddressVM
                 {
                     Id = address.Id,
                     FullAddress = address.FullAddress,
                     Street1 = address.Street1,
                     Street2 = address.Street2,
                     City = address.City,
                     Country = address.Country,
                     PostalCode = address.PostalCode,
                     AddedDate = address.AddedDate,
                     ModifiedDate = address.ModifiedDate,
                     IsDeleted = address.IsDeleted
                 })
                 .FirstOrDefault()
            }).FirstOrDefault();

            return user;

        }

        public int UpdateById(int userId, UserDetailVM  user)
        {
            Users? previousUser = _context.Users.FirstOrDefault(r => r.Id == userId && !r.IsDeleted);
            if (previousUser == null)
            {
                // Handle the case where the user with the given ID is not found
                return -1; // You might want to return an error code or throw an exception
            }

            // Update the properties of the existing user
            Users updateUser = previousUser;
            updateUser.ModifiedDate = DateTime.Now;
            updateUser.FirstName = user.FirstName;
            updateUser.LastName = user.LastName;
            updateUser.PhoneNumber = user.PhoneNumber;
            updateUser.AddressId = user.AddressId;
            updateUser.Role = user.Role;
            updateUser.Gender = user.Gender;
            updateUser.Email = user.Email;
            updateUser.ImageUrl = user.ImageUrl;
            _context.SaveChanges();
            return updateUser.Id;
        }
    }
}
