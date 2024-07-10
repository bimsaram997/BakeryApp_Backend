using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Data.Stock
{
    public class Stocks
    {
        public int Id { get; set; }
        public string StockCode { get; set; }   
        public int Unit { get; set; }
        public int ProductId { get; set; }
        public int CostCode { get; set; }
        public double SellingPrice { get; set; }
        public double CostPrice { get; set; }
        public int RecipeId { get; set; }
        public int SupplyTypeId { get; set; }
        public string? SupplierName { get; set; }
        public DateTime ManufacturedDate { get; set; }
        public DateTime ExpiredDate { get; set; }
        public int ItemQuantity { get; set; }
        public int ReorderLevel { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public int BatchId { get; set; }
        public Stocks()
        {
            IsDeleted = false;

        }
    }
}
