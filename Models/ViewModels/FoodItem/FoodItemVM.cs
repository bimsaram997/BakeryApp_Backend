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
        public DateTime? AddedDate { get; set; }
        public string ProductDescription { get; set; }
        public double? ProductPrice { get; set; }
        public string ImageURL { get; set; }
        public int FoodTypeId { get; set; }
        public long BatchId { get; set; }
        public string? FoodTypeName { get; set; }
        public bool? IsSold { get; set; }

    }
}
