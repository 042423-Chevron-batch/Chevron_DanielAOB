using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ChainStoreApiBusiness;
using ChainStoreApiModel;
using System.Text.Json;




using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Microsoft.Extensions.Caching.Memory;

namespace ChainStoreApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChainStoreApi : ControllerBase
    {
        private readonly ILogger<ChainStoreApi> _logger;
        private readonly IMemoryCache _memoryCache;


        public ChainStoreApi(ILogger<ChainStoreApi> logger, IMemoryCache memoryCache)
        {
            _logger = logger;
            _memoryCache = memoryCache;
        }



        public AddCustToDb addCustToDb = new AddCustToDb();


        public Store? selectedStoreObj;




        // we will add another method
        [HttpPost("RegisterUser")]// define what verb this action method requires
        public ActionResult<Person> RegUser([FromBody] Register x)// get a json string object from the body and match it to the defined class.
        {



            ChainstoreBusinessUtilityLayer rpsb = new ChainstoreBusinessUtilityLayer();

            Person ret = rpsb.UserRegistration(x);
            if (ret != null)
            {
                return Ok(ret);
            }
            else
            {
                return BadRequest(new { message = "There was a problem with the new registration" });

            }

            // return Created("http://www.mysite.com/path/to/this/resource/on/the/web", ret1);

        }
        //[HttpGet("login/username/password")]

        [HttpPost("login")]
        public ActionResult<Person> Login([FromBody] LogIn ln)
        {
            //create an instance of the business layer
            ChainstoreBusinessUtilityLayer rpsb = new ChainstoreBusinessUtilityLayer();
            // send the loginDto to the business layer to do whatever it does.
            Person? p = rpsb.UserLogin(ln);
            if (p == null)
            {
                return BadRequest(new { message = "There is not yet a user with that login/password combo." });
            }
            else
            {

                SavedUserOb.logInUser = p;
                return Ok(p!);
            }
        }

        //Show customer all store locations
        [HttpGet("availableLocations")]
        public ActionResult<List<string>> StoreLocation()
        {
            ChainstoreBusinessUtilityLayer rpsb = new ChainstoreBusinessUtilityLayer();
            List<Store> stores = rpsb.GetStoreLoc();

            if (stores != null && stores.Count > 0)
            {
                List<string> storeLocations = stores.Select(store => store.StoreLoc).ToList();

                HashSet<string> distinctLocations = new HashSet<string>(storeLocations);



                return Ok(distinctLocations);
            }
            else
            {
                return NotFound();
            }
        }


        [HttpPost("SelectStoreLocation")]
        public ActionResult<List<Store>> SelectLocation([FromBody] LocationRequest locationRequest)
        {
            string selectedLocation = locationRequest.SelectLocation;
            ChainstoreBusinessUtilityLayer rpsb = new ChainstoreBusinessUtilityLayer();
            List<Store>? stores = rpsb.getStores();

            if (stores != null)
            {
                List<Store>? availableStores = new List<Store>();

                foreach (Store store in stores!)
                {
                    if (store.StoreLoc == selectedLocation)
                    {

                        // Found the selected store location
                        availableStores.Add(new Store
                        (
                            store.StoreId,
                            store.StoreName,
                            store.StoreLoc
                        ));

                    }
                }

                if (availableStores != null)
                {

                    SavedStoreObj.AvailableStores = availableStores;

                    List<string> StoreNames = availableStores.Select(s => s.StoreName).ToList();

                    return Ok(StoreNames);
                }
                else
                {
                    // No stores found in the selected location
                    return NotFound();
                }
            }
            else
            {
                // Unable to retrieve stores
                return StatusCode(500, "Unable to retrieve stores. Please try again later.");
            }
        }


        //Get all products in the chosen store

        [HttpPost("GetProducts")]
        public ActionResult<List<Product>> ProductsInStore([FromBody] StoreRequest storeRequest)
        {
            List<Store> availableStores = SavedStoreObj.AvailableStores;

            if (storeRequest is not null)
            {
                selectedStoreObj = availableStores!.FirstOrDefault(s => s.StoreName == storeRequest.selectStore);



                if (selectedStoreObj != null)
                {

                    _memoryCache.Set("SelectedStore", selectedStoreObj);
                    // create an instance of the business layer class.
                    ChainstoreBusinessUtilityLayer rpsb = new ChainstoreBusinessUtilityLayer();
                    //List<Product> products = rpsb.productsInStore();
                    List<Product> products = rpsb.productsInStore(selectedStoreObj);




                    if (products.Count > 0)
                    {

                        SavedProdObj.ProductSaved = products;

                        return Ok(products);
                    }
                    else
                    {
                        return NotFound();

                    }
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return StatusCode(500);
            }
        }



        //Let customer choose product(s) and quantities
        [HttpPost("ChooseProduct")]
        public ActionResult<Order> SelectProduct([FromBody] ProdQuantRequest prodAndQuant)
        {
            List<Product> availableProducts = SavedProdObj.ProductSaved;

            if (availableProducts != null)
            {
                Product? selectedProductObj = availableProducts!.FirstOrDefault(p => p.Productname == prodAndQuant.SelectProduct);

                //Console.Write(selectedStoreObj!.StoreLoc);
                Console.Write(selectedProductObj!.Productname);

                if (selectedProductObj != null)
                {
                    // Retrieve the quantity of the selected product
                    int Availablequantity = selectedProductObj.Prodquant;

                    if (prodAndQuant.orderQuant <= Availablequantity)
                    {

                        if (!_memoryCache.TryGetValue("SelectedStore", out selectedStoreObj))
                        {
                            return BadRequest(new { message = "No store selected." });
                        }


                        //Add customer order to AddCustToDb
                        Order order = new Order(selectedProductObj.ProductId, selectedProductObj.Productname!, selectedProductObj.Description!, prodAndQuant.orderQuant, selectedProductObj.Price, prodAndQuant.orderQuant * selectedProductObj.Price, selectedStoreObj!);

                        // Add the order to the Orders list in AddCustToDb
                        addCustToDb.Orders.Add(order);

                        Person loggedInPerson = SavedUserOb.logInUser;

                        ChainstoreBusinessUtilityLayer rpsp = new ChainstoreBusinessUtilityLayer();
                        (bool PurchAddedToOrder, Order OrderedItem) = rpsp.AddCustomerPurchase(addCustToDb.Orders, loggedInPerson);

                        if (PurchAddedToOrder)
                        {
                            ChainstoreBusinessUtilityLayer rpsb = new ChainstoreBusinessUtilityLayer();

                            // Decrease inventory and process the order

                            bool UpdatedInventory = rpsb.DecreaseInventory(selectedProductObj.Productname, prodAndQuant.orderQuant);

                            return OrderedItem;
                        }
                        else
                        {

                            return BadRequest(new { message = "Your order was not successful, maybe ordered quantity is more than what we have in stock" });
                        }
                    }
                    else
                    {
                        //insufficient quantities
                        return BadRequest(new { message = "Quantity more than what we have in stock" });
                    }
                }
                else
                {
                    // Selected product not found
                    return BadRequest(new { message = "Did yo select a product and quantity? " });
                }
            }
            else
            {
                // Error occurred in retrieving the products
                return BadRequest(new { message = "Product object is empty " });
            }

        }

        //Get order history of customer

        [HttpPost("CustOrderHistory")]
        public ActionResult<List<orderHist>> OrderHist([FromBody] LogIn logindetail)
        {

            // create an instance of the business layer class.
            ChainstoreBusinessUtilityLayer rpsb = new ChainstoreBusinessUtilityLayer();
            // call the business layer method to get the stores.
            List<orderHist>? ordersHist = rpsb.OrderHist(logindetail);

            if (ordersHist != null) return Ok(ordersHist);
            else return StatusCode(422, new { message = "There was a problem fetching the stores list. Please try again." });
        }


        //customerStatistics

        [HttpPost("customerStatistics")]
        public ActionResult<OrderStat> OrderStats([FromBody] LogIn logindetail)
        {

            // create an instance of the business layer class.
            ChainstoreBusinessUtilityLayer rpsb = new ChainstoreBusinessUtilityLayer();
            // call the business layer method to get the stores.
            OrderStat orderStatics = rpsb.customerStatistics(logindetail);

            if (orderStatics != null) return Ok(orderStatics);
            else return StatusCode(422, new { message = "There was a problem fetching order statistics. Please try again." });
        }

    }// EoC
}// EoN
