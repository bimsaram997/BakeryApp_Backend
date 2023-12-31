using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Data.FoodItemData
{
    public class BatchFoodItem
    {
        public int Id { get; set; }
        public long BatchId { get; set; }
        public int FoodItemId { get; set; }
    }
}
