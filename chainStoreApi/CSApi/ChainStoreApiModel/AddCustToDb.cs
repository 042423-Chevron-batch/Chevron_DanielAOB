

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ChainStoreApiModel;

namespace ChainStoreApiModel
{
    public class AddCustToDb
    {


        public List<Order> Orders { get; set; } = new List<Order>();

        public Person OrderPerson { get; set; } = new Person();


    }


}

