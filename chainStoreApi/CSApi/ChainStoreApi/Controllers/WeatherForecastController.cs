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
using Newtonsoft.Json;

using System.Text.Json.Serialization;

namespace ChainStoreApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChainStoreApi : ControllerBase
    {
        private readonly ILogger<ChainStoreApi> _logger;


        public ChainStoreApi(ILogger<ChainStoreApi> logger)
        {
            _logger = logger;
        }





        public Store? selectedStoreObj = null;
        public AddCustToDb addCustToDb = new AddCustToDb();


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
                HttpContext.Session.SetString("LoggedInPerson", JsonConvert.SerializeObject(p));

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
                return Ok(storeLocations);
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
                        availableStores!.Add(new Store
                        (
                            store.StoreId,
                            store.StoreName,
                            store.StoreLoc
                        ));




                    }
                }

                if (availableStores != null)
                {
                    // Return stores within the selected location
                    return Ok(availableStores);
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
        public ActionResult<List<Product>> ProductsInStore([FromBody] LocationStoreRequest request)
        {
            // Call the method that returns ActionResult<List<Store>>
            ActionResult<List<Store>>? result = SelectLocation(new LocationRequest { SelectLocation = request.SelectLocation });

            if (result.Result is OkObjectResult okResult && okResult.Value is List<Store> stores)

            {
                selectedStoreObj = stores!.FirstOrDefault(s => s.StoreName == request.SelectStore);

                //add store details to orderlist

                //Store orderStore = AddCustToDb.Storedetails(selectedStoreObj!);

                //AddCustToDb.Storedetails(orderStore);


                if (selectedStoreObj != null)
                {
                    // create an instance of the business layer class.
                    ChainstoreBusinessUtilityLayer rpsb = new ChainstoreBusinessUtilityLayer();
                    //List<Product> products = rpsb.productsInStore();
                    List<Product> products = rpsb.productsInStore(selectedStoreObj);
                    if (products.Count > 0)
                    {
                        // Products found in the store
                        return Ok(products);
                    }
                    else
                    {
                        // No products found in the store
                        return NotFound();
                    }

                }
                else
                {
                    // Selected store not found
                    return NotFound();
                }


            }
            else if (result.Result is NotFoundResult)
            {


                return NotFound();
            }
            else
            {
                // Handle any other unexpected scenarios
                // ...

                return StatusCode(500);
            }

        }


        //Let customer choose product(s) and quantities
        [HttpPost("ChooseProduct")]
        public ActionResult<bool> SelectProduct([FromBody] ProdQuantRequest prodAndQuant)
        {
            // Retrieve the selected store from the request body
            string selectedStore = prodAndQuant.SelectedStore;

            // Call the ProductsInStore method to retrieve the products
            ActionResult<List<Product>> result = ProductsInStore(new LocationStoreRequest { SelectStore = prodAndQuant.SelectedStore, SelectLocation = prodAndQuant.SelectLocation });

            //ActionResult<Person> logindeta = Login();

            if (result.Result is OkObjectResult okResult && okResult.Value is List<Product> products)
            {
                Product? selectedProductObj = products!.FirstOrDefault(p => p.Productname == prodAndQuant.SelectProduct);

                if (selectedProductObj != null)
                {
                    // Retrieve the quantity of the selected product
                    int Availablequantity = selectedProductObj.Prodquant;

                    if (prodAndQuant.orderQuant <= Availablequantity)
                    {
                        if (selectedStoreObj != null)
                        {
                            //Add customer order to AddCustToDb
                            Order order = new Order(selectedProductObj!, prodAndQuant.orderQuant, prodAndQuant.orderQuant * selectedProductObj.Price, selectedStoreObj!);



                            // Add the order to the Orders list in AddCustToDb
                            addCustToDb.Orders.Add(order);

                            string? serializedPerson = HttpContext.Session.GetString("LoggedInPerson");
                            if (serializedPerson == null)
                            {
                                // Person not found, handle the error accordingly
                                return BadRequest(new { message = "User not logged in." });
                            }

                            // Deserialize the Person object
                            Person loggedInPerson = JsonConvert.DeserializeObject<Person>(serializedPerson);


                            ChainstoreBusinessUtilityLayer rpsp = new ChainstoreBusinessUtilityLayer();
                            bool PurchAddedToOrder = rpsp.AddCustomerPurchase(addCustToDb.Orders, loggedInPerson);

                            ChainstoreBusinessUtilityLayer rpsb = new ChainstoreBusinessUtilityLayer();
                            // Decrease inventory and process the order
                            bool UpdatedInventory = rpsb.DecreaseInventory(selectedProductObj.Productname, prodAndQuant.orderQuant);

                            if (UpdatedInventory)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return StatusCode(500);

                        }


                    }
                    else
                    {
                        //insufficient quabtities
                        return false;
                    }
                }
                else
                {
                    // Selected product not found
                    return false;
                }
            }
            else
            {
                // Error occurred in retrieving the products
                return false;
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

    }// EoC
}// EoN
