using Models.ViewModels.EnumType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class RolesVM
    {
       
            public int Id { get; set; }
            public string RoleName { get; set; }
            public DateTime AddedDate { get; set; }
            public DateTime? ModifiedDate { get; set; }
            public bool IsDeleted { get; set; }
            
        
    }

    public class ReturnRoles
    {
        public List<RolesVM> Items { get; set; }
    }
}
