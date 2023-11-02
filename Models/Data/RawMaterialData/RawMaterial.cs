﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Data.RawMaterialData
{
    public class RawMaterial
    {
        public int Id { get; set; }
        public string RawMaterialCode { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public DateTime? AddedDate { get; set; }
        public string ImageURL { get; set; }
    }
}
