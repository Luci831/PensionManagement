using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace PensionManagementTrial.Models
{
        public class Category
        {
            [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int CatId { get; set; }
            [Required(ErrorMessage = "please select the category")]
            public string CategoryType { get; set; }
            public virtual ICollection<User> Users { get; set; }

        }
}
