using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Data.User
{
    public class Users
    {
        public int Id { get; set; }
        public string UserCode { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public int Role { get; set; }
        public string Email { get; set; }
        public string HashPassword { get; set; }
        public string ImageUrl { get; set; }
        public int Gender { get; set; }
        public bool IsDeleted { get; set; }
        public int AddressId { get; set; }
        public Users()
        {
            IsDeleted = false;
        }
    }
}
