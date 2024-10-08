﻿using Models.Data.RawMaterialData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels.RawMaterial
{

    public class AllRawMaterialVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string RawMaterialCode { get; set; }
        public DateTime? AddedDate { get; set; }
        public int MeasureUnit { get; set; }
        public double? Quantity { get; set; }
        public double Price { get; set; }
        public string ImageURL { get; set; }
        public int LocationId { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string MeasureUnitName { get; set; }
    }


    public class PaginatedRawMaterials
    {
        public List<AllRawMaterialVM> Items { get; set; }
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
