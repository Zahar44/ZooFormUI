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

        public Food()
        {
            Id = 0;
            Name = "";
            Amount = 0;
            Category = "Other";
            Freeze = true;
            RotAt = DateTime.Now;
        }
        public Food GetCopy()
        {
            return (Food)this.MemberwiseClone();
            //return new Food
            //{
            //    Id = this.Id,
            //    Name = this.Name,
            //    Amount = this.Amount,
            //    Category = this.Category,
            //    Freeze = this.Freeze,
            //    RotAt = this.RotAt,
            //    AnimalFoods = this.AnimalFoods
            //};
        }
        public void SetNewValue(Food other)
        {
            this.Id = other.Id;
            this.Name = other.Name;
            this.Amount = other.Amount;
            this.Category = other.Category;
            this.Freeze = other.Freeze;
            this.RotAt = other.RotAt;
            this.AnimalFoods = other.AnimalFoods;
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            //       
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237  
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            // TODO: write your implementation of Equals() here
            return GetHashCode() == obj.GetHashCode();
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            // TODO: write your implementation of GetHashCode() here
            return Id * Name.GetHashCode();
        }
        public override string ToString() => Name;
    }
}
