using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace ChainStoreApiModel
{
    public class orderHist
    {
        public Guid OrderId { get; set; }
        public DateTime OrderTime { get; set; }
        public Guid CustomerId { get; set; }
        public string CustFirstName { get; set; }
        public string CustLastName { get; set; }
        public string UserName { get; set; }
        public string CustEmail { get; set; }
        public Guid ProductId { get; set; }
        public string ProdName { get; set; }
        public int OrderedQuant { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public Guid StoreId { get; set; }
        public string StoreLoc { get; set; }
        public string StoreName { get; set; }

        public orderHist(Guid orderId, DateTime orderTime, Guid customerId, string custFirstName, string custLastName, string username, string custEmail, Guid productId, string prodName, int orderedQuant, decimal unitPrice, decimal totalPrice, Guid storeId, string storeLoc, string storeName)
        {
            OrderId = orderId;
            OrderTime = orderTime;
            CustomerId = customerId;
            CustFirstName = custFirstName;
            CustLastName = custLastName;
            UserName = username;
            CustEmail = custEmail;
            ProductId = productId;
            ProdName = prodName;
            OrderedQuant = orderedQuant;
            UnitPrice = unitPrice;
            TotalPrice = totalPrice;
            StoreId = storeId;
            StoreLoc = storeLoc;
            StoreName = storeName;
        }
    }
}
