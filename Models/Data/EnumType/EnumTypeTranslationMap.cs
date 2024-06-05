using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Data.EnumType
{
    public class EnumTypeTranslationMap
    {
        public int Id { get; set; }
        public string EnumTypeName { get; set; }
        public string EnumTypeDisplayValue { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
 
        public EnumTypeTranslationMap()
        {
            IsDeleted = false;
        }
    }

    public enum EnumType
    {
        Currency = 1,
        MeasuringUnit = 2,
         ItemUnit = 8,
        Gender = 9,
        Roles=11
    }
}
