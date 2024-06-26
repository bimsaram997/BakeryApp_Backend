using Models.Data;
using Models.Data.Stock;
using Models.ViewModels.StockItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.StockRepository
{
    public interface IStockItemRepository
    {
        public int[] AddStockItem(StockItemVM stockItem);
    }
    public class StockItemRepository: IStockItemRepository
    {
        private AppDbContext _context;
        public StockItemRepository(AppDbContext context)
        {
            _context = context;
        }

        public int[] AddStockItem(StockItemVM stockItem)
        {
            List<int> addedStockItemIds = new List<int>();

            for (int i = 0; i < stockItem.ItemQuantity; i++)
            {
                string newProductCode = Guid.NewGuid().ToString();
                StockItem stock = new StockItem()
                {
                    StockItemCode = newProductCode,
                    ProductId = stockItem.ProductId,
                    StockId = stockItem.StockId,
                    SellingPrice = stockItem.SellingPrice,
                    CostPrice = stockItem.CostPrice,
                    BatchId = stockItem.BatchId,
                    ManufacturedDate = stockItem.ManufacturedDate,
                    ExpiredDate = stockItem.ExpiredDate,
                    SupplierId = stockItem.SupplierId,
                    IsDeleted = stockItem.IsDeleted,
                    IsSold = stockItem.IsSold,
                 
                };

                _context.StockItem.Add(stock);
                _context.SaveChanges(); 
                addedStockItemIds.Add(stock.Id);
            }
            return addedStockItemIds.ToArray();
        }
    }
}
