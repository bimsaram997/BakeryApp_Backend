using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Requests.Update_Requests
{
    public class UpdateSupplier
    {
        public string SupplierFirstName { get; set; }
        public string SupplierLastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int AddressId { get; set; }
        public bool? IsRawMaterial { get; set; }
        public bool? IsProduct { get; set; }
        public AddressRequest Address { get; set; }
        public List<int>? ProductIds { get; set; }
        public List<int>? RawMaterialIds { get; set; }
    }
}
