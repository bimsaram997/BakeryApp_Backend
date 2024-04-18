using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Requests
{
    public class AddFoodItemRequest
    {
  
        public string FoodDescription { get; set; }
        public double FoodPrice { get; set; }
        public string ImageURL { get; set; }
        public DateTime AddedDate { get; set; }
        public int FoodTypeId { get; set; }
        public int FoodItemCount { get; set; }
    }
}
