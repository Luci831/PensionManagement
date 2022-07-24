//Banks
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PensionCalculationMicroservice.Models
{
    public class Banks
    {
        public int BankId { get; set; }
        [ForeignKey("BankTypes")]
        public int BType { get; set; }
        [Required(ErrorMessage = "Bankname can`t be blank")]
        public string BankName { get; set; }
    }
}