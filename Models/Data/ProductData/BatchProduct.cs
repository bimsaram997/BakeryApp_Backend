using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Data.ProductData
{
    public class BatchProduct
    {
        public int Id { get; set; }
        public long BatchId { get; set; }
        public int ProductId { get; set; }
    }
}
