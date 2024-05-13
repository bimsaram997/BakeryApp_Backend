﻿using Models.Data.RawMaterialData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Requests.Update_Requests
{
    public class UpdateRawMaterial
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string ImageURL { get; set; }
        public MeasureUnit MeasureUnit { get; set; }
    }
}
