using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Data
{
    public class FoodType
    {
            public int Id { get; set; }
            public string FoodTypeCode { get; set; }
            public string FoodTypeName { get; set; }
            public DateTime? AddedDate { get; set; }
            public int FoodTypeCount { get; set; }
            public string ImageURL { get; set; }
       
    }
}
