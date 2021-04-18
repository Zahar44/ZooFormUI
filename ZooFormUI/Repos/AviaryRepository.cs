using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZooFormUI.Database;

namespace ZooFormUI.Repos
{
    class AviaryRepository : IAviaryRepository
    {
        private readonly IZooDbContext dbContext;
        public AviaryRepository(IZooDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public Aviary Create(Aviary aviary)
        {
            dbContext.Aviaries.Attach(aviary);
            dbContext.Aviaries.Add(aviary);
            if (aviary.AviaryKinds != null)
                dbContext.AviaryKinds.AddRange(aviary.AviaryKinds);
            dbContext.SaveChanges();
            return aviary;
        }

        public Aviary Get(int id)
        {
            return dbContext.Aviaries.FirstOrDefault(x => x.Id == id);
        }

        public List<Aviary> GetAll()
        {
            return dbContext.Aviaries.ToList();
        }

        public void Remove(int id)
        {
            var aviary = dbContext.Aviaries.FirstOrDefault(x => x.Id == id);
            if (aviary != null)
            {
                dbContext.Aviaries.Remove(aviary);
                dbContext.SaveChanges();
            }
        }

        public Aviary Update(Aviary aviary)
        {
            dbContext.Aviaries.Attach(aviary);
            dbContext.Aviaries.Update(aviary);
            if(aviary.AviaryKinds != null)
                dbContext.AviaryKinds.UpdateRange(aviary.AviaryKinds);
            dbContext.SaveChanges();
            return aviary;
        }
    }
}
