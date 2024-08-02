using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Requests.Update_Requests
{
    public class UpdateRole
    {
        public string RoleName { get; set; }
        public int Status { get; set; }
        public string? RoleDescription { get; set; }
        public int LocationId { get; set; }
    }
}
