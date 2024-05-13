using Microsoft.AspNetCore.Mvc;
using Models.Filters;
using Models.Requests;
using Models.Requests.Update_Requests;
using Models.ViewModels.RawMaterial;
using Models.ViewModels.Recipe;
using Repositories;
using Repositories.RawMarerialRepository;
using Repositories.RecipeRepository;

namespace BakeryApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
                /*   RecipeVM recipeVM = _iRecipeRepository.GetByFoodTypeId(recipeRequest.foodTypeId);*/
                /*RecipeVM recipeVM = null;
                if (recipeVM != null)
                {
                    throw new Exception("Recipe has been added to correspondig food type");
                }*/

                

                // add new recipe
                var recipe = new RecipeVM
                {
                    RawMaterials = recipeRequest.rawMaterials,
                    AddedDate = DateTime.Now,
                    Description = recipeRequest.Description,
                    Instructions = recipeRequest.Instructions,
                    RecipeName = recipeRequest.RecipeName

                };
                int productId = _recipeRepository.Add(recipe);
                return Created(nameof(AddRecipe), productId);

            }
            catch (Exception ex)
            {
                return BadRequest($"Error adding recipe: {ex.Message}");
            }


        }

        [HttpGet("getRecipeById/{recipeId}")]
        public IActionResult GetRecipeById(int recipeId)
        {
            try
            {
                // Call the repository to get the recipe by ID
                 RecipeVM recipe = _recipeRepository.GetById(recipeId);

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
                return BadRequest($"Error retrieving recipe: {ex.Message}");
            }
        }

        [HttpPut("updateRecipe/{recipeId}")]
        public IActionResult UpdateRecipe(int recipeId, [FromBody] UpdateRecipe updatedRecipe)
        {

            try
            {
               
              
                RecipeVM recipe = new RecipeVM
                {
                    RecipeName = updatedRecipe.RecipeName,
                    Description =  updatedRecipe.Description,
                    Instructions = updatedRecipe.Instructions,
                    RawMaterials = updatedRecipe.RawMaterials,
                };
                int updatedRecipeId = _recipeRepository.UpdateById(recipeId, recipe);
                if (updatedRecipeId != -1)
                {
                    // Return a successful response
                    return Created(nameof(UpdateRecipe), updatedRecipeId);
                }
                else
                {
                    // Handle the case where the recipe is not found
                    return NotFound($"Recipe with ID {recipeId} not found.");
                }
              
            } catch(Exception ex)
            {
                return BadRequest($"Error updating recipe: {ex.Message}");
            }  
            
        }

        [HttpDelete("deleteRecipe/{recipeId}")]
        public IActionResult DeleteRecipe(int recipeId)
        {
            try
            {
                // Call the repository to delete the recipe by ID
                int deletedRecipeId = _recipeRepository.DeleteById(recipeId);

                if (deletedRecipeId != -1)
                {
                    // Return a successful response
                    return 
                        
                        Created(nameof(DeleteRecipe), deletedRecipeId);
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
                return BadRequest($"Error deleting recipe: {ex.Message}");
            }
        }


        [HttpPost("listAdvance")]
        public IActionResult GetAlRecipes([FromBody] RecipeListAdvanceFilter recipeListAdvanceFilter)
        {
            try
            {
                var _recipes = _iRecipeRepository.GetAll(recipeListAdvanceFilter);
                return Created(nameof(GetAlRecipes), _recipes);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error loading raw materials: {ex.Message}");
            }

        }

        /*  public bool IsmissingRawMaterials(List<RecipeRawMaterial> rawMaterials)
          {
              //check recipes has raw materials
              var missingRawMaterials = rawMaterials
                  .Where(rawMaterial => _rawMaterialRepository.GetById(rawMaterial.rawMaterialId) == null)
                  .ToList();

              if (missingRawMaterials.Any())
              {
                  return true;
              } else
              {
                  return false;
              }
          }*/
    }
}
