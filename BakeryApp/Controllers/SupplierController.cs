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
using Repositories.SupplierRepository;
using Models.Filters;
using Models.Data.User;
using Models.Requests.Update_Requests;

namespace BakeryApp.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController: ControllerBase
    {
        public IRepositoryBase<SupplierVM> _supplierRepository;
        public IRepositoryBase<AddressVM> _iAddressRepository;
        public IRepositoryBase<MasterDataVM> _masterDataRepository;
        public ISupplierRepository _iSupplierRepository;
        public SupplierController(IRepositoryBase<SupplierVM> supplierRepository,
            IRepositoryBase<AddressVM> addressRepository, IRepositoryBase<MasterDataVM> masterDataRepository, ISupplierRepository iSupplerRepository
            )
        {
            _supplierRepository = supplierRepository;
            _iAddressRepository = addressRepository;
            _masterDataRepository = masterDataRepository;
            _iSupplierRepository = iSupplerRepository;
        }

        [HttpPost("addSupplier")]
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
                    ProductIds = supplierRequest.ProductIds?.Select(id => (int?)id).ToList(),
                    RawMaterialIds = supplierRequest?.RawMaterialIds?.Select(id => (int?)id).ToList()

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

        [HttpGet("getSupplierById/{supplierId}")]
        public CustomActionResult<ResultView<SupplierVM>> GetSupplierById(int supplierId)
        {
            try
            {
                SupplierVM user = _supplierRepository.GetById(supplierId);
                if (user != null)
                {
                    var result = new ResultView<SupplierVM>
                    {
                        Item = user

                    };

                    var responseObj = new CustomActionResultVM<ResultView<SupplierVM>>
                    {
                        Data = result

                    };
                    return new CustomActionResult<ResultView<SupplierVM>>(responseObj);
                }
                else
                {
                    var result = new ResultView<UserDetailVM>
                    {
                        Exception = new Exception($"Supplier with Id {supplierId} not found")
                    };

                    var responseObj = new CustomActionResultVM<ResultView<SupplierVM>>
                    {
                        Exception = result.Exception
                    };

                    return new CustomActionResult<ResultView<SupplierVM>>(responseObj);
                }
            }
            catch (Exception ex)
            {
                var responseObj = new CustomActionResultVM<ResultView<SupplierVM>>
                {
                    Exception = ex
                };

                return new CustomActionResult<ResultView<SupplierVM>>(responseObj);
            }
        }


        [HttpPost("listAdvance")]
        public CustomActionResult<ResultView<PaginatedSuppliers>> GetAllSuppliers([FromBody] SupplierListAdvanceFilter supplierAdvanceListFilter)
        {
            try
            {
                PaginatedSuppliers _suppliers = _iSupplierRepository.GetAll(supplierAdvanceListFilter);


                var result = new ResultView<PaginatedSuppliers>
                {
                    Item = _suppliers

                };

                var responseObj = new CustomActionResultVM<ResultView<PaginatedSuppliers>>
                {
                    Data = result

                };
                return new CustomActionResult<ResultView<PaginatedSuppliers>>(responseObj);
            }
            catch (Exception ex)
            {
                var responseObj = new CustomActionResultVM<ResultView<PaginatedSuppliers>>
                {
                    Exception = ex
                };

                return new CustomActionResult<ResultView<PaginatedSuppliers>>(responseObj);
            }

        }

        [HttpPut("updateSupplier/{supplierId}")]
        public CustomActionResult<AddResultVM> UpdateSupplier(int supplierId, [FromBody] UpdateSupplier updateSupplier)
        {

            try
            {

                SupplierVM supplierVM = _supplierRepository.GetById(supplierId);
                if (supplierVM != null)
                {
                    MasterDataVM _masterData = _masterDataRepository.GetById(updateSupplier.Address.Country);
                    int addressId;
                    AddressVM cuurentAddress = _iAddressRepository.GetById(updateSupplier.AddressId);
                    if (cuurentAddress != null && _masterData != null)
                    {
                        AddressVM address = new AddressVM
                        {
                            FullAddress = updateSupplier.Address.Street1 + " " + updateSupplier.Address.Street2 + " " + updateSupplier.Address.PostalCode + " " +
                        updateSupplier.Address.City + " " + _masterData.MasterDataName,
                            Street1 = updateSupplier.Address.Street1,
                            Street2 = updateSupplier.Address.Street2,
                            City = updateSupplier.Address.City,
                            Country = updateSupplier.Address.Country,
                            PostalCode = updateSupplier.Address.PostalCode
                        };
                        addressId = _iAddressRepository.UpdateById(updateSupplier.AddressId, address);
                    }
                    else
                    {
                        var address = new AddressVM
                        {
                            FullAddress = updateSupplier.Address.Street1 + " " + updateSupplier.Address.Street2 + " " + updateSupplier.Address.PostalCode + " " +
                        updateSupplier.Address.City + " " + updateSupplier.Address.Country,
                            Street1 = updateSupplier.Address.Street1,
                            Street2 = updateSupplier.Address.Street2,
                            City = updateSupplier.Address.City,
                            Country = updateSupplier.Address.Country,
                            PostalCode = updateSupplier.Address.PostalCode
                        };
                        addressId = _iAddressRepository.Add(address);
                    }


                    SupplierVM supplier = new SupplierVM
                    {
                        SupplierFirstName = updateSupplier.SupplierFirstName,
                        SupplierLastName = updateSupplier.SupplierLastName,
                        PhoneNumber = updateSupplier.PhoneNumber,
                        IsProduct = updateSupplier.IsProduct,
                        IsRawMaterial = updateSupplier.IsRawMaterial,
                        Email = updateSupplier.Email,
                        ProductIds = updateSupplier.ProductIds?.Select(id => (int?)id).ToList(),
                        RawMaterialIds = updateSupplier?.RawMaterialIds?.Select(id => (int?)id).ToList(),

                        AddressId = addressId
                    };
                    int updatedUserId = _supplierRepository.UpdateById(supplierId, supplier);
                    var result = new AddResultVM
                    {
                        Id = updatedUserId
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
                        Exception = new Exception($"Supplier with Id {supplierId} not found.")
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

        [HttpPost("listSimpleSuppliers")]
        public CustomActionResult<ResultView<SupplierListSimpleVM[]>> GetSupplierListSimple([FromBody] SupplerListSimpleFilter supplerListSimpleFilter)
        {
            try
            {
                SupplierListSimpleVM[] _suppliers = _iSupplierRepository.GetSupplierListSimple(supplerListSimpleFilter);


                var result = new ResultView<SupplierListSimpleVM[]>
                {
                    Item = _suppliers

                };

                var responseObj = new CustomActionResultVM<ResultView<SupplierListSimpleVM[]>>
                {
                    Data = result

                };
                return new CustomActionResult<ResultView<SupplierListSimpleVM[]>>(responseObj);
            }
            catch (Exception ex)
            {
                var responseObj = new CustomActionResultVM<ResultView<SupplierListSimpleVM[]>>
                {
                    Exception = ex
                };

                return new CustomActionResult<ResultView<SupplierListSimpleVM[]>>(responseObj);
            }

        }
    }
}
