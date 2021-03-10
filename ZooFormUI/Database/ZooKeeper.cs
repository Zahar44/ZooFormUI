using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ZooFormUI.Database
{
    public class ZooKeeper : Employee
    {
        public ICollection<Animal> Animals { get; set; }

    }
}
