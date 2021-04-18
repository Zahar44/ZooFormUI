using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ZooFormUI.Database;

namespace ZooFormUI.Repos
{
    class AnimalRepository : IAnimalRepository
    {
        private readonly IZooDbContext dbContext;
        public AnimalRepository(IZooDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public Animal Create(Animal animal)
        {
            dbContext.Animals.Attach(animal);
            dbContext.Animals.Add(animal);
            if(animal.AnimalFoods != null)
                dbContext.AnimalFoods.AddRange(animal.AnimalFoods);
            dbContext.SaveChanges();
            return animal;
        }

        public Animal Get(int id)
        {
            return dbContext
                .Animals
                .Include(x => x.Kind)
                .Include(x => x.ZooKeeper)
                .Include(x => x.AnimalFoods)
                .FirstOrDefault(x => x.Id == id);
        }

        public List<Animal> GetAll()
        {
            return dbContext.Animals
                .ToList();
        }

        public void Remove(int id)
        {
            var animal = dbContext.Animals.FirstOrDefault(x => x.Id == id);
            if(animal != null)
            {
                dbContext.Animals.Remove(animal);
                dbContext.SaveChanges();
            }
        }

        public Animal Update(Animal animal)
        {
            dbContext.Animals.Attach(animal);
            animal.Kind = dbContext.Kinds.FirstOrDefault(x => x.Id == animal.KindId);
            animal.ZooKeeper = dbContext.ZooKeepers.FirstOrDefault(x => x.Id == animal.ZooKeeperId);
            dbContext.Animals.Update(animal);
            if(animal.AnimalFoods != null)
                dbContext.AnimalFoods.UpdateRange(animal.AnimalFoods);
            dbContext.SaveChanges();
            return animal;
        }
    }
}
