using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Requests
{
    public class AddProductRequest
    {
  
        public string ProductDescription { get; set; }
        public double ProductPrice { get; set; }
        public string ImageURL { get; set; }
        public DateTime AddedDate { get; set; }
        public int FoodTypeId { get; set; }
        public int ProductCount { get; set; }
    }
}
