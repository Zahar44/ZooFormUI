using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ZooFormUI.Database
{
    public class Person
    {
        [Key]
        public int Id { get; set; }
        public enum family { male, female, unknown}
        public string Name { get; set; }
        public family Family { get; set; }
        public Person()
        {
            Name = "unset";
            Family = family.unknown;
        }

    }
}
