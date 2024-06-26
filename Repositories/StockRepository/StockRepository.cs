using Models.Data;
using Models.Data.Stock;
using Models.ViewModels.Recipe;
using Models.ViewModels.Stock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.StockRepository
{
    public class StockRepository: IRepositoryBase<StockVM>
    {
        private AppDbContext _context;
        public StockRepository(AppDbContext context)
        {
            _context = context;
        }

        public int Add(StockVM entity)
        {
            string newStockCode = Guid.NewGuid().ToString();
            Stocks _stock = new Stocks()
            {
                StockCode = newStockCode,
                ProductId = entity.ProductId,
                CostCode = entity.CostCode,
                SellingPrice = entity.SellingPrice,
                CostPrice = entity.CostPrice,
                RecipeId = entity.RecipeId,
                SupplyTypeId = entity.SupplyTypeId,
                SupplierId = entity.SupplierId,
                ManufacturedDate = entity.ManufacturedDate,
                ExpiredDate = entity.ExpiredDate,
                ItemQuantity = entity.ItemQuantity,
                ReorderLevel = entity.ReorderLevel,
                AddedDate = entity.AddedDate,
                BatchId = entity.BatchId
            };
            _context.Stock.Add(_stock);
            object value = _context.SaveChanges();
            int addedStockId = _stock.Id;
            if(addedStockId > 0 && entity.RawMaterialQuantities.Length > 0 && entity.SupplierId == null)
            {
                foreach (var stockRawMaterial in entity.RawMaterialQuantities)
                {
                    var _stockRawMaterial = new StockRawMaterialHistory()
                    {
                        StockId = addedStockId,
                        RawMaterialId = stockRawMaterial.RawMaterialId,
                        RawMaterialQuantity = stockRawMaterial.TotalQuantityNeeded
                    };
                    _context.StockRawMaterialHistory.Add(_stockRawMaterial);
                }
            }
            return addedStockId;
        }

        public int DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public StockVM GetById(int id)
        {
            throw new NotImplementedException();
        }

        public int UpdateById(int id, StockVM entity)
        {
            throw new NotImplementedException();
        }
    }
}
