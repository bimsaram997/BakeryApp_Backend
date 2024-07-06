using Models.ViewModels.RawMaterial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels.Stock
{
    public class AllStockVM
    {
        public int Id { get; set; }
        public string StockCode { get; set; }   
        public string? MeasureUnitName { get; set; }
        public string? ProductName { get; set; }
        public string? CostCode { get; set; }
        public double SellingPrice { get; set; }
        public double CostPrice { get; set; }
        public string? RecipeName { get; set; }
        public string? SupplyTypeName { get; set; }
        public string? SupplierName { get; set; }
        public DateTime ManufacturedDate { get; set; }
        public DateTime ExpiredDate { get; set; }
        public int ItemQuantity { get; set; }
        public int ReorderLevel { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public int BatchId { get; set; }
    }

    public class PaginatedStocks
    {
        public List<AllStockVM> Items { get; set; }
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
