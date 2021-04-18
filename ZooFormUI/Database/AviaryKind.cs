using System;
using System.Collections.Generic;
using System.Text;

namespace ZooFormUI.Database
{
    public class AviaryKind
    {
        public int Id { get; set; }
        public int AviaryId { get; set; }
        public Aviary Aviary { get; set; }
        
        public int KindId { get; set; }
        public Kind Kind { get; set; }
    }
}
