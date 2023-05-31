namespace ChainStoreApiBusiness
{
    public class ChainstoreUtility
    {
        public ChainstoreUtility() { }




        public static string getproductname(string productname)
        {

            if (string.IsNullOrEmpty(productname))
            {
                Console.WriteLine("product name cannot be empty.");
                return "noName";

            }
            else
            {
                return productname;
            }


        }

        public static bool verifyEmail(string email)
        {
            if (email.Contains('@'))
            {
                return true;


            }
            else
            {
                return false;
            }
        }

        public static bool validatePassword(string password)
        {

            if (password.Length > 6)
            {
                return true;
            }
            else
            {
                return false;

            }
        }


        // validate if prod quant is a number
        public static int OrderProductQuant(string quant)
        {

            if (!int.TryParse(quant, out int quantity))
            {
                throw new FormatException();
            }
            else
            {
                return quantity;

            }
        }






        public static int QuantInstoc(string productQuant)
        {
            if (!int.TryParse(productQuant, out int quantstock))
            {
                Console.WriteLine("Invalid quantity of product");
                return 0;
            }
            else
            {
                return quantstock;

            }
        }

    }

}