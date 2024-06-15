using Microsoft.AspNetCore.Mvc;
using Models.ActionResults;
using Models.Requests;
using Models.ViewModels.Custom_action_result;
using Models.ViewModels;
using Models.ViewModels.RawMaterial;
using Models.ViewModels.Recipe;
using Models.ViewModels.Supplier;
using Repositories;
using Repositories.RecipeRepository;
using Models.Data.ReferenceData;
using Models.ViewModels.Address;
using Models.ViewModels.MasterData;

namespace BakeryApp.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController: ControllerBase
    {
        public IRepositoryBase<SupplierVM> _supplierRepository;
        public IRepositoryBase<AddressVM> _iAddressRepository;
        public IRepositoryBase<MasterDataVM> _masterDataRepository;
        public SupplierController(IRepositoryBase<SupplierVM> supplierRepository,
            IRepositoryBase<AddressVM> addressRepository, IRepositoryBase<MasterDataVM> masterDataRepository
            )
        {
            _supplierRepository = supplierRepository;
            _iAddressRepository = addressRepository;
            _masterDataRepository = masterDataRepository;
        }

        [HttpPost("addRecipe")]
        public CustomActionResult<AddResultVM> AddRecipe([FromBody] AddSupplierRquest supplierRequest)
        {

            try
            {
                MasterDataVM _masterData = _masterDataRepository.GetById(supplierRequest.Address.Country);
                var address = new AddressVM
                {
                    FullAddress = supplierRequest.Address.Street1 + " " + supplierRequest.Address.Street2 + " " + supplierRequest.Address.PostalCode + " " +
                    supplierRequest.Address.City + " " + _masterData.MasterDataName,
                    AddedDate = supplierRequest.AddedDate,
                    City = supplierRequest.Address.City,
                    Country = supplierRequest.Address.Country,
                    PostalCode = supplierRequest.Address.PostalCode,
                    Street1 = supplierRequest.Address.Street1,
                    Street2 = supplierRequest.Address?.Street2,
                };
                int addressId = _iAddressRepository.Add(address);
                var supplier = new SupplierVM
                {
                  SupplierFirstName = supplierRequest.SupplierFirstName,
                  SupplierLastName = supplierRequest.SupplierLastName,
                  AddedDate = supplierRequest.AddedDate,
                  PhoneNumber = supplierRequest.PhoneNumber,
                  Email = supplierRequest.Email,
                  IsProduct = supplierRequest.IsProduct,
                  IsRawMaterial = supplierRequest.IsRawMaterial,
                  AddressId = addressId,
                  ProductIds = supplierRequest.ProductIds,
                  RawMaterialIds = supplierRequest.RawMaterialIds

                };
                int supplierId = _supplierRepository.Add(supplier);
                if (supplierId > 0)
                {
                    var result = new AddResultVM
                    {
                        Id = supplierId
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
                        Exception = new Exception("Supplier can't add!")
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
    }
}
