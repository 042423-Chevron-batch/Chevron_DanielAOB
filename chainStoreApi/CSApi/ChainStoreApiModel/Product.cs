using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChainStoreApiModel
{
    public class Product
    {


        public Product(Guid productId, string prodname, decimal price, int prodquant, string description)
        {
            this.ProductId = productId;
            this.Productname = prodname;
            this.Price = price;
            this.Prodquant = prodquant;
            this.Description = description;


        }
        public Guid ProductId { get; set; }

        public string Productname { get; set; }

        public decimal Price { get; set; }
        public int Prodquant { get; set; }

        public string? Description { get; set; }









    }
}