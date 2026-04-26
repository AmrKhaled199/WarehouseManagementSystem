using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Models;

namespace WMS.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<StorageFee> StorageFees { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Product -> StorageFee (one-to-one)
            modelBuilder.Entity<Product>()
                .HasOne(p => p.StorageFee)
                .WithOne(s => s.Product)
                .HasForeignKey<StorageFee>(s => s.ProductId);

            // Product -> Notifications (one-to-many)
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Product)
                .WithMany(p => p.Notifications)
                .HasForeignKey(n => n.ProductId);
        }
    }
}