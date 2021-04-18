using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ZooFormUI.Database
{
    public class ZooKeeper : Employee
    {
        public ICollection<Animal> Animals { get; set; }

        public void SetNewValue(ZooKeeper zooKeeper)
        {
            this.Address    = zooKeeper.Address;
            this.Animals    = zooKeeper.Animals;
            this.Family     = zooKeeper.Family;
            this.FName      = zooKeeper.FName;
            this.FullName   = zooKeeper.FullName;
            this.Id         = zooKeeper.Id;
            this.MName      = zooKeeper.MName;
            this.SName      = zooKeeper.SName;
            this.Telephone  = zooKeeper.Telephone;
        }
        public ZooKeeper GetCopy()
        {
            return (ZooKeeper)this.MemberwiseClone();
        }
    }
}
