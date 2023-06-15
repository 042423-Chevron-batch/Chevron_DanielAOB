using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace ChainStoreApiModel
{
    public class Order
    {

        public Order() { }
        public Order(Guid productId, string productname, string description, int orderedQuant, decimal unitPrice, decimal totalPrice, Store store)
        {
            //this.Person = person;
            this.ProductId = productId;
            this.Productname = productname;
            this.Description = description;
            this.OrderedQuant = orderedQuant;
            this.TotalPrice = totalPrice;
            this.Store = store;
            this.UnitPrice = unitPrice;
        }

        public Guid OrderId { set; get; }
        public DateTime OrderTime { get; set; }
        //public Person Person { get; set; }

        public Guid ProductId { get; set; }
        public string Productname { get; set; }
        public string Description { get; set; }

        public int OrderedQuant { get; set; }

        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }

        public Store Store { get; set; }

        // public OrderPerson? Customer { get; set; }





    }


}