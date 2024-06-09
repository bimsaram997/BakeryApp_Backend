using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels.Custom_action_result
{
    public class CustomActionResultVM<T>
    {
        public Exception Exception { get; set; }
        public T Data { get; set; }
    }
}
