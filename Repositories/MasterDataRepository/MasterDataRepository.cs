using Models.Data;
using Models.Data.ProductData;
using Models.Data.ReferenceData;
using Models.Filters;
using Models.Helpers;
using Models.Requests.Update_Requests;
using Models.ViewModels;
using Models.ViewModels.MasterData;
using Models.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.MasterDataRepository
{
    public interface IMasterDataRepository
    {

        AllMasterData GetByEnumType(int enumTypeId);
        PaginatedMasterData GetAll(MasterDataListAdvanceFilter filter);
    }
    public class MasterDataRepository : IRepositoryBase<MasterDataVM>, IMasterDataRepository
    {
        private AppDbContext _context;
        public MasterDataRepository(AppDbContext context)
        {
            _context = context;
        }
        public int Add(MasterDataVM entity)
        {
            string newMatserDataCode = Guid.NewGuid().ToString();
            var _masterData = new MasterData()
            {
                MasterDataCode = newMatserDataCode,
                MasterDataName = entity.MasterDataName,
                MasterColorCode = entity.MasterColorCode,
                MasterDataSymbol = entity.MasterDataSymbol,
                AddedDate = entity.AddedDate,
                EnumTypeId = entity.EnumTypeId

            };
            _context.MasterData.Add(_masterData);
            object value = _context.SaveChanges();

       
            int addedItemId = _masterData.Id;

            return addedItemId;
        }

        public int DeleteById(int id)
        {
            MasterData? previousMasterData = _context.MasterData.FirstOrDefault(p => p.Id == id && !p.IsDeleted);
            if (previousMasterData == null)
            {
                return -1;
            }
            MasterData updateMasterData = previousMasterData;
            updateMasterData.ModifiedDate = DateTime.Now;
            updateMasterData.IsDeleted = true;
            _context.SaveChanges();
            return updateMasterData.Id;
        }

        public AllMasterData GetByEnumType(int enumTypeId)
        {
            var masterDataVM = _context.MasterData.Where(fi => fi.EnumTypeId == enumTypeId && !fi.IsDeleted)
                .Select(fi => new MasterDataVM
                {
                    Id = fi.Id,
                    MasterDataCode = fi.MasterDataCode,
                    MasterDataSymbol = fi.MasterDataSymbol,
                    MasterDataName = fi.MasterDataName,
                    MasterColorCode = fi.MasterColorCode,
                    EnumTypeId = fi.EnumTypeId,
                    AddedDate = fi.AddedDate,
                    ModifiedDate = fi.ModifiedDate,
                    IsDeleted = fi.IsDeleted
                }).ToList();

            var result = new AllMasterData
            {
                Items = masterDataVM
            };

            return result;
        }

        public MasterDataVM GetById(int id)
        {
            MasterDataVM? masterDataVM = _context.MasterData.Where(fi => fi.Id == id && !fi.IsDeleted)
                .Select(fi=> new MasterDataVM
                {
                    Id = fi.Id,
                    MasterDataCode = fi.MasterDataCode,
                    MasterDataSymbol = fi.MasterDataSymbol,
                    MasterDataName = fi.MasterDataName,
                    MasterColorCode = fi.MasterColorCode,
                    EnumTypeId =  fi.EnumTypeId,
                    AddedDate = fi.AddedDate,
                    ModifiedDate =  fi.ModifiedDate,
                    IsDeleted = fi.IsDeleted
                }).FirstOrDefault();
            return masterDataVM;
        }

        public int UpdateById(int id, MasterDataVM entity)
        {
            MasterData? previousMasterData = _context.MasterData.FirstOrDefault(p => p.Id == id && !p.IsDeleted);
            if (previousMasterData == null)
            {
                return -1;
            }
            MasterData updateMasterData = previousMasterData;
            updateMasterData.ModifiedDate = DateTime.Now;
            updateMasterData.MasterDataName = entity.MasterDataName;
            updateMasterData.MasterDataSymbol = entity.MasterDataSymbol;
            updateMasterData.MasterColorCode = entity.MasterColorCode;
            updateMasterData.EnumTypeId = entity.EnumTypeId;

            _context.SaveChanges();
            return updateMasterData.Id;
        }

        public PaginatedMasterData GetAll(MasterDataListAdvanceFilter filter)
        {
            IQueryable<MasterData> query = _context.MasterData
                .Where(fi => !fi.IsDeleted);

            query = SortHelper.ApplySorting(query.AsQueryable(), filter.SortBy, filter.IsAscending);

            if (filter != null)
            {
              
                if (filter.EnumTypeId.HasValue)
                {
                    query = query.Where(fi => fi.EnumTypeId == filter.EnumTypeId);
                }

                

                if (!string.IsNullOrEmpty(filter.SearchString))
                {
                    query = query.Where(fi =>
                        fi.MasterDataCode.Contains(filter.SearchString) || fi.MasterDataSymbol.Contains(filter.SearchString) ||
                        fi.MasterDataName.Contains(filter.SearchString) ||
                        fi.MasterDataSymbol.Contains(filter.SearchString)
                    );
                }



                if (!string.IsNullOrEmpty(filter.AddedDate))
                {
                    if (DateTime.TryParse(filter.AddedDate, out DateTime filterDate))
                    {
                        query = query.Where(fi => fi.AddedDate >= filterDate && fi.AddedDate < filterDate.AddDays(1));
                    }

                }
            }

            int totalCount = query.Count();

            query = query.Skip((filter.Pagination.PageIndex - 1) * filter.Pagination.PageSize).Take(filter.Pagination.PageSize);

            var paginatedResult = query
                .Select(fi => new AllMasterDataVM
                {
                    Id = fi.Id,
                    MasterDataSymbol = fi.MasterDataSymbol,
                    MasterDataName = fi.MasterDataName,
                    AddedDate = fi.AddedDate,
                    MasterColorCode = fi.MasterColorCode,
                    ModifiedDate = fi.ModifiedDate,
                   MasterDataCode =  fi.MasterDataCode,
                    EnumType = _context.EnumTypeTranslationMap
                        .Where(enumType => enumType.Id == fi.EnumTypeId)
                        .Select(enumType => enumType.EnumTypeDisplayValue)
                        .FirstOrDefault()


                })
                .ToList();

            var result = new PaginatedMasterData
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
