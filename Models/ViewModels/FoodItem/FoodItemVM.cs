using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels.Product
{
    public class ProductVM
    {
        public int Id { get; set; }
        public string ProductCode { get; set; }
        public string Name { get; set; }
        public int Unit { get; set; }
        public int CostCode { get; set; }
        public double CostPrice { get; set; }
        public double SellingPrice { get; set; }
        public int RecipeId { get; set; }
        public string ProductDescription { get; set; }
        public string ImageURL { get; set; }
        public DateTime? AddedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public bool IsDeleted { get; set; }

    }

    public class ProductListSimpleVM
    {
        public int Id { get; set; }
        public string Name { get; set; }    

    }
}
