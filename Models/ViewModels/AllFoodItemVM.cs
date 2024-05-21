using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class AllProductVM
    {
        public int Id { get; set; }
        public string Name { get; set; }            
        public string ProductCode { get; set; }
        public DateTime? AddedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ProductDescription { get; set; }
        public double? SellingPrice { get; set; }
        public double? CostPrice { get; set; }
        public string ImageURL { get; set; }

        public int? Unit { get; set; }
        public int? CostCode { get; set; }
        public int? RecipeId { get; set; }
        public string RecipeName { get; set; }


    }


    public class PaginatedProducts
    {
        public List<AllProductVM> Items { get; set; }
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
