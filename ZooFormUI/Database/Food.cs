using System;
using System.Collections.Generic;
using System.Text;

namespace ZooFormUI.Database
{
    public class Food
    {
        public int Id { get; set; }
        public int Size { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public bool Freeze { get; set; }
        public DateTime RotAt { get; set; }
        public ICollection<AnimalFood> AnimalFoods { get; set; }

        public override string ToString()
        {
            return String.Format(Name);
        }
    }
}
