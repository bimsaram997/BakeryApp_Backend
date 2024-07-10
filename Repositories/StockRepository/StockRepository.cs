using Microsoft.IdentityModel.Tokens;
using Models.Data;
using Models.Data.RawMaterialData;
using Models.Data.Stock;
using Models.Filters;
using Models.Helpers;
using Models.ViewModels.Product;
using Models.ViewModels.RawMaterial;
using Models.ViewModels.Recipe;
using Models.ViewModels.Stock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.StockRepository
{
    public interface IStockRepository
    {
        public int CheckProductAssociatedWithStock(int stockId);
        PaginatedStocks GetAll(StockListAdvanceFilter filter);
    }
    public class StockRepository: IRepositoryBase<StockVM>, IStockRepository
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
                BatchId = entity.BatchId,
                Unit = entity.Unit
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

        public PaginatedStocks GetAll(StockListAdvanceFilter filter)
        {
            IQueryable<Stocks> query = _context.Stock
               .Where(fi => !fi.IsDeleted);
            if (!string.IsNullOrEmpty(filter.SearchString))
            {
                query = query.Where(fi =>
                   fi.StockCode.Contains(filter.SearchString) || fi.BatchId.ToString().Contains(filter.SearchString) || fi.StockCode.Contains(filter.SearchString)
                );
            }
            if (filter.ProductId.HasValue)
            {
                query = query.Where(fi => fi.ProductId == filter.ProductId);
            }
            if (filter.Unit.HasValue)
            {
                query = query.Where(fi => fi.Unit == filter.Unit);
            }
            if (filter.CostCode.HasValue)
            {
                query = query.Where(fi => fi.CostCode == filter.CostCode);
            }
            if (filter.RecipeId.HasValue)
            {
                query = query.Where(fi => fi.RecipeId == filter.RecipeId);
            }
            if (filter.SupplyTypeId.HasValue)
            {
                query = query.Where(fi => fi.SupplyTypeId == filter.SupplyTypeId);
            }
            if (filter.SupplyTypeId.HasValue)
            {
                query = query.Where(fi => fi.SupplyTypeId == filter.SupplyTypeId);
            }
            if (filter.SupplierId.HasValue)
            {
                query = query.Where(fi => fi.SupplierId == filter.SupplierId);
            }
            if (!string.IsNullOrEmpty(filter.ManufacturedDate) && DateTime.TryParse(filter.ManufacturedDate, out DateTime filterManufacturedDate))
            {
                DateTime nextDay = filterManufacturedDate.AddDays(1);
                query = query.Where(fi => fi.ManufacturedDate >= filterManufacturedDate && fi.ManufacturedDate < nextDay);
            }
            if (!string.IsNullOrEmpty(filter.ExpiredDate) && DateTime.TryParse(filter.ExpiredDate, out DateTime filterExpiredDate))
            {
                DateTime nextDay = filterExpiredDate.AddDays(1);
                query = query.Where(fi => fi.ExpiredDate >= filterExpiredDate && fi.ExpiredDate < nextDay);
            }
            if (!string.IsNullOrEmpty(filter.AddedDate) && DateTime.TryParse(filter.AddedDate, out DateTime filterDate))
            {
                DateTime nextDay = filterDate.AddDays(1);
                query = query.Where(fi => fi.AddedDate >= filterDate && fi.AddedDate < nextDay);
            }
            if (filter.SellingPrice.HasValue)
            {
                query = query.Where(fi => fi.SellingPrice == filter.SellingPrice);
            }
            if (filter.CostPrice.HasValue)
            {
                query = query.Where(fi => fi.CostPrice == filter.CostPrice);
            }
            int totalCount = query.Count();

            // Apply sorting
            query = SortHelper.ApplySorting(query.AsQueryable(), filter.SortBy, filter.IsAscending);

            // Apply pagination
            query = query.Skip((filter.Pagination.PageIndex - 1) * filter.Pagination.PageSize)
                         .Take(filter.Pagination.PageSize);
            var paginatedResult = query
            .Select(fi => new AllStockVM
            {
                Id = fi.Id,
                StockCode= fi.StockCode,
                MeasureUnitName = _context.MasterData
                    .Where(masterData => masterData.Id == fi.Unit)
                    .Select(masterData => masterData.MasterDataName)
                    .FirstOrDefault(),
                ProductName = _context.Product
                    .Where(product => product.Id == fi.ProductId)
                    .Select(product => product.Name)
                    .FirstOrDefault(),
                CostCode = _context.MasterData
                    .Where(masterData => masterData.Id == fi.CostCode)
                    .Select(masterData => masterData.MasterDataName)
                    .FirstOrDefault(),
                SellingPrice = fi.SellingPrice,
                CostPrice = fi.CostPrice,
                RecipeName = _context.Recipes
                    .Where(recipe => recipe.Id == fi.RecipeId)
                    .Select(recipe => recipe.RecipeName)
                    .FirstOrDefault(),
                SupplyTypeName = _context.MasterData
                    .Where(masterData => masterData.Id == fi.SupplyTypeId)
                    .Select(masterData => masterData.MasterDataName)
                    .FirstOrDefault(),
                SupplierName = _context.Supplier
                    .Where(supplier => supplier.Id == fi.SupplierId)
                    .Select(supplier => (supplier.SupplierFirstName + " " + supplier.SupplierLastName))
                    .FirstOrDefault(),
                ManufacturedDate =  fi.ManufacturedDate,
                ExpiredDate =  fi.ExpiredDate,
                ItemQuantity = fi.ItemQuantity,
                ReorderLevel =  fi.ReorderLevel,
                AddedDate = fi.AddedDate,
                BatchId = fi.BatchId,
                ModifiedDate = fi.ModifiedDate,
               
               
            })
            .ToList();

            // Create PaginatedRawMaterials object
            var result = new PaginatedStocks
            {
                Items = paginatedResult,
                TotalCount = totalCount,
                PageIndex = filter.Pagination.PageIndex,
                PageSize = filter.Pagination.PageSize
            };

            return result;

        }

        public int DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public StockVM GetById(int id)
        {
            StockVM? stock = _context.Stock
        .Where(fi => fi.Id == id && !fi.IsDeleted)
        .Select(fi => new StockVM
        {
            Id = fi.Id,
            Unit = fi.Unit,
            AddedDate = fi.AddedDate,
            ProductId = fi.ProductId,
            CostCode = fi.CostCode,
            SellingPrice = fi.SellingPrice,
            CostPrice = fi.CostPrice,
            RecipeId = fi.RecipeId,
            SupplyTypeId = fi.SupplyTypeId,
            SupplierId = fi.SupplierId,
            IsDeleted = fi.IsDeleted,
            ModifiedDate = fi.ModifiedDate,
            ManufacturedDate = fi.ManufacturedDate,
            ExpiredDate = fi.ExpiredDate,
            ItemQuantity = fi.ItemQuantity,
            ReorderLevel = fi.ReorderLevel,
            BatchId = fi.BatchId


        })
        .FirstOrDefault();

            return stock;
        }

        public int UpdateById(int id, StockVM entity)
        {
            throw new NotImplementedException();
        }

        public int CheckProductAssociatedWithStock(int productd)
        {
            var productId = _context.Stock
           .Where(stock => stock.ProductId == productd && !stock.IsDeleted)
           .Select(stock => stock.ProductId)
           .FirstOrDefault();

            return productId;
        }
    }
}
