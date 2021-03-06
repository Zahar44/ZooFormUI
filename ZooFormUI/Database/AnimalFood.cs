using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ZooFormUI.Database
{
    public class AnimalFood
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int AnimalId { get; set; }
        public Animal Animal { get; set; }

        public int FoodId { get; set; }
        public Food Food { get; set; }
    }
}
