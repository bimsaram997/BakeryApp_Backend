using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels.User
{
    public class UserVM
    {
        public int Id { get; set; }
        public string userCode {get; set;}
        public DateTime AddedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int AddressId { get; set; }
        public string PhoneNumber { get; set; }
        public int Role { get; set; }
        public int Gender { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ImageUrl { get; set; }
    }
    public enum Role
    {
        Admin,
        User
    }
}
