using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class AllFoodItemVM
    {
        public int Id { get; set; }
        public string FoodCode { get; set; }
        public DateTime? AddedDate { get; set; }
        public string FoodDescription { get; set; }
        public double? FoodPrice { get; set; }
        public string ImageURL { get; set; }
        public int FoodTypeId { get; set; }
        public string FoodTypeName { get; set; }
    }
}
