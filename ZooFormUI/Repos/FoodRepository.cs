using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZooFormUI.Database;

namespace ZooFormUI.Repos
{
    class FoodRepository : IFoodRepository
    {
        private readonly IZooDbContext dbContext;
        public FoodRepository(IZooDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public Food Create(Food food)
        {
            dbContext.Foods.Add(food);
            dbContext.SaveChanges();
            return food;
        }

        public Food Get(int id)
        {
            return dbContext.Foods.FirstOrDefault(x => x.Id == id);
        }

        public List<Food> GetAll()
        {
            return dbContext.Foods.ToList();
        }

        public void Remove(int id)
        {
            var food = dbContext.Foods.FirstOrDefault(x => x.Id == id);
            if (food != null)
            {
                dbContext.Foods.Remove(food);
                dbContext.SaveChanges();
            }
        }

        public Food Update(Food food)
        {
            dbContext.Foods.Update(food);
            dbContext.SaveChanges();
            return food;
        }
    }
}
