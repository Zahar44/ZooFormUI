using System;
using System.Collections.Generic;
using System.Text;
using ZooFormUI.Database;

namespace ZooFormUI.Repos
{
    interface IKindRepository
    {
        List<Kind> GetAll();

        Kind Get(int id);

        Kind Create(Kind kind);

        Kind Update(Kind kind);

        void Remove(int id);
    }
}
