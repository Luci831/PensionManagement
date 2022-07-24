//Banks
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PensionManagementTrial.Models
{
    public class Banks
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BankId { get; set; }
        [Required]
        [ForeignKey("BankTypes")]
        public int BType { get; set; }
        [Required(ErrorMessage = "Bankname can`t be blank")]
        public string BankName { get; set; }

        public virtual BankTypes BankTypes { get; set; }
    }
}