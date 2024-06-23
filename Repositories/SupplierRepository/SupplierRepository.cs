using Models.Data;
using Models.Data.RawMaterialData;
using Models.Data.RecipeData;
using Models.Data.Supplier;
using Models.Filters;
using Models.Helpers;
using Models.ViewModels.Address;
using Models.ViewModels.Recipe;
using Models.ViewModels.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.SupplierRepository
{
    public interface ISupplierRepository
    {
        PaginatedSuppliers GetAll(SupplierListAdvanceFilter filter);
        SupplierListSimpleVM[] GetSupplierListSimple(SupplerListSimpleFilter filter);
    }
        public class SupplierRepository : IRepositoryBase<SupplierVM>, ISupplierRepository
    {
        private AppDbContext _context;
        public SupplierRepository(AppDbContext context)
        {
            _context = context;
        }

        public int Add(SupplierVM entity)
        {
            string newSupplierCode = Guid.NewGuid().ToString();
            var _supplier = new Suppliers
            {
                SupplierCode = newSupplierCode,
                SupplierFirstName = entity.SupplierFirstName,
                SupplierLastName = entity.SupplierLastName,
                AddedDate =   entity.AddedDate,
                ModifiedDate= entity.ModifiedDate,
                PhoneNumber= entity.PhoneNumber,
                Email = entity.Email,
                IsProduct = entity.IsProduct,
                IsRawMaterial = entity.IsRawMaterial,
                AddressId = entity.AddressId,
            };
            _context.Supplier.Add(_supplier);
            object value =_context.SaveChanges();
            int addedSupplier = _supplier.Id;
            if (addedSupplier > 0)
            {
                if (entity.ProductIds != null && entity.ProductIds.Count() > 0)
                {
                    foreach (int productId in entity.ProductIds)
                    {
                        var _supplierProduct = new SupplierProduct()
                        {
                            ProductId = productId,
                            SupplierId = addedSupplier
                        };
                        _context.SupplierProduct.Add(_supplierProduct);
                        _context.SaveChanges();
                    }
                }
                if (entity.RawMaterialIds != null && entity.RawMaterialIds.Count() > 0)
                {
                    foreach (int rawMaterialId in entity.RawMaterialIds)
                    {
                        var _supplierRawMaterial = new SupplierRawMaterial()
                        {
                            RawMaterialId = rawMaterialId,
                            SupplierId = addedSupplier
                        };
                        _context.SupplierRawMaterial.Add(_supplierRawMaterial);
                        _context.SaveChanges();
                    }
                }
            } 
            
            return addedSupplier;
        }

        public PaginatedSuppliers GetAll(SupplierListAdvanceFilter filter)
        {
            IQueryable<Suppliers> query = _context.Supplier
         .Where(fi => !fi.IsDeleted);

            // Apply filtering
            if (!string.IsNullOrEmpty(filter.SearchString))
            {
                query = query.Where(fi =>
                    fi.SupplierFirstName.Contains(filter.SearchString) || fi.SupplierLastName.Contains(filter.SearchString)
                    || fi.SupplierCode.Contains(filter.SearchString)
                );
            }

            if (filter.RawMaterialIds != null && filter.RawMaterialIds.Any())
            {
                query = query.Where(fi => _context.SupplierRawMaterial
                    .Any(rm => rm.SupplierId == fi.Id && filter.RawMaterialIds.Contains(rm.RawMaterialId)));
            }

            if (filter.ProductIds != null && filter.ProductIds.Any())
            {
                query = query.Where(fi => _context.SupplierProduct
                    .Any(rm => rm.SupplierId == fi.Id && filter.ProductIds.Contains(rm.ProductId)));
            }

            if (!string.IsNullOrEmpty(filter.PhoneNumber))
            {
                query = query.Where(fi => fi.PhoneNumber == filter.PhoneNumber);
            }

            if (!string.IsNullOrEmpty(filter.Email))
            {
                query = query.Where(fi => fi.Email == filter.Email);
            }

            if (filter.IsProduct.HasValue && filter.IsProduct == true)
            {
                query = query.Where(fi => fi.IsProduct == filter.IsProduct);
            }

            if (filter.IsRawMaterial.HasValue && filter.IsRawMaterial == true)
            {
                query = query.Where(fi => fi.IsRawMaterial == filter.IsRawMaterial);
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
          .Select(fi => new AllSupplierVM
          {
              Id = fi.Id,
              SupplierCode = fi.SupplierCode,
              SupplierFirstName = fi.SupplierFirstName,
              SupplierLastName =  fi.SupplierLastName,
              AddedDate = fi.AddedDate,
              Email = fi.Email,
              PhoneNumber = fi.PhoneNumber,
              ModifiedDate = fi.ModifiedDate,
              IsProduct = fi.IsProduct,
              IsRawMaterial = fi.IsRawMaterial,
              // Add raw material details using provided queries
              RawMaterialDetails = _context.RawMaterials
                  .Where(rawMat => _context.SupplierRawMaterial
                      .Any(rm => rm.SupplierId == fi.Id && rm.RawMaterialId == rawMat.Id))
                  .Select(rawMat => rawMat.Name)
                  .ToList(),
              ProductDetails = _context.Product
                  .Where(product => _context.SupplierProduct
                      .Any(rm => rm.SupplierId == fi.Id && rm.ProductId == product.Id))
                  .Select(product => product.Name)
                  .ToList(),
              Addresses = _context.Address
                        .Where(address => address.Id == fi.AddressId)
                        .Select(address => address)
                        .FirstOrDefault(),
          })
          .ToList();



            // Create PaginatedRawMaterials object
            var result = new PaginatedSuppliers
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

        public SupplierVM GetById(int id)
        {

            SupplierVM ? supplier = _context.Supplier.Where(supplier => supplier.Id == id && !supplier.IsDeleted).Select(supplier => new SupplierVM()
            {
                Id = supplier.Id,
                AddedDate = supplier.AddedDate,
                SupplierFirstName = supplier.SupplierFirstName,
                SupplierLastName = supplier.SupplierLastName,
                SupplierCode = supplier.SupplierCode,
                Email = supplier.Email,
                PhoneNumber = supplier.PhoneNumber,
                IsProduct = supplier.IsProduct,
                IsRawMaterial = supplier.IsRawMaterial,
                //  foodTypeId = supplier.FoodTypeId,
                ModifiedDate = supplier.ModifiedDate,
                AddressId = supplier.AddressId,
                ProductIds = _context.SupplierProduct
                .Where(prod => _context.SupplierProduct
                    .Any(sp => sp.SupplierId == supplier.Id))
                .Select(prod => (int?)prod.ProductId)
                .ToList(),
                RawMaterialIds = _context.SupplierRawMaterial
                  .Where(rawMat => _context.SupplierRawMaterial
                      .Any(rm => rm.SupplierId == supplier.Id ))
                  .Select(rawMat => (int?)rawMat.RawMaterialId)
                  .ToList(),
                Address = _context.Address
                 .Where(address => address.Id == supplier.AddressId)
                 .Select(address => new AddressVM
                 {
                     Id = address.Id,
                     FullAddress = address.FullAddress,
                     Street1 = address.Street1,
                     Street2 = address.Street2,
                     City = address.City,
                     Country = address.Country,
                     PostalCode = address.PostalCode,
                     AddedDate = address.AddedDate,
                     ModifiedDate = address.ModifiedDate,
                     IsDeleted = address.IsDeleted
                 })
                 .FirstOrDefault()

            }).FirstOrDefault();
            return supplier;
        }

        public int UpdateById(int id, SupplierVM entity)
        {
           Suppliers? previousSupplier =  _context.Supplier.FirstOrDefault(r => r.Id == id && !r.IsDeleted);
            if (previousSupplier ==  null)
            {
                return -1;
            }
            Suppliers existingSupplier = previousSupplier;
            existingSupplier.SupplierFirstName = entity.SupplierFirstName;
            existingSupplier.SupplierLastName = entity.SupplierLastName;
            existingSupplier.PhoneNumber= entity.PhoneNumber;
            existingSupplier.Email = entity.Email;
            existingSupplier.IsRawMaterial = entity.IsRawMaterial;
            existingSupplier.IsProduct = entity.IsProduct;
            existingSupplier.ModifiedDate = DateTime.Now;
            existingSupplier.AddressId  = entity.AddressId;

            if(entity?.IsRawMaterial == true)
            {
                var existingSupplierProducts = _context.SupplierProduct
            .Where(rm => rm.SupplierId == existingSupplier.Id)
            .ToList();

                // Delete existing SupplierRawmat records

                _context.SupplierProduct.RemoveRange(existingSupplierProducts);
            }

            if (entity?.IsProduct == true)
            {
                var existingSupplierRawMaterial = _context.SupplierRawMaterial
            .Where(rm => rm.SupplierId == existingSupplier.Id)
            .ToList();

                // Delete existing SupplierRawmaterials records

                _context.SupplierRawMaterial.RemoveRange(existingSupplierRawMaterial);
            }

                if (entity.RawMaterialIds != null && entity.RawMaterialIds.Count() > 0)
            {
                var existingSupplierRawMaterial = _context.SupplierRawMaterial
              .Where(rm => rm.SupplierId == existingSupplier.Id)
              .ToList();

                // Delete existing SupplierRawmaterials records

                _context.SupplierRawMaterial.RemoveRange(existingSupplierRawMaterial);

                // Add new RawMaterialRecipe records

               
                    foreach (int rawMaterialId in entity.RawMaterialIds)
                    {
                        var _supplierRawMaterial = new SupplierRawMaterial()
                        {
                            RawMaterialId = rawMaterialId,
                            SupplierId = existingSupplier.Id
                        };
                        _context.SupplierRawMaterial.Add(_supplierRawMaterial);
                        _context.SaveChanges();
                    }
                
            }

              

            if (entity.ProductIds != null && entity.ProductIds.Count() > 0)
            {
                var existingSupplierProducts = _context.SupplierProduct
             .Where(rm => rm.SupplierId == existingSupplier.Id)
             .ToList();

                // Delete existing SupplierRawmat records

                _context.SupplierProduct.RemoveRange(existingSupplierProducts);
                foreach (int productId in entity.ProductIds)
                {
                    var _supplierProduct = new SupplierProduct()
                    {
                        ProductId = productId,
                        SupplierId = existingSupplier.Id
                    };
                    _context.SupplierProduct.Add(_supplierProduct);
                    _context.SaveChanges();
                }
            }
            _context.SaveChanges();
            return existingSupplier.Id;

        }
        public SupplierListSimpleVM[] GetSupplierListSimple(SupplerListSimpleFilter filter)
        {
            IQueryable<Suppliers> query = _context.Supplier
      .Where(fi => !fi.IsDeleted);

            if (filter.IsProduct.HasValue && filter.IsProduct == true)
            {
                query = query.Where(fi => fi.IsProduct == filter.IsProduct);
            }

            if (filter.IsRawMaterial.HasValue && filter.IsRawMaterial == true)
            {
                query = query.Where(fi => fi.IsRawMaterial == filter.IsRawMaterial);
            }
            if (filter.ProductIds != null && filter.ProductIds.Any())
            {
                query = query.Where(fi => _context.SupplierProduct
                    .Any(rm => rm.SupplierId == fi.Id && filter.ProductIds.Contains(rm.ProductId)));
            }
            var paginatedResult = query
       .Select(fi => new SupplierListSimpleVM
       {
            Id = fi.Id,
                AddedDate = fi.AddedDate,
                SupplierFirstName = fi.SupplierFirstName,
                SupplierLastName = fi.SupplierLastName,
                SupplierCode = fi.SupplierCode,
                Email = fi.Email,
                PhoneNumber = fi.PhoneNumber,
                IsProduct = fi.IsProduct,
                IsRawMaterial = fi.IsRawMaterial,
                //  foodTypeId = supplier.FoodTypeId,
                ModifiedDate = fi.ModifiedDate,
            
       })
       .ToArray();

            return paginatedResult;
        }
    }
}
