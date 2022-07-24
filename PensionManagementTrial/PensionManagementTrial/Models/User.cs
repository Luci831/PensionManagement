using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PensionManagementTrial.Models
{
    public class User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [DisplayName("First Name")]
        [Required(ErrorMessage = "First Name Can't be Blank")]
        [RegularExpression(@"^[A-Z][a-zA-Z]*$", ErrorMessage = "First letter should be Uppercase and no space between letters")]
        [MaxLength(15), MinLength(3)]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        [Required(ErrorMessage = "Last Name Can't be Blank")]
        [RegularExpression(@"^[A-Z][a-zA-Z]*$", ErrorMessage = "First letter should be Uppercase and no space between letters")]
        [MaxLength(15), MinLength(3)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please Provide Valid Date")]
        [DisplayName("Date Of Birth")]
        public DateTime Dob { get; set; }

        public byte[]? Picture { get; set; }

        [Required(ErrorMessage = "Contact Number can`t be blank")]
        [RegularExpression(@"^[6-9]{1}[0-9]{9}$", ErrorMessage = "Please Provide Valid Contact Number")]
        [StringLength(10)]
        [DisplayName("Contact Number")]
        public string ContactNumber { get; set; }

        [DisplayName("Email")]
        [Required(ErrorMessage = "Please Provide Valid Email Id")]
        [EmailAddress]
        public string Email { get; set; }

        [DisplayName("Password")]
        [MaxLength(16), MinLength(8)]
        [Required(ErrorMessage = "Password can`t be blank")]
        [PasswordPropertyText]
        public string Password { get; set; }

        [Required]
        [ForeignKey("Category")]
        [DisplayName("Role")]
        public int CatId { get; set; }
        [Required(ErrorMessage = "Please Select The Security Question")]
        [ForeignKey("SecurityQuestion")]
        [DisplayName("Security Question")]
        public int SqId { get; set; }
        [Required(ErrorMessage = "Answer can`t be blank")]
        [RegularExpression(@"^[A-Za-z0-9_ -]*$",
            ErrorMessage = "Answer should have only uppercase,lowercase,digit,space,_,-")]
        [MaxLength(25)]
        public string Answer { get; set; }
        public virtual Category Category { get; set; }
        [DisplayName("Security Question")]
        public virtual SecurityQuestion SecurityQuestion { get; set; }
        public virtual ICollection<PensionerDetails> PensionerDetails { get; set; }
    }
}