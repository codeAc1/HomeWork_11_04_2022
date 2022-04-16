using FrontToBack.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.DAL
{
    public class AppDbContext: IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options){}
        public DbSet<Slider> Sliders { get; set; }
        public DbSet <Captian> Captians { get; set; }
        public DbSet <Product> Products{ get; set; }
        public DbSet <Category> Categories{ get; set; }
        public DbSet<About> About { get; set; }
        public DbSet<Bio> Bio { get; set; }
    }
}
