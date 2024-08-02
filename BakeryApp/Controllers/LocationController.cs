using Microsoft.AspNetCore.Mvc;
using Models.ViewModels.RawMaterial;
using Repositories.RecipeRepository;
using Repositories;
using Models.ViewModels.Location;
using Repositories.LocationRepository;
using Models.ActionResults;
using Models.Requests;
using Models.ViewModels.Address;
using Models.ViewModels.Custom_action_result;
using Models.ViewModels.MasterData;
using Models.ViewModels.Supplier;
using Models.ViewModels;
using Repositories.SupplierRepository;
using Models.Requests.Update_Requests;
using Models.Filters;
using Models.Data.Location;
using Repositories.RawMarerialRepository;

namespace BakeryApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private IRepositoryBase<LocationVM>_locationRepository;
        private ILocationRepository _ilocationRepository;
        public IRepositoryBase<MasterDataVM> _masterDataRepository;
        public ISupplierRepository _iSupplierRepository;
        public IRepositoryBase<AddressVM> _iAddressRepository;
        public  LocationController(IRepositoryBase<LocationVM> locationRepository,
            ILocationRepository ilocationRepository, IRepositoryBase<MasterDataVM> masterDataRepository,
            IRepositoryBase<AddressVM> addressRepository)
        {
            _locationRepository = locationRepository;
            _ilocationRepository = ilocationRepository;
            _masterDataRepository = masterDataRepository;
            _iAddressRepository = addressRepository;
        }


        [HttpPost("addLocation")]
        public CustomActionResult<AddResultVM> AddLocation([FromBody] AddLocationRequest locationRequest)
        {

            try
            {
                MasterDataVM _masterData = _masterDataRepository.GetById(locationRequest.Address.Country);
                var address = new AddressVM
                {
                    FullAddress = locationRequest.Address.Street1 + " " + locationRequest.Address.Street2 + " " + locationRequest.Address.PostalCode + " " +
                    locationRequest.Address.City + " " + _masterData.MasterDataName,
                    AddedDate = locationRequest.AddedDate,
                    City = locationRequest.Address.City,
                    Country = locationRequest.Address.Country,
                    PostalCode = locationRequest.Address.PostalCode,
                    Street1 = locationRequest.Address.Street1,
                    Street2 = locationRequest.Address?.Street2,
                };
                int addressId = _iAddressRepository.Add(address);
                var location = new LocationVM
                {
                    LocationName = locationRequest.LocationName,
                    Status = locationRequest.Status,
                    AddedDate = locationRequest.AddedDate,
                    AddressId = addressId,
                  

                };
                int locationId = _locationRepository.Add(location);
                if (locationId > 0)
                {
                    var result = new AddResultVM
                    {
                        Id = locationId
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
                        Exception = new Exception("Location can't add!")
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

        [HttpGet("listSimpleLocations")]
        public CustomActionResult<ResultView<LocationlListSimpleVM[]>> ListSimpleLocations()
        {
            try
            {
                // Call the repository to get the list of simple FoodTypes
                LocationlListSimpleVM[] locations = _ilocationRepository.LocationlListSimpleVM();

                var result = new ResultView<LocationlListSimpleVM[]>
                {
                    Item = locations

                };

                var responseObj = new CustomActionResultVM<ResultView<LocationlListSimpleVM[]>>
                {
                    Data = result

                };
                return new CustomActionResult<ResultView<LocationlListSimpleVM[]>>(responseObj);
            }
            catch (Exception ex)
            {
                var responseObj = new CustomActionResultVM<ResultView<LocationlListSimpleVM[]>>
                {
                    Exception = ex
                };

                return new CustomActionResult<ResultView<LocationlListSimpleVM[]>>(responseObj); ;
            }
        }

        [HttpGet("getLocationById/{locationId}")]
        public CustomActionResult<ResultView<LocationVM>> GetLocationById(int locationId)
        {
            try
            {
                LocationVM location = _locationRepository.GetById(locationId);
                if (location != null)
                {
                    var result = new ResultView<LocationVM>
                    {
                        Item = location

                    };

                    var responseObj = new CustomActionResultVM<ResultView<LocationVM>>
                    {
                        Data = result

                    };
                    return new CustomActionResult<ResultView<LocationVM>>(responseObj);
                }
                else
                {
                    var result = new ResultView<LocationVM>
                    {
                        Exception = new Exception($"Location with Id {locationId} not found")
                    };

                    var responseObj = new CustomActionResultVM<ResultView<LocationVM>>
                    {
                        Exception = result.Exception
                    };

                    return new CustomActionResult<ResultView<LocationVM>>(responseObj);
                }
            }
            catch (Exception ex)
            {
                var responseObj = new CustomActionResultVM<ResultView<LocationVM>>
                {
                    Exception = ex
                };

                return new CustomActionResult<ResultView<LocationVM>>(responseObj);
            }
        }

        [HttpPut("updateLocation/{locationId}")]
        public CustomActionResult<AddResultVM> UpdateLocation(int locationId, [FromBody] UpdateLocation updateLocation)
        {

            try
            {

                LocationVM locationVM = _locationRepository.GetById(locationId);
                if (locationVM != null)
                {
                    MasterDataVM _masterData = _masterDataRepository.GetById(updateLocation.Address.Country);
                    int addressId;
                    AddressVM cuurentAddress = _iAddressRepository.GetById(updateLocation.AddressId);
                    if (cuurentAddress != null && _masterData != null)
                    {
                        AddressVM address = new AddressVM
                        {
                            FullAddress = updateLocation.Address.Street1 + " " + updateLocation.Address.Street2 + " " + updateLocation.Address.PostalCode + " " +
                        updateLocation.Address.City + " " + _masterData.MasterDataName,
                            Street1 = updateLocation.Address.Street1,
                            Street2 = updateLocation.Address.Street2,
                            City = updateLocation.Address.City,
                            Country = updateLocation.Address.Country,
                            PostalCode = updateLocation.Address.PostalCode
                        };
                        addressId = _iAddressRepository.UpdateById(updateLocation.AddressId, address);
                    }
                    else
                    {
                        var address = new AddressVM
                        {
                            FullAddress = updateLocation.Address.Street1 + " " + updateLocation.Address.Street2 + " " + updateLocation.Address.PostalCode + " " +
                        updateLocation.Address.City + " " + updateLocation.Address.Country,
                            Street1 = updateLocation.Address.Street1,
                            Street2 = updateLocation.Address.Street2,
                            City = updateLocation.Address.City,
                            Country = updateLocation.Address.Country,
                            PostalCode = updateLocation.Address.PostalCode
                        };
                        addressId = _iAddressRepository.Add(address);
                    }


                    LocationVM location = new LocationVM
                    {
                        LocationName = updateLocation.LocationName,
                        Status = updateLocation.Status,
                        AddressId = addressId
                    };
                    int updateLocationId = _locationRepository.UpdateById(locationId, location);
                    var result = new AddResultVM
                    {
                        Id = updateLocationId
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
                        Exception = new Exception($"Location with Id {locationId} not found.")
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
        public CustomActionResult<ResultView<PaginatedLocations>> GetAllSuppliers([FromBody] LocationAdvanceFilter locationAdvanceFilter)
        {
            try
            {
                PaginatedLocations _location = _ilocationRepository.GetAll(locationAdvanceFilter);


                var result = new ResultView<PaginatedLocations>
                {
                    Item = _location

                };

                var responseObj = new CustomActionResultVM<ResultView<PaginatedLocations>>
                {
                    Data = result

                };
                return new CustomActionResult<ResultView<PaginatedLocations>>(responseObj);
            }
            catch (Exception ex)
            {
                var responseObj = new CustomActionResultVM<ResultView<PaginatedLocations>>
                {
                    Exception = ex
                };

                return new CustomActionResult<ResultView<PaginatedLocations>>(responseObj);
            }

        }

        
        [HttpDelete("deleteloocationById/{locationId}")]
        public CustomActionResult<AddResultVM> DeleteLocation(int locationId)
        {

            try
            {
                LocationVM locationVM = _locationRepository.GetById(locationId);
                int deletedlocationId = 0;
                if (locationVM != null)
                {
                     deletedlocationId = _locationRepository.DeleteById(locationId);
                }else
                {
                    return new CustomActionResult<AddResultVM>(new CustomActionResultVM<AddResultVM>
                    {
                        Exception = new Exception("Location can't found!")
                    });
                }

                if (deletedlocationId > 0)
                {
                    var result = new AddResultVM
                    {
                        Id = locationId
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
                        Exception = new Exception("Location can't delete!")
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
