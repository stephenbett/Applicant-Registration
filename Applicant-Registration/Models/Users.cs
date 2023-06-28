using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Applicant_Registration.Models
{
    public class Users
    {
        public int UsersId { get; set; }

        
        public string FirstName { get; set; }

       
        public string LastName { get; set; }
        public string UserName { get; set; }


        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}