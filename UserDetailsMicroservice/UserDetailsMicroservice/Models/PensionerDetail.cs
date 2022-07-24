//PensionerDetails
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UserDetailsMicroservice.Models
{
    public class PensionerDetails
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Prid { get; set; }

        [Required]
        [ForeignKey("User")]
        public int? UserId { get; set; }
        public string Name { get; set; }
        public DateTime Dob { get; set; }

        [StringLength(10, MinimumLength = 10)]//length
        [DisplayName("Pan")]
        [Required(ErrorMessage = "Pan can`t be blank")]
        public string Pan { get; set; }

        //[RegularExpression(@"^\[0-9]{10}$", ErrorMessage = "enter 10 digit number")]//length
        [DisplayName("Aadhar")]
        public string AadharNumber { get; set; }
        public string AccountNo { get; set; }
        public decimal SalaryEarned { get; set; }
        public decimal Allowances { get; set; }
        public int Ptid { get; set; }
        public int BankId { get; set; }
        public decimal PensionAmount { get; set; }
        public virtual User User { get; set; }
    }
}
