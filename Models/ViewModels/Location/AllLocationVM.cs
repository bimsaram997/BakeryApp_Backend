using Models.Data.Address;
using Models.ViewModels.Address;
using Models.ViewModels.RawMaterial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels.Location
{
    public class AllLocationVM
    {
        public int Id { get; set; }
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public DateTime AddedDate { get; set; }
        public int AddressId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int Status { get; set; }
        public string? StatusName { get; set; }
        public Addresses? Addresses { get; set; }
    }

    public class PaginatedLocations
    {
        public List<AllLocationVM> Items { get; set; }
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
