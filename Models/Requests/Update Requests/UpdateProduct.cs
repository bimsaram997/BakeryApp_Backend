using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Requests.Update_Requests
{
    public class UpdateProduct
    {
        public string Name { get; set; }
        public int Unit { get; set; }
        public int CostCode { get; set; }
        public double CostPrice { get; set; }
        public double SellingPrice { get; set; }
        public int RecipeId { get; set; }
        public string ProductDescription { get; set; }
        public string ImageURL { get; set; }
        public double Weight { get; set; }
        public int Status { get; set; }
        public int DaysToExpires { get; set; }
        public int ReOrderLevel { get; set; }
    }
}
