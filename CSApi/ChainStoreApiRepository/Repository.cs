
using System;
using System.Data.SqlClient;
namespace ChainStoreApiRepository
{
    public class Repository
    {


        private static readonly string azuredb = "Server=tcp:chevron-batch-server.database.windows.net," +
        "1433;Initial Catalog=chevron_sql_database;Persist Security Info=False;" +
        "User ID=chevronbatchserver;Password=;MultipleActiveResultSets=False;Encrypt=True;" +
        "TrustServerCertificate=False;Connection Timeout=30;";

        //Register new customers into database

        public static (bool, string) UserRegistration(string Fname, string Lname, string EmailAddress, string Password)
        {
            string tableName = "Customers";
            string createTableQuery = $"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = '{tableName}') " +
        "BEGIN " +
            $"CREATE TABLE {tableName} ( " +
                "CustomerId UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY, " +
                "CustFirstName VARCHAR(40), " +
                "CustLastName VARCHAR(40), " +
                "CustomerEmail VARCHAR(40), " +
                "CustomerPassword VARCHAR(20) )  END";



            //Insert new customer with login details into Customers database
            string insertQuery = "INSERT INTO Customers (CustFirstName,CustLastName,CustomerEmail,CustomerPassword) VALUES (@Fname, @Lname, @EmailAddress, @Password);SELECT SCOPE_IDENTITY()";

            using (SqlConnection connection = new SqlConnection(azuredb))
            {


                connection.Open();
                using (SqlCommand createTableCommand = new SqlCommand(createTableQuery, connection))
                {
                    createTableCommand.ExecuteNonQuery();

                }

                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {

                    command.Parameters.AddWithValue("@Fname", Fname);
                    command.Parameters.AddWithValue("@Lname", Lname);
                    command.Parameters.AddWithValue("@EmailAddress", EmailAddress);
                    command.Parameters.AddWithValue("@Password", Password);

                    //SqlDataReader reader = command.ExecuteReader();

                    string retrievedCustId = command.ExecuteScalar()?.ToString() ?? string.Empty;


                    //string? retrievedCustId = reader["CustomerId"].ToString();

                    int rowsAffected = command.ExecuteNonQuery();
                    return (rowsAffected == 1, retrievedCustId);
                }
                //changes


            }



        }



        //Login customers if they already exist in database and return customer name

        public static (bool, string, string, string) LogIn(string email, string Password)
        {
            string selectQuery = "SELECT * FROM Customers WHERE CustomerEmail = @email AND CustomerPassword = @Password";

            using (SqlConnection connection = new SqlConnection(azuredb))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@email", email);
                    command.Parameters.AddWithValue("@Password", Password);
                    SqlDataReader reader = command.ExecuteReader();

                    string? retrievedFirstName = reader["CustFirstName"].ToString();
                    string? retrievedLastName = reader["CustLastName"].ToString();
                    string? retrievedCustId = reader["CustomerId"].ToString();

                    return (reader.Read(), retrievedFirstName!, retrievedLastName!, retrievedCustId!);
                }
            }

        }






        // Get store names and locations from database for user to choose from
        public static Dictionary<string, string> Store()
        {

            Dictionary<string, string> storeList = new Dictionary<string, string>();
            string query = "SELECT StoreName,StoreLoc FROM Store";

            using (SqlConnection connection = new SqlConnection(azuredb))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataReader reader = command.ExecuteReader();

                    //Dictionary<string, string> storeInfo = new Dictionary<string, string>();
                    while (reader.Read())
                    {
                        string? storeName = reader["StoreName"].ToString();
                        string? location = reader["StoreLoc"].ToString();

                        storeList.Add(storeName!, location!);

                    }
                    reader.Close();

                }
                return storeList;
            }


        }

        //Get Stores at User selected Location



        //Select Product details from chosen storelocation and name
        public static List<Dictionary<string, string>> ProductDetails(string StoreLoc, string StoreName)
        {

            List<Dictionary<string, string>> ProductList = new List<Dictionary<string, string>>();

            string query = "SELECT P.ProductId, P.Prodname, P.Price, P.ProdDescription, SP.ProdQuant " +
                "FROM Product P " +
                "JOIN StoreProduct SP ON P.ProductId = SP.ProductId " +
                "JOIN Store S ON SP.StoreId = S.StoreId " +
                "WHERE LOWER(S.StoreLoc) = LOWER(@StoreLoc) AND LOWER(S.StoreName) = LOWER(@StoreName) ";


            using (SqlConnection connection = new SqlConnection(azuredb))
            {

                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StoreLoc", StoreLoc);
                    command.Parameters.AddWithValue("@StoreName", StoreName);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string? productId = reader["ProductId"].ToString();
                        string? productname = reader["Prodname"].ToString();
                        string? price = reader["Price"].ToString();
                        string? productdesc = reader["ProdDescription"].ToString();
                        string? quantity = reader["ProdQuant"].ToString();



                        Dictionary<string, string> ProducInfo = new Dictionary<string, string>();

                        ProducInfo.Add("productid", productId!);
                        ProducInfo.Add("productname", productname!);
                        ProducInfo.Add("price", price!);
                        ProducInfo.Add("prodDescription", productdesc!);
                        ProducInfo.Add("QuantInStock", quantity!);

                        ProductList.Add(ProducInfo);

                    }
                    reader.Close();
                }
            }
            return ProductList;

        }


        // Check to see if product exist at the store location and name
        public static bool ProductExists(string Prodname, string StoreName)
        {


            string query = "SELECT COUNT(*) FROM Product " +
            "INNER JOIN Store ON Store.ProductId = Product.ProductId " +
            "WHERE Product.Prodname = @Prodname AND Store.StoreName = @StoreName";

            using (SqlConnection connection = new SqlConnection(azuredb))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Prodname", Prodname);
                    command.Parameters.AddWithValue("@StoreName", StoreName);

                    int count = Convert.ToInt32(command.ExecuteScalar());

                    return count > 0;
                }
            }
        }


        //Update inventory of the product after an order is made based on quantity ordered 
        public static bool DecreaseInventory(string Prodname, int quantity)
        {

            string updateInventoryquery = "UPDATE Inventory SET ProdQuant = ProdQuant - @quantity WHERE Prodname = @Prodname " +
            "AND ProdQuant >= @minquantity";

            string updateStoreProductQuery = "UPDATE StoreProduct SET ProdQuant = i.ProdQuant " +
            "FROM StoreProduct sp " +
            "JOIN Inventory i ON sp.SPId = i.StoreProductId " +
            "WHERE i.Prodname = @prodname";



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

        public static bool AddToCustomerOrder(string custFirstName, string custLastName, string prodname, int OrderedQuant, string storeName, string storeLoc)
        {

            DateTime orderTime = DateTime.Now;
            Guid ProductId = Guid.NewGuid();
            Guid StoreId = Guid.NewGuid();
            string tableName = "Orders";

            string createTableQuery = $@"
        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = '{tableName}')
        BEGIN
            CREATE TABLE {tableName} (
                    OrderId UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
                    CustFirstName VARCHAR(40),
                    CustLastName VARCHAR(40),
                    ProductId UNIQUEIDENTIFIER,
                    Prodname VARCHAR(40),
                    OrderedQuant int,
                    UnitPrice decimal,
                    TotalPrice decimal,
                    OrderTime DATETIME DEFAULT GETDATE(),
                    StoreId UNIQUEIDENTIFIER,
                    StoreName VARCHAR(40),
                    StoreLoc  VARCHAR(40),
                    FOREIGN KEY (ProductId) REFERENCES Product(ProductId),
                    FOREIGN KEY (StoreId) REFERENCES Store(StoreId)
                ) 
                END";
            string query = "INSERT INTO Orders (OrderId, CustFirstName, CustLastName, ProductId, Prodname, OrderedQuant, UnitPrice, TotalPrice, OrderTime, StoreId, StoreName, StoreLoc) " +
                    "VALUES (NEWID(), @custFirstName, @custLastName, " +
                    "(SELECT ProductId FROM Product WHERE Prodname = @prodname), " +
                    "@prodname, @OrderedQuant, " +
                    "(SELECT Price FROM Product WHERE Prodname = @prodname), " +
                    "(@OrderedQuant * (SELECT Price FROM Product WHERE Prodname = @prodname)), " +
                    "GETDATE(), " +
                    "(SELECT StoreId FROM Store WHERE StoreName = @storeName), " +
                    "@storeName, @storeLoc)";
            using (SqlConnection connection = new SqlConnection(azuredb))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@custFirstName", custFirstName);
                    command.Parameters.AddWithValue("@custLastName", custLastName);
                    //command.Parameters.AddWithValue("@productId", ProductId);
                    command.Parameters.AddWithValue("@prodname", prodname);
                    command.Parameters.AddWithValue("@OrderedQuant", OrderedQuant);
                    //command.Parameters.AddWithValue("@StoreId", StoreId);
                    command.Parameters.AddWithValue("@storeName", storeName);
                    command.Parameters.AddWithValue("@storeLoc", storeLoc);


                    int rowsAffected = command.ExecuteNonQuery();

                    return rowsAffected > 0;

                }
            }


        }

        public static List<Dictionary<string, string>> customerOrderHistory(string custFirstName, string custLastName)
        {
            List<Dictionary<string, string>> CustomerOrderHist = new List<Dictionary<string, string>>();

            string queryCustomerOder = "SELECT O.CustFirstName,O.CustLastName," +
            "O.Prodname,O.StoreName,O.StoreLoc, " +
            "O.OrderedQuant,O.UnitPrice,O.TotalPrice,OrderTime " +
            "From Orders O " +
            "Join Product P ON P.ProductId=O.ProductId " +
            "WHERE CustFirstName =@custFirstName AND CustLastName =@custLastName ";


            using (SqlConnection connection = new SqlConnection(azuredb))
            {
                connection.Open();

                using (SqlCommand CustomerOrdercommand = new SqlCommand(queryCustomerOder, connection))
                {
                    CustomerOrdercommand.Parameters.AddWithValue("@custFirstName", custFirstName);
                    CustomerOrdercommand.Parameters.AddWithValue("@custLastName", custLastName);
                    SqlDataReader reader = CustomerOrdercommand.ExecuteReader();

                    while (reader.Read())
                    {
                        string? CustFirstName = reader["CustFirstName"].ToString();
                        string? CustLastName = reader["CustLastName"].ToString();
                        string? Prodname = reader["Prodname"].ToString();
                        string? StoreName = reader["StoreName"].ToString();
                        string? StoreLoc = reader["StoreLoc"].ToString();
                        string? OrderedQuant = reader["OrderedQuant"].ToString();
                        string? UnitPrice = reader["UnitPrice"].ToString();
                        string? OrderTime = reader["OrderTime"].ToString();
                        string? TotalPrice = reader["TotalPrice"].ToString();


                        Dictionary<string, string> OrderCust = new Dictionary<string, string>();
                        OrderCust.Add("CustFirstName", CustFirstName!);
                        OrderCust.Add("CustLastName", CustLastName!);
                        OrderCust.Add("Prodname", Prodname!);
                        OrderCust.Add("StoreName", StoreName!);
                        OrderCust.Add("StoreLoc", StoreLoc!);
                        OrderCust.Add("OrderedQuant", OrderedQuant!);
                        OrderCust.Add("UnitPrice", UnitPrice!);
                        OrderCust.Add("OrderTime", OrderTime!);
                        OrderCust.Add("TotalPrice", OrderTime!);

                        CustomerOrderHist.Add(OrderCust);

                    }
                    return CustomerOrderHist;
                }
            }




        }



        //basic statistics



        public static Dictionary<string, object> GetCustomerOrderStatistics(string custFirstName, string custLastName)
        {
            string queryCustomerOrder = "SELECT COUNT(*) AS TotalOrders, " +
                                        "AVG(O.TotalPrice) AS AverageOrderPrice, " +
                                        "MAX(O.TotalPrice) AS MaximumOrderPrice, " +
                                        "MIN(O.TotalPrice) AS MinimumOrderPrice " +
                                        "FROM Orders O " +
                                        "JOIN Product P ON P.ProductId = O.ProductId " +
                                        "WHERE CustFirstName = @custFirstName AND CustLastName = @custLastName";

            Dictionary<string, object> orderStatistics = new Dictionary<string, object>();

            using (SqlConnection connection = new SqlConnection(azuredb))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(queryCustomerOrder, connection))
                {
                    command.Parameters.AddWithValue("@custFirstName", custFirstName);
                    command.Parameters.AddWithValue("@custLastName", custLastName);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        orderStatistics["TotalOrders"] = reader["TotalOrders"];
                        orderStatistics["AverageOrderPrice"] = reader["AverageOrderPrice"];
                        orderStatistics["MaximumOrderPrice"] = reader["MaximumOrderPrice"];
                        orderStatistics["MinimumOrderPrice"] = reader["MinimumOrderPrice"];
                    }
                }
            }

            return orderStatistics;
        }





    }

}

