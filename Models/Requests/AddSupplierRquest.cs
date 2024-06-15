using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Requests
{
    public class AddSupplierRquest
    {
        public string SupplierFirstName { get; set; }
        public string SupplierLastName { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool? IsRawMaterial { get; set; }
        public bool? IsProduct { get; set; }
        public bool IsDeleted { get; set; }
        public AddressRequest Address { get; set; }
        public List<int> ProductIds { get; set; }
        public List<int> RawMaterialIds { get; set; }
    }
}
