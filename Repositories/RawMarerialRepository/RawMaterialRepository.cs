﻿using Models.Data;
using Models.Data.FoodItemData;
using Models.Data.RawMaterialData;
using Models.ViewModels.FoodItem;
using Models.ViewModels.FoodType;
using Models.ViewModels.RawMaterial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.RawMarerialRepository
{
    public class RawMaterialRepository : IRepositoryBase<RawMaterialVM>
    {
        private AppDbContext _context;
        public RawMaterialRepository(AppDbContext context)
        {
            _context = context;
        }

        public int Add(RawMaterialVM rawMaterial)
        {

            var lastRawMaterial = _context.RawMaterials.OrderByDescending(fi => fi.RawMaterialCode).FirstOrDefault();
            int newRowMaterialNumber = 1; // Default if no existing records

            if (lastRawMaterial != null)
            {
                // Extract the number part of the FoodCode and increment it
                if (int.TryParse(lastRawMaterial.RawMaterialCode.Substring(1), out int lastCodeNumber))
                {
                    newRowMaterialNumber = lastCodeNumber + 1;
                }
            }
            string newRawMaterialCode = $"RM{newRowMaterialNumber:D4}";
            var _rawMaterial = new RawMaterial()
            {
                RawMaterialCode = newRawMaterialCode,
                Name = rawMaterial.name,
                AddedDate = rawMaterial.addedDate,
                Quantity= rawMaterial.quantity,
                ImageURL = rawMaterial.imageURL,
                RawMaterialQuantityType = rawMaterial.rawMaterialQuantityType
            };
            _context.RawMaterials.Add(_rawMaterial);
            object value = _context.SaveChanges();
            // After SaveChanges, _rawMaterial will have the ID generated by the database.
            int addedRawMaterialId = _rawMaterial.Id;
            return addedRawMaterialId;
        }

        public int DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public RawMaterialVM GetById(int Id)
        {
            var rawMaterial = _context.RawMaterials.Where(n => n.Id == Id).Select(rawMaterial => new RawMaterialVM()
            {
                id = rawMaterial.Id,
                rawMaterialCode = rawMaterial.RawMaterialCode,
                name = rawMaterial.Name,
                imageURL = rawMaterial.ImageURL,
                quantity = rawMaterial.Quantity,
                addedDate = rawMaterial.AddedDate,
                rawMaterialQuantityType = rawMaterial.RawMaterialQuantityType
            }).FirstOrDefault();
            return rawMaterial;
        }

        public int UpdateById(int id, RawMaterialVM entity)
        {
            throw new NotImplementedException();
        }
    }
}
