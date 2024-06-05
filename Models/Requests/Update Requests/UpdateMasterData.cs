using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Requests.Update_Requests
{
    public  class UpdateMasterData
    {
        public string MasterDataName { get; set; }
        public int EnumTypeId { get; set; }
        public string? MasterDataSymbol { get; set; }
        public string? MasterColorCode { get; set; }
    
    }
}
