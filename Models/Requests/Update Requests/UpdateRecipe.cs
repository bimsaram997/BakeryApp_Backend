using Models.ViewModels.Recipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Requests.Update_Requests
{
    public class UpdateRecipe
    {

        public int foodTypeId { get; set; }
        public List<RecipeRawMaterial> rawMaterials { get; set; }
    }
}
