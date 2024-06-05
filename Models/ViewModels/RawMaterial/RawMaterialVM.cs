using Models.Data.RawMaterialData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels.RawMaterial
{
    public class RawMaterialVM
    {
        public int Id { get; set; }
        public string RawMaterialCode { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int LocationId { get; set; }
        public double Quantity { get; set; }
        public DateTime? AddedDate { get; set; }
        public string ImageURL { get; set; }
        public int MeasureUnit { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? ModifiedDate { get; set; }

    }

    public class RawMaterialListSimpleVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Quantity { get; set; }
        public int MeasureUnit { get; set; }
 


    }
}
