namespace ChainStoreApiModel
{
    public class Customer
    {
        //public Customer(){}

        
        //constructer for customer class with ID number, first name, last name
        public Customer(string fname, string lname){
            this.Lname = lname;
            this.Fname = fname;
            //this.ProdDetails = prodDetails;
            
        }
        //create properties for class

        
        private string fname;
        public string Fname 
        {
            get{
                return fname;
            }
            set{
                if (value.Length < 1){
                    throw new FormatException();
                }
                else{
                    this.fname=value;
                }
            }
        }
        private string lname;
        public string Lname
        {
            get{
                return lname;
            }
            set 
            {
                if (value.Length < 1){
                    throw new Exception();
                }
                else{
                    this.lname = value;
                }
            }



        }
        
        public List<Product> productDetails {get; set;} = new List<Product>();
        
        

    }
    
}