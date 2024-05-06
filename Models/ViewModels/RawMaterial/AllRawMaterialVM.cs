using Models.Data.RawMaterialData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels.RawMaterial
{

    public class AllRawMaterialVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string RawMaterialCode { get; set; }
        public DateTime? AddedDate { get; set; }
        public RawMaterialQuantityType RawMaterialQuantityType { get; set; }
        public double? Quantity { get; set; }
        public string ImageURL { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }


    public class PaginatedRawMaterials
    {
        public List<AllRawMaterialVM> Items { get; set; }
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
