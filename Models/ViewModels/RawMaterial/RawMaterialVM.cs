using Models.Data.RawMaterialData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels.RawMaterial
{
    public class RawMaterialVM
    {
        public int id { get; set; }
        public string rawMaterialCode { get; set; }
        public string name { get; set; }
        public double quantity { get; set; }
        public DateTime? addedDate { get; set; }
        public string imageURL { get; set; }
        public RawMaterialQuantityType rawMaterialQuantityType { get; set; }
        public bool isDeleted { get; set; }
        public DateTime? modifiedDate { get; set; }

    }
}
