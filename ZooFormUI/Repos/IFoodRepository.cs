using System;
using System.Collections.Generic;
using System.Text;
using ZooFormUI.Database;

namespace ZooFormUI.Repos
{
    interface IFoodRepository
    {
        List<Food> GetAll();

        Food Get(int id);

        Food Create(Food food);

        Food Update(Food food);

        void Remove(int id);
    }
}
