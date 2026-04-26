using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace WMS.DAL
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=WarehouseDB;Trusted_Connection=True;TrustServerCertificate=True");

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}