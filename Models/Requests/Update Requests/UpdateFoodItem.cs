using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Requests.Update_Requests
{
    public class UpdateFoodItem
    {
        public DateTime? AddedDate { get; set; }
        public string FoodDescription { get; set; }
        public double? FoodPrice { get; set; }
        public string ImageURL { get; set; }
        public bool? IsSold { get; set; }
        public int Id { get; set; }
    }
}
