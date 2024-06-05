using Microsoft.EntityFrameworkCore;
using Models.Data;
using Models.Data.EnumType;
using Models.Data.User;
using Models.Filters;
using Models.Requests;
using Models.ViewModels.EnumType;
using Models.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.EnumTypeRepository
{
    public interface IEnumTypeRepository
    {
        
        public ReturnEnumTypes GetAll();

    }
    public class EnumTypeRepository: IEnumTypeRepository
    {
        private AppDbContext _context;

        public EnumTypeRepository(AppDbContext context)
        {
            _context = context;
        }

        public ReturnEnumTypes GetAll()
        {
            IQueryable<EnumTypeTranslationMap> query = _context.EnumTypeTranslationMap
       .Where(fi => !fi.IsDeleted);
            var values = query.Select(fi => new AllEnumTypeVM
            {
                Id = fi.Id,
                EnumTypeName = fi.EnumTypeName,
                AddedDate = fi.AddedDate,
                ModifiedDate= fi.ModifiedDate,
                EnumTypeDisplayValue = fi.EnumTypeDisplayValue,
            }).ToList();

            var result = new ReturnEnumTypes
            {
                Items = values
            };
            return result;


        }

    
        
    }
}
