using Microsoft.EntityFrameworkCore;

namespace ZooFormUI.Database
{
    class ZooDbContext : DbContext, IZooDbContext
    {
        private static bool _conn = false;
        public static bool Connected
        {
            get { return _conn; }
            set
            {
                _conn = value;
                UCMain.Instanse.OnDBConnected(null, new System.EventArgs(), _conn);
            }
        }
        public DbSet<Animal> Animals { get; set; }
        public DbSet<ZooKeeper> ZooKeepers { get; set; }
        public DbSet<Aviary> Aviaries { get; set; }
        public DbSet<Kind> Kinds { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<AnimalFood> AnimalFoods { get; set; }
        public DbSet<AviaryKind> AviaryKinds { get; set; }

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
