using Microsoft.EntityFrameworkCore;

namespace ZooFormUI.Database
{
    interface IZooDbContext
    {
        public DbSet<Animal> Animals { get; set; }
        public DbSet<ZooKeeper> ZooKeepers { get; set; }
        public DbSet<Aviary> Aviaries { get; set; }
        public DbSet<Kind> Kinds { get; set; }
        public DbSet<Food> Foods { get; set; }

        int SaveChanges();
    }
}
