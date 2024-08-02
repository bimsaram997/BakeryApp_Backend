using Models.ViewModels.Address;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels.Location
{
    public class LocationVM
    {
        public int Id { get; set; }
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public DateTime AddedDate { get; set; }
        public int AddressId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int Status { get; set; }
        public AddressVM? Address { get; set; }
    }

    public class LocationlListSimpleVM
    {
        public int Id { get; set; }
        public string LocationName { get; set; }
       



    }
}
