using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZooFormUI.Database;

namespace ZooFormUI.Repos
{
    class KindRepository : IKindRepository
    {
        private readonly IZooDbContext dbContext;
        public KindRepository(IZooDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public Kind Create(Kind kind)
        {
            dbContext.Kinds.Add(kind);
            dbContext.SaveChanges();
            return kind;
        }

        public Kind Get(int id)
        {
            return dbContext.Kinds.FirstOrDefault(x => x.Id == id);
        }

        public List<Kind> GetAll()
        {
            return dbContext.Kinds.ToList();
        }

        public void Remove(int id)
        {
            var kind = dbContext.Kinds.FirstOrDefault(x => x.Id == id);
            if (kind != null)
            {
                dbContext.Kinds.Remove(kind);
                dbContext.SaveChanges();
            }
        }

        public Kind Update(Kind kind)
        {
            dbContext.Kinds.Update(kind);
            dbContext.SaveChanges();
            return kind;
        }
    }
}
