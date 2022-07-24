using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationMicroservice.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int CatId { get; set; }
    }
}
