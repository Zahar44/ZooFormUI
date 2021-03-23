using System;
using System.Collections.Generic;
using System.Linq;
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
            dbContext.Animals.Add(animal);
            dbContext.SaveChanges();
            return animal;
        }

        public Animal Get(int id)
        {
            return dbContext.Animals.FirstOrDefault(x => x.Id == id);
        }

        public List<Animal> GetAll()
        {
            return dbContext.Animals.ToList();
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
            //var response = dbContext.Animals.FirstOrDefault(x => x.Id == animal.Id);
            dbContext.Animals.Update(animal);
            dbContext.SaveChanges();
            return animal;
        }
    }
}
