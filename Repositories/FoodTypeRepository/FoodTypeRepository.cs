﻿using Microsoft.EntityFrameworkCore;
using Models.Data;
using Models.Data.FoodItemData;
using Models.Data.RawMaterialData;
using Models.ViewModels.FoodItem;
using Models.ViewModels.FoodType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.FoodTypeRepository
{
    public class FoodTypeRepository : IRepositoryBase<FoodTypeVM>, IFoodTypeRepository
    {
        private AppDbContext _context;
        public FoodTypeRepository(AppDbContext context)
        {
            _context = context;
        }
        public int Add(FoodTypeVM foodType)
        {
            var lastFoodType = _context.FoodTypes.OrderByDescending(fi => fi.FoodTypeCode).FirstOrDefault();
            int newFoodTypeNumber = 1; // Default if no existing records

            if (lastFoodType != null)
            {
                // Extract the number part of the FoodCode and increment it
                if (int.TryParse(lastFoodType.FoodTypeCode.Substring(1), out int lastCodeNumber))
                {
                    newFoodTypeNumber = lastCodeNumber + 1;
                }
            }

            string newFoodCode = $"FT{newFoodTypeNumber:D4}";
            var _foodType = new FoodType()
            {
                FoodTypeCode = newFoodCode,
                FoodTypeName = foodType.FoodTypeName,
                FoodTypeCount = foodType.FoodTypeCount,
                AddedDate = DateTime.Now,
                ImageURL = foodType.ImageURL,
            };

            _context.FoodTypes.Add(_foodType);
            object value = _context.SaveChanges();

            // After SaveChanges, _foodItem will have the ID generated by the database.
            int addedFoodTypeId = _foodType.Id;
            foreach (var id in foodType.RawMaterialIds)
            {
                var _rawMaterial_FoodType = new RawMaterial_FoodType()
                {
                    RawMaterialId = id,
                    FoodTypeId = _foodType.Id
                };
                _context.RawMaterial_FoodTypes.Add(_rawMaterial_FoodType);
                _context.SaveChanges();
            }

            return addedFoodTypeId;
        }

        public int UpdateFoodTypeCountByFoodTypeId(int Id)
        {

            var foodType = _context.FoodTypes.FirstOrDefault(ft => ft.Id == Id);
            if (foodType != null)
            {
                foodType.FoodTypeCount =  foodType.FoodTypeCount+ 1;
                _context.FoodTypes.Update(foodType);
                object value = _context.SaveChanges();
            }
            
            return foodType.Id;
        }
        public int DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public FoodTypeVM GetById(int Id)
        {
            var foodType = _context.FoodTypes.Where(n => n.Id == Id).Select(foodType => new FoodTypeVM()
            {
                Id = foodType.Id,
                FoodTypeCode = foodType.FoodTypeCode,
                FoodTypeCount = foodType.FoodTypeCount,
                ImageURL = foodType.ImageURL,
                FoodTypeName = foodType.FoodTypeName,
                AddedDate = foodType.AddedDate,
            }).FirstOrDefault();
            return foodType;
        }

        public int UpdateById(FoodTypeVM entity)
        {
            throw new NotImplementedException();
        }

       
    }
}
