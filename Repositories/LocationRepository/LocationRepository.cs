using Models.Data;
using Models.Data.Location;
using Models.Data.RawMaterialData;
using Models.Filters;
using Models.Helpers;
using Models.ViewModels.Address;
using Models.ViewModels.Location;
using Models.ViewModels.Product;
using Models.ViewModels.RawMaterial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.LocationRepository
{
    public interface ILocationRepository
    {
        LocationlListSimpleVM[] LocationlListSimpleVM();
        PaginatedLocations GetAll(LocationAdvanceFilter filter);
    }
    public class LocationRepository : IRepositoryBase<LocationVM>, ILocationRepository
    {
        private AppDbContext _context;
        public LocationRepository(AppDbContext context)
        {
            _context = context;
        }

        public int Add(LocationVM entity)
        {
            string newLocCode = Guid.NewGuid().ToString();
            var _location = new Locations()
            {
                LocationCode = newLocCode,
                LocationName = entity.LocationName,
                AddedDate = entity.AddedDate,
                AddressId = entity.AddressId,
                Status = entity.Status,
            };
            _context.Location.Add(_location);
            object value = _context.SaveChanges();
            int addedItemId = _location.Id;
            return addedItemId;
        }

        public int DeleteById(int id)
        {
            Locations location = _context.Location.FirstOrDefault(r => r.Id == id && !r.IsDeleted);
            if (location == null)
            {
                // Handle the case where the raw material with the given ID is not found
                return -1; // You might want to return an error code or throw an exception
            }
            location.IsDeleted = true;
            location.ModifiedDate = DateTime.Now;
            // Save changes to the database
            _context.SaveChanges();
            return location.Id;
        }

        public LocationVM GetById(int id)
        {
            LocationVM? location = _context.Location.Where(l => l.Id == id && !l.IsDeleted).Select(location => new LocationVM()
            {
                Id =  location.Id,
                LocationCode =  location.LocationCode,
                LocationName =  location.LocationName,
                Status = location.Status,
                AddedDate = location.AddedDate,
                ModifiedDate = location.ModifiedDate,
                IsDeleted = location.IsDeleted,
                AddressId = location.AddressId,
                Address = _context.Address
                 .Where(address => address.Id == location.AddressId)
                 .Select(address => new AddressVM
                 {
                     Id = address.Id,
                     FullAddress = address.FullAddress,
                     Street1 = address.Street1,
                     Street2 = address.Street2,
                     City = address.City,
                     Country = address.Country,
                     PostalCode = address.PostalCode,
                     AddedDate = address.AddedDate,
                     ModifiedDate = address.ModifiedDate,
                     IsDeleted = address.IsDeleted
                 })
                 .FirstOrDefault()
            }).FirstOrDefault();
            return location;
        }

        public int UpdateById(int id, LocationVM entity)
        {
            Locations? previousLocation = _context.Location.FirstOrDefault(l => l.Id == id && !l.IsDeleted);
            if (previousLocation == null)
            {
                // Handle the case where the  raw Material with the given ID is not found
                return -1; // You might want to return an error code or throw an exception
            }
            previousLocation.LocationName = entity.LocationName;
            previousLocation.Status = entity.Status;
            previousLocation.AddressId = entity.AddressId;
            previousLocation.ModifiedDate = DateTime.Now;

            _context.SaveChanges();
            return previousLocation.Id;
        }

        public LocationlListSimpleVM[] LocationlListSimpleVM()
        {
            var simpleLocation = _context.Location
                 .Where(ft => !ft.IsDeleted)
                 .Select(location => new LocationlListSimpleVM()
                 {
                     Id = location.Id,
                     LocationName = location.LocationName,
                 })
                 .ToArray();

            return simpleLocation;
        }


        public PaginatedLocations GetAll(LocationAdvanceFilter filter)
        {
            IQueryable<Locations> query = _context.Location
                .Where(fi => !fi.IsDeleted);

            // Apply filtering
            if (!string.IsNullOrEmpty(filter.SearchString))
            {
                query = query.Where(fi =>
                    fi.LocationName.Contains(filter.SearchString) || fi.LocationCode.Contains(filter.SearchString)
                );
            }

            if (filter.Status.HasValue)
            {
                query = query.Where(fi => fi.Status == filter.Status);
            }

            if (!string.IsNullOrEmpty(filter.AddedDate) && DateTime.TryParse(filter.AddedDate, out DateTime filterDate))
            {
                // Adjust date filter to consider the whole day
                DateTime nextDay = filterDate.AddDays(1);
                query = query.Where(fi => fi.AddedDate >= filterDate && fi.AddedDate < nextDay);
            }

            // Get total count before pagination
            int totalCount = query.Count();

            // Apply sorting
            query = SortHelper.ApplySorting(query.AsQueryable(), filter.SortBy, filter.IsAscending);

            // Apply pagination
            query = query.Skip((filter.Pagination.PageIndex - 1) * filter.Pagination.PageSize)
                         .Take(filter.Pagination.PageSize);

            // Project and materialize the results
            var paginatedResult = query
                .Select(fi => new AllLocationVM
                {
                    Id = fi.Id,
                    LocationName = fi.LocationName,
                    LocationCode = fi.LocationCode,
                    AddedDate = fi.AddedDate,
                    Status = fi.Status,
                    StatusName = _context.MasterData
                        .Where(masterData => masterData.Id == fi.Status)
                        .Select(masterData => masterData.MasterDataName)
                        .FirstOrDefault(),
                    Addresses = _context.Address
                        .Where(address => address.Id == fi.AddressId)
                        .Select(address => address)
                        .FirstOrDefault(),
               
                })
                .ToList();


            var result = new PaginatedLocations
            {
                Items = paginatedResult,
                TotalCount = totalCount,
                PageIndex = filter.Pagination.PageIndex,
                PageSize = filter.Pagination.PageSize
            };

            return result;
        }
    }
}
