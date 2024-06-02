using Models.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Helpers
{
    public class EnumTranslator
    {
        public static string GetRolesString(IEnumerable<Role> roles)
        {
            return string.Join(",", roles.Select(r => r.ToString()));
        }
    }
}
