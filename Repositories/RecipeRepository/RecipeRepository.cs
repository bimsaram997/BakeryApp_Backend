using Models.Data.FoodItemData;
using Models.Data;
using Models.ViewModels.FoodType;
using Models.ViewModels.Recipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Data.RecipeData;
using Models.Migrations;
using Models.Data.RawMaterialData;
using Models.ViewModels.RawMaterial;

namespace Repositories.RecipeRepository
{
    public interface IRecipeRepository
    {
        public AllRecipeVM AllGetByFoodTypeId(int foodTypeId);
        public bool CheckRawMaterialsAssociatedWithRecipe(int rawMateialId);
        bool IsFoodTypeLinked(int foodTypeId);
        public RecipeVM GetByFoodTypeId(int foodTypeId);
    }
    public class RecipeRepository : IRepositoryBase<RecipeVM>, IRecipeRepository
    {
        private AppDbContext _context;
        public RecipeRepository(AppDbContext context)
        {
            _context = context;
        }

        public int Add(RecipeVM recipe)
        {
            //Add Food Recipe to the database
            Recipe? lastRecipe = _context.Recipes.OrderByDescending(fi => fi.RecipeCode).FirstOrDefault();
            int newRecipeNumber = 1; // Default if no existing records

            if (lastRecipe != null)
            {
                // Extract the number part of the FoodCode and increment it
                if (int.TryParse(lastRecipe.RecipeCode.Substring(1), out int lastCodeNumber))
                {
                    newRecipeNumber = lastCodeNumber + 1;
                }
            }

            string newRecipeCode = $"R{newRecipeNumber:D4}";
            Recipe _recipe = new Recipe()
            {
                RecipeCode = newRecipeCode,
                AddedDate = DateTime.Now,
                FoodTypeId = recipe.foodTypeId,
                IsDeleted = recipe.isDeleted,
            };
            _context.Recipes.Add(_recipe);
            object value = _context.SaveChanges();

            //Add data to RawMaterialRecipe table to store Raw material details of the recipe
            int addedRecipeId = _recipe.Id;
            if (addedRecipeId > 0)
            {
                foreach (var recipeRawMaterial in recipe.rawMaterials)
                {
                    var _rawMaterialRecipe = new RawMaterialRecipe()
                    {

                        RawMaterialId = recipeRawMaterial.rawMaterialId,
                        RawMaterialQuantity = recipeRawMaterial.rawMaterialQuantity,
                        RecipeId = addedRecipeId
                    };
                    _context.RawMaterialRecipe.Add(_rawMaterialRecipe);
                    _context.SaveChanges();
                }

            }
            return addedRecipeId;
        }

        public int DeleteById(int id)
        {
            Recipe recipe = _context.Recipes.FirstOrDefault(r => r.Id == id && !r.IsDeleted);

            if (recipe == null)
            {
                // Handle the case where the recipe with the given ID is not found
                return -1; // You might want to return an error code or throw an exception
            }

            // Set IsDeleted to true for the Recipe
            recipe.IsDeleted = true;
            recipe.ModifiedDate = DateTime.Now;
            // Set IsDeleted to true for associated RawMaterialRecipe records
            var rawMaterialRecipes = _context.RawMaterialRecipe
                .Where(rm => rm.RecipeId == recipe.Id)
                .ToList();

            _context.RawMaterialRecipe.RemoveRange(rawMaterialRecipes);

            // Save changes to the database
            _context.SaveChanges();

            return recipe.Id;
        }


        public RecipeVM GetById(int id)
        {

            RecipeVM? recipe = _context.Recipes.Where(recipe => recipe.Id == id && !recipe.IsDeleted).Select(recipe => new RecipeVM()
            {
                id = recipe.Id,
                addedDate = recipe.AddedDate,
                recipeCode = recipe.RecipeCode,
                foodTypeId = recipe.FoodTypeId,
                modifiedDate = recipe.ModifiedDate,
                rawMaterials = _context.RawMaterialRecipe
                   .Where(rm => rm.RecipeId == recipe.Id)
                   .Select(rm => new RecipeRawMaterial
                   {
                       rawMaterialId = rm.RawMaterialId,
                       rawMaterialQuantity = rm.RawMaterialQuantity
                   }).ToList()

            }).FirstOrDefault();
            return recipe;

        }

        public int UpdateById(int id, RecipeVM recipe)
        {

            Recipe? previousRecipe = _context.Recipes.FirstOrDefault(r => r.Id == id && !r.IsDeleted);
            if (previousRecipe == null)
            {
                // Handle the case where the recipe with the given ID is not found
                return -1; // You might want to return an error code or throw an exception
            }

            // Update the properties of the existing recipe
            Recipe existingRecipe = previousRecipe;
            existingRecipe.ModifiedDate = DateTime.Now;

            // Update RawMaterialRecipe details
            var existingRawMaterialRecipes = _context.RawMaterialRecipe
                .Where(rm => rm.RecipeId == existingRecipe.Id)
                .ToList();

            // Delete existing RawMaterialRecipe records

            _context.RawMaterialRecipe.RemoveRange(existingRawMaterialRecipes);

            // Add new RawMaterialRecipe records
            foreach (var recipeRawMaterial in recipe.rawMaterials)
            {
                var newRawMaterialRecipe = new RawMaterialRecipe()
                {
                    RawMaterialId = recipeRawMaterial.rawMaterialId,
                    RawMaterialQuantity = recipeRawMaterial.rawMaterialQuantity,
                    RecipeId = existingRecipe.Id
                };
                _context.RawMaterialRecipe.Add(newRawMaterialRecipe);
            }

            // Save changes to the database
            _context.SaveChanges();
            return recipe.id;
        }

        public AllRecipeVM AllGetByFoodTypeId(int foodTypeId)
        {
            AllRecipeVM? recipe = _context.Recipes.Where(n => n.FoodTypeId == foodTypeId && !n.IsDeleted).Select(recipe => new AllRecipeVM()
            {
                id = recipe.Id,
                addedDate = recipe.AddedDate,
                foodTypeId = recipe.FoodTypeId,
                isDeleted = recipe.IsDeleted,
                rawMaterials = _context.RawMaterialRecipe
                   .Where(rm => rm.RecipeId == recipe.Id)
                   .Select(rm => new RecipeRawMaterial
                   {
                       rawMaterialId = rm.RawMaterialId,
                       rawMaterialQuantity = rm.RawMaterialQuantity
                   }).ToList(),
                rawMaterialDetails = _context.RawMaterials
                   .Where(rawMat => _context.RawMaterialRecipe
                       .Any(rm => rm.RecipeId == recipe.Id && rm.RawMaterialId == rawMat.Id))
                   .Select(rawMat => new RawMaterialVM
                   {
                       Id = rawMat.Id,
                       RawMaterialCode = rawMat.RawMaterialCode,
                       Name = rawMat.Name,
                      Quantity = rawMat.Quantity,
                       AddedDate = rawMat.AddedDate,
                       ImageURL = rawMat.ImageURL,
                       RawMaterialQuantityType = rawMat.RawMaterialQuantityType
                   }).ToList()
            }).FirstOrDefault();
            return recipe;
        }


        public RecipeVM GetByFoodTypeId(int foodTypeId)
        {
            RecipeVM? recipe = _context.Recipes.Where(n => n.FoodTypeId == foodTypeId && !n.IsDeleted).Select(recipe => new RecipeVM()
            {
                id = recipe.Id,
                addedDate = recipe.AddedDate,
                foodTypeId = recipe.FoodTypeId,
                isDeleted = recipe.IsDeleted,
                modifiedDate = recipe.ModifiedDate,
                rawMaterials = _context.RawMaterialRecipe
                   .Where(rm => rm.RecipeId == recipe.Id)
                   .Select(rm => new RecipeRawMaterial
                   {
                       rawMaterialId = rm.RawMaterialId,
                       rawMaterialQuantity = rm.RawMaterialQuantity
                   }).ToList()
            }).FirstOrDefault();
            return recipe;
        }

        public bool CheckRawMaterialsAssociatedWithRecipe(int rawMaterialId)
        {
            bool hasRecords = _context.RawMaterialRecipe.Any(rm => rm.RawMaterialId == rawMaterialId);
            return hasRecords;
        }

        public bool IsFoodTypeLinked(int foodTypeId)
        {
            bool isLinked = _context.Recipes.Any(fi => fi.FoodTypeId == foodTypeId);
            return isLinked;

        }
    }
}
