//BankTypes
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PensionManagementTrial.Models
{
    public class BankTypes
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BType { get; set; }
        [Required]
        public string BankType { get; set; }

        public virtual ICollection<Banks> Banks { get; set; }
    }
}
