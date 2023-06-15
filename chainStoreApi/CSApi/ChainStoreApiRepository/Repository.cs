using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;


using ChainStoreApiModel;
namespace ChainStoreApiRepository
{
    public class RepositoryLayer
    {

        private static readonly string azuredb = "Server=tcp:chainstore.database.windows.net,1433;Initial Catalog=chainstoreApi;Persist Security Info=False;User ID=kwabena;Password=Kwa054129.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        //Register new customers into database
        public (bool, Person) Registration(Register reg)
        {
            string tableName = "Customers";
            string createTableQuery = $"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = '{tableName}') " +
                "BEGIN " +
                $"CREATE TABLE {tableName} ( " +
                "CustomerId UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY, " +
                "CustFirstName VARCHAR(40), " +
                "CustLastName VARCHAR(40), " +
                "UserName VARCHAR(20), " +
                "CustomerEmail VARCHAR(40), " +
                "CustomerPassword VARCHAR(20) ) END";


            // Insert new customer with login details into Customers database


            string insertQuery = "INSERT INTO Customers " +
                "(CustFirstName, CustLastName, UserName, CustomerEmail, CustomerPassword) " +
                "VALUES (@Fname, @Lname, @Username, @EmailAddress, @Password)";


            using (SqlConnection connection = new SqlConnection(azuredb))
            {
                connection.Open();
                using (SqlCommand createTableCommand = new SqlCommand(createTableQuery, connection))
                {
                    createTableCommand.ExecuteNonQuery();
                }

                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@Fname", reg.FirstName);
                    command.Parameters.AddWithValue("@Lname", reg.LastName);
                    command.Parameters.AddWithValue("@Username", reg.UserName);
                    command.Parameters.AddWithValue("@EmailAddress", reg.Email);
                    command.Parameters.AddWithValue("@Password", reg.Password);

                    try
                    {
                        int rowsAffected = command.ExecuteNonQuery();

                        Person p = new Person(Guid.NewGuid(), reg.FirstName, reg.LastName, reg.UserName, reg.Email, reg.Password);

                        return (rowsAffected == 1, p);
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"The exception was {ex.Message} - {ex.InnerException}");
                        return (false, null);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"The exception was {ex.Message} - {ex.InnerException}");
                        return (false, null);
                    }
                }
            }
        }




        //Login customers if they already exist in database and return customer name

        public (bool, Person) LogIn(LogIn login)
        {
            string selectQuery = "SELECT * FROM Customers WHERE UserName = @userName AND CustomerPassword = @password";


            using (SqlConnection connection = new SqlConnection(azuredb))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@userName", login.userName);
                    command.Parameters.AddWithValue("@password", login.password);

                    SqlDataReader reader = command.ExecuteReader();

                    try
                    {

                        if (!reader.Read())
                        {
                            connection.Close();
                            return (false, null);
                        }
                        else
                        {
                            //Guid id = reader.GetGuid(0); // Assuming the Guid value is in the first column (index 0) of the SqlDataReader

                            // Convert the Guid value to the desired format
                            //string formattedId = id.ToString();
                            Person p = new Person(reader.GetGuid(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5));
                            connection.Close();

                            return (true, p!);
                        }
                    }
                    catch (SqlException ex)
                    {
                        // write this exception to a file, exception.
                        Console.WriteLine($"the exception was {ex.Message} - {ex.InnerException}");
                        connection.Close();
                        return (false, null);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"the exception was {ex.Message} - {ex.InnerException}");
                        connection.Close();
                        return (false, null);
                    }



                }
            }

        }



        // Get store names and locations from database for user to choose from
        public List<Store> StoreDetails()
        {

            Dictionary<string, string> storeList = new Dictionary<string, string>();
            string query = "SELECT * FROM Store";
            List<Store> stores = new List<Store>();



            using (SqlConnection connection = new SqlConnection(azuredb))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    try
                    {
                        while (reader.Read())
                        {
                            //stores.Add(new Store { reader.GetGuid(0), reader.GetString(1), reader.GetString(2) });
                            stores.Add(new Store(reader.GetGuid(0), reader.GetString(1), reader.GetString(2)));

                        }
                        connection.Close();
                        return stores!;
                    }
                    catch (SqlException ex)
                    {

                        Console.WriteLine($"the exception was {ex.Message} - {ex.InnerException}");
                        connection.Close();
                        return stores!;
                    }
                    reader.Close();


                }

            }


        }


        public List<Product> ProductDetails(Store storeInfo)
        {
            List<Product> Products = new List<Product>();
            string query = "SELECT P.ProductId, P.Prodname, P.Price, P.ProdDescription, SP.ProdQuant " +
                "FROM Product P " +
                "JOIN StoreProduct SP ON P.ProductId = SP.ProductId " +
                "JOIN Store S ON SP.StoreId = S.StoreId " +
                "WHERE LOWER(S.StoreLoc) = LOWER(@StoreLoc) AND LOWER(S.StoreName) =  LOWER(@StoreName)";
            using (SqlConnection connection = new SqlConnection(azuredb))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StoreName", storeInfo.StoreName);
                    command.Parameters.AddWithValue("@StoreLoc", storeInfo.StoreLoc);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        int quantity = reader.GetInt32(reader.GetOrdinal("ProdQuant"));

                        Products.Add(new Product(reader.GetGuid(0), reader.GetString(1), reader.GetDecimal(2), quantity, reader.GetString(3)));
                    }

                    reader.Close();
                }
            }

            return Products;
        }



        //Update inventory of the product after an order is made based on quantity ordered 
        public bool UpdateInventory(string Prodname, int quantity)
        {

            string updateInventoryquery = "UPDATE Inventory SET ProdQuant = ProdQuant - @quantity WHERE Prodname = @Prodname " +
            "AND ProdQuant >= @minquantity";

            string updateStoreProductQuery = "UPDATE StoreProduct SET ProdQuant = i.ProdQuant " +
            "FROM StoreProduct sp " +
            "JOIN Inventory i ON sp.SPId = i.StoreProductId " +
            "WHERE LOWER(i.Prodname) = LOWER(@prodname)";



            using (SqlConnection connection = new SqlConnection(azuredb))
            {
                connection.Open();


                using (SqlCommand Inventorycommand = new SqlCommand(updateInventoryquery, connection))
                {
                    Inventorycommand.Parameters.AddWithValue("@quantity", quantity);
                    Inventorycommand.Parameters.AddWithValue("@Prodname", Prodname);

                    //Only update if ordered amount will get ProdQuant to less than 0
                    Inventorycommand.CommandText += " AND ProdQuant >= @minquantity";
                    Inventorycommand.Parameters.AddWithValue("@minquantity", 0);

                    int inventoryRowsAffected = Inventorycommand.ExecuteNonQuery();


                }
                using (SqlCommand StoreProductcommand = new SqlCommand(updateStoreProductQuery, connection))
                {
                    StoreProductcommand.Parameters.AddWithValue("@quantity", quantity);
                    StoreProductcommand.Parameters.AddWithValue("@Prodname", Prodname);

                    int StoreProductRowsAffected = StoreProductcommand.ExecuteNonQuery();


                }
                return true;
            }
        }



        // Create Order tabel an add customer after confirming that product exit

        public (bool, Order) AddToCustomerOrder(Order order, Person orderperson)
        {
            //DateTime orderTime = DateTime.Now;

            string tableName = "Orders";

            string createTableQuery = $@"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = '{tableName}')
                BEGIN
                CREATE TABLE {tableName} (
                    OrderId UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
                    OrderTime DATETIME DEFAULT GETDATE(),
                    CustomerId UNIQUEIDENTIFIER,
                    CustFirstName VARCHAR(40),
                    CustLastName VARCHAR(40),
                    CustUserName VARCHAR(40),
                    CustEmail VARCHAR(40),
                    ProductId UNIQUEIDENTIFIER,
                    Prodname VARCHAR(40),
                    OrderedQuant INT,
                    UnitPrice DECIMAL(18, 2),
                    TotalPrice DECIMAL(18, 2),
                    StoreId UNIQUEIDENTIFIER,
                    StoreName VARCHAR(40),
                    StoreLoc VARCHAR(40),
                    FOREIGN KEY (ProductId) REFERENCES Product(ProductId),
                    FOREIGN KEY (StoreId) REFERENCES Store(StoreId)
                )
                END";
            string query = "INSERT INTO Orders (OrderId, OrderTime, CustomerId, CustFirstName, CustLastName,CustUserName, CustEmail, ProductId, Prodname, OrderedQuant, UnitPrice, TotalPrice, StoreId, StoreName, StoreLoc) " +
    "VALUES (NEWID(), GETDATE(), @customerId, @custFirstName, @custLastName, @custUserName, @custEmail, " +
    "(SELECT ProductId FROM Product WHERE Prodname = @prodname), " +
    "@prodname, @orderedQuant, " +
    "(SELECT Price FROM Product WHERE Prodname = @prodname), " +
    "(@orderedQuant * (SELECT Price FROM Product WHERE Prodname = @prodname)), " +
    "(SELECT StoreId FROM Store WHERE StoreName = @storeName), " +
    "@storeName, @storeLoc)";

            using (SqlConnection connection = new SqlConnection(azuredb))
            {
                connection.Open();

                using (SqlCommand createtab = new SqlCommand(createTableQuery, connection))
                {
                    createtab.ExecuteNonQuery();
                }

                using (SqlCommand commandquery = new SqlCommand(query, connection))
                {
                    commandquery.Parameters.AddWithValue("@customerId", orderperson.CustomerId);
                    commandquery.Parameters.AddWithValue("@custFirstName", orderperson.Fname);
                    //commandquery.Parameters.AddWithValue("@custFirstName", orderperson.Fname);
                    commandquery.Parameters.AddWithValue("@custLastName", orderperson.Lname);
                    commandquery.Parameters.AddWithValue("@custUserName", orderperson.UserName);
                    commandquery.Parameters.AddWithValue("@custEmail", orderperson.Email);
                    commandquery.Parameters.AddWithValue("@prodname", order.Productname);
                    commandquery.Parameters.AddWithValue("@orderedQuant", order.OrderedQuant);
                    commandquery.Parameters.AddWithValue("@storeName", order.Store.StoreName);
                    commandquery.Parameters.AddWithValue("@storeLoc", order.Store.StoreLoc);


                    int rowsAffected = commandquery.ExecuteNonQuery();

                    if (rowsAffected <= 0)
                    {
                        return (false, null);
                    }
                }
            }
            return (true, order);
        }


        //Get the order history of a customer
        public List<orderHist> customerOrderHistory(LogIn custOderHist)
        {

            List<orderHist> ordersHist = new List<orderHist>();

            string queryCustomerOder = "SELECT O.OrderId,O.OrderTime,O.CustomerId, O.CustFirstName,O.CustLastName,O.CustUserName, " +
            "O.CustEmail,O.ProductId,O.Prodname,O.OrderedQuant, " +
            "O.UnitPrice,O.TotalPrice,O.StoreId, O.StoreName,O.StoreLoc " +
            "From Orders O " +
            "JOIN Customers C ON C.CustomerId=O.CustomerId " +
            "WHERE LOWER(UserName) =LOWER(@Username) AND  LOWER(CustomerPassword)=LOWER(@password) ";


            using (SqlConnection connection = new SqlConnection(azuredb))
            {
                connection.Open();

                using (SqlCommand CustomerOrdercommand = new SqlCommand(queryCustomerOder, connection))
                {

                    CustomerOrdercommand.Parameters.AddWithValue("@Username", custOderHist.userName);
                    CustomerOrdercommand.Parameters.AddWithValue("@password", custOderHist.password);
                    SqlDataReader reader = CustomerOrdercommand.ExecuteReader();


                    while (reader.Read())
                    {
                        ordersHist.Add(new orderHist(
                            reader.GetGuid(0),
                            reader.GetDateTime(1),

                            reader.GetGuid(2),
                            reader.GetString(3),
                            reader.GetString(4),
                            reader.GetString(5),
                            reader.GetString(6),

                            reader.GetGuid(7),
                            reader.GetString(8),
                            reader.GetInt32(9),
                            reader.GetDecimal(10),
                            reader.GetDecimal(11),

                            reader.GetGuid(12),
                            reader.GetString(13),
                            reader.GetString(14)
                        ));
                    }
                    return ordersHist;

                }

            }
        }





        //basic statistics
        public OrderStat GetOrderStatistics(LogIn login)
        {
            string queryCustomerOrder = @"
                SELECT COUNT(*) AS TotalOrders,
                AVG(O.TotalPrice) AS AverageOrderPrice,
                MAX(O.TotalPrice) AS MaximumOrderPrice,
                MIN(O.TotalPrice) AS MinimumOrderPrice,
                (
                    SELECT TOP 1 Prodname
                    FROM Orders
                    GROUP BY Prodname
                    ORDER BY COUNT(*) DESC
                ) AS MostOrderedProduct
                FROM Orders O
                JOIN Customers C ON C.CustomerId = O.CustomerId
                WHERE LOWER(CustUserName) = LOWER(@Username) AND LOWER(CustomerPassword) = LOWER(@password)";


            using (SqlConnection connection = new SqlConnection(azuredb))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(queryCustomerOrder, connection))
                {
                    command.Parameters.AddWithValue("@Username", login.userName);
                    command.Parameters.AddWithValue("@password", login.password);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        OrderStat orderStatistics = new OrderStat(reader.GetInt32(0), reader.GetString(4), reader.GetDecimal(1), reader.GetDecimal(2), reader.GetDecimal(3));

                        return orderStatistics;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }



    }
}






