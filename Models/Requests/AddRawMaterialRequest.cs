using Models.Data.RawMaterialData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Requests
{
    public class AddRawMaterialRequest
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public int LocationId { get; set; }
        public DateTime? AddedDate { get; set; }
        public string ImageURL { get; set; }
        public int MeasureUnit { get; set; }
    }
}
