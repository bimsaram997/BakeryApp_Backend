
using Models.Data.RecipeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Data.RawMaterialData
{
    public class RawMaterialRecipe
    {
            public int Id { get; set; }
            public int RawMaterialId { get; set; }
            public double RawMaterialQuantity { get; set; }
            public RawMaterial RawMaterial { get; set; }
            public int RecipeId { get; set; }
            public Recipe recipe { get; set; }
            

    }
}
