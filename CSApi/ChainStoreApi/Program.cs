
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using ChainStoreApiRepository;
using ChainStoreApiBusiness;

namespace ChainStoreApi
{
    class Program
    {


        static void Main(string[] args)
        {



            bool logInSuccess = false, productExists = false, RegistrationSuccessful = false, rightEmailAddr = false, rightPassword = false;

            string FirstName = string.Empty, EmailAddress = string.Empty, LastName = string.Empty, chosenStore = string.Empty, logOut = string.Empty;
            string selectedProduct = string.Empty, chosenLocation = string.Empty, customerId = string.Empty, Password = string.Empty;



            //attempts limit for do while loops

            int logInattempts = 4, regAttempts = 4, storeAttempts = 4, locAttempts = 4, attempts = 4;

            Console.WriteLine("Do you already have an account with us? (Y/N): ");
            string HaveAnAccount = Console.ReadLine().ToUpper();


            if (HaveAnAccount.ToLower() == "y")
            {
                //Loop to allow registered customers login, Failed to proceed upto 4 attempts

                do

                {
                    try
                    {
                        Console.WriteLine("\nPlease enter your login details below: ");

                        Console.WriteLine("\nPlease enter your email address below: ");
                        string EnteredEmailAddress = Console.ReadLine().ToUpper();

                        Console.Write("\nPlease enter your Password: ");
                        string EnteredPassword = Console.ReadLine();
                        logInattempts--;

                        //Get customer first and last name if login succeeds

                        (logInSuccess, FirstName, LastName, customerId) = Repository.LogIn(EnteredEmailAddress, EnteredPassword);

                    }
                    catch (InvalidOperationException ex)
                    {
                        Console.WriteLine("\nEither your email address or password is wrong " + ex.Message);
                        logInSuccess = false;
                        //attempts = 0; // Exit the loop

                    }

                } while (!logInSuccess && logInattempts > 0);

                if (logInSuccess)
                {
                    // Authentication successful
                    Console.WriteLine($"\nLogin successful! Welcome, {FirstName} {LastName}");
                }
                else
                {
                    Console.Write("\nLogin failed, exiting program");
                    return;
                }



            }

            //Registration of new customers(4 attempts)

            else
            {
                do
                {
                    try
                    {
                        Console.WriteLine("\nYou need to register: ");


                        Console.WriteLine("\nPlease enter your firstname: ");
                        FirstName = Console.ReadLine();

                        Console.WriteLine("\nPlease enter your Lastname: ");
                        LastName = Console.ReadLine();

                        //verify that email contains @,.
                        do
                        {
                            Console.WriteLine("\nPlease enter your email address: ");
                            EmailAddress = Console.ReadLine();

                            //validate that email contains @,.com
                            rightEmailAddr = ChainstoreUtility.verifyEmail(EmailAddress);


                        } while (!rightEmailAddr);

                        //Password

                        do
                        {

                            Console.Write("\nPlease enter your password (longer than 6)");
                            Password = Console.ReadLine();
                            rightPassword = ChainstoreUtility.validatePassword(Password);

                        } while (!rightPassword);
                        regAttempts--;

                        //Register customers
                        (RegistrationSuccessful, customerId) = Repository.UserRegistration(FirstName, LastName, EmailAddress, Password);

                    }
                    catch (InvalidOperationException ex)
                    {
                        Console.WriteLine("\nCheck your registration details. " + ex.Message);

                    }

                } while (!RegistrationSuccessful && regAttempts < 0);

                if (RegistrationSuccessful)

                {
                    Console.WriteLine($"\nRegistration successful! Welcome, {FirstName} {LastName}!: ");

                }
                else
                {
                    Console.WriteLine("\nRegistration failed!: ");
                    return;
                }
            }

            do
            {

                //Choose store names and locations from database
                Dictionary<string, string> storeNameLocation = Repository.Store();

                //Show only distict store location for cities with multiple stores
                HashSet<string> distinctLocations = new HashSet<string>(storeNameLocation.Values);

                //Ask user to choose a store location 
                //chosenLocation = Console.ReadLine().ToUpper();


                // List locations, ask user to select and verify for max attempts of 4

                do
                {

                    if (locAttempts < 4)
                    {
                        Console.WriteLine($"\nYou only required to choose from this list: ");
                    }
                    Console.WriteLine("Here is the list of locations ");
                    //Print to screen all store lcations in database
                    foreach (string l in distinctLocations)
                    {
                        Console.WriteLine(l);
                    }
                    Console.WriteLine("\nChoose your store location from the list below: ");
                    chosenLocation = Console.ReadLine().ToUpper();
                    locAttempts--;
                    //verify if user-selected location is part of locations listed above
                } while (!storeNameLocation.Values.Any(v => string.Equals(v, chosenLocation, StringComparison.OrdinalIgnoreCase)) && locAttempts > 0);


                //List store, ask user to select, and verify for max attempts of 4

                do
                {
                    if (storeAttempts < 4)
                    {
                        Console.WriteLine("\nError! Choose a store name from the list provided above");
                    }
                    Console.WriteLine($"\nHere are the stores in {chosenLocation}: ");
                    foreach (KeyValuePair<string, string> storeInLocation in storeNameLocation)
                    {

                        if (storeInLocation.Value.ToUpper() == chosenLocation)
                        {
                            Console.Write(storeInLocation.Key);
                        }

                    }
                    Console.Write("\nChoose a store: ");
                    chosenStore = Console.ReadLine().ToUpper();
                    storeAttempts--;

                } while (!storeNameLocation.Keys.Any(k => string.Equals(k, chosenStore, StringComparison.OrdinalIgnoreCase)) && storeAttempts > 0);


                //Get all the products and their details at selected store name
                List<Dictionary<string, string>> productsInStore = Repository.ProductDetails(chosenLocation, chosenStore);
                Console.WriteLine($"Here are the products in {chosenStore} ");
                // Print table header
                Console.WriteLine("===================================================================================================");
                Console.WriteLine("|    Product Name                |Price             | QuantInStock                 |  Description |");
                Console.WriteLine("===================================================================================================");

                // Print all products and their details in the store

                foreach (Dictionary<string, string> productInfo in productsInStore)
                {
                    string prodname = productInfo["productname"];
                    string price = productInfo["price"];
                    string InStock = productInfo["QuantInStock"];
                    string description = productInfo["prodDescription"];

                    // Print row content
                    Console.WriteLine($"|   {prodname,-25} |   {price,-15} | {InStock,-15} |  {description,-30} |");
                }

                // Print table footer
                Console.WriteLine("=================================================================================================================== \n");


                //a loop to alloow customer to make multiple orders

                do
                {
                    try
                    {
                        Console.Write("\nSelect the name of the product you want to buy: ");
                        selectedProduct = ChainstoreUtility.getproductname(Console.ReadLine());

                        productExists = productsInStore.Any(productInfo => productInfo.ContainsKey("productname") &&
                        string.Equals(productInfo["productname"], selectedProduct, StringComparison.OrdinalIgnoreCase));
                        attempts--;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("\nProduct cannot be found in this store. " + ex.Message);

                    }
                } while (!productExists && attempts > 0);





                // Check if product name exists at selected location
                if (productExists)

                {
                    Console.Write("\nEnter the quantity: ");
                    int orderedQuant = ChainstoreUtility.OrderProductQuant(Console.ReadLine());

                    // Retrieve product details from productsInStore based on the selected name
                    Dictionary<string, string>? productDetails = productsInStore.FirstOrDefault(dict => dict["productname"] == selectedProduct);
                    if (productDetails! != null)
                    {
                        int availableQuant = Convert.ToInt32(productDetails!["QuantInStock"]);

                        if (availableQuant >= orderedQuant)

                        {
                            // Decrease inventory and process the order
                            bool UpdateInventory = Repository.DecreaseInventory(selectedProduct, orderedQuant);

                            if (UpdateInventory)
                            {
                                Console.WriteLine($"\nInventory for product {selectedProduct} decreased by {orderedQuant} units.\n");
                            }
                            else
                            {
                                Console.WriteLine($"Failed to update Inventory");
                            }

                            Console.WriteLine($"\nHey {FirstName}!, your Order for {orderedQuant} units of {selectedProduct} accepted. \n");

                            //put customer details into cart,  //if product name exists, add product details to cart

                            bool OrderAdded = Repository.AddToCustomerOrder(FirstName, LastName, selectedProduct, orderedQuant, chosenStore, chosenLocation);
                            if (OrderAdded)
                            {
                                Console.WriteLine($"\nDetails of {selectedProduct} of customer {FirstName} details was added to Order. \n");
                            }
                            else
                            {
                                Console.WriteLine("\nFailed to add to order table. \n");
                            }

                        }
                        else
                        {
                            Console.WriteLine("\nHey {FirstName}, you ordered more quantities of {selectedProduct} than we have in stock");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nProduct details not found.");
                    }
                }
                else
                {
                    Console.WriteLine("\nProduct name not found.");
                }

                Console.Write($"\nHey {FirstName}, Do you want to continue shopping? (Yes/No)");
                logOut = Console.ReadLine().ToUpper();


            } while (logOut[0] != 'N');




            //GET Customer Order History

            List<Dictionary<string, string>> orderHistory = Repository.customerOrderHistory(FirstName!, LastName!);


            //First, extract some basics stats about customer order history

            Dictionary<string, object> customerStatistics = Repository.GetCustomerOrderStatistics(FirstName!, LastName!);

            int totalOrders = (int)customerStatistics["TotalOrders"];
            decimal averageOrderPrice = (decimal)customerStatistics["AverageOrderPrice"];
            decimal maximumOrderPrice = (decimal)customerStatistics["MaximumOrderPrice"];
            decimal minimumOrderPrice = (decimal)customerStatistics["MinimumOrderPrice"];

            int count = 0;
            foreach (Dictionary<string, string> custOrderInfo in orderHistory)
            {
                string prodname = custOrderInfo["Prodname"];
                string OrderedQuant = custOrderInfo["OrderedQuant"];
                string Unitprice = custOrderInfo["UnitPrice"];
                string TotalPrice = custOrderInfo["TotalPrice"];

                string StoreName = custOrderInfo["StoreName"];
                string StoreLoc = custOrderInfo["StoreLoc"];


                if (count < 1)
                {
                    string CustFirstName = custOrderInfo["CustFirstName"];
                    string CustLastName = custOrderInfo["CustLastName"];
                    string OrderTime = custOrderInfo["OrderTime"];


                    Console.WriteLine("***************************************");
                    Console.WriteLine($"Custorm Name:{CustFirstName} {CustLastName}");
                    Console.WriteLine($"DateTime:{OrderTime}");
                    Console.WriteLine($"TotalOrders: {totalOrders}");
                    Console.WriteLine($"AverageOrderPrice: {averageOrderPrice}");
                    Console.WriteLine($"maximumOrderPrice: {maximumOrderPrice}");
                    Console.WriteLine($"minimumOrderPrice: {minimumOrderPrice}");

                    Console.WriteLine("***************************************\n");

                    Console.WriteLine("------------------------------------------------------------------------------");
                    Console.WriteLine("===================================================================================================");
                    Console.WriteLine("|    Product Name         |OrderedQuant        | Unitprice        |  StoreName  | StoreLoc|");
                    Console.WriteLine("===================================================================================================");

                }
                count++;

                // Print row content

                Console.WriteLine($"| {prodname,-25} |   {OrderedQuant,-10} | {Unitprice,-10} | {TotalPrice,-10} | {StoreName,-20} |  {StoreLoc,-20}");

            }
            Console.WriteLine("------------------------------------------------------------------------------");


            Console.Write("\nThank you for shopping with us, Come back again");


        }


    }
}

