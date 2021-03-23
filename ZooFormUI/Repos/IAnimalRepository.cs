using System;
using System.Collections.Generic;
using System.Text;
using ZooFormUI.Database;

namespace ZooFormUI.Repos
{
    interface IAnimalRepository
    {
        List<Animal> GetAll();

        Animal Get(int id);

        Animal Create(Animal animal);
        
        Animal Update(Animal animal);

        void Remove(int id);
    }
}
