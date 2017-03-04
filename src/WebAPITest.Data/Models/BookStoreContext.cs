using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPITest.Data.Models
{
    public class BookStoreContext : DbContext
    {
        public BookStoreContext()
        {
            Database.EnsureCreated();
        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //var connstring = Startup.Configuration["Data:BookContextConnection"];
            optionsBuilder.UseSqlServer("Server=wins12r2brij2;Database=BookDbDemo2;Trusted_Connection=true;MultipleActiveResultSets=true");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
