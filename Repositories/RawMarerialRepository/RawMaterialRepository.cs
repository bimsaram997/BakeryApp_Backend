﻿using Models.Data;
using Models.Data.RawMaterialData;
using Models.Filters;
using Models.Helpers;
using Models.ViewModels;
using Models.ViewModels.Product;
using Models.ViewModels.FoodType;
using Models.ViewModels.RawMaterial;


namespace Repositories.RawMarerialRepository
{
    public interface IRawMaterialRepository
    {
         RawMatRecipeVM GetRawMaterialRecipeByRawMatIdAndRecipeId(int recipeId, int rawMaterialId);
         int UpdateRawMaterialCountbyRawMatId(int rawMatId, double newStockQuantity);
        void StoreRawMaterialQuantitiesUsed(int productId, Dictionary<int, double> rawMaterialQuantitiesUsed);
        PaginatedRawMaterials GetAll(RawMaterialListAdvanceFilter filter);
        RawMaterialListSimpleVM[] ListSimpeRawMaterials();

    }
    public class RawMaterialRepository : IRepositoryBase<RawMaterialVM>, IRawMaterialRepository
    {
        private AppDbContext _context;
        public RawMaterialRepository(AppDbContext context)
        {
            _context = context;
        }



        public PaginatedRawMaterials GetAll(RawMaterialListAdvanceFilter filter)
        {
            IQueryable<RawMaterial> query = _context.RawMaterials
                .Where(fi => !fi.IsDeleted);

            // Apply filtering
            if (!string.IsNullOrEmpty(filter.SearchString))
            {
                query = query.Where(fi =>
                    fi.Name.Contains(filter.SearchString) || fi.RawMaterialCode.Contains(filter.SearchString)
                );
            }

            if (filter.MeasureUnit.HasValue)
            {
                query = query.Where(fi => fi.MeasureUnit == filter.MeasureUnit);
            }

            if (filter.Quantity.HasValue)
            {
                query = query.Where(fi => fi.Quantity == filter.Quantity);
            }

            if (!string.IsNullOrEmpty(filter.AddedDate) && DateTime.TryParse(filter.AddedDate, out DateTime filterDate))
            {
                // Adjust date filter to consider the whole day
                DateTime nextDay = filterDate.AddDays(1);
                query = query.Where(fi => fi.AddedDate >= filterDate && fi.AddedDate < nextDay);
            }

            // Get total count before pagination
            int totalCount = query.Count();

            // Apply sorting
            query = SortHelper.ApplySorting(query.AsQueryable(), filter.SortBy, filter.IsAscending);

            // Apply pagination
            query = query.Skip((filter.Pagination.PageIndex - 1) * filter.Pagination.PageSize)
                         .Take(filter.Pagination.PageSize);

            // Project and materialize the results
            var paginatedResult = query
                .Select(fi => new AllRawMaterialVM
                {
                    Id = fi.Id,
                    Name = fi.Name,
                    Price = fi.Price,
                    AddedDate = fi.AddedDate,
                    Quantity = fi.Quantity,
                    MeasureUnit = fi.MeasureUnit,
                    ImageURL = fi.ImageURL,
                    ModifiedDate = fi.ModifiedDate,
                    LocationId =  fi.LocationId,
                    RawMaterialCode = fi.RawMaterialCode,

                })
                .ToList();

            // Create PaginatedRawMaterials object
            var result = new PaginatedRawMaterials
            {
                Items = paginatedResult,
                TotalCount = totalCount,
                PageIndex = filter.Pagination.PageIndex,
                PageSize = filter.Pagination.PageSize
            };

            return result;
        }


        public int Add(RawMaterialVM rawMaterial)
        {


            string newRawMaterialCode = Guid.NewGuid().ToString();
            RawMaterial _rawMaterial = new RawMaterial()
            {
                RawMaterialCode = newRawMaterialCode,
                Price =  rawMaterial.Price,
                Name = rawMaterial.Name,
                AddedDate = rawMaterial.AddedDate,
                Quantity= rawMaterial.Quantity,
                ImageURL = rawMaterial.ImageURL,
                MeasureUnit = rawMaterial.MeasureUnit,
                LocationId =  rawMaterial.LocationId
            };
            _context.RawMaterials.Add(_rawMaterial);
            object value = _context.SaveChanges();
            // After SaveChanges, _rawMaterial will have the ID generated by the database.
            int addedRawMaterialId = _rawMaterial.Id;
            return addedRawMaterialId;
        }

        public int DeleteById(int id)
        {
            RawMaterial rawMaterial = _context.RawMaterials.FirstOrDefault(r => r.Id == id && !r.IsDeleted);
            if (rawMaterial == null)
            {
                // Handle the case where the raw material with the given ID is not found
                return -1; // You might want to return an error code or throw an exception
            }
            rawMaterial.IsDeleted = true;
            rawMaterial.ModifiedDate = DateTime.Now;
            // Save changes to the database
            _context.SaveChanges();
            return rawMaterial.Id;
        }

        public RawMaterialVM GetById(int Id)
        {
            RawMaterialVM? rawMaterial = _context.RawMaterials.Where(n => n.Id == Id && !n.IsDeleted).Select(rawMaterial => new RawMaterialVM()
            {
                Id = rawMaterial.Id,
                RawMaterialCode = rawMaterial.RawMaterialCode,
                Name = rawMaterial.Name,
                ImageURL = rawMaterial.ImageURL,
                Quantity = rawMaterial.Quantity,
                Price = rawMaterial.Price,
                AddedDate = rawMaterial.AddedDate,
                MeasureUnit = rawMaterial.MeasureUnit,
                IsDeleted =  rawMaterial.IsDeleted,
                ModifiedDate = rawMaterial.ModifiedDate,
                LocationId =  rawMaterial.LocationId
                
            }).FirstOrDefault();
            return rawMaterial;
        }

        public int UpdateById(int id, RawMaterialVM rawMaterial)
        {
            RawMaterial? previousRawMaterial = _context.RawMaterials.FirstOrDefault(r => r.Id == id && !r.IsDeleted);
            if (previousRawMaterial == null)
            {
                // Handle the case where the  raw Material with the given ID is not found
                return -1; // You might want to return an error code or throw an exception
            }
            previousRawMaterial.Name = rawMaterial.Name;
            previousRawMaterial.Price = rawMaterial.Price;
            previousRawMaterial.LocationId = rawMaterial.LocationId;
            previousRawMaterial.ImageURL = rawMaterial.ImageURL;
            previousRawMaterial.Quantity = rawMaterial.Quantity;
            previousRawMaterial.MeasureUnit = rawMaterial.MeasureUnit;
            previousRawMaterial.ModifiedDate = DateTime.Now;
            previousRawMaterial.LocationId =  rawMaterial.LocationId;
            
            _context.SaveChanges();
            return previousRawMaterial.Id;
        }

        public RawMaterialListSimpleVM[] ListSimpeRawMaterials()
        {
            var simpleRawMaterials = _context.RawMaterials
                 .Where(ft => !ft.IsDeleted)
                 .Select(rawMaterial => new RawMaterialListSimpleVM()
                 {
                     Id = rawMaterial.Id,
                     Quantity = rawMaterial.Quantity,
                     MeasureUnit = rawMaterial.MeasureUnit,
                     Name = rawMaterial.Name,
                    
                 })
                 .ToArray();

            return simpleRawMaterials;
        }



        public RawMatRecipeVM GetRawMaterialRecipeByRawMatIdAndRecipeId(int rawMaterialId, int recipeId)
        {
            RawMatRecipeVM? rawMaterialRecipe = _context.RawMaterialRecipe.Where(n => n.RecipeId == recipeId && n.RawMaterialId == rawMaterialId).Select(rawMaterialRecipe => new RawMatRecipeVM()
            {
                id = rawMaterialRecipe.Id,
                rawMaterialId = rawMaterialRecipe.RawMaterialId,
                recipeId = rawMaterialRecipe.RecipeId,
                rawMaterialQuantity = rawMaterialRecipe.RawMaterialQuantity


            }).FirstOrDefault();
            return rawMaterialRecipe;
        }

        public int UpdateRawMaterialCountbyRawMatId(int rawMatId, double newStockQuantity)
        {
            RawMaterial? previousRawMaterial = _context.RawMaterials.FirstOrDefault(r => r.Id == rawMatId && !r.IsDeleted);
            if (previousRawMaterial == null)
            {
                // Handle the case where the  raw Material with the given ID is not found
                return -1; // You might want to return an error code or throw an exception
            }
             previousRawMaterial.Quantity = newStockQuantity;
             previousRawMaterial.ModifiedDate = DateTime.Now;
            
            _context.SaveChanges();
            return previousRawMaterial.Id;
        }

        public void StoreRawMaterialQuantitiesUsed(int productId, Dictionary<int, double> rawMaterialQuantitiesUsed)
        {
            foreach (var entry in rawMaterialQuantitiesUsed)
            {
                var rawMaterialUsage = new RawMaterialUsage
                {
                    ProductId = productId,
                    RawMaterialId = entry.Key,
                    QuantityUsed = entry.Value
                };

                _context.rawMaterialUsage.Add(rawMaterialUsage);
            }

            _context.SaveChanges();
        }

      
    }
}
