using System;
using System.Collections.Generic;
using System.Text;

namespace ZooFormUI.Database
{
    public class Kind
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsWormBlooded { get; set; }
        public string Description { get; set; }
        public string Сonditions { get; set; }
        public override string ToString() => Name;
    }
}
