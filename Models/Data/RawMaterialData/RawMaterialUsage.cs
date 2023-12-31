using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Data.RawMaterialData
{
    public class RawMaterialUsage
    {
        public int Id { get; set; }
        public int FoodItemId { get; set; }
        public int RawMaterialId { get; set; }
        public double QuantityUsed { get; set; }
    }
}
