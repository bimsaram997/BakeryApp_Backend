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
        public AllRecipeVM GetByFoodTypeId(int foodTypeId);
    }
    public class RecipeRepository: IRepositoryBase<RecipeVM>, IRecipeRepository
    {
        private AppDbContext _context;
        public RecipeRepository(AppDbContext context)
        {
            _context = context;
        }

        public int Add(RecipeVM recipe)
        {
            //Add Food Recipe to the database
            var lastRecipe = _context.Recipes.OrderByDescending(fi => fi.RecipeCode).FirstOrDefault();
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
            var _recipe = new Recipe()
            {
                RecipeCode = newRecipeCode,
                AddedDate = DateTime.Now,
                FoodTypeId = recipe.foodTypeId
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
            throw new NotImplementedException();
        }

         public AllRecipeVM GetByFoodTypeId(int foodTypeId)
        {
            var recipe = _context.Recipes.Where(n => n.FoodTypeId == foodTypeId).Select(recipe => new AllRecipeVM()
            {
                id = recipe.Id,
                addedDate = recipe.AddedDate,
                foodTypeId = recipe.FoodTypeId,
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
                       id = rawMat.Id,
                       rawMaterialCode = rawMat.RawMaterialCode,
                       name = rawMat.Name,
                       quantity = rawMat.Quantity,
                       addedDate = rawMat.AddedDate,
                       imageURL = rawMat.ImageURL,
                       rawMaterialQuantityType = rawMat.RawMaterialQuantityType
                   }).ToList()
            }).FirstOrDefault();
            return recipe;
        }

        public RecipeVM GetById(int id)
        {
            var recipe = _context.Recipes.Where(recipe => recipe.Id == id).Select(recipe => new RecipeVM()
            {
                id= recipe.Id,
                addedDate= recipe.AddedDate,
                recipeCode= recipe.RecipeCode,
                foodTypeId = recipe.FoodTypeId,
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

        public int UpdateById(int id,RecipeVM recipe)
        {

            var previousRecipe = _context.Recipes.Find(id);
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
    }
}
