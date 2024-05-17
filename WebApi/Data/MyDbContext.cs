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
                new City() { Id = 1, Name = "Cairo" },
                new City() { Id = 2, Name = "New Yourk" },
                new City() { Id = 3, Name = "Paris" },
                new City() { Id = 4, Name = "Giza" }
                );

            modelBuilder.Entity<User>().HasData(
                new User() { Id = 1, Name = "aaa", Password = "123" },
                new User() { Id = 2, Name = "bbb", Password = "456" },
                new User() { Id = 3, Name = "ccc", Password = "789" }
                );
        }
        public DbSet<City> Cities{ get; set; }
        public DbSet<User> Users{ get; set; }
    }
}
