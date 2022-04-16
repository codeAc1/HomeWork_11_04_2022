using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.Models
{
    public class Slider
    {
        public int Id { get; set; }
        [Required, StringLength(255)]
        public string ImageUrl { get; set; }
        //[NotMapped,Required(ErrorMessage ="Şəkil Seçilməyib")]
        //public IFormFile Photo { get; set; }
        [NotMapped, Required(ErrorMessage = "Şəkil Seçilməyib")]
        public List<IFormFile>  Photos { get; set; }


    }
}
