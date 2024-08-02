using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Requests
{
    public class AddLocationRequest
    {
        public string LocationName { get; set; }
        public DateTime AddedDate { get; set; }
        public AddressRequest Address { get; set; }
        public int Status { get; set; }
    }
}
