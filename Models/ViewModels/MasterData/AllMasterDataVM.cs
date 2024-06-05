using Models.ViewModels.EnumType;
using Models.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels.MasterData
{
    public class AllMasterDataVM
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
        public string EnumType { get; set; }
    }

    public class PaginatedMasterData
    {
        public List<AllMasterDataVM> Items { get; set; }
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
