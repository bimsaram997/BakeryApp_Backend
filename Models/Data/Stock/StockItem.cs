﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Data.Stock
{
    public class StockItem
    {
        public int Id { get; set; }
        public string StockItemCode { get; set; }
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
        public StockItem()
        {
            IsDeleted = false;
            IsSold = false;
        }
    }
}
