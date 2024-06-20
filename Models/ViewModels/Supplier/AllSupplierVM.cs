using Models.Data.Address;
using Models.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels.Supplier
{
    public class AllSupplierVM
    {
        public int Id { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierFirstName { get; set; }
        public string SupplierLastName { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool? IsRawMaterial { get; set; }
        public bool? IsProduct { get; set; }
        public bool IsDeleted { get; set; }
        public int AddressId { get; set; }
        public Addresses Addresses { get; set; }
        public List<string> RawMaterialDetails { get; set; }
        public List<string> ProductDetails { get; set; }
    }

    public class PaginatedSuppliers
    {
        public List<AllSupplierVM> Items { get; set; }
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
