using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Data.Stock
{
    public class GRN
    {
        public int Id { get; set; }
        public string GRNNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public GRN() {
            IsDeleted = false;
        }
    }

    public class GRNSequenceValue
    {
        public int Value { get; set; }
    }
}
