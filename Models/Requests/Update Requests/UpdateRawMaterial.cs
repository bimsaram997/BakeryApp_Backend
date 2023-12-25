using Models.Data.RawMaterialData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Requests.Update_Requests
{
    public class UpdateRawMaterial
    {
        public string name { get; set; }
        public int quantity { get; set; }
        public string imageURL { get; set; }
        public RawMaterialQuantityType rawMaterialQuantityType { get; set; }
    }
}
