using Microsoft.AspNetCore.Mvc;
using Models.Requests;
using Models.Requests.Update_Requests;
using Models.ViewModels.RawMaterial;
using Models.ViewModels.Recipe;
using Repositories;
using Repositories.RecipeRepository;

namespace BakeryApp.Controllers
{
    public class RecipeController : ControllerBase
    {
        public IRepositoryBase<RecipeVM> _recipeRepository;
        public IRepositoryBase<RawMaterialVM> _rawMaterialRepository;
        public IRecipeRepository _iRecipeRepository;

        public RecipeController(IRepositoryBase<RecipeVM> recipeRepository,
            IRepositoryBase<RawMaterialVM> rawMaterialRepository,
            IRecipeRepository iRecipeRepository)
        {
            _recipeRepository = recipeRepository;
            _rawMaterialRepository = rawMaterialRepository;
            _iRecipeRepository = iRecipeRepository;
        }
        //Add recipe
        [HttpPost("addRecipe")]
        public IActionResult AddRecipe([FromBody] AddRecipeRequest recipeRequest)
        {

            try
            {
                //check food type has a corresponding recipe
                AllRecipeVM recipeVM = _iRecipeRepository.GetByFoodTypeId(recipeRequest.foodTypeId);
                if (recipeVM != null)
                {
                    throw new Exception("Recipe has been added to correspondig food type");
                }

                //check recipes has raw materials
                var missingRawMaterials = recipeRequest.rawMaterials
                    .Where(rawMaterial => _rawMaterialRepository.GetById(rawMaterial.rawMaterialId) == null)
                    .ToList();

                if (missingRawMaterials.Any())
                {
                    // Throw an exception if any raw material is not found
                    throw new Exception("One or more raw materials are not available in the database.");
                }

                // add new recipe
                var recipe = new RecipeVM
                {
                    foodTypeId = recipeRequest.foodTypeId,
                    rawMaterials = recipeRequest.rawMaterials,
                    addedDate = DateTime.Now,

                };
                int foodId = _recipeRepository.Add(recipe);
                return Created(nameof(AddRecipe), foodId);

            }
            catch (Exception ex)
            {
                return BadRequest($"Error adding recipe: {ex.Message}");
            }


        }

        [HttpGet("getRecipe/{recipeId}")]
        public IActionResult GetRecipeById(int recipeId)
        {
            try
            {
                // Call the repository to get the recipe by ID
                var recipe = _recipeRepository.GetById(recipeId);

                if (recipe != null)
                {
                    return Created(nameof(GetRecipeById), recipe);
                }
                else
                {
                    // Handle the case where the recipe is not found
                    return NotFound($"Recipe with ID {recipeId} not found.");
                }
            }
            catch (Exception ex)
            {
                // Handle other exceptions if needed
                return BadRequest($"Error retrieving recipe: {ex.Message}");
            }
        }

        [HttpPut("updateRecipe/{recipeId}")]
        public IActionResult UpdateRecipe(int recipeId, [FromBody] UpdateRecipe updatedRecipe)
        {

            try
            {
                RecipeVM recipeVM = _recipeRepository.GetById(recipeId);
                if (recipeVM == null)
                {
                    throw new Exception("No recipe!");
                }
                var recipe = new RecipeVM
                {
                    rawMaterials = updatedRecipe.rawMaterials,
                };
                int updatedRecipeId = _recipeRepository.UpdateById(recipeId, recipe);
                return Created(nameof(UpdateRecipe), 1);
            } catch(Exception ex)
            {
                return BadRequest($"Error adding recipe: {ex.Message}");
            }

            
            
        }
    }
}
