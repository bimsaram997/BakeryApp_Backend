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
        public int id { get; set; }
        public string recipeCode { get; set; }
        public int foodTypeId { get; set; }
        public DateTime addedDate { get; set; }
        public DateTime? modifiedDate { get; set; }
        public bool isDeleted { get; set; }
        public List<RecipeRawMaterial> rawMaterials { get; set; }
    }

    public class AllRecipeVM
    {
        public double? rawMaterialQuantity;

        public int id { get; set; }
        public string recipeCode { get; set; }
        public int foodTypeId { get; set; }
        public DateTime? addedDate { get; set; }
        public bool isDeleted { get; set; }
        public List<RecipeRawMaterial> rawMaterials { get; set; }
        public List<RawMaterialVM> rawMaterialDetails { get; set; }
    }

    public class RecipeRawMaterial
    {
        public int rawMaterialId { get; set; }
        public double rawMaterialQuantity { get; set; }
        
    }
}
