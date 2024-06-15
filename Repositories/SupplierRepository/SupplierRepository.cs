using Models.Data;
using Models.Data.RawMaterialData;
using Models.Data.Supplier;
using Models.ViewModels.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.SupplierRepository
{
    public class SupplierRepository : IRepositoryBase<SupplierVM>
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
                if (entity.ProductIds.Count() > 0)
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
                if (entity.RawMaterialIds.Count() > 0)
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

        public int DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public SupplierVM GetById(int id)
        {
            throw new NotImplementedException();
        }

        public int UpdateById(int id, SupplierVM entity)
        {
            throw new NotImplementedException();
        }
    }
}
