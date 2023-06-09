using ChainStoreApiModel;
namespace ChainStoreApiModel
{
    public class OrderPerson
    {

        public Guid CustomerId { get; set; }
        public string? Fname { get; set; }

        public string? Lname { get; set; }
        public string? Email { get; set; }

        public string? UserName { get; set; }

        public static OrderPerson CustomerDetails(Person p)
        {
            return new OrderPerson
            {
                CustomerId = p.CustomerId,
                Fname = p.Fname,
                Lname = p.Lname,
                Email = p.Email
            };

        }
    }
}