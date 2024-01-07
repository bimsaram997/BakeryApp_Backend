using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Filters
{
    public class ProductListAdvanceFilter
    {
        public int? FoodTypeId { get; set; }
        public double? FoodPrice { get; set; }
    }
}
