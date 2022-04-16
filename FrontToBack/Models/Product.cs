using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public string ImageUrl { get; set; }
        public int CategoryId { get; set; }
        public bool? IsDeleted { get; set; }
        [NotMapped,Required(ErrorMessage ="Şəkil Seçilməyib")]
        public IFormFile Photo { get; set; }
        public virtual  Category Category { get; set; }

    }
}
