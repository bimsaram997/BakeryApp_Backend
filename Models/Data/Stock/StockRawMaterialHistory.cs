using Models.Data.RawMaterialData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Data.Stock
{
    public class StockRawMaterialHistory
    {
        public int Id { get; set; }
        public int RawMaterialId { get; set; }
        public double RawMaterialQuantity { get; set; }
        public RawMaterial RawMaterial { get; set; }
        public int StockId { get; set; }
        public Stocks stock { get; set; } 
    }
}
