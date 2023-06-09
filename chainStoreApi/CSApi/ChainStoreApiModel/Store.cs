using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChainStoreApiModel
{
    public class Store
    {
        public Store(Guid storeId, string storeName, string storeLocation)
        {

            this.StoreId = storeId;
            this.StoreName = storeName;
            this.StoreLoc = storeLocation;
        }

        public Guid StoreId { get; set; }
        public string StoreName { get; set; }
        public string StoreLoc { get; set; }

    }

}

