using Microsoft.EntityFrameworkCore;

namespace ZooFormUI.Database
{
    interface IZooDbContext
    {
        DbSet<Animal> Animals { get; set; }
        DbSet<ZooKeeper> ZooKeepers { get; set; }

        int SaveChanges();
    }
}
