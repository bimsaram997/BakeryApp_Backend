using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Requests.Update_Requests
{
    public  class UpdateUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int AddressId { get; set; }
        public string PhoneNumber { get; set; }
        public int Role { get; set; }
        public int Gender { get; set; }
        public string Email { get; set; }
        public string ImageUrl { get; set; }
        public AddressRequest Address { get; set; }
    }
}
