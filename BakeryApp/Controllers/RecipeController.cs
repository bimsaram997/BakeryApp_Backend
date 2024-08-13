using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.ActionResults;
using Models.Data.User;
using Models.Filters;
using Models.Migrations;
using Models.Requests;
using Models.Requests.Update_Requests;
using Models.ViewModels;
using Models.ViewModels.Custom_action_result;
using Models.ViewModels.RawMaterial;
using Models.ViewModels.Recipe;
using Models.ViewModels.User;
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
        public CustomActionResult<AddResultVM> AddRecipe([FromBody] AddRecipeRequest recipeRequest)
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
                int recipeId = _recipeRepository.Add(recipe);
                if (recipeId > 0)
                {
                    var result = new AddResultVM
                    {
                        Id = recipeId
                    };
                    var responseObj = new CustomActionResultVM<AddResultVM>
                    {
                        Data = result
                    };
                    return new CustomActionResult<AddResultVM>(responseObj);
                } else
                {
                    return new CustomActionResult<AddResultVM>(new CustomActionResultVM<AddResultVM>
                    {
                        Exception = new Exception("Recipe can't add!")
                    });
                }
               

            }
            catch (Exception ex)
            {
                return new CustomActionResult<AddResultVM>(new CustomActionResultVM<AddResultVM>
                {
                    Exception = ex
                });
            }


        }

        [HttpGet("getRecipeById/{recipeId}")]
        public CustomActionResult<ResultView<RecipeVM>> GetRecipeById(int recipeId)
        {
            try
            {
                // Call the repository to get the recipe by ID
                 RecipeVM recipe = _recipeRepository.GetById(recipeId);

                if (recipe != null)
                {
                    var result = new ResultView<RecipeVM>
                    {
                        Item = recipe

                    };

                    var responseObj = new CustomActionResultVM<ResultView<RecipeVM>>
                    {
                        Data = result

                    };
                    return new CustomActionResult<ResultView<RecipeVM>>(responseObj);
                }
                else
                {
                    var result = new ResultView<RecipeVM>
                    {
                        Exception = new Exception($"Recipe with Id {recipeId} not found")
                    };

                    var responseObj = new CustomActionResultVM<ResultView<RecipeVM>>
                    {
                        Exception = result.Exception
                    };

                    return new CustomActionResult<ResultView<RecipeVM>>(responseObj);
                }
            }
            catch (Exception ex)
            {
                var responseObj = new CustomActionResultVM<ResultView<RecipeVM>>
                {
                    Exception = ex
                };

                return new CustomActionResult<ResultView<RecipeVM>>(responseObj);
            }
        }

        [HttpPut("updateRecipe/{recipeId}")]
        public CustomActionResult<AddResultVM> UpdateRecipe(int recipeId, [FromBody] UpdateRecipe updatedRecipe)
        {

            try
            {
                RecipeVM recipeVM = _recipeRepository.GetById(recipeId);
                if(recipeVM != null)
                {
                    RecipeVM recipe = new RecipeVM
                    {
                        RecipeName = updatedRecipe.RecipeName,
                        Description = updatedRecipe.Description,
                        Instructions = updatedRecipe.Instructions,
                        RawMaterials = updatedRecipe.RawMaterials,
                    };
                    int updatedRecipeId = _recipeRepository.UpdateById(recipeId, recipe);
                    var result = new AddResultVM
                    {
                        Id = updatedRecipeId
                    };
                    var responseObj = new CustomActionResultVM<AddResultVM>
                    {
                        Data = result
                    };
                    return new CustomActionResult<AddResultVM>(responseObj);

                }
                else
                {
                    return new CustomActionResult<AddResultVM>(new CustomActionResultVM<AddResultVM>
                    {
                        Exception = new Exception($"Recipe with Id {recipeId} not found.")
                    });
                }

            } catch(Exception ex)
            {
                return new CustomActionResult<AddResultVM>(new CustomActionResultVM<AddResultVM>
                {
                    Exception = ex
                });
            }  
            
        }

        [HttpDelete("deleteRecipe/{recipeId}")]
        public CustomActionResult<AddResultVM> DeleteRecipe(int recipeId)
        {
            try
            {
                RecipeVM recipeVM = _recipeRepository.GetById(recipeId);
                if(recipeVM != null)
                {
                    int deletedRecipeId = _recipeRepository.DeleteById(recipeId);
                    var result = new AddResultVM
                    {
                        Id = deletedRecipeId
                    };
                    var responseObj = new CustomActionResultVM<AddResultVM>
                    {
                        Data = result
                    };
                    return new CustomActionResult<AddResultVM>(responseObj);
                }
                else
                {
                    return new CustomActionResult<AddResultVM>(new CustomActionResultVM<AddResultVM>
                    {
                        Exception = new Exception($"Recipe with Id {recipeId} not found.")
                    });
                }
            }
            catch (Exception ex)
            {
                return new CustomActionResult<AddResultVM>(new CustomActionResultVM<AddResultVM>
                {
                    Exception = ex
                });
            }
        }


        [HttpPost("listAdvance")]
        public CustomActionResult<ResultView<PaginatedRecipes>> GetAlRecipes([FromBody] RecipeListAdvanceFilter recipeListAdvanceFilter)
        {
            try
            {
                var _recipes = _iRecipeRepository.GetAll(recipeListAdvanceFilter);
                var result = new ResultView<PaginatedRecipes>
                {
                    Item = _recipes

                };

                var responseObj = new CustomActionResultVM<ResultView<PaginatedRecipes>>
                {
                    Data = result

                };
                return new CustomActionResult<ResultView<PaginatedRecipes>>(responseObj);
            }
            catch (Exception ex)
            {
                var responseObj = new CustomActionResultVM<ResultView<PaginatedRecipes>>
                {
                    Exception = ex
                };

                return new CustomActionResult<ResultView<PaginatedRecipes>>(responseObj);
            }

        }

        [HttpGet("listSimpleRecipes")]
        public CustomActionResult<ResultView<RecipeListSimpleVM[]>> ListSimpleRecipes()
        {
            try
            {
                // Call the repository to get the list of simple FoodTypes
                RecipeListSimpleVM[] recipes = _iRecipeRepository.ListSimpeRecipes();

                var result = new ResultView<RecipeListSimpleVM[]>
                {
                    Item = recipes

                };

                var responseObj = new CustomActionResultVM<ResultView<RecipeListSimpleVM[]>>
                {
                    Data = result

                };
                return new CustomActionResult<ResultView<RecipeListSimpleVM[]>>(responseObj);
            }
            catch (Exception ex)
            {
                var responseObj = new CustomActionResultVM<ResultView<RecipeListSimpleVM[]>>
                {
                    Exception = ex
                };

                return new CustomActionResult<ResultView<RecipeListSimpleVM[]>>(responseObj);
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
