using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChainStoreApiModel
{
    public class Person
    {
        public Person() { }


        public Person(Guid id, string fname, string lname, string username, string email, string password)
        {
            this.CustomerId = id;
            this.Fname = fname;
            this.Lname = lname;
            this.UserName = username;
            this.Email = email;
            this.Password = password;

        }
        //create properties for class


        public Guid CustomerId { get; set; }// when you create a property with a setter, C# creates a private backing variable of the same name for you. You don't see it.

        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Fname { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Lname { get; set; }// this is a "property"
        //public string Remarks { get; set; }
        //public DateTime LastOrderDate { get; set; }
        // [Range(0, 100)]
        // public int age { get; set; } = 0;// fields are not visible to swagger.

        // https://www.oreilly.com/library/view/regular-expressions-cookbook/9781449327453/ch04s01.html
        // [EmailAddress()]
        //[RegularExpression("^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")]// send in a specific regex pattern
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Got an error")]// The part before the @ restricts to characters commonly used in emails. The part after the @ restricts to characters allowed in domain names.
        public string Email { get; set; }



    }

}