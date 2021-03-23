using System;
using System.Collections.Generic;
using System.Text;
using ZooFormUI.Database;

namespace ZooFormUI.Repos
{
    interface IAviaryRepository
    {
        List<Aviary> GetAll();

        Aviary Get(int id);

        Aviary Create(Aviary aviary);

        Aviary Update(Aviary aviary);

        void Remove(int id);
    }
}
