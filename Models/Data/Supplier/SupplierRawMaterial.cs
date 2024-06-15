using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Data.Supplier
{
    public class SupplierRawMaterial
    {
        public int Id { get; set; }
        public int RawMaterialId { get; set; }
        public int SupplierId { get; set; }
        public Suppliers Supplier { get; set; }
    }
}
