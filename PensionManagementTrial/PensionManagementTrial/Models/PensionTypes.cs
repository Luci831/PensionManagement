//PensionType
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PensionManagementTrial.Models
{
    public class PensionTypes
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Ptid { get; set; }
        [Required(ErrorMessage = "pensionertype should be selected")]
        public string PensionerType { get; set; }

    }
}
