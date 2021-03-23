using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZooFormUI.Database;

namespace ZooFormUI.Repos
{
    class ZooKeeperRepository : IZooKeeperRepository
    {
        private readonly IZooDbContext dbContext;
        public ZooKeeperRepository(IZooDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public ZooKeeper Create(ZooKeeper zk)
        {
            dbContext.ZooKeepers.Add(zk);
            dbContext.SaveChanges();
            return zk;
        }

        public ZooKeeper Get(int id)
        {
            return dbContext.ZooKeepers.FirstOrDefault(x => x.Id == id);
        }

        public List<ZooKeeper> GetAll()
        {
            return dbContext.ZooKeepers.ToList();
        }

        public void Remove(int id)
        {
            var zk = dbContext.ZooKeepers.FirstOrDefault(x => x.Id == id);
            if (zk != null)
            {
                dbContext.ZooKeepers.Remove(zk);
                dbContext.SaveChanges();
            }
        }

        public ZooKeeper Update(ZooKeeper zk)
        {
            dbContext.ZooKeepers.Update(zk);
            dbContext.SaveChanges();
            return zk;
        }
    }
}
