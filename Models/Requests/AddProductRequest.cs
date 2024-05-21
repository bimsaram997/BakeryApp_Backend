using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Requests
{
    public class AddProductRequest
    {
        public string Name { get; set; }
        public int Unit { get; set; }
        public int CostCode { get; set; }
        public double CostPrice { get; set; }
        public double SellingPrice { get; set; }
        public int RecipeId { get; set; }
        public string ProductDescription { get; set; }
        public string ImageURL { get; set; }
        public DateTime AddedDate { get; set; }
    }
}
