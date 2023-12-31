using Models.Data.RawMaterialData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Requests
{
    public class AddFoodTypeRequest
    {
        public string FoodTypeName { get; set; }
        public DateTime? AddedDate { get; set; }
        public string ImageURL { get; set; }
      
    }
}
