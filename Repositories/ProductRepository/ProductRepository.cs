﻿
using Microsoft.EntityFrameworkCore;
using Models.Data;
using Models.Data.ProductData;
using Models.Filters;
using Models.Helpers;
using Models.Pagination;
using Models.Requests.Update_Requests;
using Models.ViewModels;
using Models.ViewModels.Product;
using Models.ViewModels.RawMaterial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.ProductRepository
{
    public interface IProductRepository 
    {

     
        PaginatedProducts GetAll(ProductListAdvanceFilter filter);

        ProductListSimpleVM[] ListSimpeProducts();
    }
    public class ProductRepository : IRepositoryBase<ProductVM>, IProductRepository
    {
        private AppDbContext _context;
        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

     
        public int Add(ProductVM product)
        {
            string newProductCode = Guid.NewGuid().ToString();
            var _product = new Product()
            {
                ProductCode = newProductCode,
                Name = product.Name,
                Unit =  product.Unit,
                CostCode= product.CostCode,
                CostPrice =  product.CostPrice,
                SellingPrice =  product.SellingPrice,
                RecipeId = product.RecipeId,
                ProductDescription = product.ProductDescription,
                AddedDate = DateTime.Now,
                ImageURL = product.ImageURL,
                Weight = product.Weight,
                Status = product.Status,
                DaysToExpires = product.DaysToExpires,
                ReOrderLevel = product.ReOrderLevel

            };

            _context.Product.Add(_product);
            object value = _context.SaveChanges();

            // After SaveChanges, _product will have the ID generated by the database.
            int addedItemId = _product.Id;

            return addedItemId;
        }


        public int DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        

        public ProductVM GetById(int id)
        {
            ProductVM? product = _context.Product
       .Where(fi => fi.Id == id && !fi.IsDeleted)
       .Select(fi => new ProductVM
       {
           Id = fi.Id,
           ProductCode = fi.ProductCode,
           AddedDate = fi.AddedDate,
           ProductDescription = fi.ProductDescription,
           Name = fi.Name,
           Unit = fi.Unit,
           CostPrice = fi.CostPrice,
           SellingPrice = fi.SellingPrice,
           CostCode = fi.CostCode,
           RecipeId = fi.RecipeId,
           IsDeleted =  fi.IsDeleted,
           ModifiedDate =  fi.ModifiedDate,
           ImageURL = fi.ImageURL,
           Weight = fi.Weight,
           Status = fi.Status,
           DaysToExpires = fi.DaysToExpires,
           ReOrderLevel = fi.ReOrderLevel


       })
       .FirstOrDefault();

            return product;
        }

        public int UpdateById(int id, ProductVM entity)
        {
            Product? previousProduct = _context.Product.FirstOrDefault(p => p.Id == id && !p.IsDeleted);
            if (previousProduct == null)
            {
                return -1;
            }

            Product updateProduct = previousProduct;
            updateProduct.Name = entity.Name;
            updateProduct.Unit = entity.Unit;
            updateProduct.CostPrice = entity.CostPrice;
            updateProduct.SellingPrice = entity.SellingPrice;
            updateProduct.CostCode = entity.CostCode;
            updateProduct.RecipeId = entity.RecipeId;
            updateProduct.ProductDescription = entity.ProductDescription;
            updateProduct.ImageURL = entity.ImageURL;
            updateProduct.ModifiedDate = DateTime.Now;
            updateProduct.Weight = entity.Weight;
            updateProduct.Status = entity.Status;
            updateProduct.ReOrderLevel= entity.ReOrderLevel;
            updateProduct.DaysToExpires = entity.DaysToExpires;

            _context.SaveChanges();
            return updateProduct.Id;
        }

        

        public PaginatedProducts GetAll(ProductListAdvanceFilter filter)
        {
            IQueryable<Product> query = _context.Product
                .Where(fi => !fi.IsDeleted);

            query = SortHelper.ApplySorting(query.AsQueryable(), filter.SortBy, filter.IsAscending);

           if (filter != null)
            {
                if (filter.SellingPrice.HasValue)
                {
                    query = query.Where(fi => fi.SellingPrice == filter.SellingPrice);
                }

                if (filter.CostPrice.HasValue)
                {
                    query = query.Where(fi => fi.CostPrice == filter.CostPrice);
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

                if (!string.IsNullOrEmpty(filter.SearchString))
                {
                    query = query.Where(fi =>
                        fi.ProductCode.Contains(filter.SearchString) || fi.Name.Contains(filter.SearchString) ||
                        fi.ProductDescription.Contains(filter.SearchString) 
                    );
                }



                if (!string.IsNullOrEmpty(filter.AddedDate))
                {
                    if (DateTime.TryParse(filter.AddedDate, out DateTime filterDate))
                    {
                        query = query.Where(fi => fi.AddedDate >= filterDate && fi.AddedDate < filterDate.AddDays(1));
                    }
                   
                }

                if (filter.Weight.HasValue)
                {
                    query = query.Where(fi => fi.Weight == filter.Weight);
                }

                if (filter.ReOrderLevel.HasValue)
                {
                    query = query.Where(fi => fi.ReOrderLevel == filter.ReOrderLevel);
                }

                if (filter.DaysToExpires.HasValue)
                {
                    query = query.Where(fi => fi.DaysToExpires == filter.DaysToExpires);
                }
            }

            int totalCount = query.Count();

            query = query.Skip((filter.Pagination.PageIndex - 1) * filter.Pagination.PageSize).Take(filter.Pagination.PageSize);

            var paginatedResult = query
                .Select(fi => new AllProductVM
                {
                    Id = fi.Id,
                    Name =  fi.Name,
                    ProductCode = fi.ProductCode,
                    AddedDate = fi.AddedDate,
                    ProductDescription = fi.ProductDescription,
                    ModifiedDate = fi.ModifiedDate,
                    SellingPrice = fi.SellingPrice,
                    CostPrice= fi.CostPrice,
                    CostCode= fi.CostCode,
                    Unit = fi.Unit,
                    RecipeId = fi.RecipeId,
                    RecipeName = _context.Recipes
                        .Where(recipe => recipe.Id == fi.RecipeId)
                        .Select(recipe => recipe.RecipeName)
                        .FirstOrDefault(),
                    UnitName = _context.MasterData
                        .Where(masterData => masterData.Id == fi.Unit)
                        .Select(masterData => masterData.MasterDataName)
                        .FirstOrDefault(),
                    Weight = fi.Weight,
                    Status = fi.Status,
                    DaysToExpires = fi.DaysToExpires,
                    ReOrderLevel = fi.ReOrderLevel


                })
                .ToList();

            var result = new PaginatedProducts
            {
                Items = paginatedResult,
                TotalCount = totalCount,
                PageIndex = filter.Pagination.PageIndex,
                PageSize = filter.Pagination.PageSize
            };

            return result;
        }


        public ProductListSimpleVM[] ListSimpeProducts()
        {
            var simpleProducts = _context.Product
                 .Where(ft => !ft.IsDeleted)
                 .Select(product => new ProductListSimpleVM()
                 {
                     Id = product.Id,
                     Name = product.Name,
                     CostCode = product.CostCode,
                     RecipeId = product.RecipeId,
                     Unit = product.Unit,
                     CostPrice = product.CostPrice,
                     SellingPrice = product.SellingPrice

                 })
                 .ToArray();

            return simpleProducts;
        }






    }
}
