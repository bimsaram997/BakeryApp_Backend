using Models.Data.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class ResultView<T>
    {
        public T? Item { get; set; }
        public Exception? Exception { get; set; }
    }
}
