using Models.Data.RawMaterialData;
using Models.ViewModels.RawMaterial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels.Recipe
{
    public class RecipeVM
    {
        public int Id { get; set; }
        public string RecipeCode { get; set; }
        public string RecipeName { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public List<RecipeRawMaterial> RawMaterials { get; set; }
        public string Description { get; set; }
        public string Instructions { get; set; }
    }

   
    public class RecipeRawMaterial
    {
        public int rawMaterialId { get; set; }
        public double rawMaterialQuantity { get; set; }
        public int measureUnit { get; set; }

    }

    public class RecipeListSimpleVM
    {
        public int Id { get; set; }
        public string RecipeName { get; set; }

    }
}
