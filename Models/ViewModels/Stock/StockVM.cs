﻿using Models.ViewModels.Recipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels.Stock
{
    public class StockVM
    {
        public int Id { get; set; }
        public int Unit { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int CostCode { get; set; }
        public double SellingPrice { get; set; }
        public double CostPrice { get; set; }
        public int RecipeId { get; set; }
        public int SupplyTypeId { get; set; }
        public int? SupplierId { get; set; }
        public DateTime ManufacturedDate { get; set; }
        public DateTime ExpiredDate { get; set; }
        public int ItemQuantity { get; set; }
        public int ReorderLevel { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public int BatchId { get; set; }
        public RawMaterialQuantity[]? RawMaterialQuantities { get; set; }
    }
}
