using Models.ViewModels.RawMaterial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels.Recipe
{
    public class AllRecipeVM
    {
        public int Id { get; set; }
        public string RecipeCode { get; set; }
        public DateTime? AddedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string RecipeName { get; set; }
        public string Description { get; set;}
        public string Instructions { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public List<string> RawMaterialDetails { get; set; }

    }

    public class PaginatedRecipes
    {
        public List<AllRecipeVM> Items { get; set; }
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

}
