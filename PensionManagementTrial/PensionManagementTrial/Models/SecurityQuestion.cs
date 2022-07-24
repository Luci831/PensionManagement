//SecurityQuestion
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PensionManagementTrial.Models
{
    public class SecurityQuestion
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Sqid { get; set; }
        [Required(ErrorMessage = "please choose a question")]
        public string Questions { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}

