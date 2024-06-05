using Models.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels.EnumType
{
    public class AllEnumTypeVM
    {
        
            public int Id { get; set; }
            public string EnumTypeName { get; set; }
            public string EnumTypeDisplayValue { get; set; }
            public DateTime AddedDate { get; set; }
            public DateTime? ModifiedDate { get; set; }
            public bool IsDeleted { get; set; }

    }
    public class ReturnEnumTypes
    {
        public List<AllEnumTypeVM> Items { get; set; }
    }
}
