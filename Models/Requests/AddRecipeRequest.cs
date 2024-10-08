﻿using Models.ViewModels.Recipe;
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
        public string RecipeName { get; set; }

        public string Description { get; set; }
        public string Instructions { get; set; }
        public List<RecipeRawMaterialRequest> rawMaterials { get; set; }
    }
}
