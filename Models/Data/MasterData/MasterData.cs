using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Data.ReferenceData
{
    public class MasterData
    {
        public int Id { get; set; }
        public string MasterDataCode { get; set; }
        public string MasterDataName { get; set; }
        public int EnumTypeId { get; set; }
        public string? MasterDataSymbol { get; set; }
        public string? MasterColorCode { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
   
        public MasterData()
        {
            IsDeleted = false;

        }
    }
}
