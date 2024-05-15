using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Data
{
    public class MyDbContext :DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext>opt):base(opt)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>().HasData(
                new City() { Id=1, Name = "Cairo" },
                new City() { Id = 2, Name = "New Yourk" },
                new City() { Id = 3, Name = "Paris" },
                new City() { Id = 4, Name = "Giza" }
                );
        }
        public DbSet<City> Cities{ get; set; }
    }
}
