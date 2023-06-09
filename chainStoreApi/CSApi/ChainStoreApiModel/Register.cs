using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace ChainStoreApiModel
{
    public class Register
    {

        public Register(string fname, string lname, string username, string email, string password)
        {
            this.FirstName = fname;
            this.LastName = lname;
            this.UserName = username;
            this.Email = email;
            this.Password = password;
        }

        [JsonConstructor]
        public Register() { }

        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "The {0} length must be between {2} and {1}.")]
        public string UserName { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 5)]
        public string Password { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "The {0} length must be between {2} and {1}.")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "The {0} length must be between {2} and {1}.")]
        public string LastName { get; set; }

        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Got an error")]
        public string Email { get; set; }
    }
}
