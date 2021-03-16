using System;
using System.Collections.Generic;
using System.Text;

namespace ZooFormUI.Database
{
    public class Aviary
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public int Width { get; set; }
        public int Length { get; set; }
        public int Height { get; set; }
        public int MaxAnimalsSize { get; set; }
        public ICollection<Kind> CanHold { get; set; }
        public ICollection<Animal> Animals { get; set; }

        public override string ToString()
        {
            return String.Format(Address);
        }
    }
}
