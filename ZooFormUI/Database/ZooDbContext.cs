﻿using Microsoft.EntityFrameworkCore;

namespace ZooFormUI.Database
{
    class ZooDbContext : DbContext, IZooDbContext
    {
        public DbSet<Animal> Animals { get; set; }
        public DbSet<ZooKeeper> ZooKeepers { get; set; }
        public DbSet<Aviary> Aviaries { get; set; }
        public DbSet<Kind> Kinds { get; set; }
        public DbSet<Food> Foods { get; set; }
        public ZooDbContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer("Server=(local);Initial Catalog=Zoo;Trusted_Connection=True;MultipleActiveResultSets=true");
        }
    }
}
