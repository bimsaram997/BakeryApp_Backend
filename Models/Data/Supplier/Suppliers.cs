using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Data.Supplier
{
    public class Suppliers
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

        public ICollection<SupplierProduct> SupplierProduct { get; set; }
        public ICollection<SupplierRawMaterial> SupplierRawMaterial { get; set; }
    }
}
