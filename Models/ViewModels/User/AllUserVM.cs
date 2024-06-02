using Models.Data.Address;
using Models.ViewModels.RawMaterial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels.User
{
    public class AllUserVM
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
        public string ImageUrl { get; set; }
        public int Gender { get; set; }
        public bool IsDeleted { get; set; }
        public int AddressId { get; set; }
        public Addresses Addresses { get; set; }
    }

    public class PaginatedUsers
    {
        public List<AllUserVM> Items { get; set; }
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
