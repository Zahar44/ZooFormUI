using System;
using System.Collections.Generic;
using System.Text;

namespace ZooFormUI.Database
{
    public class Food
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public string Category { get; set; }
        public bool Freeze { get; set; }
        public DateTime RotAt { get; set; }
        public ICollection<AnimalFood> AnimalFoods { get; set; }

        public override string ToString() => Name;
    }
}
