using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public string Name { get; set; }
        public int Age { get; set; }
        public bool IsPredator { get; set; }

        public int KindId { get; set; }
        public Kind Kind { get; set; }

        public int ZooKeeperId { get; set; }
        public ZooKeeper ZooKeeper { get; set; }

        public ICollection<AnimalFood> AnimalFoods { get; set; }

        public Animal()
        {
            Name = "";
            Age = 0;
            IsPredator = true;
        }

        public Animal GetCopy()
        {
            return (Animal)this.MemberwiseClone();
        }
        public void SetNewValue(Animal other)
        {
            this.Id = other.Id;
            this.Name = other.Name;
            this.Age = other.Age;
            this.IsPredator = other.IsPredator;
            this.KindId = other.KindId;
            this.Kind = other.Kind;
            this.ZooKeeper = other.ZooKeeper;
            this.ZooKeeperId = other.ZooKeeperId;
            this.AnimalFoods = other.AnimalFoods;
        }
        public override string ToString() => Name;
    }
}
