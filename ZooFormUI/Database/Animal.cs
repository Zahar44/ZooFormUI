using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooFormUI.Database;

namespace ZooFormUI
{
    public class Animal
    {
        [Key]
        public int Id { get; set; }
        public Kind Kind { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public bool IsPredator { get; set; }

        public int ZooKeeperId { get; set; }
        public ZooKeeper ZooKeeper { get; set; }

        public ICollection<AnimalFood> AnimalFoods { get; set; }
        public Aviary Aviary { get; set; }
    }
}
