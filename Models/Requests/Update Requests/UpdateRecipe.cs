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
        public string RecipeName { get; set; }

        public string Description { get; set; }
        public string Instructions { get; set; }

        public List<RecipeRawMaterial> RawMaterials { get; set; }
    }
}
