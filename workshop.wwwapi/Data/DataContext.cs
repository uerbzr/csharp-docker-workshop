using Microsoft.EntityFrameworkCore;
using workshop.wwwapi.DataModels;

namespace workshop.wwwapi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            //this.Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<Car>().HasData(new Car { Id = 1, Make = "VW", Model = "Beetle" });
            modelBuilder.Entity<Car>().HasData(new Car { Id = 2, Make = "VW", Model = "T5 California" });
            modelBuilder.Entity<Car>().HasData(new Car { Id = 3, Make = "VW", Model = "Up" });
            modelBuilder.Entity<Car>().HasData(new Car { Id = 4, Make = "VW", Model = "id5" });


        }
        public DbSet<Car> Cars { get; set; }
    }
}
