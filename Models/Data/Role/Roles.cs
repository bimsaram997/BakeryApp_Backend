using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Data.Role
{
    public class Roles
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public int Status { get; set; }
        public string? RoleDescription { get; set; }
        public int LocationId { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public Roles()
        {
            IsDeleted = false;
            Status = 22;
        }
    }
}
