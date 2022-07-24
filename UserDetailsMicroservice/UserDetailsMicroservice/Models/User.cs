using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UserDetailsMicroservice.Models
{
    public class User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required(ErrorMessage = "First Name Can't be Null")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name Can't be Null")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Date of Birth can`t be blank")]
        public DateTime Dob { get; set; }

        public byte[] Picture { get; set; }

        [Required]
        public string ContactNumber { get; set; }

        [DisplayName("Email")]
        [Required(ErrorMessage = "Email id can`t be blank")]
        [EmailAddress]
        public string Email { get; set; }

        [DisplayName("Password")]
        [PasswordPropertyText]
        [Required(ErrorMessage = "Password can`t be blank")]
        public string Password { get; set; }

        [Required]
        [DisplayName("category")]
        public int CatId { get; set; }
        [Required(ErrorMessage = "please select the question")]
        [DisplayName("SecurityQuestion")]
        public int SqId { get; set; }
        [Required(ErrorMessage = "Answer can`t be blank")]
        public string Answer { get; set; }
    }
}
