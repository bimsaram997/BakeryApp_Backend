﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Data.User
{
    public class LoginResult
    {
        public string Token { get; set; }
        public Exception? Exception { get; set; }
    }
}
