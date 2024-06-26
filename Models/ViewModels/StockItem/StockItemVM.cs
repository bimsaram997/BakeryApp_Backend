using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels.StockItem
{
    public class StockItemVM
    {
        public int ProductId { get; set; }
        public int StockId { get; set; }
        public double SellingPrice { get; set; }
        public double CostPrice { get; set; }
        public int BatchId { get; set; }
        public DateTime ManufacturedDate { get; set; }
        public DateTime ExpiredDate { get; set; }
        public int? SupplierId { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsSold { get; set; }
        public int ItemQuantity { get; set; }
       
    }

   
}
