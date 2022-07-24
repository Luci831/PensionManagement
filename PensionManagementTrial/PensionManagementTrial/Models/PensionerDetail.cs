//PensionerDetails
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PensionManagementTrial.Models
{
    public class PensionerDetails
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Prid { get; set; }

        [Required]
        [DisplayName("User")]
        [ForeignKey("User")]
        public int? UserId { get; set; }
        public string Name { get; set; }

        [DisplayName("Date Of Birth")]
        public DateTime Dob { get; set; }

        [StringLength(10, MinimumLength = 10)]//length
        [DisplayName("Pan")]
        [Required(ErrorMessage = "Pan can`t be blank")]
        public string Pan { get; set; }

        //[RegularExpression(@"^\[0-9]{10}$", ErrorMessage = "enter 10 digit number")]//length
        [StringLength(12, MinimumLength = 12)]//length
        [DisplayName("Adhar Number")]
        [Required(ErrorMessage = "Adhar can`t be blank")]
        public string AadharNumber { get; set; }
        [DisplayName("Account Number")]
        [Required(ErrorMessage = "Account Number can`t be blank")]
        public string AccountNo { get; set; }
        [DisplayName("Salary Earned")]
        [Required(ErrorMessage = "Salary Earned can`t be blank")]
        public decimal SalaryEarned { get; set; }
        [Required(ErrorMessage = "Allowances can`t be blank")]
        public decimal Allowances { get; set; }
        [DisplayName("Pension Type")]
        [ForeignKey("PensionTypes")]
        public int Ptid { get; set; }
        [DisplayName("Bank")]
        [ForeignKey("Banks")]
        public int BankId { get; set; }
        [DisplayName("Pension Amount")]
        public decimal PensionAmount { get; set; }
        public virtual User User { get; set; }
        public virtual PensionTypes PensionTypes { get; set; }
        public virtual Banks Banks { get; set; }
    }
}
