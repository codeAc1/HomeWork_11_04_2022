using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.Models
{
    public class AppUser:IdentityUser
    {
        [Required(ErrorMessage ="Ad mutleq daxil edilmelidir")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Soyad mutleq daxil edilmelidir")]
        public string SurName { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsOnline { get; set; }
    }
}
