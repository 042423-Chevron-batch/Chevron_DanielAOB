


using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChainStoreApiModel
{
    public class LogIn
    {
        [Required]
        public string userName { get; set; }

        [Required]
        public string password { get; set; }

    }
}