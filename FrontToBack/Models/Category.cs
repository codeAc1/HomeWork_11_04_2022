using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required (ErrorMessage ="Bu sətri boş buraxmaq olmaz"), StringLength(100,ErrorMessage ="Maksimum 100 simvol daxil edilə bilər")]
        public string Name { get; set; }
        [Required (ErrorMessage = "Bu sətri boş buraxmaq olmaz")]
        public string  Description { get; set; }
        public bool IsDeleted { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
