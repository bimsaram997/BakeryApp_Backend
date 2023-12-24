using Models.ViewModels.Recipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Requests
{
    public class AddRecipeRequest
    {
        public DateTime? AddedDate { get; set; }
        public int foodTypeId { get; set; }
        public List<RecipeRawMaterial> rawMaterials { get; set; }
    }
}
