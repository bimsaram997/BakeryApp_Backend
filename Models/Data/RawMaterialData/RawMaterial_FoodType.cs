using Models.Data.FoodItemData;
using Models.ViewModels.RawMaterial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Data.RawMaterialData
{
    public class RawMaterial_FoodType
    {
        public int Id { get; set; }

        //navigation properties
        public int RawMaterialId { get; set; }
        public RawMaterial RawMaterial { get; set; }

        public int FoodTypeId { get; set; }
        public  FoodType FoodType { get; set; }
    }
}
