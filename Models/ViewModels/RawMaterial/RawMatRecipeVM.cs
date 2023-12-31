using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels.RawMaterial
{
    public class RawMatRecipeVM
    {
        public int id { get; set; }
        public int rawMaterialId { get; set; }
        public double rawMaterialQuantity { get; set; }
        public int recipeId { get; set; }
    }
}
