using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;



namespace ChainStoreApiModel
{
    public class LocationStoreRequest
    {

        public string SelectLocation { get; set; }
        public string SelectStore { get; set; }

    }
}