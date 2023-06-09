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
        public Order(Product prod, int orderedQuant, decimal totalPrice, Store store)
        {
            //this.Person = person;
            this.Prod = prod;
            this.OrderedQuant = orderedQuant;
            this.TotalPrice = totalPrice;
            this.Store = store;
        }

        public Guid OrderId { set; get; }
        public DateTime OrderTime { get; set; }
        //public Person Person { get; set; }

        public Product Prod { get; set; }

        public int OrderedQuant { get; set; }

        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }

        public Store Store { get; set; }

        public OrderPerson? Customer { get; set; }





    }


}