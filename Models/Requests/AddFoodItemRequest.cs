using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Requests
{
    public class AddFoodItemRequest
    {
        public string FoodCode { get; set; }
        public string FoodName { get; set; }
        public string FoodDescription { get; set; }
        public double? FoodPrice { get; set; }
        public string ImageURL { get; set; }
        public DateTime? AddedDate { get; set; }
    }
}
