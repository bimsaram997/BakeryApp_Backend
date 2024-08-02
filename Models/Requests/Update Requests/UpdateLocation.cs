using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Requests.Update_Requests
{
    public class UpdateLocation
    {
        public string LocationName { get; set; }
        public int Status { get; set; }
        public int AddressId { get; set; }
        public AddressRequest Address { get; set; }
    }
}
