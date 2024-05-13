using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Requests.Update_Requests
{
    public class UpdateProduct
    {
        public DateTime? AddedDate { get; set; }
        public string ProductDescription { get; set; }
        public double? ProductPrice { get; set; }
        public string ImageURL { get; set; }
        public bool? IsSold { get; set; }
        public int Id { get; set; }
    }
}
