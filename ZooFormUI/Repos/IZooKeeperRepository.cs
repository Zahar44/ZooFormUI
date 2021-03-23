using System;
using System.Collections.Generic;
using System.Text;
using ZooFormUI.Database;

namespace ZooFormUI.Repos
{
    interface IZooKeeperRepository
    {
        List<ZooKeeper> GetAll();

        ZooKeeper Get(int id);

        ZooKeeper Create(ZooKeeper zk);

        ZooKeeper Update(ZooKeeper zk);

        void Remove(int id);
    }
}
